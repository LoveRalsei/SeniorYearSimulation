using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidApp1.Actions;
using AndroidApp1.UIData;

namespace AndroidApp1
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private GameManager? _gameManager;
        private ActionCategoryManager? _categoryManager;
        private TextView? _propertiesTextView;
        private LinearLayout? _resultContainer;
        private GameIntro? _gameIntro;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // ── Core game ────────────────────────────────────────────

            _gameManager = new GameManager(this);
            _gameManager.OnStudentChanged += RefreshPropertiesTextView;

            // ── Property display ─────────────────────────────────────

            _propertiesTextView = FindViewById<TextView>(Resource.Id.propertiesTextView);
            _resultContainer = FindViewById<LinearLayout>(Resource.Id.resultContainer);

            // ── Action categories ────────────────────────────────────

            _categoryManager = new ActionCategoryManager();
            _categoryManager.SetModifier(_gameManager.Modifier);

            // Register categories (data-driven: add new category here in one place)
            _categoryManager.AddCategory(new ActionCategory(
                Resource.Id.action_study, "学习",
                new Data_StudyActionButtons(_gameManager.Modifier).ActionButtons));
            _categoryManager.AddCategory(new ActionCategory(
                Resource.Id.action_spendmoney, "消费",
                new Data_SpendMoneyActionButtonsConfig(_gameManager.Modifier).ActionButtons));
            _categoryManager.AddCategory(new ActionCategory(
                Resource.Id.action_doactivity, "活动",
                new Data_DoActivityActionButtonsConfig(_gameManager.Modifier).ActionButtons));
            _categoryManager.AddCategory(new ActionCategory(
                Resource.Id.action_pastime, "消遣",
                new Data_PastTimeActionButtonsConfig(_gameManager.Modifier).ActionButtons));
            _categoryManager.AddCategory(new ActionCategory(
                Resource.Id.action_lazy, "偷懒",
                new Data_LazyActionButtonsConfig(_gameManager.Modifier).ActionButtons));

            _categoryManager.Initialize(_resultContainer, this);

            // Wire up property refresh after every action completes
            foreach (var cat in _categoryManager.Categories)
                foreach (var btn in cat.Buttons)
                    btn.OnActionCompleted = () => _gameManager.NotifyStudentChanged();

            // Bind toggle button clicks through the strip
            var buttonStrip = FindViewById<LinearLayout>(Resource.Id.button_strip);
            _categoryManager.BindToggleButtons(this, buttonStrip);

            // ── End turn button ──────────────────────────────────────

            var endTurnButton = FindViewById<Button>(Resource.Id.endturnbutton);
            endTurnButton.Click += (s, e) => _gameManager.EndTurn();

            // ── Misc setup ───────────────────────────────────────────

            FitTextToWidth(_propertiesTextView, minSp: 10f, maxSp: 36f);

            ActionBar.SetDisplayShowCustomEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(false);

            _gameIntro = new GameIntro(this);

            // Show initial category and start game
            _categoryManager.SwitchTo(Resource.Id.action_study);
            _gameManager.StartGame();

            base.OnCreate(savedInstanceState);
        }

        // ── Property display ─────────────────────────────────────────

        public void RefreshPropertiesTextView()
        {
            var student = _gameManager?.StudentData;
            if (_propertiesTextView == null || student == null) return;

            _propertiesTextView.Text =
                $"\t{student.name}\t\t\t\t金钱：{student.money}\n" +
                $"\t健康：{student.health}\t\t精力：{student.energy}\t\t心情：{student.happiness}\n" +
                $"\t魅力：{student.charm}\t\t懒惰：{student.laziness}\t\t迷茫：{student.confusion}\n" +
                $"\t语文：{student.chinese}\t\t数学：{student.math}\t\t英语：{student.english}\n" +
                $"\t{student.crouse1Name}：{student.crouse1Grade}\t\t" +
                $"{student.crouse2Name}：{student.crouse2Grade}\t\t" +
                $"{student.crouse3Name}：{student.crouse3Grade}";
        }

        // ── Text fitting util ────────────────────────────────────────

        private void FitTextToWidth(TextView tv, float minSp = 8f, float maxSp = 40f)
        {
            var paint = new Paint(tv.Paint);
            float targetPx = tv.Width - tv.PaddingLeft - tv.PaddingRight;
            float lo = minSp, hi = maxSp, best = minSp;

            while (hi - lo > 0.5f)
            {
                float mid = (hi + lo) / 2f;
                float midPx = TypedValue.ApplyDimension(ComplexUnitType.Sp, mid, tv.Resources!.DisplayMetrics!);
                paint.TextSize = midPx;
                float measured = paint.MeasureText(tv.Text ?? "");
                if (measured > targetPx)
                    hi = mid;
                else
                {
                    best = mid;
                    lo = mid;
                }
            }

            tv.SetTextSize(ComplexUnitType.Sp, best * 2);
        }
    }
}

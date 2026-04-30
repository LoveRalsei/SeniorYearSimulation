using Android.Graphics;
using Android.Util;
using Android.Views;
using AndroidApp1.UIData;

namespace AndroidApp1
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        public GameManager _gameManager;

        TextView propertiesTextView;

        LinearLayout resultContainer;
        List<Button> buttons;

        // ScrollView中的按钮列表
        List<Button> scrollViewButtons;
        List<StudyActionButton> buttonGroup_Study;
        List<ActionButton> buttonGroup_SpendMoney;
        List<ActionButton> buttonGroup_DoActivity;
        List<ActionButton> buttonGroup_PastTime;
        List<ActionButton> buttonGroup_Lazy;

        GameIntro _gameIntro;
        Button _endTurnButton;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _gameManager = new GameManager(this);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            propertiesTextView = FindViewById<TextView>(Resource.Id.propertiesTextView);

            RefreshPropertiesTextView();


            // 找到控件
            var btnA = FindViewById<Button>(Resource.Id.action_study);
            var btnB = FindViewById<Button>(Resource.Id.action_spendmoney);
            var btnC = FindViewById<Button>(Resource.Id.action_doactivity);
            var btnD = FindViewById<Button>(Resource.Id.action_pastime);
            var btnE = FindViewById<Button>(Resource.Id.action_lazy);
            _endTurnButton = FindViewById<Button>(Resource.Id.endturnbutton);
            //resultContainer = FindViewById<LinearLayout>(Resource.Id.resultContainer);
            resultContainer = FindViewById<LinearLayout>(Resource.Id.resultContainer);

            buttons = new List<Button> { btnA, btnB, btnC, btnD ,btnE};
            // 公共点击处理器

            // 绑定事件
            btnA.Click += (s, e) => HandleButtonClick(btnA, "学习按钮");
            btnB.Click += (s, e) => HandleButtonClick(btnB, "消费按钮");
            btnC.Click += (s, e) => HandleButtonClick(btnC, "活动按钮");
            btnD.Click += (s, e) => HandleButtonClick(btnD, "消遣按钮");
            btnE.Click += (s, e) => HandleButtonClick(btnE, "偷懒按钮");
            //_endTurnButton.Click += (s, e) => _gameManager.EndTurn();


            FitTextToWidth(propertiesTextView, minSp: 10f, maxSp: 36f);
            /*var json = JsonFileReader.FindArrayByKey("text.json", "test1");
            btnA.Text = FindViewById<Button>(Resource.Id.testButton)==null ? "默认文本" : "找到按钮";*/

            // 初始化ScrollView中的按钮列表
            scrollViewButtons = new List<Button>();
            buttonGroup_Study = new List<StudyActionButton>();
            buttonGroup_SpendMoney = new List<ActionButton>();
            buttonGroup_DoActivity = new List<ActionButton>();
            buttonGroup_PastTime = new List<ActionButton>();
            buttonGroup_Lazy = new List<ActionButton>();


            base.OnCreate(savedInstanceState);

            // 初始化行动按钮
            InitializeActionButtons();

            // 可选：如果想一开始就让某个按钮处于启用并显示其内容，调用一次：
            HandleButtonClick(btnA, "学习按钮");
            // 设置自定义标题栏布局
            ActionBar.SetDisplayShowCustomEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(false);

            _gameIntro=new GameIntro(this);

            _gameManager.StartGame();
        }

        // 处理按钮点击的公共方法
        void HandleButtonClick(Button selected, string label)
        {
            // 视觉反馈：把非选中按钮变灰，但保持可点击
            foreach (var b in buttons)
            {
                b.Enabled = true;                 // 确保可点击（移除把其它按钮设为 false 的代码）
                b.Alpha = (b == selected) ? 1f : 0.5f; // 1.0 正常，0.5 灰显
            }

            // 根据点击的按钮切换显示对应的按钮组
            if (selected.Id == Resource.Id.action_study)
            {
                ShowStudyActionButtons(buttonGroup_Study);
                HideActionButtons(buttonGroup_SpendMoney);
                HideActionButtons(buttonGroup_DoActivity);
                HideActionButtons(buttonGroup_PastTime);
                HideActionButtons(buttonGroup_Lazy);
            }
            else if (selected.Id == Resource.Id.action_spendmoney)
            {
                HideStudyActionButtons(buttonGroup_Study);
                ShowActionButtons(buttonGroup_SpendMoney);
                HideActionButtons(buttonGroup_DoActivity);
                HideActionButtons(buttonGroup_PastTime);
                HideActionButtons(buttonGroup_Lazy);
            }
            else if(selected.Id == Resource.Id.action_doactivity)
            {
                HideStudyActionButtons(buttonGroup_Study);
                HideActionButtons(buttonGroup_SpendMoney);
                ShowActionButtons(buttonGroup_DoActivity);
                HideActionButtons(buttonGroup_PastTime);
                HideActionButtons(buttonGroup_Lazy);
            }
            else if (selected.Id == Resource.Id.action_pastime)
            {
                HideStudyActionButtons(buttonGroup_Study);
                HideActionButtons(buttonGroup_SpendMoney);
                HideActionButtons(buttonGroup_DoActivity);
                ShowActionButtons(buttonGroup_PastTime);
                HideActionButtons(buttonGroup_Lazy);
            }
            else if(selected.Id == Resource.Id.action_lazy)
            {
                HideStudyActionButtons(buttonGroup_Study);
                HideActionButtons(buttonGroup_SpendMoney);
                HideActionButtons(buttonGroup_DoActivity);
                HideActionButtons(buttonGroup_PastTime);
                ShowActionButtons(buttonGroup_Lazy);
            }
        }
        // 适配文本大小以适应 TextView 宽度的函数
        void FitTextToWidth(TextView tv, float minSp = 8f, float maxSp = 40f)
        { // 如果宽度还没计算好，等布局完成后再执行 if (tv.Width == 0) { tv.ViewTreeObserver.GlobalLayout += new ViewTreeObserverGlobalLayoutListener(() => { FitTextToWidth(tv, minSp, maxSp); }); return; }
            var paint = new Paint(tv.Paint);
            float targetPx = tv.Width - tv.PaddingLeft - tv.PaddingRight;
            float lo = minSp, hi = maxSp, best = minSp;

            while (hi - lo > 0.5f)
            {
                float mid = (hi + lo) / 2f;
                float midPx = TypedValue.ApplyDimension(ComplexUnitType.Sp, mid, tv.Resources.DisplayMetrics);
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
        // 在 OnCreate 中示例调用： var tv = FindViewById<TextView>(Resource.Id.myTextView); FitTextToWidth(tv, minSp: 10f, maxSp: 36f);

        // 刷新属性显示的函数
        public void RefreshPropertiesTextView()
        {
            if (GameManager.StudentData != null)
                propertiesTextView?.Text = $"\t{GameManager.StudentData.name}\t\t\t\t金钱：{GameManager.StudentData.money}\n" +
                    $"\t健康：{GameManager.StudentData.health}\t\t精力：{GameManager.StudentData.energy}\t\t心情：{GameManager.StudentData.happiness}\n" +
                    $"\t魅力：{GameManager.StudentData.charm}\t\t懒惰：{GameManager.StudentData.laziness}\t\t迷茫：{GameManager.StudentData.confusion}\n" +
                    $"\t语文：{GameManager.StudentData.chinese}\t\t数学：{GameManager.StudentData.math}\t\t英语：{GameManager.StudentData.english}\n" +
                    $"\t{GameManager.StudentData.crouse1Name}：{GameManager.StudentData.crouse1Grade}\t\t{GameManager.StudentData.crouse2Name}：{GameManager.StudentData.crouse2Grade}\t\t{GameManager.StudentData.crouse3Name}：{GameManager.StudentData.crouse3Grade}";
        }

        //显示和隐藏按钮列表的按钮
        #region
        // 显示一个列表内所有按钮
        public void ShowActionButtons(List<ActionButton> buttonList)
        {
            if (buttonList == null) return;

            foreach (var button in buttonList)
            {
                if (button != null)
                {
                    button.Visibility = ViewStates.Visible;
                }
            }
        }

        public void ShowStudyActionButtons(List<StudyActionButton> buttonList)
        {
            if (buttonList == null) return;

            foreach (var button in buttonList)
            {
                if (button != null)
                {
                    button.Visibility = ViewStates.Visible;
                }
            }
        }

        // 隐藏一个列表内所有按钮
        public void HideActionButtons(List<ActionButton> buttonList)
        {
            if (buttonList == null) return;

            foreach (var button in buttonList)
            {
                if (button != null)
                {
                    button.Visibility = ViewStates.Gone;
                }
            }
        }

        public void HideStudyActionButtons(List<StudyActionButton> buttonList)
        {
            if (buttonList == null) return;

            foreach (var button in buttonList)
            {
                if (button != null)
                {
                    button.Visibility = ViewStates.Gone;
                }
            }
        }
        #endregion//

        public void InitializeActionButtons()
        {
            Data_StudyActionButtons data_StudyActionButtonsConfig = new Data_StudyActionButtons(this);
            Data_SpendMoneyActionButtonsConfig data_SpendMoneyActionButtonsConfig = new Data_SpendMoneyActionButtonsConfig(this);
            Data_DoActivityActionButtonsConfig data_DoActivityActionButtonsConfig = new Data_DoActivityActionButtonsConfig(this);
            Data_PastTimeActionButtonsConfig data_PastTimeActionButtonsConfig = new Data_PastTimeActionButtonsConfig(this);
            Data_LazyActionButtonsConfig data_LazyActionButtonsConfig = new Data_LazyActionButtonsConfig(this);

            foreach (var config in data_StudyActionButtonsConfig.ActionButtons)
            {
                var actionBtn = new StudyActionButton(this);

                var currentBtn = actionBtn;

                actionBtn.SetAllTexts(config.Title, config.Description, config.CostText);
                currentBtn.SetDialog(config.DialogTitle,config.DialogIntro,config.DialogFinish,config.CostEnergy,config.Effects);
                actionBtn.SetOnClickListener((s, e) =>
                {
                    currentBtn.ShowDialog();
                });
                buttonGroup_Study.Add(actionBtn);
                resultContainer.AddView(actionBtn);
            }
            foreach(var config in data_SpendMoneyActionButtonsConfig.ActionButtons)
            {
                var actionBtn1 = new ActionButton(this);
                var currentBtn1 = actionBtn1;
                actionBtn1.SetAllTexts(config.Title, config.Description, config.CostText);
                currentBtn1.SetDialog(config.DialogTitle, config.DialogIntro, config.DialogFinish, config.CostEnergy, config.Effects);
                actionBtn1.SetOnClickListener((s, e) =>
                {
                    currentBtn1.ShowDialog();
                });
                buttonGroup_SpendMoney.Add(actionBtn1);
                resultContainer.AddView(actionBtn1);
            }
            foreach(var config in data_DoActivityActionButtonsConfig.ActionButtons)
            {
                var actionBtn2 = new ActionButton(this);
                var currentBtn2 = actionBtn2;
                actionBtn2.SetAllTexts(config.Title, config.Description, config.CostText);
                currentBtn2.SetDialog(config.DialogTitle, config.DialogIntro, config.DialogFinish, config.CostEnergy, config.Effects);
                actionBtn2.SetOnClickListener((s, e) =>
                {
                    currentBtn2.ShowDialog();
                });
                buttonGroup_DoActivity.Add(actionBtn2);
                resultContainer.AddView(actionBtn2);
            }
            foreach(var config in data_PastTimeActionButtonsConfig.ActionButtons)
            {
                var actionBtn3 = new ActionButton(this);
                var currentBtn3 = actionBtn3;
                actionBtn3.SetAllTexts(config.Title, config.Description, config.CostText);
                currentBtn3.SetDialog(config.DialogTitle, config.DialogIntro, config.DialogFinish, config.CostEnergy, config.Effects);
                actionBtn3.SetOnClickListener((s, e) =>
                {
                    currentBtn3.ShowDialog();
                });
                buttonGroup_PastTime.Add(actionBtn3);
                resultContainer.AddView(actionBtn3);
            }
            foreach (var config in data_LazyActionButtonsConfig.ActionButtons)
            {
                var actionBtn4 = new ActionButton(this);
                var currentBtn4 = actionBtn4;
                actionBtn4.SetAllTexts(config.Title, config.Description, config.CostText);
                currentBtn4.SetDialog(config.DialogTitle, config.DialogIntro, config.DialogFinish, config.CostEnergy, config.Effects);
                actionBtn4.SetOnClickListener((s, e) =>
                {
                    currentBtn4.ShowDialog();
                });
                buttonGroup_Lazy.Add(actionBtn4);
                resultContainer.AddView(actionBtn4);
            }
            /*UI_StudyActionButtons actionButtons = new UI_StudyActionButtons(this);
            foreach (var config in actionButtons.ActionButtons)
            {
                var actionBtn = new ActionButton(this);

                var currentBtn = actionBtn;

                actionBtn.SetAllTexts(config.Title, config.Description, config.Cost);
                currentBtn.SetDialog(config.DialogTitle, config.DialogIntro, config.DialogFinish, config.ClickAction);
                actionBtn.SetOnClickListener((s, e) =>
                {
                    currentBtn.ShowDialog();
                });
                buttonGroup_Study.Add(actionBtn);
                resultContainer.AddView(actionBtn);
            }*/
        }
    }
}
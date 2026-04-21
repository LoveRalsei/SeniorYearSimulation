using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace AndroidApp1
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private GameManager _gameManager;

        //Student student = new Student("111", 2, 3, 4, 5, 6,6, 6, 7, 8, 9, "物理", 1, "化学", 2, "生物", 3);
        
        TextView propertiesTextView;

        LinearLayout resultContainer;
        List<Button> buttons;

        // ScrollView中的按钮列表
        List<Button> scrollViewButtons;
        List<Button> buttonGroup1;
        List<Button> buttonGroup2;
        List<Button> buttonGroup3;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _gameManager= new GameManager(this);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            propertiesTextView = FindViewById<TextView>(Resource.Id.propertiesTextView);

            RefreshPropertiesTextView();


            // 找到控件
            var btnA = FindViewById<Button>(Resource.Id.btnA);
            var btnB = FindViewById<Button>(Resource.Id.btnB);
            var btnC = FindViewById<Button>(Resource.Id.btnC);
            //resultContainer = FindViewById<LinearLayout>(Resource.Id.resultContainer);

            buttons = new List<Button> { btnA, btnB, btnC };
            // 公共点击处理器

            // 绑定事件
            btnA.Click += (s, e) => HandleButtonClick(btnA, "按钮 A");
            btnB.Click += (s, e) => HandleButtonClick(btnB, "按钮 B");
            btnC.Click += (s, e) => HandleButtonClick(btnC, "按钮 C");

            // 可选：如果想一开始就让某个按钮处于启用并显示其内容，调用一次：
            HandleButtonClick(btnA, "按钮 A");

            FitTextToWidth(propertiesTextView, minSp: 10f, maxSp: 36f);
            /*var json = JsonFileReader.FindArrayByKey("text.json", "test1");
            btnA.Text = FindViewById<Button>(Resource.Id.testButton)==null ? "默认文本" : "找到按钮";*/

            // 初始化ScrollView中的按钮列表
            scrollViewButtons = new List<Button>();
            buttonGroup1 = new List<Button>();
            buttonGroup2 = new List<Button>();
            buttonGroup3 = new List<Button>();

            // 找到ScrollView中的所有按钮并添加到对应列表
            var testButton = FindViewById<Button>(Resource.Id.testButton);
            var testButton11 = FindViewById<Button>(Resource.Id.testButton11);
            var testButton12 = FindViewById<Button>(Resource.Id.testButton12);
            var testButton3 = FindViewById<Button>(Resource.Id.testButton3);
            var testButton14 = FindViewById<Button>(Resource.Id.testButton14);

            if (testButton != null) { scrollViewButtons.Add(testButton); buttonGroup1.Add(testButton); }
            if (testButton11 != null) { scrollViewButtons.Add(testButton11); buttonGroup1.Add(testButton11); }
            if (testButton12 != null) { scrollViewButtons.Add(testButton12); buttonGroup2.Add(testButton12); }
            if (testButton3 != null) { scrollViewButtons.Add(testButton3); buttonGroup2.Add(testButton3); }
            if (testButton14 != null) { scrollViewButtons.Add(testButton14); buttonGroup3.Add(testButton14); }

            // 为testButton添加点击事件，显示CustomDialog弹窗
            if (testButton != null)
            {
                testButton.Click += (s, e) =>
                {
                    var dialog = new CustomDialog(this);
                    dialog.SetTitle("提示");
                    dialog.SetMessage("这是一个自定义弹窗，点击确定按钮关闭。");
                    dialog.SetButtonText("确定");
                    // 设置按钮点击事件：关闭弹窗
                    dialog.SetOnButtonClick(() => dialog.Hide());
                    dialog.Show();
                    dialog.EnableTypewriterEffect(true); // 启用打字机效果（可选）
                    dialog.SetOnTypewriterComplete(() =>
                    {
                        // 弹窗打字机完成后的逻辑
                    });
                    dialog.SetMessage("这是一个逐字显示的文本效果，每0.2秒显示一个字符。");
                    dialog.SetScrollButton("滚动按钮", () => {
                        // 点击响应逻辑
                    });
                    dialog.CancelOnTouchOutside=false; // 点击外部区域关闭弹窗（可选）
                };
            }

            // 示例：初始显示第一组按钮，隐藏其他组
            ShowButtons(buttonGroup1);
            HideButtons(buttonGroup2);
            HideButtons(buttonGroup3);

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
            if (selected.Id == Resource.Id.btnA)
            {
                ShowButtons(buttonGroup1);
                HideButtons(buttonGroup2);
                HideButtons(buttonGroup3);
            }
            else if (selected.Id == Resource.Id.btnB)
            {
                HideButtons(buttonGroup1);
                ShowButtons(buttonGroup2);
                HideButtons(buttonGroup3);
            }
            else if (selected.Id == Resource.Id.btnC)
            {
                HideButtons(buttonGroup1);
                HideButtons(buttonGroup2);
                ShowButtons(buttonGroup3);
            }

            // 隐藏其它按钮所生成的控件：清空容器并只添加当前按钮的控件
            //resultContainer.RemoveAllViews();

            // 示例：为当前选中按钮动态添加若干测试 TextView（按需修改数量与样式）
            for (int i = 1; i <= 5; i++)
            {
                var tv = new TextView(this)
                {
                    Text = $"{label} 的项目 {i}",
                    TextSize = 16
                };
                var lp = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.WrapContent);
                lp.TopMargin = (int)(4 * Resources.DisplayMetrics.Density);
                tv.LayoutParameters = lp;
                //resultContainer.AddView(tv);
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

            tv.SetTextSize(ComplexUnitType.Sp, best*2);
        }
        // 在 OnCreate 中示例调用： var tv = FindViewById<TextView>(Resource.Id.myTextView); FitTextToWidth(tv, minSp: 10f, maxSp: 36f);

        // 刷新属性显示的函数
        protected void RefreshPropertiesTextView()
        {
            Student student = _gameManager.Student;
            if (student != null) 
            propertiesTextView?.Text = $"\t健康：{student.health}\t\t精力：{student.energy}\t\t心情：{student.happiness}\n" +
                $"\t魅力：{student.charm}\t\t懒惰：{student.laziness}\t\t迷茫：{student.confusion}\n" +
                $"\t语文：{student.chinese}\t\t数学：{student.math}\t\t英语：{student.english}\n" +
                $"\t{student.crouse1Name}：{student.crouse1Grade}\t\t{student.crouse2Name}：{student.crouse2Grade}\t\t{student.crouse3Name}：{student.crouse3Grade}";
        }

        /*private bool NoLazy()
        {
            int randInt = Random.Shared.Next(1, 100);
            return randInt >= student.laziness;
        }

        private bool NoConfusion()
        {
            int randInt = Random.Shared.Next(1, 100);
            return randInt >= student.confusion;
        }*/

        // 显示一个列表内所有按钮
        public void ShowButtons(List<Button> buttonList)
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
        public void HideButtons(List<Button> buttonList)
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
    }
}

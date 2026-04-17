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
        Student student = new Student("111", 2, 3, 4, 5, 6, 6, 7, 8, 9, "物理", 1, "化学", 2, "生物", 3);
        TextView propertiesTextView;

        LinearLayout resultContainer;
        List<Button> buttons;
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            propertiesTextView = FindViewById<TextView>(Resource.Id.propertiesTextView);

            RefreshPropertiesTextView();


            // 找到控件
            var btnA = FindViewById<Button>(Resource.Id.btnA);
            var btnB = FindViewById<Button>(Resource.Id.btnB);
            var btnC = FindViewById<Button>(Resource.Id.btnC);
            resultContainer = FindViewById<LinearLayout>(Resource.Id.resultContainer);

            buttons = new List<Button> { btnA, btnB, btnC };
            // 公共点击处理器

            // 绑定事件
            btnA.Click += (s, e) => HandleButtonClick(btnA, "按钮 A");
            btnB.Click += (s, e) => HandleButtonClick(btnB, "按钮 B");
            btnC.Click += (s, e) => HandleButtonClick(btnC, "按钮 C");

            // 可选：如果想一开始就让某个按钮处于启用并显示其内容，调用一次：
            HandleButtonClick(btnA, "按钮 A");

            FitTextToWidth(propertiesTextView, minSp: 10f, maxSp: 36f);
            var json = JsonFileReader.FindArrayByKey("text.json", "test1");
            btnA.Text = json;
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

            // 隐藏其它按钮所生成的控件：清空容器并只添加当前按钮的控件
            resultContainer.RemoveAllViews();

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
                resultContainer.AddView(tv);
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

            tv.SetTextSize(ComplexUnitType.Sp, best*3);
        }
        // 在 OnCreate 中示例调用： var tv = FindViewById<TextView>(Resource.Id.myTextView); FitTextToWidth(tv, minSp: 10f, maxSp: 36f);

        // 刷新属性显示的函数
        protected void RefreshPropertiesTextView()
        {
            propertiesTextView?.Text = $"\t健康：{student.health}\t精力：{student.energy}\t心情：{student.happiness}\n" +
                $"\t魅力：{student.charm}\t懒惰：{student.laziness}\t迷茫：{student.confusion}\n" +
                $"\t语文：{student.chinese}\t数学：{student.math}\t英语：{student.english}\n" +
                $"\t{student.crouse1Name}：{student.crouse1Grade}\t{student.crouse2Name}：{student.crouse2Grade}\t{student.crouse3Name}：{student.crouse3Grade}";
        }

        private bool NoLazy()
        {
            int randInt = Random.Shared.Next(1, 100);
            return randInt >= student.laziness;
        }

        private bool NoConfusion()
        {
            int randInt = Random.Shared.Next(1, 100);
            return randInt >= student.confusion;
        }
    }
}
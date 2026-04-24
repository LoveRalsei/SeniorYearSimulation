using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.Content;
using Android.Widget;
using Android.Views;

namespace AndroidApp1
{
    //行动按钮的自定义控件，继承自LinearLayout
    public class ActionButton : LinearLayout
    {
        // 三个TextView成员变量
        private TextView titleText;
        private TextView descText;
        private TextView costText;

        private CustomDialog _dialog;

        // 构造函数
        public ActionButton(Context context) : base(context)
        {
            Initialize(context);
        }

        // 初始化布局和控件
        private void Initialize(Context context)
        {
            // 1. 设置LinearLayout的基本属性
            this.Orientation = Orientation.Vertical;
            this.SetPadding(20, 20, 20, 20);
            this.SetBackgroundColor(Android.Graphics.Color.ParseColor("#F5F5F5"));

            // 2. 创建水平布局用于标题和花费
            var headerLayout = new LinearLayout(context);
            headerLayout.Orientation = Orientation.Horizontal;
            headerLayout.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

            // 标题TextView
            titleText = new TextView(context);
            titleText.TextSize = 16;
            titleText.SetTypeface(null, Android.Graphics.TypefaceStyle.Bold);
            titleText.LayoutParameters = new LayoutParams(0, LayoutParams.WrapContent, 1);
            headerLayout.AddView(titleText);

            // 花费TextView
            costText = new TextView(context);
            costText.TextSize = 14;
            costText.LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
            headerLayout.AddView(costText);

            // 3. 创建描述TextView
            descText = new TextView(context);
            descText.TextSize = 14;
            descText.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

            // 4. 添加到LinearLayout
            this.AddView(headerLayout);
            this.AddView(descText);
        }

        // 公共方法：设置标题
        public void SetTitle(string title)
        {
            titleText.Text = title;
        }

        // 公共方法：设置介绍文本
        public void SetDescription(string description)
        {
            descText.Text = description;
        }

        // 公共方法：设置花费文本
        public void SetCost(string cost)
        {
            costText.Text = cost;
        }

        // 公共方法：一次性设置所有文本
        public void SetAllTexts(string title, string description, string cost)
        {
            titleText.Text = title;
            descText.Text = description;
            costText.Text = cost;
        }

        public void SetDialog(string title, string intro, string finishText, System.Action onclick)
        {
            _dialog = new CustomDialog(Context);
            _dialog.BecomeActionDialog(title, intro, finishText, onclick);
            _dialog.CancelOnTouchOutside = true; // 点击外部区域关闭弹窗
        }

        public void ShowDialog()
        {
            _dialog.Show();
        }

        // 公共方法：设置点击事件
        public void SetOnClickListener(EventHandler onClick)
        {
            this.Click += onClick;
        }
    }
}

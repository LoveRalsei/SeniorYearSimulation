using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Graphics;

namespace AndroidApp1
{
    internal class GameIntro
    {
        private Button _button;

        private CustomDialog _dialog;

        public GameIntro(MainActivity mainActivity)
        {
            // 创建按钮
            var gameIntroButton = new Button(mainActivity)
            {
                Text = "游戏玩法",
                TextSize = 14
            };
            gameIntroButton.SetTextColor(Color.Gray);
            gameIntroButton.SetBackgroundColor(Color.Transparent);

            _dialog =new CustomDialog(mainActivity);
            _dialog.SetTitle("游戏玩法");
            _dialog.SetMessage("给你9个月，高考能考多少分？\n"
                +"点击各项行动，获得属性加成！"
                +"（1月=1回合）\n"
                +"******属性介绍******\n"
                +"金钱：可供花费。\n"
                +"健康：影响每回合恢复的精力量。\n"
                +"精力：用于各项行动。每回合结束恢复健康*0.5+心情*0.5的精力。\n"
                +"心情：影响每回合恢复的精力量。\n"
                +"魅力：决定活动行动的效果，乘数为魅力*1%。\n"
                +"懒惰：决定学习行动的成功率。成功率为1-懒惰*1%。\n"
                +"迷茫：决定非学习行动的成功率。成功率为1-迷茫*1%。\n"
                +"语文/数学/英语等：学科成绩。每回合自然衰减5。");
            _dialog.SetOnButtonClick(() =>
            {
                _dialog.Hide();
            });

            // 设置按钮点击事件
            gameIntroButton.Click += (s, e) =>
            {
                // 这里写点击后的逻辑
                //Toast.MakeText(mainActivity, "游戏玩法按钮被点击", ToastLength.Short).Show();
                _dialog.Show();
            };

            // 将按钮放入布局
            var layoutParams = new ActionBar.LayoutParams(
                ActionBar.LayoutParams.WrapContent,
                ActionBar.LayoutParams.WrapContent,
                GravityFlags.End | GravityFlags.CenterVertical);

            gameIntroButton.LayoutParameters = layoutParams;
            mainActivity.ActionBar.CustomView = gameIntroButton;
        }
    }
}

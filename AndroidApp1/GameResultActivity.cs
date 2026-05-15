using Android.Views;
using Android.Widget;

namespace AndroidApp1
{
    /// <summary>
    /// Game ending / result screen. Displays final scores and an ending
    /// description based on the student's final state.
    /// </summary>
    [Activity(Label = "游戏结算")]
    public class GameResultActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Receive student data from intent
            string? studentJson = Intent?.GetStringExtra("StudentData");
            Student? student = null;
            if (!string.IsNullOrEmpty(studentJson))
            {
                student = System.Text.Json.JsonSerializer.Deserialize<Student>(studentJson);
            }

            // Build layout programmatically
            var layout = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical,
                LayoutParameters = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.MatchParent)
            };
            layout.SetPadding(40, 40, 40, 40);
            layout.SetGravity(Android.Views.GravityFlags.Center);

            if (student == null)
            {
                var errorText = new TextView(this)
                {
                    Text = "数据加载失败",
                    TextSize = 20
                };
                layout.AddView(errorText);
                SetContentView(layout);
                return;
            }

            // Calculate scores
            int totalScore = student.chinese + student.math + student.english
                           + student.crouse1Grade + student.crouse2Grade + student.crouse3Grade;

            string endingTitle = GetEndingTitle(totalScore);
            string endingDesc = GetEndingDescription(totalScore, student);

            // Title
            var titleView = new TextView(this)
            {
                Text = "游戏结算",
                TextSize = 28
            };
            titleView.SetTypeface(null, Android.Graphics.TypefaceStyle.Bold);
            titleView.Gravity = Android.Views.GravityFlags.Center;
            layout.AddView(titleView);

            // Ending
            var endingView = new TextView(this)
            {
                Text = endingTitle,
                TextSize = 22,
                Gravity = Android.Views.GravityFlags.Center
            };
            endingView.SetTextColor(Android.Graphics.Color.ParseColor("#E65100"));
            var endingParams = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            endingParams.TopMargin = 32;
            layout.AddView(endingView, endingParams);

            // Description
            var descView = new TextView(this)
            {
                Text = endingDesc,
                TextSize = 16,
                Gravity = Android.Views.GravityFlags.Center
            };
            descView.SetTextColor(Android.Graphics.Color.DarkGray);
            var descParams = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            descParams.TopMargin = 24;
            layout.AddView(descView, descParams);

            // Score summary
            var scoreView = new TextView(this)
            {
                Text = $"总分：{totalScore}\n\n" +
                       $"语文：{student.chinese}  数学：{student.math}  英语：{student.english}\n\n" +
                       $"{student.crouse1Name}：{student.crouse1Grade}\n" +
                       $"{student.crouse2Name}：{student.crouse2Grade}\n" +
                       $"{student.crouse3Name}：{student.crouse3Grade}",
                TextSize = 16,
                Gravity = Android.Views.GravityFlags.Center
            };
            var scoreParams = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            scoreParams.TopMargin = 24;
            layout.AddView(scoreView, scoreParams);

            // Restart button
            var restartButton = new Button(this)
            {
                Text = "重新开始"
            };
            restartButton.SetTextColor(Android.Graphics.Color.White);
            restartButton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#2196F3"));
            var btnParams = new LinearLayout.LayoutParams(400, ViewGroup.LayoutParams.WrapContent);
            btnParams.TopMargin = 40;
            restartButton.Click += (s, e) =>
            {
                var intent = new Android.Content.Intent(this, typeof(MainActivity));
                intent.SetFlags(Android.Content.ActivityFlags.ClearTop | Android.Content.ActivityFlags.NewTask);
                StartActivity(intent);
                Finish();
            };
            layout.AddView(restartButton, btnParams);

            SetContentView(layout);
        }

        private static string GetEndingTitle(int totalScore)
        {
            if (totalScore >= 600) return "清北录取！";
            if (totalScore >= 500) return "一本稳了！";
            if (totalScore >= 400) return "二本保底";
            if (totalScore >= 300) return "勉强上线";
            return "高考落榜";
        }

        private static string GetEndingDescription(int totalScore, Student student)
        {
            if (totalScore >= 600)
                return "你以优异的成绩被顶尖大学录取！\n未来一片光明。";
            if (totalScore >= 500)
                return "你考上了一所不错的大学，\n为高三的努力画上了圆满的句号。";
            if (totalScore >= 400)
                return "成绩马马虎虎，\n大学生活还在等着你。";
            if (totalScore >= 300)
                return "勉强过线，\n也许复读是个选择？";
            return $"你的总分只有{totalScore}分。\n高考失利了，但人生还有很多路可以走。";
        }
    }
}

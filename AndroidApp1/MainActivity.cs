namespace AndroidApp1
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Student student = new Student("111",2,3,4,5,6,6,7,8,9,"1",1,"2",2,"3",3);
        TextView propertiesTextView;
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            propertiesTextView = FindViewById<TextView>(Resource.Id.propertiesTextView);

            RefreshPropertiesTextView();
        }

        protected void RefreshPropertiesTextView()
        {
            propertiesTextView?.Text = $"\t健康：{student.health}\t精力：{student.energy}\t心情：{student.happiness}\n" +
                $"\t魅力：{student.charm}\t懒惰：{student.laziness}\t迷茫：{student.confusion}\n" +
                $"\t语文：{student.chinese}\t数学：{student.math}\t英语：{student.english}\n" +
                $"\t{student.crouse1Name}：{student.crouse1Grade}\t{student.crouse2Name}：{student.crouse2Grade}\t{student.crouse3Name}：{student.crouse3Grade}";
        }
    }
}
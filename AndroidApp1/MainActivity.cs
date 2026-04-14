namespace AndroidApp1
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Student student = new Student("111",2,3,4,5,6,6,7,8,9,"1",1,"2",2,"3",3);
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            TextView propertiesTextView = FindViewById<TextView>(Resource.Id.propertiesTextView);

            propertiesTextView.Text = "\t½¡¿µ£º" + student.health + "\t¾«Á¦£º" + student.energy + "\tÐÒ¸££º" + student.happiness + "\n111";
        }
    }
}
using Android.Content;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidApp1
{
    public class StudyActionDialog : ActionDialog
    {
        public StudentProperty _chooseProperty;

        public StudyActionDialog(Context context) : base(context)
        {
            _chooseProperty = StudentProperty.Chinese;
        }

        public void BecomeStudyActionDialog(string title, string intro, string finishText, int costEnergy, List<KeyValuePair<StudentProperty, int>> effects)
        {
            if (_actionDialog != null)
                return;

            _actionDialog = new ActionDialog(_context);

            SetTitle(title);
            SetMessage(intro);
            SetButtonText("执行");
            _actionDialog.SetTitle(title);
            _actionDialog.SetOnButtonClick(() =>
            {
                _actionDialog.Hide();
            });
            _actionDialog.SetOnTypewriterComplete(() =>
            {
                if(GameManager.StudentData !=null)
                { 
                    //GameManager.StudentData.IncreaseProperty(_chooseProperty, studyEffect); 
                    foreach (var effect in effects)
                    {
                        GameManager.StudentData.IncreaseProperty(_chooseProperty, effect.Value);
                    }
                    GameManager.StudentData.ReduceEnergy(costEnergy);
                }
                else
                    Toast.MakeText(_context, "_studentData为空！", ToastLength.Short).Show();
                
                GameManager.MainActivity.RefreshPropertiesTextView();
            });
            SetOnButtonClick(() =>
            {
                if(GameManager.StudentData.EnoughEnergy(costEnergy) == false)
                {
                    SetMessage("精力不足！");
                    var handler = new Handler(Looper.MainLooper);
                    handler.PostDelayed(() =>
                    {
                        SetMessage(intro);
                    }, 2000);
                    return;
                }
                Hide();
                _actionDialog.Show();
                _actionDialog.EnableTypewriterEffect(true);
                _actionDialog.SetMessage(finishText);
                _actionDialog.SetButtonText("关闭");

            });
        }
    }
}

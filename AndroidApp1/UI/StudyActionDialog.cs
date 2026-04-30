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

                //输出结果
                _actionDialog.EnableTypewriterEffect(false);
                List<KeyValuePair<StudentProperty, int>> aimEffects = new List<KeyValuePair<StudentProperty, int>>();
                foreach(var effect in effects)
                {
                    if(effect.Key==StudentProperty.Chinese || effect.Key == StudentProperty.Math || effect.Key == StudentProperty.English || effect.Key == StudentProperty.Crouse1Grade || effect.Key == StudentProperty.Crouse2Grade || effect.Key == StudentProperty.Crouse3Grade)
                    {
                        KeyValuePair<StudentProperty, int> aimEffect = new KeyValuePair<StudentProperty, int>(_chooseProperty, effect.Value);
                        aimEffects.Add(aimEffect);
                    }
                    else
                        aimEffects.Add(effect);
                }
                _actionDialog.SetMessage(GetEffectsString(true, costEnergy, aimEffects));
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

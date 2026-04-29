using Android.Content;
using Android.Media.Effect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidApp1
{
    public class ActionDialog : CustomDialog
    {

        protected ActionDialog _actionDialog;

        public ActionDialog(Context context) : base(context)
        {
            
        }

        public void BecomeActionDialog(string title, string intro, string finishText, int costEnergy, List<KeyValuePair<StudentProperty, int>> effects)
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
                if (GameManager.StudentData != null)
                {
                    //GameManager.StudentData.IncreaseProperty(_chooseProperty, studyEffect); 
                    foreach (var effect in effects)
                    {
                        GameManager.StudentData.IncreaseProperty(effect.Key, effect.Value);
                    }
                    GameManager.StudentData.ReduceEnergy(costEnergy);
                }
                else
                    Toast.MakeText(_context, "_studentData为空！", ToastLength.Short).Show();

                GameManager.MainActivity.RefreshPropertiesTextView();

                //输出结果
                _actionDialog.EnableTypewriterEffect(false);
                _actionDialog.SetMessage(GetEffectsString(true, costEnergy, effects));
            });
            SetOnButtonClick(() =>
            {
                Hide();
                _actionDialog.Show();
                _actionDialog.EnableTypewriterEffect(true);
                _actionDialog.SetMessage(finishText);
                _actionDialog.SetButtonText("关闭");

            });
        }

        public string GetEffectsString(bool isSucceed,int costEnergy, List<KeyValuePair<StudentProperty, int>> effects)
        {
            string result = "";

            result += "精力" + (costEnergy >= 0 ? "-" : "+") + $"{costEnergy}\n";
            foreach(var effect in effects)
            {
                string effectName = "";
                switch (effect.Key)
                {
                    case StudentProperty.Money:
                        effectName = "金钱";
                        break;
                    case StudentProperty.Health:
                        effectName = "健康";
                        break;
                    case StudentProperty.Happiness:
                        effectName = "快乐";
                        break;
                    case StudentProperty.Charm:
                        effectName = "魅力";
                        break;
                    case StudentProperty.Laziness:
                        effectName = "懒惰";
                        break;
                    case StudentProperty.Confusion:
                        effectName = "迷茫";
                        break;

                    case StudentProperty.Chinese:
                        effectName = "语文";
                        break;
                    case StudentProperty.Math:
                        effectName = "数学";
                        break;
                    case StudentProperty.English:
                        effectName = "英语";
                        break;
                    case StudentProperty.Crouse1Grade:
                        effectName = GameManager.StudentData.crouse1Name;
                        break;
                    case StudentProperty.Crouse2Grade:
                        effectName = GameManager.StudentData.crouse2Name;
                        break;
                    case StudentProperty.Crouse3Grade:
                        effectName = GameManager.StudentData.crouse3Name;
                        break;
                }
                result += effectName + (effect.Value >= 0 ? "+" : "-") + $"{effect.Value}\n";
            }

            return result;
        }
    }
}

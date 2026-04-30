using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidApp1
{
    public class StudyActionButton : ActionButton
    {
        protected StudyActionDialog _studyActionDialog;
        protected bool _haveScrollButton = false;

        public StudyActionButton(Context context) : base(context)
        {
        }

        public void SetDialog(string title, string intro, string finishText, int costEnergy, List<KeyValuePair<StudentProperty, int>> effects)
        {

            _studyActionDialog = new StudyActionDialog(Context);
            _studyActionDialog.BecomeStudyActionDialog(title, intro, finishText, costEnergy, effects);
            _studyActionDialog.CancelOnTouchOutside = true; // 点击外部区域关闭弹窗
        }

        public void ShowDialog()
        {
            _studyActionDialog.Show();

            if (_haveScrollButton == false)
            {
                //if (_studentData == null)
                //    return;
                _studyActionDialog.AddScrollButton("语文", () =>
                {
                    _studyActionDialog._chooseProperty = StudentProperty.Chinese;
                });
                _studyActionDialog.AddScrollButton("数学", () =>
                {
                    _studyActionDialog._chooseProperty = StudentProperty.Math;
                });
                _studyActionDialog.AddScrollButton("英语", () =>
                {
                    _studyActionDialog._chooseProperty = StudentProperty.English;
                });
                _studyActionDialog.AddScrollButton(GameManager.StudentData?.crouse1Name, () =>
                {
                    _studyActionDialog._chooseProperty = StudentProperty.Crouse1Grade;
                });
                _studyActionDialog.AddScrollButton(GameManager.StudentData?.crouse2Name, () =>
                {
                    _studyActionDialog._chooseProperty = StudentProperty.Crouse2Grade;
                });
                _studyActionDialog.AddScrollButton(GameManager.StudentData?.crouse3Name, () =>
                {
                    _studyActionDialog._chooseProperty = StudentProperty.Crouse3Grade;
                });
                _studyActionDialog._chooseProperty = StudentProperty.Chinese;

                _haveScrollButton = true;
            }
        }
    }
}

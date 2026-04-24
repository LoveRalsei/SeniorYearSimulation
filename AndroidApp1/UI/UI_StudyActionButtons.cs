using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidApp1.UI
{
    internal class UI_StudyActionButtons
    {
        private MainActivity _mainActivity;
        private Student _student;

        public List<ActionButtonConfig> ActionButtons;

        public UI_StudyActionButtons(MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
            _student=_mainActivity._gameManager.Student;
            ActionButtons = new List<ActionButtonConfig>
            {
                new ActionButtonConfig
                {
                    ResourceId = 1,
                    Title = "学习",
                    Description = "学习",
                    Cost = "精力20",
                    ClickAction = () => { 
                        _mainActivity._gameManager.Student.ReduceEnergy(10); 
                        _mainActivity.RefreshPropertiesTextView();
                    },
                    DialogTitle = "学习",
                    DialogIntro = "学习",
                    DialogFinish = "学习中······"
                }
            };
        }
    }
}

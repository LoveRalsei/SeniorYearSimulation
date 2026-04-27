using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidApp1.UI
{
    internal class Data_StudyActionButtons
    {
        private MainActivity _mainActivity;
        private Student _student;

        public List<ActionButtonConfig> ActionButtons;

        public Data_StudyActionButtons(MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
            _student=GameManager.StudentData;
            ActionButtons = new List<ActionButtonConfig>
            {
                new ActionButtonConfig
                {
                    ResourceId = 1,
                    Title = "学习",
                    Description = "学习",
                    CostText = "精力20",
                    CostEnergy = 20,
                    Effects = new List<KeyValuePair<StudentProperty, int>>
                    {
                        new KeyValuePair<StudentProperty, int>(StudentProperty.Chinese, 20),
                    },
                    DialogTitle = "学习",
                    DialogIntro = "学习",
                    DialogFinish = "学习中······"
                    
                },
                new ActionButtonConfig
                {
                    ResourceId = 2,
                    Title = "休息",
                    Description = "休息",
                    CostText = "休息",
                    CostEnergy = 0,
                    Effects = new List<KeyValuePair<StudentProperty, int>>
                    {
                        new KeyValuePair<StudentProperty, int>(StudentProperty.Chinese, 10)
                    },
                    DialogTitle = "休息",
                    DialogIntro = "休息",
                    DialogFinish = "休息中······"
                },
            };
        }
    }
}

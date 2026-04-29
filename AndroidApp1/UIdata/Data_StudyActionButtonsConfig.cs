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
                    Title = "写习题册",
                    Description = "写习题册",
                    CostText = "精力10",
                    CostEnergy = 10,
                    Effects = new List<KeyValuePair<StudentProperty, int>>
                    {
                        new KeyValuePair<StudentProperty, int>(StudentProperty.Chinese, 7),
                    },
                    DialogTitle = "写习题册",
                    DialogIntro = "写习题册",
                    DialogFinish = "学习中······"
                    
                },
                new ActionButtonConfig
                {
                    ResourceId = 2,
                    Title = "做卷子",
                    Description = "做卷子",
                    CostText = "做卷子",
                    CostEnergy = 10,
                    Effects = new List<KeyValuePair<StudentProperty, int>>
                    {
                        new KeyValuePair<StudentProperty, int>(StudentProperty.Chinese, 7)
                    },
                    DialogTitle = "做卷子",
                    DialogIntro = "做卷子",
                    DialogFinish = "做卷子中······"
                },
            };
        }
    }
}

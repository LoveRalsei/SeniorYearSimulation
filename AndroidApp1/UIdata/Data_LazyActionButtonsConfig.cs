using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidApp1.UIData
{
    internal class Data_LazyActionButtonsConfig:Data_ActionButtonsConfig
    {

        public Data_LazyActionButtonsConfig(MainActivity mainActivity):base(mainActivity)
        {

            ActionButtons = new List<ActionButtonConfig>
            {
                new ActionButtonConfig()
                {
                    ResourceId = 10001,
                    Title = "不写作业",
                    Description = "少写一科作业，但要担心被老师批评",
                    CostText = "精力0",
                    CostEnergy = 0,
                    Effects = new List<KeyValuePair<StudentProperty, int>>
                    {
                        new KeyValuePair<StudentProperty, int>(StudentProperty.Energy, 5),
                        new KeyValuePair<StudentProperty, int>(StudentProperty.Happiness,-1)
                    },
                    DialogTitle = "不写作业",
                    DialogIntro = "",
                    DialogFinish = "偷懒中"
                }
            };
        }
    }
}

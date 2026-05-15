namespace AndroidApp1.UIData
{
    internal class Data_LazyActionButtonsConfig : Data_ActionButtonsConfig
    {
        public Data_LazyActionButtonsConfig(StudentModifier modifier) : base(modifier)
        {
            ActionButtons = new List<ActionButtonConfig>
            {
                new ActionButtonConfig
                {
                    ResourceId = 10001,
                    Title = "不写作业",
                    Description = "少写一科作业，但要担心被老师批评",
                    CostText = "精力0",
                    CostEnergy = 0,
                    Effects = new List<PropertyEffect>
                    {
                        new PropertyEffect(StudentProperty.Energy, 5),
                        new PropertyEffect(StudentProperty.Happiness, -1)
                    },
                    DialogTitle = "不写作业",
                    DialogIntro = "",
                    DialogFinish = "偷懒中"
                }
            };
        }
    }
}

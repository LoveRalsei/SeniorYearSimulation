namespace AndroidApp1.UIData
{
    internal class Data_DoActivityActionButtonsConfig : Data_ActionButtonsConfig
    {
        public Data_DoActivityActionButtonsConfig(StudentModifier modifier) : base(modifier)
        {
            ActionButtons = new List<ActionButtonConfig>
            {
                new ActionButtonConfig
                {
                    ResourceId = 101,
                    Title = "运动",
                    Description = "运动",
                    CostText = "精力10",
                    CostEnergy = 10,
                    Effects = new List<PropertyEffect>
                    {
                        new PropertyEffect(StudentProperty.Health, 2),
                    },
                    DialogTitle = "运动",
                    DialogIntro = "运动",
                    DialogFinish = "运动中···"
                }
            };
        }
    }
}

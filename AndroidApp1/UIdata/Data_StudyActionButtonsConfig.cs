namespace AndroidApp1.UIData
{
    internal class Data_StudyActionButtons : Data_ActionButtonsConfig
    {
        public Data_StudyActionButtons(StudentModifier modifier) : base(modifier)
        {
            ActionButtons = new List<ActionButtonConfig>
            {
                new ActionButtonConfig
                {
                    ResourceId = 1,
                    Title = "写习题册",
                    Description = "写习题册",
                    CostText = "精力10",
                    CostEnergy = 10,
                    Effects = new List<PropertyEffect>
                    {
                        new PropertyEffect(StudentProperty.Chinese, 7),
                    },
                    DialogTitle = "写习题册",
                    DialogIntro = "写习题册",
                    DialogFinish = "学习中······",
                    RequiresSubjectChoice = true
                },
                new ActionButtonConfig
                {
                    ResourceId = 2,
                    Title = "做卷子",
                    Description = "做卷子",
                    CostText = "精力10",
                    CostEnergy = 10,
                    Effects = new List<PropertyEffect>
                    {
                        new PropertyEffect(StudentProperty.Chinese, 7)
                    },
                    DialogTitle = "做卷子",
                    DialogIntro = "做卷子",
                    DialogFinish = "做卷子中······",
                    RequiresSubjectChoice = true
                },
                new ActionButtonConfig
                {
                    ResourceId = 3,
                    Title = "读课本",
                    Description = "读课本",
                    CostText = "精力5",
                    CostEnergy = 5,
                    Effects = new List<PropertyEffect>
                    {
                        new PropertyEffect(StudentProperty.Chinese, 3)
                    },
                    DialogTitle = "读课本",
                    DialogIntro = "读课本",
                    DialogFinish = "读课本中······",
                    RequiresSubjectChoice = true
                }
            };
        }
    }
}

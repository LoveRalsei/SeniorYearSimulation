namespace AndroidApp1.UIData
{
    public class ActionButtonConfig
    {
        public int ResourceId { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string CostText { get; set; } = "";
        public int CostEnergy { get; set; }
        public List<PropertyEffect> Effects { get; set; } = new();
        public string DialogTitle { get; set; } = "";
        public string DialogIntro { get; set; } = "";
        public string DialogFinish { get; set; } = "";

        /// <summary>
        /// If true, the action dialog will show scroll buttons for the player
        /// to choose which subject to apply the effects to.
        /// (Replaces the need for a separate StudyActionButton class.)
        /// </summary>
        public bool RequiresSubjectChoice { get; set; }
    }
}

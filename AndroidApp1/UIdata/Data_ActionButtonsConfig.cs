namespace AndroidApp1.UIData
{
    internal class Data_ActionButtonsConfig
    {
        protected StudentModifier _modifier;

        public List<ActionButtonConfig> ActionButtons = new();

        public Data_ActionButtonsConfig(StudentModifier modifier)
        {
            _modifier = modifier;
        }
    }
}

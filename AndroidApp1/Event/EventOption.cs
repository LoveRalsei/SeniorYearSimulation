namespace AndroidApp1.Event
{
    /// <summary>
    /// A single choice option within a random event.
    /// </summary>
    public class EventOption
    {
        /// <summary>Button text shown in the scroll list.</summary>
        public string Text { get; set; } = "";

        /// <summary>Text shown after the option is selected (result description).</summary>
        public string ResultText { get; set; } = "";

        /// <summary>Effects applied when this option is chosen.</summary>
        public List<PropertyEffect> Effects { get; set; } = new();
    }
}

namespace AndroidApp1.Event
{
    /// <summary>
    /// A random event that may trigger at the end of a turn.
    /// Associated with a specific character.
    /// </summary>
    public class RandomEvent
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string CharacterName { get; set; } = "";

        /// <summary>Probability [0, 1] of triggering when conditions are met.</summary>
        public float TriggerProbability { get; set; } = 1f;

        /// <summary>Minimum turn before this event can trigger.</summary>
        public int MinTurn { get; set; } = 1;

        /// <summary>Maximum turn for this event. -1 means no max.</summary>
        public int MaxTurn { get; set; } = -1;

        /// <summary>
        /// Optional condition. If null, always matches (subject to turn range + probability).
        /// Receives the student so it can inspect current attribute values.
        /// </summary>
        public Func<Student, bool>? TriggerCondition { get; set; }

        /// <summary>Choices presented to the player.</summary>
        public List<EventOption> Options { get; set; } = new();

        /// <summary>Text for the bottom-right button (empty callback). Default "取消".</summary>
        public string BottomButtonText { get; set; } = "取消";
    }
}

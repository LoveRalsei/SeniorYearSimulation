using System.Text.Json.Serialization;

namespace AndroidApp1.Event
{
    /// <summary>
    /// JSON DTO for a single event option within a random event.
    /// </summary>
    public class EventOptionData
    {
        /// <summary>Button title text shown in the scroll list.</summary>
        [JsonPropertyName("text")]
        public string Text { get; set; } = "";

        /// <summary>Text shown after the option is selected (result description).</summary>
        [JsonPropertyName("resultText")]
        public string ResultText { get; set; } = "";

        /// <summary>Effects applied when this option is chosen.</summary>
        [JsonPropertyName("effects")]
        public List<PropertyEffect> Effects { get; set; } = new();
    }

    /// <summary>
    /// JSON DTO for a random event entry in a character's turn-indexed event list.
    /// If the JSON value is null, this represents "no event for this turn".
    /// If Title is null/empty, it is also treated as no event.
    /// </summary>
    public class RandomEventData
    {
        /// <summary>Optional stable identifier for the event.</summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>Title displayed at the top of the event dialog.</summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>Explanation text shown below the title.</summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>Probability [0, 1] of triggering. Default 1.0 (always).</summary>
        [JsonPropertyName("probability")]
        public float TriggerProbability { get; set; } = 1f;

        /// <summary>Text for the bottom-right button. Default "取消".</summary>
        [JsonPropertyName("bottomButtonText")]
        public string BottomButtonText { get; set; } = "取消";

        /// <summary>Options presented as scroll buttons.</summary>
        [JsonPropertyName("options")]
        public List<EventOptionData> Options { get; set; } = new();

        /// <summary>Whether this entry represents a valid event (non-null, has title).</summary>
        public bool IsValid => !string.IsNullOrEmpty(Title);
    }

    /// <summary>
    /// JSON DTO for a character entry in studentdata.json, used to extract
    /// the embedded events array alongside student data.
    /// </summary>
    public class StudentDataWithEvents
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("events")]
        public List<RandomEventData?>? Events { get; set; }
    }
}

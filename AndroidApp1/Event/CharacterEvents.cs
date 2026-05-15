namespace AndroidApp1.Event
{
    /// <summary>
    /// All random events for a specific character.
    /// Supports both flat event lists (MinTurn/MaxTurn based) and
    /// turn-indexed lists (index = turn - 1, null = no event).
    /// </summary>
    public class CharacterEvents
    {
        public string CharacterName { get; }

        /// <summary>Flat event list (MinTurn/MaxTurn based). Legacy/complex conditions.</summary>
        public List<RandomEvent> Events { get; } = new();

        /// <summary>
        /// Turn-indexed event list. Index 0 = turn 1, index 1 = turn 2, etc.
        /// Null entries mean no event for that turn.
        /// If non-null and non-empty, this takes priority over the flat list.
        /// </summary>
        public List<RandomEvent?>? TurnEvents { get; set; }

        public CharacterEvents(string characterName)
        {
            CharacterName = characterName;
        }

        public void AddEvent(RandomEvent evt)
        {
            evt.CharacterName = CharacterName;
            Events.Add(evt);
        }
    }
}

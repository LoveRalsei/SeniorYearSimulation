using System.Text;

namespace AndroidApp1.Event
{
    /// <summary>
    /// Registry of all character events. Queried by EventDispatcher to find
    /// matching events for the current turn/character.
    /// Supports turn-indexed event lists (priority) and flat eligibility lists.
    /// </summary>
    public class EventRegistry
    {
        private readonly Dictionary<string, CharacterEvents> _characters = new();

        public void Register(CharacterEvents characterEvents)
        {
            _characters[characterEvents.CharacterName] = characterEvents;
        }

        /// <summary>
        /// Get the event for a specific character and turn.
        /// Turn-indexed list takes priority if available.
        /// Returns null if no event is configured for this turn.
        /// </summary>
        public RandomEvent? GetEventForTurn(string characterName, int turn)
        {
            if (!_characters.TryGetValue(characterName, out var charEvents))
                return null;

            // Priority 1: Turn-indexed event list
            if (charEvents.TurnEvents != null && charEvents.TurnEvents.Count > 0)
            {
                int index = turn - 1;
                if (index >= 0 && index < charEvents.TurnEvents.Count)
                    return charEvents.TurnEvents[index];
                // Beyond the list range → no event
                return null;
            }

            // Priority 2: Legacy flat eligibility list
            return null; // handled separately by GetEligibleEvents
        }

        /// <summary>
        /// Get all events for a character that are eligible for the given turn
        /// (legacy flat-list mode, used when TurnEvents is not set).
        /// </summary>
        public List<RandomEvent> GetEligibleEvents(string characterName, int turn, Student student)
        {
            if (!_characters.TryGetValue(characterName, out var charEvents))
                return new List<RandomEvent>();

            // If TurnEvents is set, flat list is not used
            if (charEvents.TurnEvents != null && charEvents.TurnEvents.Count > 0)
                return new List<RandomEvent>();

            return charEvents.Events
                .Where(e => turn >= e.MinTurn
                         && (e.MaxTurn < 0 || turn <= e.MaxTurn)
                         && (e.TriggerCondition == null || e.TriggerCondition(student)))
                .ToList();
        }
    }
}

using System.Text.Json;
using AndroidApp1.Event;

namespace AndroidApp1.UIData
{
    /// <summary>
    /// Central data source for all character-specific random events.
    /// Reads from studentdata.json — each character entry can have an
    /// "events" array. Adding a new character with events to studentdata.json
    /// automatically makes them available.
    /// </summary>
    public static class Data_Events
    {
        private const string DataFile = "studentdata.json";

        public static EventRegistry BuildRegistry()
        {
            var registry = new EventRegistry();

            var keys = JsonFileReader.GetAllKeys(DataFile);
            foreach (var key in keys)
            {
                // Load the full character JSON entry
                var json = JsonFileReader.GetValueByKey(DataFile, key);
                if (string.IsNullOrEmpty(json)) continue;

                try
                {
                    var data = JsonSerializer.Deserialize<StudentDataWithEvents>(json);
                    if (data == null || string.IsNullOrEmpty(data.Name)) continue;

                    // Skip if no events array
                    if (data.Events == null || data.Events.Count == 0) continue;

                    var charEvents = new CharacterEvents(data.Name);

                    // Convert JSON event data to RandomEvent list (turn-indexed)
                    var turnEvents = new List<RandomEvent?>();
                    foreach (var eventData in data.Events)
                    {
                        if (eventData == null || !eventData.IsValid)
                        {
                            turnEvents.Add(null);
                            continue;
                        }

                        var randomEvent = ConvertToRandomEvent(eventData, data.Name);
                        turnEvents.Add(randomEvent);
                    }

                    charEvents.TurnEvents = turnEvents;
                    registry.Register(charEvents);
                }
                catch (JsonException)
                {
                    // Skip malformed entries
                }
            }

            return registry;
        }

        /// <summary>
        /// Get the list of character display names from studentdata.json.
        /// Used for auto-building the character selection UI.
        /// </summary>
        public static List<CharacterInfo> GetAvailableCharacters()
        {
            var characters = new List<CharacterInfo>();
            var keys = JsonFileReader.GetAllKeys(DataFile);

            foreach (var key in keys)
            {
                var json = JsonFileReader.GetValueByKey(DataFile, key);
                if (string.IsNullOrEmpty(json)) continue;

                try
                {
                    var data = JsonSerializer.Deserialize<StudentDataWithEvents>(json);
                    if (data != null && !string.IsNullOrEmpty(data.Name))
                    {
                        characters.Add(new CharacterInfo(key, data.Name));
                    }
                }
                catch (JsonException) { }
            }

            return characters;
        }

        private static RandomEvent ConvertToRandomEvent(RandomEventData data, string characterName)
        {
            var evt = new RandomEvent
            {
                Id = data.Id ?? "",
                Title = data.Title ?? "",
                Description = data.Description ?? "",
                CharacterName = characterName,
                TriggerProbability = data.TriggerProbability,
                BottomButtonText = data.BottomButtonText,
                Options = data.Options.Select(o => new EventOption
                {
                    Text = o.Text,
                    ResultText = o.ResultText,
                    Effects = o.Effects
                }).ToList()
            };
            return evt;
        }
    }

    /// <summary>
    /// Lightweight info for character selection UI.
    /// </summary>
    public class CharacterInfo
    {
        public string Key { get; }
        public string Name { get; }

        public CharacterInfo(string key, string name)
        {
            Key = key;
            Name = name;
        }
    }
}

namespace AndroidApp1
{
    /// <summary>
    /// Central registry of all Student property metadata.
    /// Adding a new property only requires one entry here — display names,
    /// min/max clamping, and turn decay all flow from this single definition.
    /// </summary>
    public class PropertyMetadata
    {
        public StudentProperty Key { get; }
        public string DisplayName { get; }
        public string PropertyName { get; }
        public int MinValue { get; }
        public int MaxValue { get; }
        public int DecayPerTurn { get; }
        public PropertyCategory Category { get; }

        private PropertyMetadata(StudentProperty key, string displayName, string propertyName,
            int min, int max, int decayPerTurn, PropertyCategory category)
        {
            Key = key;
            DisplayName = displayName;
            PropertyName = propertyName;
            MinValue = min;
            MaxValue = max;
            DecayPerTurn = decayPerTurn;
            Category = category;
        }

        // ── Static registry ──────────────────────────────────────────

        private static readonly Dictionary<StudentProperty, PropertyMetadata> _registry;

        static PropertyMetadata()
        {
            _registry = new Dictionary<StudentProperty, PropertyMetadata>
            {
                [StudentProperty.Money]       = new(StudentProperty.Money,       "金钱",   "money",       0, 9999, 0, PropertyCategory.General),
                [StudentProperty.Health]      = new(StudentProperty.Health,      "健康",   "health",      0,  100, 0, PropertyCategory.General),
                [StudentProperty.Energy]      = new(StudentProperty.Energy,      "精力",   "energy",      0,  200, 0, PropertyCategory.General),
                [StudentProperty.Happiness]   = new(StudentProperty.Happiness,   "心情",   "happiness",   0,  100, 0, PropertyCategory.General),
                [StudentProperty.Charm]       = new(StudentProperty.Charm,       "魅力",   "charm",       0,  200, 0, PropertyCategory.General),
                [StudentProperty.Laziness]    = new(StudentProperty.Laziness,    "懒惰",   "laziness",    0,  100, 0, PropertyCategory.General),
                [StudentProperty.Confusion]   = new(StudentProperty.Confusion,   "迷茫",   "confusion",   0,  100, 0, PropertyCategory.General),
                [StudentProperty.Chinese]     = new(StudentProperty.Chinese,     "语文",   "chinese",     0,  150, 5, PropertyCategory.Subject),
                [StudentProperty.Math]        = new(StudentProperty.Math,        "数学",   "math",        0,  150, 5, PropertyCategory.Subject),
                [StudentProperty.English]     = new(StudentProperty.English,     "英语",   "english",     0,  150, 5, PropertyCategory.Subject),
                [StudentProperty.Crouse1Grade]= new(StudentProperty.Crouse1Grade,"课程1",  "crouse1Grade",0,  100, 5, PropertyCategory.Subject),
                [StudentProperty.Crouse2Grade]= new(StudentProperty.Crouse2Grade,"课程2",  "crouse2Grade",0,  100, 5, PropertyCategory.Subject),
                [StudentProperty.Crouse3Grade]= new(StudentProperty.Crouse3Grade,"课程3",  "crouse3Grade",0,  100, 5, PropertyCategory.Subject),
            };
        }

        /// <summary>All registered property keys.</summary>
        public static IEnumerable<StudentProperty> AllKeys => _registry.Keys;

        /// <summary>Look up metadata for a property.</summary>
        public static PropertyMetadata Get(StudentProperty key) => _registry[key];

        /// <summary>
        /// Get the display name for a property. For course-grade properties,
        /// the actual course name is resolved from the given Student (if provided).
        /// </summary>
        public static string GetDisplayName(StudentProperty key, Student? student = null)
        {
            var meta = Get(key);
            if (student != null)
            {
                return key switch
                {
                    StudentProperty.Crouse1Grade => student.crouse1Name,
                    StudentProperty.Crouse2Grade => student.crouse2Name,
                    StudentProperty.Crouse3Grade => student.crouse3Name,
                    _ => meta.DisplayName
                };
            }
            return meta.DisplayName;
        }
    }
}

using System.Reflection;
using System.Text.Json.Serialization;

namespace AndroidApp1
{
    public class Student
    {
        
        public string name { get; private set; }

        
        public int money      { get; private set; }
        public int health     { get; private set; }
        public int energy     { get; private set; }
        public int happiness  { get; private set; }
        public int charm      { get; private set; }
        public int laziness   { get; private set; }
        public int confusion  { get; private set; }
        public int chinese    { get; private set; }
        public int math       { get; private set; }
        public int english    { get; private set; }
        public string crouse1Name { get; private set; }
        public string crouse2Name { get; private set; }
        public string crouse3Name { get; private set; }
        public int crouse1Grade { get; private set; }
        public int crouse2Grade { get; private set; }
        public int crouse3Grade { get; private set; }

        // ── Static reflection cache for property access ──────────────

        private static readonly Dictionary<StudentProperty, PropertyInfo> _propInfoCache;

        static Student()
        {
            _propInfoCache = new Dictionary<StudentProperty, PropertyInfo>();
            foreach (var key in PropertyMetadata.AllKeys)
            {
                var prop = typeof(Student).GetProperty(PropertyMetadata.Get(key).PropertyName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop != null)
                    _propInfoCache[key] = prop;
            }
        }

        // ── Constructors ─────────────────────────────────────────────

        public Student()
        {
            this.name = "无参构造对象";
            this.crouse1Name = "";
            this.crouse2Name = "";
            this.crouse3Name = "";
        }

        [JsonConstructor]
        public Student(string name, int money, int health, int energy, int happiness,
            int charm, int laziness, int confusion,
            int chinese, int math, int english,
            string crouse1Name, int crouse1Grade,
            string crouse2Name, int crouse2Grade,
            string crouse3Name, int crouse3Grade)
        {
            this.name = name;
            this.money = money;
            this.health = health;
            this.energy = energy;
            this.happiness = happiness;
            this.charm = charm;
            this.laziness = laziness;
            this.confusion = confusion;
            this.chinese = chinese;
            this.math = math;
            this.english = english;
            this.crouse1Name = crouse1Name;
            this.crouse1Grade = crouse1Grade;
            this.crouse2Name = crouse2Name;
            this.crouse2Grade = crouse2Grade;
            this.crouse3Name = crouse3Name;
            this.crouse3Grade = crouse3Grade;
        }

        /// <summary>
        /// OCP-compliant copy constructor. Iterates PropertyMetadata registry
        /// so adding a new property requires NO change here.
        /// </summary>
        public Student(Student source)
        {
            if (source == null) return;

            this.name = source.name;
            this.crouse1Name = source.crouse1Name;
            this.crouse2Name = source.crouse2Name;
            this.crouse3Name = source.crouse3Name;

            foreach (var key in PropertyMetadata.AllKeys)
            {
                SetPropertyValue(key, source.GetPropertyValue(key));
            }
        }

        // ── Internal property access (used by StudentModifier) ───────

        /// <summary>Get the current value of a tracked numeric property.</summary>
        internal int GetPropertyValue(StudentProperty property)
        {
            return property switch
            {
                StudentProperty.Money        => money,
                StudentProperty.Health       => health,
                StudentProperty.Energy       => energy,
                StudentProperty.Happiness    => happiness,
                StudentProperty.Charm        => charm,
                StudentProperty.Laziness     => laziness,
                StudentProperty.Confusion    => confusion,
                StudentProperty.Chinese      => chinese,
                StudentProperty.Math         => math,
                StudentProperty.English      => english,
                StudentProperty.Crouse1Grade => crouse1Grade,
                StudentProperty.Crouse2Grade => crouse2Grade,
                StudentProperty.Crouse3Grade => crouse3Grade,
                _ => throw new ArgumentOutOfRangeException(nameof(property), property, null)
            };
        }

        /// <summary>
        /// Set a numeric property to an exact value, clamped to [Min, Max].
        /// Returns the value actually stored (may differ due to clamping).
        /// Only visible within this assembly (called by StudentModifier).
        /// </summary>
        internal int SetPropertyValue(StudentProperty property, int value)
        {
            var meta = PropertyMetadata.Get(property);
            int clamped = Math.Clamp(value, meta.MinValue, meta.MaxValue);

            switch (property)
            {
                case StudentProperty.Money:        money      = clamped; break;
                case StudentProperty.Health:       health     = clamped; break;
                case StudentProperty.Energy:       energy     = clamped; break;
                case StudentProperty.Happiness:    happiness  = clamped; break;
                case StudentProperty.Charm:        charm      = clamped; break;
                case StudentProperty.Laziness:     laziness   = clamped; break;
                case StudentProperty.Confusion:    confusion  = clamped; break;
                case StudentProperty.Chinese:      chinese    = clamped; break;
                case StudentProperty.Math:         math       = clamped; break;
                case StudentProperty.English:      english    = clamped; break;
                case StudentProperty.Crouse1Grade: crouse1Grade = clamped; break;
                case StudentProperty.Crouse2Grade: crouse2Grade = clamped; break;
                case StudentProperty.Crouse3Grade: crouse3Grade = clamped; break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(property), property, null);
            }

            return clamped;
        }

        /// <summary>Increment a property by delta, clamped; returns actual delta applied.</summary>
        internal int AdjustPropertyValue(StudentProperty property, int delta)
        {
            int oldValue = GetPropertyValue(property);
            int newValue = SetPropertyValue(property, oldValue + delta);
            return newValue - oldValue;
        }

        // ── Energy convenience ───────────────────────────────────────

        public bool EnoughEnergy(int cost) => energy >= cost;

        /// <summary>Reduce energy by an amount; returns actual amount consumed.</summary>
        internal int ReduceEnergy(int amount)
        {
            int oldEnergy = energy;
            int newEnergy = Math.Max(0, energy - amount);
            energy = newEnergy;
            return oldEnergy - newEnergy;
        }
    }
}

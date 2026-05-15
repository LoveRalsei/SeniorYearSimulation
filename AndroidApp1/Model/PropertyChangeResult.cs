using System.Text;

namespace AndroidApp1
{
    /// <summary>
    /// A single property change record (before / after / delta).
    /// </summary>
    public class PropertyChange
    {
        public StudentProperty Property { get; }
        public string DisplayName { get; }
        public int OldValue { get; }
        public int NewValue { get; }
        public int Delta => NewValue - OldValue;

        public PropertyChange(StudentProperty property, string displayName, int oldValue, int newValue)
        {
            Property = property;
            DisplayName = displayName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    /// <summary>
    /// Result of applying one or more property effects to a Student.
    /// Tracks every actual change (with clamping considered) and can produce
    /// a human-readable summary string for dialogs.
    /// </summary>
    public class PropertyChangeResult
    {
        public List<PropertyChange> Changes { get; } = new();
        public int EnergyConsumed { get; set; }

        public bool HasChanges => Changes.Count > 0 || EnergyConsumed != 0;

        /// <summary>
        /// Build a human-readable summary string suitable for dialog display.
        /// </summary>
        public string GetSummaryString()
        {
            var sb = new StringBuilder();

            if (EnergyConsumed != 0)
                sb.AppendLine($"精力-{EnergyConsumed}");

            foreach (var change in Changes)
            {
                if (change.Delta == 0) continue;
                string sign = change.Delta >= 0 ? "+" : "";
                sb.AppendLine($"{change.DisplayName}{sign}{change.Delta}");
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// Build a detailed before→after string (used in EndTurn display).
        /// </summary>
        public string GetDetailedSummaryString()
        {
            var sb = new StringBuilder("属性变化：");
            sb.AppendLine();

            foreach (var change in Changes)
            {
                if (change.Delta == 0) continue;
                sb.AppendLine($"{change.DisplayName}：{change.OldValue}→{change.NewValue}");
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// Merge multiple PropertyChangeResult instances into one.
        /// Changes targeting the same property are coalesced.
        /// </summary>
        public static PropertyChangeResult Merge(params PropertyChangeResult[] results)
        {
            var merged = new PropertyChangeResult();

            // Coalesce by property: last change wins for OldValue/NewValue,
            // but we preserve the original old value and latest new value
            var byProperty = new Dictionary<StudentProperty, PropertyChange>();

            foreach (var result in results)
            {
                merged.EnergyConsumed += result.EnergyConsumed;
                foreach (var change in result.Changes)
                {
                    if (byProperty.TryGetValue(change.Property, out var existing))
                    {
                        // Keep original OldValue, use latest NewValue
                        byProperty[change.Property] = new PropertyChange(
                            change.Property,
                            change.DisplayName,
                            existing.OldValue,
                            change.NewValue);
                    }
                    else
                    {
                        byProperty[change.Property] = change;
                    }
                }
            }

            merged.Changes.AddRange(byProperty.Values);
            return merged;
        }
    }
}

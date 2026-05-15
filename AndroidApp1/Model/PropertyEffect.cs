namespace AndroidApp1
{
    /// <summary>
    /// A single property-modification intent (property + delta).
    /// Used everywhere: action effects, event effects, turn decay.
    /// </summary>
    public class PropertyEffect
    {
        public StudentProperty Property { get; }
        public int Value { get; }

        public PropertyEffect(StudentProperty property, int value)
        {
            Property = property;
            Value = value;
        }

        public override string ToString() => $"{Property}: {(Value >= 0 ? "+" : "")}{Value}";
    }
}

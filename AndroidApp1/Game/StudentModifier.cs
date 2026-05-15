namespace AndroidApp1
{
    /// <summary>
    /// The sole write-facade for Student numeric properties.
    /// Every modification flows through here, producing a PropertyChangeResult
    /// that can be displayed in dialogs. All clamping is handled transparently.
    /// </summary>
    public class StudentModifier
    {
        private Student _student;

        public StudentModifier(Student student)
        {
            _student = student ?? throw new ArgumentNullException(nameof(student));
        }

        /// <summary>Replace the underlying student (e.g. for character selection).</summary>
        internal void ReplaceStudent(Student newStudent)
        {
            _student = newStudent ?? throw new ArgumentNullException(nameof(newStudent));
        }

        /// <summary>
        /// Apply a list of effects to the student and return a structured
        /// change record with old→new values and actual deltas.
        /// </summary>
        public PropertyChangeResult ApplyEffects(List<PropertyEffect> effects)
        {
            var result = new PropertyChangeResult();

            foreach (var effect in effects)
            {
                int oldValue = _student.GetPropertyValue(effect.Property);
                int actualDelta = _student.AdjustPropertyValue(effect.Property, effect.Value);
                int newValue = _student.GetPropertyValue(effect.Property);

                if (actualDelta != 0)
                {
                    result.Changes.Add(new PropertyChange(
                        effect.Property,
                        PropertyMetadata.GetDisplayName(effect.Property, _student),
                        oldValue,
                        newValue));
                }
            }

            return result;
        }

        /// <summary>
        /// Apply a single effect and return the change result.
        /// </summary>
        public PropertyChangeResult ApplyEffect(StudentProperty property, int value)
        {
            return ApplyEffects(new List<PropertyEffect> { new PropertyEffect(property, value) });
        }

        /// <summary>
        /// Consume energy for an action. Returns the actual amount consumed
        /// (may be less than requested if insufficient).
        /// </summary>
        public int ConsumeEnergy(int cost)
        {
            return _student.ReduceEnergy(cost);
        }

        /// <summary>
        /// Apply turn-end decay to all subject properties.
        /// </summary>
        public PropertyChangeResult ApplyTurnDecay()
        {
            var effects = new List<PropertyEffect>();
            foreach (var key in PropertyMetadata.AllKeys)
            {
                var meta = PropertyMetadata.Get(key);
                if (meta.DecayPerTurn > 0)
                {
                    effects.Add(new PropertyEffect(key, -meta.DecayPerTurn));
                }
            }
            return ApplyEffects(effects);
        }

        /// <summary>
        /// Restore energy by an amount, clamped to max. Returns change result.
        /// </summary>
        public PropertyChangeResult RestoreEnergy(int amount)
        {
            int oldValue = _student.energy;
            int actualDelta = _student.AdjustPropertyValue(StudentProperty.Energy, amount);
            int newValue = _student.energy;

            var result = new PropertyChangeResult();
            if (actualDelta != 0)
            {
                result.Changes.Add(new PropertyChange(
                    StudentProperty.Energy,
                    PropertyMetadata.GetDisplayName(StudentProperty.Energy),
                    oldValue,
                    newValue));
            }
            return result;
        }

        /// <summary>Access the underlying student (read-only).</summary>
        public Student Student => _student;
    }
}

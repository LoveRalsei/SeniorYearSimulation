namespace AndroidApp1
{
    /// <summary>
    /// Manages turn counting, energy-recovery formula, and max-turn guard.
    /// Extracted from GameManager (SRP).
    /// </summary>
    public class TurnManager
    {
        private int _currentTurn;
        private const int MAX_TURNS = 10;

        public int CurrentTurn => _currentTurn;
        public int MaxTurns => MAX_TURNS;
        public bool IsLastTurn => _currentTurn >= MAX_TURNS;

        /// <summary>Advance to the next turn.</summary>
        public void Advance()
        {
            _currentTurn++;
        }

        /// <summary>
        /// Calculate how much energy to restore at turn start.
        /// Formula: health * 0.5 + happiness * 0.5 (as described in GameIntro).
        /// </summary>
        public int CalculateEnergyRecovery(Student student)
        {
            return (int)(student.health * 0.5 + student.happiness * 0.5);
        }

        /// <summary>Reset for a new game.</summary>
        public void Reset()
        {
            _currentTurn = 0;
        }
    }
}

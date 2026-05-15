namespace AndroidApp1
{
    /// <summary>
    /// Stateless rule engine for success/failure probability checks.
    /// Extracted from Student to keep the data class pure (SRP).
    /// </summary>
    public static class SuccessRateCalculator
    {
        private static readonly Random _random = new Random();

        /// <summary>Probability check: does the student slack off?</summary>
        public static bool IsLazy(Student student)
        {
            int roll = _random.Next(1, 101);
            return roll <= student.laziness;
        }

        /// <summary>Probability check: is the student confused / distracted?</summary>
        public static bool IsConfused(Student student)
        {
            int roll = _random.Next(1, 101);
            return roll <= student.confusion;
        }

        /// <summary>Study action success rate: 1 - laziness%.</summary>
        public static double GetStudySuccessRate(Student student)
        {
            return 1.0 - student.laziness * 0.01;
        }

        /// <summary>Non-study action success rate: 1 - confusion%.</summary>
        public static double GetActivitySuccessRate(Student student)
        {
            return 1.0 - student.confusion * 0.01;
        }
    }
}

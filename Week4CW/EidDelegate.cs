namespace Week4CW
{
    /// <summary>
    /// Provides delegate-target helper methods for numeric operations.
    /// </summary>
    public static class EidDelegate
    {
        /// <summary>
        /// Calculates the sum of all values in the provided array.
        /// </summary>
        /// <param name="numbers">The array of integers to sum.</param>
        /// <returns>The total of all integers in <paramref name="numbers"/>.</returns>
        public static int SumArray(int[] numbers)
        {
            int total = 0;
            foreach (var number in numbers)
            {
                total += number;
            }
            return total;
        }

        /// <summary>
        /// Finds the maximum value in the provided array.
         /// </summary>
        /// <param name="nums">The array of integers to evaluate.</param>
        /// <returns>The largest integer in <paramref name="nums"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nums"/> is null or empty.</exception>
        /// <remarks>
        /// This method iterates through the array to find the maximum value. It assumes that the array contains at least one element and that all elements are valid integers.
        /// </remarks>
        /// <example>
        /// int[] numbers = { 3, 5, 7, 2, 8 };
        /// int max = EidDelegate.Max(numbers);
        /// Console.WriteLine(max); // Output: 8
        /// </example>
        /// <seealso cref="SumArray(int[])"/>
        /// <seealso cref="Min(int[])"/>
        /// <seealso cref="Average(int[])"/>
        /// <seealso cref="Median(int[])"/>
        /// <seealso cref="Mode(int[])"/>
        /// <seealso cref="StandardDeviation(int[])"/>
        /// <seealso cref="Variance(int[])"/>
        /// <seealso cref="Percentile(int[], double)"/>
        /// <seealso cref="Quartiles(int[])"/>
        /// <seealso cref="Range(int[])"/>
        /// <seealso cref="IEnumerable.Max()"/>
        /// <seealso cref="Math.Max(int, int)"/>
        /// 
        public static int Max(int[] nums)
        {
            if (nums == null || nums.Length == 0)
            {
                throw new ArgumentException("Input array cannot be null or empty.", nameof(nums));
            }
            int max = nums[0];
            foreach (var num in nums)
            {
                if (num > max)
                {
                    max = num;
                }
            }
            return max;
        }

        public static int Min(int[] nums)
        {
            if (nums == null || nums.Length == 0)
            {
                throw new ArgumentException("Input array cannot be null or empty.", nameof(nums));
            }
            int min = nums[0];
            foreach (var num in nums)
            {
                if (num < min)
                {
                    min = num;
                }
            }
            return min;
        }
    }
}
using System;

namespace General.Helper
{
    public static class FindMaxInt
    {
        public static int FindMaxIntOfArray(int[] numbers)
        {
            int max = int.MinValue;

            if (numbers.Length < 1)
                return -1; //If no element is in array than -1 error;
            
            for (int i = 0; i < numbers.Length; i++)
            {
                if (max > numbers[i])
                    continue;

                max = numbers[i];
            }

            return max;
        }
    }
}

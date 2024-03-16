namespace General.Helper
{
    public static class OperationHelper
    {
        public static bool CompareSameLengthArrays<T>(T[] a, T[] b)
        {
            int length = a.Length;
            for (int i = 0; i < length; i++)
            {
                if (!(Equals(a[i], b[i])))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

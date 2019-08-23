namespace Wjire.Console
{
    public class LeeCode
    {

        public static int[] PlusOne()
        {
            int[] digits = new int[] { 1, 2, 3 };
            int length = digits.Length;
            int jin = 0;
            if (digits[length - 1] == 9)
            {
                jin = 1;
            }
            for (int i = length - 1; i >= 0; i--)
            {
                digits[i] = (digits[i] + jin) % 10;
            }
        }
    }
}

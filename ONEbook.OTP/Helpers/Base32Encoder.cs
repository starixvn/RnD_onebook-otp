using System;

namespace ONEbook.OTP.Helpers
{
    /// <summary>
    /// Legacy code from base 32
    /// https://www.nuget.org/packages/Base32/1.1.0.2
    /// </summary>
    public static class Base32Encoder
    {
        private const int encodedBitCount = 5;

        private const int byteBitCount = 8;

        private const string encodingChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        private const char paddingCharacter = '=';

        public static byte[] Decode(string base32)
        {
            if (string.IsNullOrEmpty(base32))
            {
                throw new ArgumentNullException("base32");
            }
            string str = base32.ToUpperInvariant().TrimEnd(new char[] { paddingCharacter });
            string str1 = str;
            for (int i = 0; i < str1.Length; i++)
            {
                if (encodingChars.IndexOf(str1[i]) < 0)
                {
                    throw new ArgumentException("base32 contains illegal characters");
                }
            }
            int length = str.Length * encodedBitCount / byteBitCount;
            byte[] numArray = new byte[length];
            byte num = 0;
            short num1 = byteBitCount;
            int num3 = 0;
            string str2 = str;
            for (int j = 0; j < str2.Length; j++)
            {
                int num4 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".IndexOf(str2[j]);
                int num2;
                if (num1 <= encodedBitCount)
                {
                    num2 = num4 >> (encodedBitCount - num1 & 31);
                    num = (byte)(num | num2);
                    int num5 = num3;
                    num3 = num5 + 1;
                    numArray[num5] = num;
                    num = (byte)(num4 << (3 + num1 & 31));
                    num1 = (short)(num1 + 3);
                }
                else
                {
                    num2 = num4 << (num1 - encodedBitCount & 31);
                    num = (byte)(num | num2);
                    num1 = (short)(num1 - encodedBitCount);
                }
            }
            return numArray;
        }

        public static string Encode(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Length == 0)
            {
                throw new ArgumentException("data must not be empty");
            }
            int num = (int)decimal.Ceiling(data.Length / new decimal(encodedBitCount)) * byteBitCount;
            char[] chrArray = new char[num];
            byte num1 = 0;
            short num2 = encodedBitCount;
            int num3 = 0;
            byte[] numArray = data;
            for (int i = 0; i < numArray.Length; i++)
            {
                byte num4 = numArray[i];
                num1 = (byte)(num1 | num4 >> (byteBitCount - num2 & 31));
                int num5 = num3;
                num3 = num5 + 1;
                chrArray[num5] = encodingChars[num1];
                if (num2 <= 3)
                {
                    num1 = (byte)(num4 >> (3 - num2 & 31) & 31);
                    int num6 = num3;
                    num3 = num6 + 1;
                    chrArray[num6] = encodingChars[num1];
                    num2 = (short)(num2 + encodedBitCount);
                }
                num2 = (short)(num2 - 3);
                num1 = (byte)(num4 << (num2 & 31) & 31);
            }
            if (num3 != num)
            {
                int num7 = num3;
                num3 = num7 + 1;
                chrArray[num7] = encodingChars[num1];
            }
            while (num3 < num)
            {
                int num8 = num3;
                num3 = num8 + 1;
                chrArray[num8] = paddingCharacter;
            }
            return new string(chrArray);
        }
    }
}

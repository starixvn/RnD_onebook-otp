using System;

namespace ONEbook.OTP.Helpers;

public static class Base32Encoder
{
    private const int EncodedBitCount = 5;

    private const int ByteBitCount = 8;

    private const string EncodingChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    private const char PaddingCharacter = '=';

    public static byte[] Decode(string base32)
    {
        if (string.IsNullOrEmpty(base32))
        {
            throw new ArgumentNullException(nameof(base32));
        }
        var str = base32.ToUpperInvariant().TrimEnd([PaddingCharacter]);
        for (var i = 0; i < str.Length; i++)
        {
            if (EncodingChars.IndexOf(str[i]) < 0)
            {
                throw new ArgumentException("base32 contains illegal characters");
            }
        }
        var length = str.Length * EncodedBitCount / ByteBitCount;
        var numArray = new byte[length];
        byte num = 0;
        short num1 = ByteBitCount;
        var num3 = 0;
        for (var j = 0; j < str.Length; j++)
        {
            var num4 = EncodingChars.IndexOf(str[j]);
            int num2;
            if (num1 <= EncodedBitCount)
            {
                num2 = num4 >> (EncodedBitCount - num1 & 31);
                num = (byte)(num | num2);
                var num5 = num3;
                num3 = num5 + 1;
                numArray[num5] = num;
                num = (byte)(num4 << (3 + num1 & 31));
                num1 = (short)(num1 + 3);
            }
            else
            {
                num2 = num4 << (num1 - EncodedBitCount & 31);
                num = (byte)(num | num2);
                num1 = (short)(num1 - EncodedBitCount);
            }
        }
        return numArray;
    }

    public static string Encode(byte[] data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (data.Length == 0)
        {
            throw new ArgumentException("data must not be empty");
        }
        var num = (int)decimal.Ceiling(data.Length / new decimal(EncodedBitCount)) * ByteBitCount;
        var chrArray = new char[num];
        byte num1 = 0;
        short num2 = EncodedBitCount;
        var num3 = 0;
        for (var i = 0; i < data.Length; i++)
        {
            var num4 = data[i];
            num1 = (byte)(num1 | num4 >> (ByteBitCount - num2 & 31));
            var num5 = num3;
            num3 = num5 + 1;
            chrArray[num5] = EncodingChars[num1];
            if (num2 <= 3)
            {
                num1 = (byte)(num4 >> (3 - num2 & 31) & 31);
                var num6 = num3;
                num3 = num6 + 1;
                chrArray[num6] = EncodingChars[num1];
                num2 = (short)(num2 + EncodedBitCount);
            }
            num2 = (short)(num2 - 3);
            num1 = (byte)(num4 << (num2 & 31) & 31);
        }
        if (num3 != num)
        {
            var num7 = num3;
            num3 = num7 + 1;
            chrArray[num7] = EncodingChars[num1];
        }
        while (num3 < num)
        {
            var num8 = num3;
            num3 = num8 + 1;
            chrArray[num8] = PaddingCharacter;
        }
        return new string(chrArray);
    }
}
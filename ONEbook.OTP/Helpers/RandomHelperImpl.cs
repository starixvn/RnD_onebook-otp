namespace ONEbook.OTP.Helpers
{
    using System;
    using Base32;
    using OtpSharp;

    public class RandomHelperImpl : RandomHelper
    {
        public string GenerateSecretKey(int length = 16)
        {
            byte[] secretKey = KeyGeneration.GenerateRandomKey(length);

            return Base32Encoder.Encode(secretKey);
        }

        public string GenerateUsername(int length = 16)
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, length);
        }
    }
}
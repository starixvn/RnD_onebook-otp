namespace ONEbook.OTP.Helpers
{
    using System;
    using PureOtp;

    public interface RandomHelper
    {
        string GenerateUsername(int length = 10);

        string GenerateSecretKey(int length = 16);
    }

    public class RandomHelperImpl : RandomHelper
    {
        public string GenerateSecretKey(int length = 16)
        {
            var secretKey = KeyGeneration.GenerateRandomKey(length);

            return Base32Encoder.Encode(secretKey);
        }

        public string GenerateUsername(int length = 16)
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, length);
        }
    }
}
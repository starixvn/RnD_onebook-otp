namespace ONEbook.OTP.Helpers
{
    using System;
    using OtpSharp;

    public class OtpAdapterImpl : OtpAdapter
    {
        public string GenerateTotpUrl(string secretKey, string userName, int timePeriod = 30, OtpHashMode mode = OtpHashMode.Sha1, int totpSize = 6)
        {
            throw new NotImplementedException();
        }

        public bool VerifyOtp(string secretKey, string otp, int timePeriod = 30, int totpSize = 6, OtpHashMode mode = OtpHashMode.Sha1)
        {
            throw new NotImplementedException();
        }
    }
}
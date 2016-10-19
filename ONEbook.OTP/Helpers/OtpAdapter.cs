namespace ONEbook.OTP.Helpers
{
    using OtpSharp;

    public interface OtpAdapter
    {
        bool VerifyOtp(string secretKey, string otp, int timePeriod = 30, int totpSize = 6, OtpHashMode mode = OtpHashMode.Sha1);

        string GenerateTotpUrl(string secretKey, string userName, string issuer = "MyIssuer", int timePeriod = 30, OtpHashMode mode = OtpHashMode.Sha1, int totpSize = 6);
    }
}
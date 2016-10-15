namespace ONEbook.OTP.Services
{
    public interface OtpService
    {
        string GenerateQrCode(string secretKey, string userName = null, string issuer = "Application Name", int timePeriod = 30, int otpSize = 6);

        bool VerifyOtp(string secretKey, string otp, int timePeriod = 30, int otpSize = 6);

        string GenerateSecretKey(int length = 16);
    }
}
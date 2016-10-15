namespace ONEbook.OTP.Helpers
{
    public interface QrCodeAdapter
    {
        string Generate(string otpUrl);
    }
}
namespace ONEbook.OTP.Helpers
{
    public interface RandomHelper
    {
        string GenerateUsername(int length = 10);

        string GenerateSecretKey(int length = 16);
    }
}
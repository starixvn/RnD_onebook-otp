namespace ONEbook.OTP.Helpers
{
    using Base32;
    using OtpSharp;

    public class OtpAdapterImpl : OtpAdapter
    {
        public string GenerateTotpUrl(
            string secretKey, 
            string userName, 
            string issuer = "MyIssuer", 
            int timePeriod = 30, 
            OtpHashMode mode = OtpHashMode.Sha1, 
            int totpSize = 6)
        {
            var decodedSecretKEy = Base32Encoder.Decode(secretKey);
            var totpUrl = KeyUrl.GetTotpUrl(decodedSecretKEy, userName, timePeriod, OtpHashMode.Sha1, totpSize);

            return $"{totpUrl}&issuer={issuer}";
        }

        public bool VerifyOtp(
            string secretKey, 
            string otp, 
            int timePeriod = 30, 
            int totpSize = 6, 
            OtpHashMode mode = OtpHashMode.Sha1)
        {
            var decodedSecretKey = Base32Encoder.Decode(secretKey);
            var totp = new Totp(decodedSecretKey, timePeriod, mode, totpSize);
            long timeStepMatched = 0;

            return totp.VerifyTotp(otp, out timeStepMatched, new VerificationWindow(2, 2));
        }
    }
}
namespace ONEbook.OTP.Services
{
    using System;
    using Helpers;
    using OtpSharp;

    public class OtpServiceImpl : OtpService
    {
        private readonly OtpAdapter otpAdapter;
        private readonly QrCodeAdapter qrCodeAdatper;
        private readonly RandomHelper randomHelper;

        public OtpServiceImpl(
            OtpAdapter otpAdapter = null, 
            QrCodeAdapter qrCodeAdatper = null,
            RandomHelper randomHelper = null)
        {
            this.otpAdapter = otpAdapter ?? new OtpAdapterImpl();
            this.qrCodeAdatper = qrCodeAdatper ?? new QrCodeAdapterImpl();
            this.randomHelper = randomHelper ?? new RandomHelperImpl();
        }

        public string GenerateQrCode(string secretKey, string userName = null, string issuer = "Application Name", int timePeriod = 30, int otpSize = 6)
        {
            if(string.IsNullOrWhiteSpace(secretKey))
            {
                throw new ArgumentOutOfRangeException(nameof(secretKey), "Secret key cannot be null or white space");
            }

            if(string.IsNullOrWhiteSpace(userName))
            {
                userName = randomHelper.GenerateUsername();
            }

            var otpUrl = otpAdapter.GenerateTotpUrl(secretKey, userName, issuer, timePeriod, OtpHashMode.Sha1, otpSize);
            var qrCode = qrCodeAdatper.Generate(otpUrl);

            return qrCode;
        }

        public string GenerateSecretKey(int length = 16)
        {
            if(length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be negative or zero");
            }

            return randomHelper.GenerateSecretKey(length);
        }

        public bool VerifyOtp(string secretKey, string otp, int timePeriod = 30, int otpSize = 6)
        {
            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new ArgumentOutOfRangeException(nameof(secretKey), "Secret key cannot be null or white space");
            }

            if (string.IsNullOrWhiteSpace(otp))
            {
                throw new ArgumentOutOfRangeException(nameof(otp), "One time password cannot be null or white space");
            }

            var isOk = otpAdapter.VerifyOtp(secretKey, otp, timePeriod, otpSize, OtpHashMode.Sha1);

            return isOk;
        }
    }
}
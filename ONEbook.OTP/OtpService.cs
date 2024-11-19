namespace ONEbook.OTP;

using System;
using Helpers;
using OtpNet;

public interface OtpService
{
    string GenerateQrCode(string secretKey, int timePeriod = 30, int otpSize = 6, string issuer = "Application Name", string userName = null);

    bool VerifyOtp(string secretKey, string otp, int timePeriod = 30, int otpSize = 6);

    string GenerateSecretKey(int length = 16);
}

public class OtpServiceImp : OtpService
{
    private readonly OtpAdapter otpAdapter = new OtpAdapterImpl();
    private readonly QrCodeAdapter qrCodeAdapter = new QrCodeAdapterImpl();
    private readonly RandomHelper randomHelper = new RandomHelperImpl();

    public string GenerateQrCode(string secretKey, int timePeriod = 30, int otpSize = 6, string issuer = "Application Name", string userName = null)
    {
        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new ArgumentOutOfRangeException(nameof(secretKey), "Secret key cannot be null or white space");
        }

        if (string.IsNullOrWhiteSpace(userName))
        {
            userName = randomHelper.GenerateUsername();
        }

        var otpUrl = otpAdapter.GenerateTotpUrl(secretKey, userName, issuer, timePeriod, OtpHashMode.Sha1, otpSize);
        var qrCode = qrCodeAdapter.Generate(otpUrl);

        return qrCode;
    }

    public string GenerateSecretKey(int length = 16)
    {
        if (length <= 0)
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

        var isOk = otpAdapter.VerifyOtp(secretKey, otp, timePeriod, otpSize);

        return isOk;
    }
}
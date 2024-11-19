using System;

namespace Tests;

using ONEbook.OTP;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Hello World!");

        GenerateQr();

        TestOtp();
    }

    private static void GenerateQr()
    {
        var otpService = new OtpServiceImp();
        const string key = "T3JYLVIHX2WUZL45P3KLI4J2IY======";
        var img = otpService.GenerateQrCode(key, 30, 6, "NewOTPApp", "jacobphan");
            
        Console.WriteLine(img);
    }

    private static void TestOtp()
    {
        var otpService = new OtpServiceImp();
        const string key = "T3JYLVIHX2WUZL45P3KLI4J2IY======";
        while (true)
        {
            Console.WriteLine("Enter the key:");

            var otp = Console.ReadLine();

            if (string.IsNullOrEmpty(otp))
            {
                break;
            }

            var otpResult = otpService.VerifyOtp(key, otp);

            Console.WriteLine("Correct:" + otpResult);
        }
    }
}
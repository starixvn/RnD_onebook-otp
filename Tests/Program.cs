using System;
using ONEbook.OTP.Services;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            GenerateQr();

        }

        static void GenerateQr()
        {
            var otpService = new OtpServiceImpl();
            const string key = "T3JYLVIHX2WUZL45P3KLI4J2IY======";
            var img = otpService.GenerateQrCode(key, 30, 6, "NewOTPApp", "jacobphan");
        }

        static void TestOtp()
        {
            var otpService = new OtpServiceImpl();
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
}

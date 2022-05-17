using System;
using ONEbook.OTP.Services;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

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

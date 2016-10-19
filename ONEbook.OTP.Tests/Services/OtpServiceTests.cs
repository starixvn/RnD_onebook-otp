namespace ONEbook.OTP.Tests.Services
{
    using System;
    using Helpers;
    using NSubstitute;
    using NUnit.Framework;
    using OTP.Services;
    using OtpSharp;

    [TestFixture]
    public class OtpServiceTests
    {
        private OtpServiceImpl otpService;
        private OtpAdapter otpAdapter;
        private QrCodeAdapter qrCodeAdapter;
        private RandomHelper randomHelper;

        [SetUp]
        public void SetUp()
        {
            otpAdapter = Substitute.For<OtpAdapter>();
            qrCodeAdapter = Substitute.For<QrCodeAdapter>();
            randomHelper = Substitute.For<RandomHelper>();

            otpService = new OtpServiceImpl(otpAdapter, qrCodeAdapter, randomHelper);
        }

        #region Generate Qr Code

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GenerateQrCode_SecretKeyIsNullOrWhiteSpace_ThrowArgumentOutOfRangeException(string secretKey)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                otpService.GenerateQrCode(secretKey);
            });
        }

        [Test]
        public void GenerateQrCode_ExecuteGenerateTotpUrl()
        {
            // Arrange
            var secretKey = "secretKey";
            var userName = "userName";
            var issuer = "issuer";
            var timePeriod = 30;
            var otpSize = 6;

            // Act
            otpService.GenerateQrCode(secretKey, userName, issuer, timePeriod, otpSize);

            // Assert
            otpAdapter.Received().GenerateTotpUrl(secretKey, userName, issuer, timePeriod, OtpHashMode.Sha1, otpSize);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GenerateQrCode_UsernameIsNullOrWhiteSpace_ExecuteGenerateRandomUsername(string username)
        {
            // Act
            otpService.GenerateQrCode("secretKey", username);

            // Assert
            randomHelper.Received().GenerateUsername();
        }

        [Test]
        public void GenerateQrCode_ExecuteGenerateQrCodeFromOtpUrl()
        {
            // Arrange
            var expectedOtpUrl = "expectedOtpUrl";
            otpAdapter.GenerateTotpUrl(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<OtpHashMode>(),
                Arg.Any<int>()).Returns(expectedOtpUrl);

            // Act
            otpService.GenerateQrCode("secretkey");

            // Assert
            qrCodeAdapter.Received().Generate(expectedOtpUrl);
        }

        [Test]
        public void GenerateQrCode_ReturnGeneratedQrCode()
        {
            // Arrange
            var expectedQrCode = "expectedQrCode";
            var returnOtpUrl = "returnOtpUrl";
            otpAdapter.GenerateTotpUrl(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<OtpHashMode>(),
                Arg.Any<int>()).Returns(returnOtpUrl);
            qrCodeAdapter.Generate(returnOtpUrl).Returns(expectedQrCode);

            // Act
            var actualQrCode = otpService.GenerateQrCode("secretkey");

            // Assert
            Assert.AreEqual(expectedQrCode, actualQrCode);
        }

        #endregion Generate Qr Code

        #region Generate Secret Key

        [TestCase(0)]
        [TestCase(-1)]
        public void GenerateSecretKey_LengthIsNegativeOrZero_ThroughArgumentOutOfRangeException(int length)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                otpService.GenerateSecretKey(length);
            });
        }

        [Test]
        public void GenerateSecretKey_ExecuteGenerateSecretKey()
        {
            // Act
            otpService.GenerateSecretKey(10);

            // Assert
            randomHelper.Received().GenerateSecretKey(10);
        }

        #endregion

        #region Verify Otp

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void VerifyOtp_SecretKeyIsNullOrWhiteSpace_ThrowArgumentOutOfRangeException(string secretKey)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                otpService.VerifyOtp(secretKey, "otp");
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void VerifyOtp_OtpIsNullOrWhiteSpace_ThrowArgumentOutOfRangeException(string otp)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                otpService.VerifyOtp("secretKey", otp);
            });
        }

        [Test]
        public void VerifyOtp_ExecuteVerifyOtp()
        {
            // Arrange
            var secretKey = "secretKey";
            var otp = "otp";
            var timePeriod = 30;
            var otpSize = 6;

            // Act
            otpService.VerifyOtp(secretKey, otp, timePeriod, otpSize);

            // Assert
            otpAdapter.Received().VerifyOtp(secretKey, otp, timePeriod, otpSize, OtpHashMode.Sha1);
        }



        #endregion
    }
}
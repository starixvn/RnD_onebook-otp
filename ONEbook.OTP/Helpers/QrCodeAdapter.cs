namespace ONEbook.OTP.Helpers
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using QRCoder;

    public interface QrCodeAdapter
    {
        string Generate(string otpUrl);
    }

    public class QrCodeAdapterImpl : QrCodeAdapter
    {
        public string Generate(string otpUrl)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                using (var qrCodeData = qrGenerator.CreateQrCode(otpUrl, QRCodeGenerator.ECCLevel.Q))
                {
                    using (var qrCode = new QRCode(qrCodeData))
                    {
                        using (var qrCodeImage = qrCode.GetGraphic(4, Color.Black, Color.White,true))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                qrCodeImage.Save(memoryStream, ImageFormat.Jpeg);
                                return $"data:image/jpeg;base64,{Convert.ToBase64String(memoryStream.ToArray())}";
                            }
                        }
                    }
                }
            }
        }
    }
}
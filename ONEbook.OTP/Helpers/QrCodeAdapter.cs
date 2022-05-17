namespace ONEbook.OTP.Helpers
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using Gma.QrCodeNet.Encoding;
    using Gma.QrCodeNet.Encoding.Windows.Render;

    public interface QrCodeAdapter
    {
        string Generate(string otpUrl);
    }

    public class QrCodeAdapterImpl : QrCodeAdapter
    {
        public string Generate(string otpUrl)
        {
            var encoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode;
            encoder.TryEncode(otpUrl, out qrCode);

            var graphiscRenderer = new GraphicsRenderer(
                new FixedModuleSize(4, QuietZoneModules.Four),
                 Brushes.Black, Brushes.White);

            var memoryStream = new MemoryStream();
            graphiscRenderer.WriteToStream(qrCode.Matrix, ImageFormat.Jpeg, memoryStream);

            return $"data:image/jpeg;base64,{Convert.ToBase64String(memoryStream.ToArray())}";
        }
    }
}
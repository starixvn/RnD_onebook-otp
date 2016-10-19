namespace ONEbook.OTP.Helpers
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using Gma.QrCodeNet.Encoding;
    using Gma.QrCodeNet.Encoding.Windows.Render;

    public class QrCodeAdapterImpl : QrCodeAdapter
    {
        public string Generate(string otpUrl)
        {
            QrEncoder encoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode;
            encoder.TryEncode(otpUrl, out qrCode);

            GraphicsRenderer graphiscRenderer = new GraphicsRenderer(
                new FixedModuleSize(4, QuietZoneModules.Four),
                Brushes.Black, Brushes.White);

            MemoryStream ms = new MemoryStream();
            graphiscRenderer.WriteToStream(qrCode.Matrix, ImageFormat.Jpeg, ms);

            return $"data:image/jpeg;base64,{Convert.ToBase64String(ms.ToArray())}";
        }
    }
}
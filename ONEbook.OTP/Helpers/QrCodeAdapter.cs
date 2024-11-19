namespace ONEbook.OTP.Helpers;

using System;
using System.IO;
using SkiaSharp;
using SkiaSharp.QrCode;

public interface QrCodeAdapter
{
    string Generate(string otpUrl);
}

public class QrCodeAdapterImpl : QrCodeAdapter
{
    public string Generate(string otpUrl)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(otpUrl, ECCLevel.Q);

        var info = new SKImageInfo(212, 212);
        using var surface = SKSurface.Create(info);
        var canvas = surface.Canvas;
        canvas.Render(qrCodeData, info.Width, info.Height);

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 100);

        using var memoryStream = new MemoryStream();
        data.SaveTo(memoryStream);

        return $"data:image/jpeg;base64,{Convert.ToBase64String(memoryStream.ToArray())}";
    }
}
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using OtpNet;

namespace ONEbook.OTP.Helpers;

public static class KeyUrl
{
    public static string GetTotpUrl(byte[] key, string user, int step = 30, OtpHashMode mode = OtpHashMode.Sha1, int totpSize = 6)
    {
        var url = GetBaseKeyUrl(key, user, OtpType.Totp, totpSize);

        if (mode != OtpHashMode.Sha1)
        {
            url += CreateParameter(UrlConstants.AlgorithmParameter, mode);
        }

        if (step != 30)
        {
            url += CreateParameter(UrlConstants.PeriodParameter, step);
        }

        return url;
    }

    private static string GetBaseKeyUrl(byte[] key, string user, OtpType otpType, int size)
    {
        if (string.IsNullOrEmpty(user))
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (key == null || key.Length == 0)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (size != 6 && size != 8)
        {
            throw new ArgumentException("size must be 6 or 8");
        }

        var url = $"otpauth://{otpType.ToString().ToLowerInvariant()}/{WebUtility.UrlEncode(user)}?{UrlConstants.SecretParameter}={Base32Encoding.ToString(key)}";

        if (size == 8)
        {
            url += CreateParameter(UrlConstants.DigitsParameter, size);
        }

        return url;
    }

    private static string CreateParameter(string name, object value)
    {
        return string.Format(UrlConstants.ParameterCreation, name, WebUtility.UrlEncode(value.ToString()));
    }

    internal static bool ValidateQueryStringFields(NameValueCollection query, params string[] acceptableValues)
    {
        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        return acceptableValues != null && query.AllKeys.All(acceptableValues.Contains);
    }
}
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;

namespace SFA.DAS.FAA.Web.Infrastructure;

public interface IDataProtectorService
{
    string EncodedData(string data);
    string? DecodeData(string data);
}
    
public class DataProtectorService : IDataProtectorService
{
    private readonly ILogger<DataProtectorService> _logger;
    private readonly IDataProtector _faaDataProtector;

    public DataProtectorService (IDataProtectionProvider provider, ILogger<DataProtectorService> logger)
    {
        _logger = logger;
        _faaDataProtector = provider.CreateProtector("FindAnApprenticeship");
    }
        
    public string EncodedData(string data)
    {
        return WebEncoders.Base64UrlEncode(_faaDataProtector.Protect(
            System.Text.Encoding.UTF8.GetBytes($"{data}")));
    }

    public string? DecodeData(string data)
    {
        try
        {
            var base64EncodedBytes = WebEncoders.Base64UrlDecode(data);
            var encodedData = System.Text.Encoding.UTF8.GetString(_faaDataProtector.Unprotect(base64EncodedBytes));
            return encodedData;
        }
        catch (FormatException e)
        {
            _logger.LogInformation(e,"Unable to decode data from request");
        }
        catch (CryptographicException e)
        {
            _logger.LogInformation(e, "Unable to decode data from request");
        }

        return null;
    }
}

public class DevDataProtectorService : IDataProtectorService
{
    public string EncodedData(string data)
    {
        return data.ToString();
    }

    public string? DecodeData(string data)
    {
        return data;
    }
}
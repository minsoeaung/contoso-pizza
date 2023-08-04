using ContosoPizza.Configurations;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ContosoPizza.Services;

public class SmsService : ISmsService
{
    private readonly SmsConfig _smsConfig;

    public SmsService(IOptions<SmsConfig> smsConfig)
    {
        _smsConfig = smsConfig.Value;
    }

    public Task SendSmsAsync(string number, string message)
    {
        var accountSid = _smsConfig.SmsAccountIdentification;
        var authToken = _smsConfig.SmsAccountPassword;

        TwilioClient.Init(accountSid, authToken);

        return MessageResource.CreateAsync(
            to: new PhoneNumber(number),
            from: new PhoneNumber(_smsConfig.SmsAccountFrom),
            body: message);
    }
}
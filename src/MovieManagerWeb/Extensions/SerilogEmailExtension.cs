using MovieManager.Core.Helper;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MovieManagerWeb
{
	public static class SerilogEmailExtension
	{
        public static LoggerConfiguration EmailCustom(this LoggerSinkConfiguration sinkConfiguration,
           string fromEmail,
           string toEmail,
           string enableSsl,
           string mailSubject,
           string isBodyHtml,
           string mailServer,
           string networkCredentialuserName,
           string networkCredentialpassword,
           string smtpPort,
           string outputTemplate,
           string batchPostingLimit,
           string periodMinutes,
           string restrictedToMinimumLevel)
        {
            return sinkConfiguration.Email(new EmailConnectionInfo
                {
                    FromEmail = fromEmail,
                    ToEmail = toEmail,
                    MailServer = mailServer,
                    NetworkCredentials = new NetworkCredential(networkCredentialuserName, networkCredentialpassword),
                    EnableSsl = BasicHelpers.GetBoolean(enableSsl),
                    EmailSubject = mailSubject,
                    IsBodyHtml = BasicHelpers.GetBoolean(isBodyHtml),
                    Port = BasicHelpers.GetInt(smtpPort)
                }, 
                outputTemplate, 
                GetLevel(restrictedToMinimumLevel),
                BasicHelpers.GetInt(batchPostingLimit), 
                TimeSpan.FromMinutes(BasicHelpers.GetInt(periodMinutes))
            );
        }

        private static LogEventLevel GetLevel(string restrictedtominimumlevel)
        {
            return Enum.TryParse(restrictedtominimumlevel, true,
                out LogEventLevel level) ? level : LogEventLevel.Warning;
        }
    }
}

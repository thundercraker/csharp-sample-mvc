using System;
using System.IO;
using System.Threading.Tasks;
using EngineerTest.Extensions;

namespace EngineerTest.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
    
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }

    public class DevOnlyEmailService : IEmailService
    {
        public async Task SendEmailAsync(
            string email, string subject, string message)
        {
            string homePath = (Environment.OSVersion.Platform == PlatformID.Unix || 
                               Environment.OSVersion.Platform == PlatformID.MacOSX)
                ? Environment.GetEnvironmentVariable("HOME")
                : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            Directory.CreateDirectory($"{homePath}/devemail"); 
            using (var writer = File.AppendText($"{homePath}/devemail/{subject}_{DateTime.Now.ToUnixTimeStamp()}.txt"))
            {
                await writer.WriteAsync(email);
                await writer.WriteLineAsync();
                await writer.WriteAsync(message);
            }
        }
    }
}
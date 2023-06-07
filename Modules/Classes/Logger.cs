using Lavalink4NET.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND_DC_Music_Bot.Modules.Classes
{
    public class Logger : ILogger
    {
        public void Log(object source, string message, LogLevel level = LogLevel.Information, Exception? exception = null)
        {
            Console.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} [{source.ToString()}] {message}");
        }
    }
}

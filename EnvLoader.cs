using System;
using System.IO;

namespace DND_DC_Music_Bot
{
    public class EnvLoader
    {
        public static void Load(string filePath)
        {
            // src: https://dusted.codes/dotenv-in-dotnet
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Unable to find .env file");

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(
                    '=',
                    StringSplitOptions.RemoveEmptyEntries);sdfs

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}

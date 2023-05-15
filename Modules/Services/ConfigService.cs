using Newtonsoft.Json;

namespace DND_DC_Music_Bot.Modules.Services
{
    /// <summary>
    /// Configuration class used to Store and Retrieve Configdata from the Configuration.json File.
    /// </summary>
    public class ConfigService
    {
        private string discordToken;
        private string mongoDBConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigService"/> class.
        /// </summary>
        public ConfigService()
        {
            this.discordToken = string.Empty;
            this.mongoDBConnectionString = string.Empty;
        }

        /// <summary>
        /// Discord API Token used for establishing a connection to the Discord Bot Account.
        /// </summary>
        public string DiscordToken
        {
            get => this.discordToken;
            set { this.discordToken = value; }
        }

        /// <summary>
        /// MongoDB Connection String used for establishing a connection to the MongoDB Database.
        /// </summary>
        public string MongoDBConnectionString
        {
            get => this.mongoDBConnectionString;
            set { this.mongoDBConnectionString = value; }
        }

        /// <summary>
        /// Loads the Configuration.json File and stores the Configdata in the Config Class.
        /// </summary>
        /// <param name="configfilepath">Filepath to the configuration file.</param>
        /// <exception cref="FileNotFoundException">Thrown if File was not found.</exception>
        /// <exception cref="NullReferenceException">Thrown if File could not be deserialized.</exception>
        internal void Loader(string configfilepath)
        {
            // Check if the Configuration.json File exists.
            if (!File.Exists(configfilepath))
            {
                throw new FileNotFoundException("The Configuration.json File could not be found.");
            }

            // Read the Configuration.json File.
            string configjson = File.ReadAllText(configfilepath);

            // Check if the Configuration.json File could be read.
            if (string.IsNullOrEmpty(configjson))
            {
                throw new NullReferenceException("The Configuration.json File could not be read.");
            }

            // Deserialize the Configuration.json File.
            try
            {
                ConfigService? configdata = JsonConvert.DeserializeObject<ConfigService>(configjson) ?? throw new NullReferenceException("The Configuration File could not be deserialized.");

                // Store the Configdata in the Config Class.
                this.discordToken = configdata.DiscordToken != string.Empty ? configdata.DiscordToken : throw new NullReferenceException(nameof(this.DiscordToken) + " is required data and was not found in the JSON file.");
                this.mongoDBConnectionString = configdata.MongoDBConnectionString != string.Empty ? configdata.MongoDBConnectionString : throw new NullReferenceException(nameof(this.MongoDBConnectionString) + " is required data and was not found in the JSON file.");
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("The Configuration.json File could not be deserialized.", ex);
            }
        }
    }
}

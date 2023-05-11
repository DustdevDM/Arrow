using Newtonsoft.Json;

namespace DND_DC_Music_Bot
{
    /// <summary>
    /// Configuration class used to Store and Retrieve Configdata from the Configuration.json File.
    /// </summary>
    public class Config
    {
        private string discordToken;
        private string mongoDBConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        internal Config()
        {
            this.discordToken = string.Empty;
            this.mongoDBConnectionString = string.Empty;
        }

        /// <summary>
        /// Gets the Discord API Token used for establishing a connection to the Discord Bot Account.
        /// </summary>
        internal string DiscordToken { get => this.discordToken; }

        /// <summary>
        /// Gets the MongoDB Connection String used for establishing a connection to the MongoDB Database.
        /// </summary>
        internal string MongoDBConnectionString { get => this.mongoDBConnectionString; }

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
            Config? configdata = JsonConvert.DeserializeObject<Config>(configjson);

            // Check if the Configuration.json File could be deserialized.
            if (configdata == null)
            {
                throw new NullReferenceException("The Configuration.json File could not be deserialized.");
            }

            // Store the Configdata in the Config Class.
            this.discordToken = configdata.DiscordToken;
            this.mongoDBConnectionString = configdata.MongoDBConnectionString;
        }
    }
}

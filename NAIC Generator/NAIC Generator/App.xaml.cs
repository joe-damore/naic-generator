using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using naic.Properties;

namespace naic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public List<Provider> Providers;

        public string ProviderPath;

        public App()
        {
            // Create list to contain providers
            List<Provider> providers;

            // Get application settings
            System.Collections.Specialized.NameValueCollection settings;
            settings = ConfigurationManager.AppSettings;

            // Get provider file path
            this.ProviderPath = settings["providerFilePath"];

            // Load providers
            ProviderManager loader = new ProviderManager();
            providers = loader.LoadFromFile(this.ProviderPath);

            // Make sure providers were
            // loaded correctly
            if(providers == null)
            {
                // They weren't.
                Console.WriteLine("Failed to load providers!");
                providers = new List<Provider>();
            }

            // Assign providers
            this.Providers = providers;
        }
    }
}

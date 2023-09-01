using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace naic
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// List containing all providers.
        /// This is a copy of the list used
        /// by the main portion of the program
        /// so any unsaved changes can be
        /// discarded.
        private ObservableCollection<Provider> providers;

        /// Accessor for providers property
        public ObservableCollection<Provider> Providers
        {
            get
            {
                return this.providers;
            }
            set
            {
                this.providers = value;
            }
        }

        public SettingsWindow()
        {
            InitializeComponent();

            this.Loaded += this.OnLoad;
        }

        /**
        \brief
            Called when window
            is loaded.

        \param sender
            Event sender

        \param e
            Event arguments
        */
        public void OnLoad(object sender, RoutedEventArgs e)
        {
            // Create provider collection
            this.providers = new ObservableCollection<Provider>();

            // Reload providers
            this.ReloadProviders();

            // Link general tab to this settings window
            this.GeneralTabControl.ParentWindow = this;

            // Link provider tab to this settings window
            this.ProviderTabControl.ParentWindow = this;

            // Link region tab to this settings window
            this.RegionTabControl.ParentWindow = this;

            this.GeneralTabControl.LoadFromSettings(Properties.Settings.Default);
        }

        /**
        \brief
            Reloads the provider XML
        */
        public void ReloadProviders()
        {
            // Create list to store providers
            List<Provider> providers;

            // Get provider file path
            App app = (App)Application.Current;
            string providerPath = app.ProviderPath;

            // Load providers
            ProviderManager loader = new ProviderManager();
            providers = loader.LoadFromFile(providerPath);

            if (providers == null)
            {
                // Failed to load. Create
                // empty provider list
                providers = new List<Provider>();
            }

            // Clear the provider collection
            this.Providers.Clear();
            
            // Insert each provider from the
            // provider list to the provider
            // collection
            foreach(Provider pro in providers)
            {
                this.Providers.Add(pro);
            }
        }

        /**
        \brief
            Save providers and regions
        */
        public void SaveProviders()
        {
            // Get provider file path
            App app = (App)Application.Current;
            string providerPath = app.ProviderPath;
            
            // Create provider writer
            ProviderManager writer = new ProviderManager();

            // Write providers
            writer.WriteToFile(this.Providers.ToList(), providerPath);
        }

        /**
        \brief
            Save user settings
        */
        public void SaveSettings()
        {
            // Refers to user settings
            Properties.Settings settings = Properties.Settings.Default;

            // Store settings from general tab
            this.GeneralTabControl.SaveToSettings(settings);

            // Save settings
            settings.Save();
        }

        private void OnApplyClick(object sender, RoutedEventArgs e)
        {
            // Save providers
            this.SaveProviders();

            // Save user settings
            this.SaveSettings();
        }


        private void OnOKClick(object sender, RoutedEventArgs e)
        {
            // Save providers
            this.SaveProviders();

            // Save user settings
            this.SaveSettings();

            // Hide window
            this.Hide();

            // Reset to main view
            tabControl.SelectedIndex = 0;
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            // Hide window
            this.Hide();

            // Reset to main view
            tabControl.SelectedIndex = 0;
        }

        private void tabControlChangedSelection(object sender, SelectionChangedEventArgs e)
        {
            // Get tab control
            TabControl tabControl = (TabControl)sender;

            switch(tabControl.SelectedIndex)
            {
                // General settings
                case 0:

                    break;

                // Provider settings
                case 1:
                    // Make sure an item
                    // is selected
                    if (ProviderTabControl.SelectedProvider == null &&
                        this.Providers.Count > 0)
                    {
                        // No provider selected
                        // Select the first
                        ProviderTabControl.SelectProviderAt(0);
                    }
                    break;

                // Region settings
                case 2:
                    // Make sure an item
                    // is selected
                    if(RegionTabControl.CurrentProvider == null &&
                        this.Providers.Count > 0)
                    {
                        // No provider is selected
                        // (or the previously selected
                        // provider has since been removed)
                        // Selected the first provider
                        RegionTabControl.CurrentProvider = this.Providers.ElementAt(0);
                    }
                    break;

                // Advanced settings
                case 3:

                    break;
            }

        }

        /**
        \brief
            Called when window is about to
            close.


        */
        protected override void OnClosing(CancelEventArgs e)
        {
            // Cancel close action
            e.Cancel = true;

            // Reset view to general tab
            tabControl.SelectedIndex = 0;

            // Hide window
            this.Hide();
        }

    }
}

using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace naic
{
    /// <summary>
    /// Interaction logic for SettingsWindowGeneralTab.xaml
    /// </summary>
    public partial class SettingsWindowGeneralTab : UserControl, INotifyPropertyChanged
    {
        /// Refers to the parent settings window
        /// that this control belongs to
        private SettingsWindow parentWindow;

        /// Accessor for parentWindow
        public SettingsWindow ParentWindow
        {
            get
            {
                // Return parent window
                return this.parentWindow;
            }
            set
            {
                // Set parent window
                this.parentWindow = value;

                // Trigger parent window change event
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("ParentWindow"));
                }
            }
        }

        /// Property changed event handler
        public event PropertyChangedEventHandler PropertyChanged;

        /**
        \brief
            Constructor
        */
        public SettingsWindowGeneralTab()
        {
            // Initialize component
            InitializeComponent();

            // Set default provider combobox
            // data context to this
            this.cbDefaultProvider.DataContext = this;
        }

        /**
        \brief
            Stores settings from this tab
            into the given settings object

        \param settings
            Settings object to store new
            settings into
        */
        public void SaveToSettings(Properties.Settings settings)
        {
            settings.userName                      = this.tbUserName.Text;
            settings.useLastUsedProvider           = this.rbDefaultProviderLastUsed.IsChecked.Value;
            settings.useLastUsedOutputFolder       = this.rbDefaultFolderLastUsed.IsChecked.Value;
            settings.defaultOutputFolder           = this.tbDefaultOutputFolder.Text;
            settings.generateOutputLogs            = this.cbGenerateLog.IsChecked.Value;
            settings.preserveOutputContentControls = this.cbPreserveContentControls.IsChecked.Value;
            settings.replaceOutputFiles            = this.cbReplaceExistingFiles.IsChecked.Value;

            // Get selected default provider
            Provider defaultProvider = (Provider)this.cbDefaultProvider.SelectedItem;

            // Make sure provider is not null
            if (defaultProvider != null)
            {
                // It is not, proceed
                settings.defaultProviderID = defaultProvider.ID;
            }
            else
            {
                // It is null, store blank
                // provider ID
                settings.defaultProviderID = Guid.Empty;
            }
        }

        /**
        \brief
            Loads settings from the given
            settings object and populates
            this tab's forms with their
            content

        \param settings
            Settings object to load from
        */
        public void LoadFromSettings(Properties.Settings settings)
        {
            // Load settings
            this.tbUserName.Text                     =  settings.userName;
            this.rbDefaultFolderLastUsed.IsChecked   =  settings.useLastUsedOutputFolder;
            this.rbDefaultFolderOther.IsChecked      = !settings.useLastUsedOutputFolder;
            this.rbDefaultProviderLastUsed.IsChecked =  settings.useLastUsedProvider;
            this.rbDefaultProviderOther.IsChecked    = !settings.useLastUsedProvider;
            this.cbGenerateLog.IsChecked             =  settings.generateOutputLogs;
            this.cbPreserveContentControls.IsChecked =  settings.preserveOutputContentControls;
            this.cbReplaceExistingFiles.IsChecked    =  settings.replaceOutputFiles;
            this.tbDefaultOutputFolder.Text          =  settings.defaultOutputFolder;

            // Select default provider
            Guid defaultID = settings.defaultProviderID;

            // Will refer to the selected
            // provider
            Provider selectedProvider = null;

            Console.WriteLine(defaultID.ToString());

            // Iterate through providers
            foreach(Provider provider in this.ParentWindow.Providers)
            {
                // See if provider ID matches
                if(provider.ID == defaultID)
                {
                    // It does, so assign this
                    // as our selected provider
                    selectedProvider = provider;
                    break;
                }
            }

            // Check if selectedProvider is
            // null
            if(selectedProvider == null)
            {
                // It is, so select the
                // first provider in the
                // list
                if(this.cbDefaultProvider.Items.Count > 0)
                {
                    this.cbDefaultProvider.SelectedIndex = 0;
                }

                // End execution
                return;
            }

            // selectedProvider is not null
            // Choose it in the combo box
            this.cbDefaultProvider.SelectedItem = selectedProvider;
        }

        /**
        \brief
            Called when "Browse..." button
            is clicked.

            Opens a folder selector dialog
            whose locatiion is determined
            using the user's settings

        \param sender
            Object sending click event
            (should be the Browse... button)

        \param e
            Event arguments
        */
        private void OnBrowseButtonClick(object sender, RoutedEventArgs e)
        {

            // Create file picker dialog
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();

            dialog.IsFolderPicker = true;                         // Only allow folders
            dialog.Multiselect    = false;                        // Only one folder allowed
            dialog.Title          = "Default NAIC Output Folder"; // Dialog window title
            
            // If the stored directory exists,
            // open our dialog box to this directory
            if(Directory.Exists(tbDefaultOutputFolder.Text))
            {
                dialog.DefaultDirectory = tbDefaultOutputFolder.Text;
            }

            // Show dialog and store result
            // in 'result' variable
            CommonFileDialogResult result;
            result = dialog.ShowDialog(this.ParentWindow);

            // Make sure that a folder was
            // selected
            if(result != CommonFileDialogResult.Ok)
            {
                // User selected cancel
                // End execution
                return;
            }

            // Update textbox text
            this.tbDefaultOutputFolder.Text = dialog.FileName;
        }
    }
}

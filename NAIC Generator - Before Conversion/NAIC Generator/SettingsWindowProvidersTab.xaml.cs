using System;
using System.Collections.Generic;
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
using System.ComponentModel;

namespace naic
{
    /// <summary>
    /// Interaction logic for SettingsWindowProvidersTab.xaml
    /// </summary>
    public partial class SettingsWindowProvidersTab : UserControl, INotifyPropertyChanged
    {
        /// Refers to the parent settings window
        /// that this control belongs to
        private SettingsWindow parentWindow;

        /// Accessor for parentWindow
        public SettingsWindow ParentWindow
        {
            get
            {
                return this.parentWindow;
            }
            set
            {
                this.parentWindow = value;

                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("ParentWindow"));
                }

                this.HookListBox();
            }
        }

        /// Refers to the currently selected
        /// provider
        public Provider SelectedProvider { get; set; }

        /// Property changed event handler
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsWindowProvidersTab()
        {
            // Init
            InitializeComponent();

            // Hook up no providers control
            this.noProviders.ParentTab = this;
        }

        /**
        \brief
            Hooks up the list box to display content
            from the parent window
        */
        public void HookListBox()
        {
            // Do nothing if parent window is
            // not set
            if(this.ParentWindow == null)
            {
                return;
            }

            // Set item source
            this.providerListBox.ItemsSource = this.ParentWindow.Providers;

            // Set item display path
            this.providerListBox.DisplayMemberPath = "Name";
        }

        /**
        \brief
            Refreshes the contents of the list box
            to recognize any changes made to the
            provider list that may not have been
            picked up
        */
        public void RefreshListBox()
        {
            // Refresh
            this.providerListBox.Items.Refresh();
        }

        /**
        \brief
            Updates the detail view to
            display details of the given
            provider.

        \param provider
            Provider to view details for
        */
        private void updateDetailView(Provider provider)
        {
            // Update provider details current provider
            this.providerDetails.CurrentProvider = provider;
        }

        /**
        \brief
            Displays the correct view depending on what,
            if anything, is selected.
        */
        private void ProviderListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Make sure something is selected
            if(e.AddedItems.Count == 0)
            {
                // Nothing is selected
                // Show the "No Providers Found."
                // screen.
                this.DetailTabControl.SelectedIndex = 1;

                // Return
                return;
            }

            // Get selected provider
            Provider provider = (e.AddedItems[0] as Provider);

            // Store selected provider
            this.SelectedProvider = provider;

            // Show the detail view in case it
            // has not already been shown
            this.DetailTabControl.SelectedIndex = 0;

            // Update detail pane
            this.updateDetailView(provider);
        }

        /**
        \brief
            Adds a new provider and selects
            it in the list box

        \param index
            Index to insert provider into.
            0 by default.

        \param provider
            Provider to be added. If none is
            specified, one is created and added

        \return
            Added provider, or null if none
            is added
        */
        public Provider AddProvider(
            int index = 0,
            Provider provider = null)
        {
            // Make sure we're hooked up
            // to the parent window correctly
            if(this.ParentWindow == null)
            {
                // We're not.
                // Bye.
                return null;
            }
            
            // Refers to the output provider
            Provider outProvider = provider;

            // See if they provided a
            // provider
            if(outProvider == null)
            {
                // Nope.
                // Create a provider
                // and define its properties
                outProvider = new Provider();

                outProvider.Name = "New Provider";
            }

            // Insert new provider into
            // provider collection
            this.ParentWindow.Providers.Insert(0, outProvider);

            // Output the added provider
            return outProvider;
        }

        /**
        \brief
            Selects the given provider in the
            ListBox

        \param provider
            Provider to select. If it does not
            exist in the provider collection,
            the first item will be selected.
        */
        public void SelectProvider(Provider provider)
        {
            // Make sure list contains the given
            // provider
            if(this.providerListBox.Items.Contains(provider))
            {
                // It does, so select it
                this.providerListBox.SelectedItem = provider;
            }
            else
            {
                // It does not
                // Make sure collection has
                // at least one item
                if(this.providerListBox.Items.Count > 0)
                {
                    // It does.
                    // Select the first and stop.
                    this.providerListBox.SelectedItem = 0;
                    return;
                }
                
                // It's empty. We can't select
                // anything.
                this.providerListBox.SelectedItem = -1;
            }
        }

        /**
        \brief
            Selects the provider at the given
            index.

            If index is out of bounds, the last
            item will be selected.

        \param index
            Index of provider to select
        */
        public void SelectProviderAt(int index)
        {
            // Make sure index is within provider
            // collection bounds
            if(index >= this.providerListBox.Items.Count)
            {
                // It's not. Make sure the
                // collection has items.
                if(this.providerListBox.Items.Count <= 0)
                {
                    // It does not.
                    // Select -1.
                    this.providerListBox.SelectedIndex = -1;

                    return;
                }
                // It does. Select the last
                // item.
                int lastIndex = providerListBox.Items.Count - 1;

                this.providerListBox.SelectedIndex = lastIndex;

                return;
            }

            // It is. Select it.
            this.providerListBox.SelectedIndex = index;
        }

        /**
        \brief
            Removes the given provider
            from the provider collection.

        \param provider
            Provider to remove.
        */
        public void RemoveProvider(Provider provider)
        {
            // Remove the given provider
            this.ParentWindow.Providers.Remove(provider);
        }

        /**
        \brief
            Removes the provider from the
            provider collection at the
            given index

        \param index
            Index of provider collection to
            remove
        */
        public void RemoveProviderAt(int index)
        {
            // Remove the provider at the
            // given index
            this.ParentWindow.Providers.RemoveAt(index);
        }

        private void ContextMenuNewProvider_Click(object sender, RoutedEventArgs e)
        {
            // Get the index of the currently
            // selected provider
            int index = this.providerListBox.SelectedIndex;

            // If index is -1, indicating that there
            // are no existing providers, make the index
            // 0.
            if(index == -1)
            {
                index = 0;
            }

            this.AddProvider(index);
        }

        private void ContextMenuDeleteProvider_Click(object sender, RoutedEventArgs e)
        {
            // Delete the currently selected
            // provider
            int selectedIndex = this.providerListBox.SelectedIndex;

            // Make sure something is selected
            if(selectedIndex == -1)
            {
                // Nothing is selected.
                // End function
                return;
            }

            // Get user confirmation. 
            // Make sure they really want to do this.
            MessageBoxResult result;

            // Get selected provider
            Provider selectedProvider = (Provider)this.providerListBox.SelectedItem;

            // Show confirmation dialog
            result = MessageBox.Show(
                "Are you sure you want to delete this provider?",
                selectedProvider.Name,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            // Stop executing if they hit no
            if(result == MessageBoxResult.No)
            {
                return;
            }

            // Remove the provider at the
            // selected index and select
            // the next item if possible
            this.ParentWindow.Providers.Remove(selectedProvider);
            this.SelectProviderAt(selectedIndex);

            Console.WriteLine("Removed selected provider named " + selectedProvider.Name);
        }
    }
}

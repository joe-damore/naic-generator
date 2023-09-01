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
    /// Interaction logic for SettingsWindowRegionsTab.xaml
    /// </summary>
    public partial class SettingsWindowRegionsTab : UserControl, INotifyPropertyChanged
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

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("ParentWindow"));
                }
            }
        }



        /// Refers to currently selected
        /// provider
        private Provider currentProvider;

        /// Accessor for currentProvider
        /// Sends notification on change
        public Provider CurrentProvider
        {
            get
            {
                return this.currentProvider;
            }
            set
            {
                this.currentProvider = value;

                if(this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("CurrentProvider"));
                }

                this.UpdateTabControl();
            }
        }

        /// Property changed event handler
        public event PropertyChangedEventHandler PropertyChanged;

        /**
        \brief
            Constructor.
        */
        public SettingsWindowRegionsTab()
        {
            InitializeComponent();

            // The window will assign this
            // list once it has loaded the
            // providers

            this.providerComboBox.DataContext = this;
            this.regionListBox.DataContext = this;
            this.noRegionsTab.ParentRegionsTab = this;
        }

        /**
        \brief
            Sets the tab view to the correct
            setting depending on what
            is currently selected.
        */
        private void UpdateTabControl()
        {
            TabControl control = this.DetailTabControl;

            Console.WriteLine(this.ParentWindow.Providers.Count + "." + this.CurrentProvider);
            //Console.WriteLine(this.CurrentProvider.Regions.Count + "." + this.CurrentRegion);

            // See if we have providers
            if(this.ParentWindow.Providers.Count <= 0 || this.CurrentProvider == null)
            {
                // No providers
                // Show no providers tab
                this.DetailTabControl.SelectedIndex = 1;

                return;
            }

            // See if we have regions
            if(this.CurrentProvider.Regions.Count <= 0 || this.RegionDetailControl.CurrentRegion == null)
            {
                // No regions
                // Show no regions tab
                this.DetailTabControl.SelectedIndex = 2;

                return;
            }

            // Show region details
            this.DetailTabControl.SelectedIndex = 0;
        }

        private void changedProviderSelection(object sender, SelectionChangedEventArgs e)
        {
            // Get combo box
            ComboBox comboBox = this.providerComboBox;

            // Make sure a region is
            // actually selected
            if(e.AddedItems.Count == 0)
            {
                // One was not
                // Do not select anything
                this.regionListBox.SelectedIndex = -1;
                this.UpdateTabControl();

                return;
            }

            // Get selected provider
            Provider provider = (Provider)e.AddedItems[0];

            Console.WriteLine(provider.Name);

            // See if provider has regions
            if(provider.Regions.Count > 0)
            {
                // It does.
                // Select first region
                this.regionListBox.SelectedIndex = 0;
                this.UpdateTabControl();

                return;
            }

            // No regions
            // Do not select anything
            this.regionListBox.SelectedIndex = -1;
            this.UpdateTabControl();
        }

        private void changedRegionSelection(object sender, SelectionChangedEventArgs e)
        {
            // Get list box
            ListBox listBox = this.regionListBox;

            // Make sure something is
            // selected
            if(e.AddedItems.Count > 0)
            {
                // Set the current region to the
                // selected region
                this.RegionDetailControl.CurrentRegion = (Region)e.AddedItems[0];
            }

            // Update tab control to reflect
            // content
            this.UpdateTabControl();
        }

        /**
        \brief
            Selects the given region in
            the region list box.

        \param region
            Region to select. If it does
            not exist in the list box,
            no action is taken.
        */
        public void SelectRegion(Region region)
        {
            // Make sure we have a current
            // provider set
            if(this.CurrentProvider == null)
            {
                // We do not.
                // End execution.
                return;
            }

            // Make sure given region exists in
            // current provider
            if(this.CurrentProvider.Regions.Contains(region))
            {
                // Select the given region
                this.regionListBox.SelectedItem = region;
            }
        }

        /**
        \brief
            Insert region at the
            specified index

        \param index
            Index to insert region at

        \param region
            Region to insert. If none
            is specified, one will
            be created using default
            values

        \return Region
            Region that is inserted into
            the list, or null on failure
        */
        public Region InsertRegion(
            int    index  = 0,
            Region region = null)
        {
            // Make sure current provider
            // exists
            if(this.CurrentProvider == null)
            {
                // It does not
                // Stop execution
                return null;
            }

            // Refer to given region
            Region reg = region;

            // See if region was provided
            if(reg == null)
            {
                // One was not
                // Create one
                reg = new Region();

                reg.Name = "New Region";
            }

            // Insert new region
            this.CurrentProvider.Regions.Insert(index, reg);

            // Return inserted region
            return reg;
        }

        /**
        \brief
            Removes the given region from
            the current provider's list of
            regions
        */
        public void RemoveRegion(Region region)
        {
            // Make sure current provider
            // exists
            if(this.CurrentProvider == null)
            {
                // It does not
                // Stop executing
                return;
            }

            // Make sure region
            // exists
            if(this.CurrentProvider.Regions.Contains(region))
            {
                // It does.
                // Remove it
                this.CurrentProvider.Regions.Remove(region);
            }
        }

        private void ClickInsertRegion(object sender, RoutedEventArgs e)
        {
            // Make sure current provider
            // exists
            if(this.CurrentProvider == null)
            {
                // It does not.
                // Stop executing.
                return;
            }

            // Refers to index to insert
            // new region at
            int insertIndex = 0;

            // See if any region is selected
            if(this.regionListBox.SelectedIndex != -1)
            {
                // One is, so get
                // the index after
                // the selected index
                insertIndex = this.regionListBox.SelectedIndex + 1;
            }

            // Insert new region
            Region reg = this.InsertRegion(insertIndex);

            // Select region
            this.SelectRegion(reg);
        }

        private void ClickRemoveRegion(object sender, RoutedEventArgs e)
        {
            // Make sure current provider
            // exists
            if(this.CurrentProvider == null)
            {
                // It does not.
                // Stop executing
                return;
            }

            // Get selected region
            Region selectedRegion = (Region)this.regionListBox.SelectedItem;

            // Make sure region exists
            if(selectedRegion == null)
            {
                // It does not
                // Stop executing
                return;
            }

            // Make sure region exists in
            // current provider
            if (this.CurrentProvider.Regions.Contains(selectedRegion))
            {
                // It does.

                // Get user confirmation. 
                // Make sure they really want to do this.
                MessageBoxResult result;

                // Show confirmation dialog
                result = MessageBox.Show(
                    "Are you sure you want to delete this region?",
                    selectedRegion.Name,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                // Stop executing if they hit no
                if (result == MessageBoxResult.No)
                {
                    return;
                }

                // Remove it.
                this.RemoveRegion(selectedRegion);
            }
        }

        private void ClickAddRegion(object sender, RoutedEventArgs e)
        {
            // Determine index of new
            // region
            int index = this.CurrentProvider.Regions.Count;

            // Insert new region
            Region reg = this.InsertRegion(index);

            // Select new region
            this.SelectRegion(reg);
        }

        private void ClickMoveRegionUp(object sender, RoutedEventArgs e)
        {
            // Make sure current provider
            // exists
            if (this.CurrentProvider == null)
            {
                // It does not.
                // Stop executing
                return;
            }

            // Get selected region
            Region selectedRegion = (Region)this.regionListBox.SelectedItem;

            // Make sure region exists
            if (selectedRegion == null)
            {
                // It does not
                // Stop executing
                return;
            }

            // Make sure region exists in
            // current provider
            if (this.CurrentProvider.Regions.Contains(selectedRegion))
            {
                // Move region up

                // Get current index
                int index = this.CurrentProvider.Regions.IndexOf(selectedRegion);

                // Copy current index
                int newIndex = index;

                // Make sure index is not 0
                if(newIndex != 0)
                {
                    // It is not
                    // Decrease by 1
                    newIndex -= 1;
                }

                // Move region
                this.CurrentProvider.Regions.Move(index, newIndex);
            }
        }

        private void ClickMoveRegionDown(object sender, RoutedEventArgs e)
        {
            // Make sure current provider
            // exists
            if (this.CurrentProvider == null)
            {
                // It does not.
                // Stop executing
                return;
            }

            // Get selected region
            Region selectedRegion = (Region)this.regionListBox.SelectedItem;

            // Make sure region exists
            if (selectedRegion == null)
            {
                // It does not
                // Stop executing
                return;
            }

            // Make sure region exists in
            // current provider
            if (this.CurrentProvider.Regions.Contains(selectedRegion))
            {
                // Move region down

                // Get current index
                int index = this.CurrentProvider.Regions.IndexOf(selectedRegion);

                // Copy current index
                int newIndex = index;

                // Make sure index is not
                // the last item
                if (newIndex != (this.CurrentProvider.Regions.Count - 1))
                {
                    // It is not
                    // Increase by 1
                    newIndex += 1;
                }

                // Move region
                this.CurrentProvider.Regions.Move(index, newIndex);
            }
        }

        private void ClickDuplicateRegion(object sender, RoutedEventArgs e)
        {
            // Make sure current provider
            // exists
            if (this.CurrentProvider == null)
            {
                // It does not.
                // Stop executing
                return;
            }

            // Get selected region
            Region selectedRegion = (Region)this.regionListBox.SelectedItem;

            // Make sure region exists
            if (selectedRegion == null)
            {
                // It does not
                // Stop executing
                return;
            }

            // Make sure region exists in
            // current provider
            if (this.CurrentProvider.Regions.Contains(selectedRegion))
            {
                // Duplicate region
                Region newRegion = new Region(selectedRegion);

                // Append "Copy" to the name
                newRegion.Name = selectedRegion.Name + " Copy";

                int selectedIndex = this.CurrentProvider.Regions.IndexOf(selectedRegion);

                // Get selected index
                if(selectedIndex >= (this.CurrentProvider.Regions.Count - 1))
                {
                    // Last item selected
                    // Simply add the duplicate
                    this.CurrentProvider.Regions.Add(newRegion);
                }
                else
                {
                    // Item selected that is not
                    // the last item. Insert it.
                    this.CurrentProvider.Regions.Insert(selectedIndex + 1, newRegion);
                }

                // Select new item
                this.SelectRegion(newRegion);
            }
        }
    }
}

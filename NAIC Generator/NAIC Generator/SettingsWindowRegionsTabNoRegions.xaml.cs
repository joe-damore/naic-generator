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

namespace naic
{
    /// <summary>
    /// Interaction logic for SettingsWindowRegionsTabNoRegions.xaml
    /// </summary>
    public partial class SettingsWindowRegionsTabNoRegions : UserControl
    {
        /// Refers to the settings window tab that this
        /// panel is displayed on
        public SettingsWindowRegionsTab ParentRegionsTab;

        /**
        \brief
            Constructor.
        */
        public SettingsWindowRegionsTabNoRegions()
        {
            InitializeComponent();
        }
        
        /**
        \brief
            Called when the "Add New Region" button
            is clicked.

            Adds a new region to the selected
            provider.

        \param sender
            Object that triggered event

        \param e
            Event arguments
        */
        private void ClickAddNewRegion(object sender, RoutedEventArgs e)
        {
            // Insert region
            Region region = this.ParentRegionsTab.InsertRegion(0, null);

            // Select new region
            this.ParentRegionsTab.SelectRegion(region);
        }
    }
}

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
    /// Interaction logic for SettingsWindowProvidersTabNoProviders.xaml
    /// </summary>
    public partial class SettingsWindowProvidersTabNoProviders : UserControl
    {
        /// Refers to the provider tab view
        // in the settings window
        public SettingsWindowProvidersTab ParentTab;

        public SettingsWindowProvidersTabNoProviders()
        {
            InitializeComponent();
        }

        private void ClickAddNewProvider(object sender, RoutedEventArgs e)
        {
            this.ParentTab.AddProvider(this.ParentTab.ParentWindow.Providers.Count);
        }
    }
}

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
    /// Interaction logic for SettingsWindowProvidersTabProviderDetail.xaml
    /// </summary>
    public partial class SettingsWindowProvidersTabProviderDetail : UserControl, INotifyPropertyChanged
    {
        // Refers to the current provider
        // being displayed in the detail view
        private Provider currentProvider;

        // The event to be thrown when
        // an observable property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Get/setter for current provider
        public Provider CurrentProvider
        {
            get
            {
                return this.currentProvider;
            }
            set
            {
                // Assign new provider
                this.currentProvider = value;

                // Throw property changed
                // event
                PropertyChangedEventHandler handler = PropertyChanged;

                if(PropertyChanged != null)
                {
                    handler(this, new PropertyChangedEventArgs("CurrentProvider"));
                }

                // Update combo boxes
                this.updateComboBoxes();
            }
        }

        public SettingsWindowProvidersTabProviderDetail()
        {
            // Init
            InitializeComponent();

            // Set each text box's data
            // context to enable data
            // binding
            this.tbProviderName.DataContext = this;
            this.tbProviderFEIN.DataContext = this;
            this.tbProviderHomeState.DataContext = this;
            this.tbProviderNumber.DataContext = this;
            this.tbContactName.DataContext = this;
            this.tbContactFax.DataContext = this;
            this.tbContactEmail.DataContext = this;
            this.tbContactPhone.DataContext = this;
            this.tbMailingStreet.DataContext = this;
            this.tbMailingCity.DataContext = this;
            this.tbMailingState.DataContext = this;
            this.tbMailingZip.DataContext = this;
            this.tbDefaultCourseTitle.DataContext = this;
            this.cbDefaultCourseMethod.DataContext = this;
            this.cbDefaultCourseDifficulty.DataContext = this;
            this.cbDefaultIsDesignation.DataContext = this;
            this.cbDefaultIsPublic.DataContext = this;
            this.cbDefaultRequiresExam.DataContext = this;
            this.lbProviderTitle.DataContext = this;
        }

        private void updateComboBoxes()
        {
            // Get the default difficulty and course type
            // for the current course
            int difficulty = (int)CurrentProvider.DefaultCourseDifficulty;
            int method = (int)CurrentProvider.DefaultCourseType;

            // Assign it to the combo boxes
            this.cbDefaultCourseDifficulty.SelectedIndex = difficulty;
            this.cbDefaultCourseMethod.SelectedIndex = method;
        }

        /**
        \brief
            Called when the default course difficulty
            combo box is changed.

            Updates the current provider to store
            the selected value.

        \param sender
            Object sending event (the combo box)

        \param e
            Event arguments
        */
        private void cbDefaultCourseDifficulty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get combo box
            ComboBox comboBox = (ComboBox)sender;

            // Make sure we have a current
            // provider
            if (this.CurrentProvider == null)
            {
                // We do not.
                // End function.

                return;
            }

            // Update course difficulty
            // using combo box value
            this.CurrentProvider.DefaultCourseDifficulty = (CourseDifficulty)comboBox.SelectedIndex;
        }

        /**
        \brief
            Called when the default course method
            combo box is changed.

            Updates the current provider to store
            the selected value.

        \param sender
            Object sending event (the combo box)

        \param e
            Event arguments
        */
        private void cbDefaultCourseMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get combo box
            ComboBox comboBox = (ComboBox)sender;

            // Make sure we have a current
            // provider
            if(this.CurrentProvider == null)
            {
                // We do not.
                // End function.

                return;
            }

            // Update course difficulty
            // using combo box value
            this.CurrentProvider.DefaultCourseType = (CourseType)comboBox.SelectedIndex;
        }
    }
}

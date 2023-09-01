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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace naic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// Settings window
        public SettingsWindow SettingsWindow;

        /// Generator job list window
        public JobListWindow JobListWindow;

        public ObservableCollection<Provider> Providers { get; set; }

        /// Stores the default selected provider
        /// Does not necessarily reflect the current
        /// selected provider
        public Provider SelectedProvider { get; set; }

        public static readonly DependencyProperty CourseProperty = DependencyProperty.Register("Course", typeof(Course), typeof(MainWindow));

        public Course Course
        {
            get
            {
                return (Course)GetValue(CourseProperty);
            }
            set
            {
                SetValue(CourseProperty, value);
            }
        }

        public MainWindow()
        {
            // Initialize component
            InitializeComponent();

            // Create provider list
            this.Providers = new ObservableCollection<Provider>();

            // Create blank course
            this.Course = new Course();

            // Settings window will be
            // created the first time it
            // is needed
            this.SettingsWindow = null;

            // Job list window will be created
            // the first time it is needed
            this.JobListWindow = new JobListWindow();

            // Set default course date
            this.Course.Date = null;
            this.Course.ApprovalDate = null;
            this.Course.ExpirationDate = null;

            // Set data contexts
            this.UpdateDataContexts();

            // Load provider information
            this.LoadProviders();

            // Set provider combo box according
            // to user settings

            // By default, get last used
            // provider ID
            Guid providerID = Properties.Settings.Default.lastUsedProviderID;

            // Make sure we should use
            // last used provider
            if (Properties.Settings.Default.useLastUsedProvider == false)
            {
                // We should not be, use
                // specified provider ID
                // instead
                providerID = Properties.Settings.Default.defaultProviderID;
            }

            Provider selectedProvider = null;

            // Iterate through each provider
            foreach (Provider pro in this.Providers)
            {
                // See if provider ID matches
                // specified provider ID
                if (pro.ID == providerID)
                {
                    // It does
                    // Select provider and
                    // stop looping.
                    selectedProvider = pro;
                    break;
                }
            }

            // See if the selected provider
            // actually exists
            if (selectedProvider == null)
            {
                // It does not.
                // Select the first item
                // instead.
                if (this.Providers.Count > 0)
                {
                    this.SelectedProvider = this.Providers[0];
                }

            }
            else
            {
                // Selected provider
                // exists
                // Select it in the combo-
                // box
                this.SelectedProvider = selectedProvider;
            }
        }

        /**
        \brief
            Loads provider information.
        */
        private void LoadProviders()
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
            foreach (Provider pro in providers)
            {
                this.Providers.Add(pro);
            }

            
        }

        public void UpdateDataContexts()
        {
            this.tbApprovalNumber.DataContext = this;
            this.tbCreditsApprovedInsuranceAdjuster.DataContext = this;
            this.tbCreditsApprovedInsuranceEthics.DataContext = this;
            this.tbCreditsApprovedInsuranceGeneral.DataContext = this;
            this.tbCreditsApprovedInsuranceLaws.DataContext = this;
            this.tbCreditsApprovedInsuranceLifeHealth.DataContext = this;
            this.tbCreditsApprovedInsuranceOther.DataContext = this;
            this.tbCreditsApprovedInsurancePropertyCasualty.DataContext = this;
            this.tbCreditsApprovedSalesAdjuster.DataContext = this;
            this.tbCreditsApprovedSalesEthics.DataContext = this;
            this.tbCreditsApprovedSalesGeneral.DataContext = this;
            this.tbCreditsApprovedSalesLaws.DataContext = this;
            this.tbCreditsApprovedSalesLifeHealth.DataContext = this;
            this.tbCreditsApprovedSalesOther.DataContext = this;
            this.tbCreditsApprovedSalesPropertyCasualty.DataContext = this;
            this.tbCreditsRequestedInsuranceAdjuster.DataContext = this;
            this.tbCreditsRequestedInsuranceEthics.DataContext = this;
            this.tbCreditsRequestedInsuranceGeneral.DataContext = this;
            this.tbCreditsRequestedInsuranceLaws.DataContext = this;
            this.tbCreditsRequestedInsuranceLifeHealth.DataContext = this;
            this.tbCreditsRequestedInsuranceOther.DataContext = this;
            this.tbCreditsRequestedInsurancePropertyCasualty.DataContext = this;
            this.tbCreditsRequestedSalesAdjuster.DataContext = this;
            this.tbCreditsRequestedSalesEthics.DataContext = this;
            this.tbCreditsRequestedSalesGeneral.DataContext = this;
            this.tbCreditsRequestedSalesLaws.DataContext = this;
            this.tbCreditsRequestedSalesLifeHealth.DataContext = this;
            this.tbCreditsRequestedSalesOther.DataContext = this;
            this.tbCreditsRequestedSalesPropertyCasualty.DataContext = this;
            this.tbCreditsTimedRequestedInsuranceAdjuster.DataContext = this;
            this.tbCreditsTimedRequestedInsuranceEthics.DataContext = this;
            this.tbCreditsTimedRequestedInsuranceGeneral.DataContext = this;
            this.tbCreditsTimedRequestedInsuranceLaws.DataContext = this;
            this.tbCreditsTimedRequestedInsuranceLifeHealth.DataContext = this;
            this.tbCreditsTimedRequestedInsuranceOther.DataContext = this;
            this.tbCreditsTimedRequestedInsurancePropertyCasualty.DataContext = this;
            this.tbCreditsTimedRequestedSalesAdjuster.DataContext = this;
            this.tbCreditsTimedRequestedSalesEthics.DataContext = this;
            this.tbCreditsTimedRequestedSalesGeneral.DataContext = this;
            this.tbCreditsTimedRequestedSalesLaws.DataContext = this;
            this.tbCreditsTimedRequestedSalesLifeHealth.DataContext = this;
            this.tbCreditsTimedRequestedSalesOther.DataContext = this;
            this.tbCreditsTimedRequestedSalesPropertyCasualty.DataContext = this;
            this.tbDesignationType.DataContext = this;
            this.tbMethodDescription.DataContext = this;
            this.tbTitle.DataContext = this;
            this.tbWordCount.DataContext = this;
            this.tbWordCountTimed.DataContext = this;
            this.cbProviders.DataContext = this;
            this.cbDesignation.DataContext = this;
            this.cbDifficulty.DataContext = this;
            this.cbExam.DataContext = this;
            this.cbMethod.DataContext = this;
            this.cbPublic.DataContext = this;
            this.cbTimedCourseDiffers.DataContext = this;
            this.dpApprovalDate.DataContext = this;
            this.dpExpirationDate.DataContext = this;
            this.dpOfferingDate.DataContext = this;
        }

        private void OnSettingsButtonClick(object sender, RoutedEventArgs e)
        {
            bool shouldReloadSettings = true;

            // Make sure settings window
            // exists
            if(this.SettingsWindow == null)
            {
                // It does not. Create
                // a new one
                this.SettingsWindow = new SettingsWindow();

                // New settings window automatically
                // loads settings, so we do not
                // need to.
                shouldReloadSettings = false;
            }

            // See if we should reload
            // settings
            if(shouldReloadSettings == true)
            {
                // We should
                this.SettingsWindow.OnLoad(this, null);
            }

            // Show settings window and
            // attempt to bring it to
            // front
            this.SettingsWindow.Show();
            this.SettingsWindow.ReloadProviders();
            this.SettingsWindow.Activate();
        }

        private void OnChangeProvider(object sender, SelectionChangedEventArgs e)
        {
            // Represents the currently selected
            // provider
            Provider selectedProvider = null;

            // See if a provider is currently
            // selected
            if(e.AddedItems[0] != null)
            {
                // It is. Assign it is
                // selectedProvider
                selectedProvider = (Provider)e.AddedItems[0];
            }

            // Only proceed if selectedProvider
            // is not null
            if(selectedProvider == null)
            {
                // It is null
                // Stop execution
                return;
            }

            // Update course with
            // provider defaults
            
            // Only update title if
            // nothing has been entered
            if(this.tbTitle.Text == "")
            {
                this.Course.Title = selectedProvider.DefaultCourseTitle;
                this.tbTitle.Text = this.Course.Title;
            }

            // Update course type selection
            this.Course.Type = selectedProvider.DefaultCourseType;
            this.cbMethod.SelectedItem = this.Course.Type;

            // Update course difficulty selection
            this.Course.Difficulty = selectedProvider.DefaultCourseDifficulty;
            this.cbDifficulty.SelectedItem = this.Course.Difficulty;

            // Update course designation
            this.Course.IsDesignation = selectedProvider.DefaultIsDesignation;
            this.cbDesignation.IsChecked = this.Course.IsDesignation;

            // Update course availability
            this.Course.IsPublic = selectedProvider.DefaultIsPublic;
            this.cbPublic.IsChecked = this.Course.IsPublic;

            // Update course exam requirement
            this.Course.DoesRequireExam = selectedProvider.DefaultRequiresExam;
            this.cbExam.IsChecked = this.Course.DoesRequireExam;
        }

        /**
        \brief
            Called when the "Generate..." button
            is clicked.

            Creates a job manager for the
            given course and provider and
            shows a job list window with
            the resulting jobs.

        \param sender
            Object that sent event. Typically
            the "Generate..." button.

        \param e
            Event arguments
        */
        private void OnGenerateButtonClick(object sender, RoutedEventArgs e)
        {
            // Create a new job manager object
            JobManager jm = new JobManager();
            
            // Set job manager course
            jm.Course = this.Course;

            // Set job manager provider
            jm.Provider = (Provider)this.cbProviders.SelectedItem;

            // Populate job manager jobs
            // using course and provider
            jm.PopulateJobs();

            // Make sure job list window exists
            if(this.JobListWindow == null)
            {
                // It does not; create a 
                // new one
                this.JobListWindow = new JobListWindow();
            }

            // Set job list window's
            // job manager to our new
            // job manager
            this.JobListWindow.JobManager = jm;

            // Show job list window and
            // try to force it to the front
            this.JobListWindow.Show();
            this.JobListWindow.Activate();
        }

        /**
        \brief
            Called when an autoselect textbox
            gains focus.

            Its contents will automatically be
            selected.

        \param sender
            Event sender. Autoselect textbox.

        \param e
            Event arguments.
        */
        private void OnAutoselectTextboxFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            // Get text box
            TextBox autoselectBox = (TextBox)sender;

            // Only select text if tab
            // key is down
            if(e.KeyboardDevice.IsKeyDown(Key.Tab))
            {
                // It is
                // Select all text
                autoselectBox.SelectAll();
            }
            
        }

        private void OnAutoselectTextboxLostFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            // Get text box
            TextBox autoselectBox = (TextBox)sender;

            // Clear textbox selection
            autoselectBox.Select(0, 0);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Destroy windows
            if(this.JobListWindow != null)
            {
                this.JobListWindow.Close();
                this.JobListWindow = null;
            }

            if(this.SettingsWindow != null)
            {
                this.SettingsWindow.Close();
                this.SettingsWindow = null;
            }

            base.OnClosing(e);

            // Destroy windows
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}

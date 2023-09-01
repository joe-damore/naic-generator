using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.ComponentModel;

namespace naic
{
    /// <summary>
    /// Interaction logic for JobListWindow.xaml
    /// </summary>
    public partial class JobListWindow : Window
    {
        public static readonly DependencyProperty JobManagerProperty = DependencyProperty.Register("JobManager", typeof(JobManager), typeof(JobListWindow));

        public JobManager JobManager
        {
            get
            {
                return (JobManager)GetValue(JobManagerProperty);
            }
            set
            {
                SetValue(JobManagerProperty, value);
            }
        }
        public JobListWindow()
        {
            InitializeComponent();

            //  this.JobsStackPanel.DataContext = this;
            this.lvJobList.DataContext = this;
        }

        private void OnGenerateButtonClick(object sender, RoutedEventArgs e)
        {
            // Show error if no jobs selected
            bool hasJobs = false;

            // Determine if there are pending
            // jobs that are selected
            foreach(Job j in this.JobManager.Jobs)
            {
                if(j.Enabled == true)
                {
                    hasJobs = true;
                    break;
                }
            }

            // See if there are any jobs
            if(hasJobs == false)
            {
                // Nope. Tell them to get a job
                // and then stop executing.
                MessageBox.Show(
                    "No jobs are pending. Please select at least one job and try again.",
                    "No Pending Jobs",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            // Get output directory
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();

            dialog.IsFolderPicker = true;
            dialog.Multiselect = false;
            dialog.Title = "NAIC Output Folder";

            // Determine dialog directory
            // Use default output directory by
            // default
            string dialogDirectory = Properties.Settings.Default.defaultOutputFolder;

            // See if we should actually be using
            // the last used directory
            if(Properties.Settings.Default.useLastUsedOutputFolder == true)
            {
                // We should be
                dialogDirectory = Properties.Settings.Default.lastUsedOutputFolder;
            }

            // Make sure given directory actually
            // exists
            if(Directory.Exists(dialogDirectory) == true)
            {
                // It does
                // Set dialog directory
                dialog.DefaultDirectory = dialogDirectory;
            }

            CommonFileDialogResult result;
            result = dialog.ShowDialog(this);

            if(result == CommonFileDialogResult.Ok)
            {
                // Store selected provider's ID
                Properties.Settings.Default.lastUsedProviderID = this.JobManager.Provider.ID;
                Properties.Settings.Default.Save();

                // Selected a file
                // Let's output it boys
                string outputFolder = dialog.FileName;

                // Assign output folder
                this.JobManager.OutputFolder = outputFolder;

                // Process jobs
                this.JobManager.ProcessAllJobs();

                // Refresh job list to
                // reflect status changes
                this.lvJobList.Items.Refresh();

                MessageBox.Show(
                    "Successfully processed NAIC jobs.",
                    "Jobs Complete",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        /**
        \brief
            Called when the window is about
            to close.

            Forces the window to hide itself
            instead.

        \param e
            Event arguments
        */
        protected override void OnClosing(CancelEventArgs e)
        {
            // Cancel close action
            e.Cancel = true;

            // Hide window instead
            this.Hide();
        }
    }
}

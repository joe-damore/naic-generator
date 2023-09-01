using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace naic
{
    /// <summary>
    /// Interaction logic for SettingsWindowRegionsTabRegionDetail.xaml
    /// </summary>
    public partial class SettingsWindowRegionsTabRegionDetail : UserControl, INotifyPropertyChanged
    {
        // The event to be thrown when
        // an observable property changes
        public event PropertyChangedEventHandler PropertyChanged;

        /// Refers to the currently
        /// selected region
        private Region currentRegion;

        /// Accessor for currentRegion
        public Region CurrentRegion
        {
            get
            {
                return this.currentRegion;
            }
            set
            {
                // Assign new provider
                this.currentRegion = value;

                // Throw property changed
                // event
                PropertyChangedEventHandler handler = PropertyChanged;

                if (PropertyChanged != null)
                {
                    handler(this, new PropertyChangedEventArgs("CurrentRegion"));
                }
            }
        }

        public SettingsWindowRegionsTabRegionDetail()
        {
            InitializeComponent();

            this.tbRegionName.DataContext                 = this;
            this.tbOutputName.DataContext                 = this;
            this.tbProviderNumber.DataContext             = this;
            this.tbRegionAbbreviation.DataContext         = this;
            this.tbCourseTitleAppendix.DataContext        = this;
            this.cbIsTimed.DataContext                    = this;
            this.lbRegionTitle.DataContext                = this;
            this.tbOverrideProviderName.DataContext       = this;
            this.tbOverrideContactName.DataContext        = this;
            this.tbOverrideContactEmail.DataContext       = this;
            this.tbOverrideContactPhone.DataContext       = this;
            this.tbOverrideContactFax.DataContext         = this;
            this.tbMaxCredits.DataContext                 = this;
            this.tbMaxCreditsPerLine.DataContext          = this;
            this.cbDoesOverrideDifficulty.DataContext     = this;
            this.cbCreditRequirementCondition.DataContext = this;
            this.cbOverrideDifficulty.DataContext         = this;
            this.tbCustomTemplatePath.DataContext         = this;
            this.cbUsesCustomTemplate.DataContext         = this;
            this.RequirementsStackPanel.DataContext       = this;
            this.TransformationsStackPanel.DataContext    = this;
        }

        /**
        \brief
            Adds a transformation to the
            current region's transformation
            list.

        \param index
            Index to insert transformation
            at.

        \param transformation
            Transformation to add. If not specified,
            a default transformation is added
        */
        public void AddTransformation(
            int                  index          = 0   ,
            CreditTransformation transformation = null)
        {
            // Get transformation
            CreditTransformation trans = transformation;

            // Make sure it is not null
            if(trans == null)
            {
                // It is
                // Create default transformation
                trans = new CreditTransformation();

                trans.Action = TransformationAction.Move;
                trans.Amount = TransformationAmount.All;
                trans.SourceType = CreditType.LifeHealth;
                trans.Destination = TransformationDestination.Over;
                trans.DestinationType = CreditType.LifeHealth;
            }

            // Make sure we have a current region
            if(this.currentRegion == null)
            {
                // We do not.
                // Stop executing.
                return;
            }

            // Insert new transformation
            this.CurrentRegion.CreditTransformations.Insert(index, trans);
        }

        /**
        \brief
            Removes the given transformation
            from the current region

        \param transformation
            Transformation to remove
        */
        public void RemoveTransformation(CreditTransformation transformation)
        {
            // Make sure we have a currently
            // selected region
            if(this.CurrentRegion == null)
            {
                // We do not.
                // Stop executing.
                return;
            }

            // Make sure the given requirement
            // actually exists in the region
            if(this.CurrentRegion.CreditTransformations.Contains(transformation))
            {
                // It does.
                // Remove it.
                this.CurrentRegion.CreditTransformations.Remove(transformation);
            }
        }

        /**
        \brief
            Adds a requirement to the
            current region's requirement
            list.

        \param index
            Index to add region to.

        \param requirement
            Requirement to add to list. If not
            specified, a default requirement
            is added.
        */
        public void AddRequirement(
            int               index       = 0   ,
            CreditRequirement requirement = null
            )
        {
            CreditRequirement req = requirement;

            if(req == null)
            {
                req = new CreditRequirement();

                // Default type Life/Health
                req.Type = CreditType.LifeHealth;

                // Default condition
                req.Condition = RequirementCondition.GreaterThan;

                // Default amount
                req.Amount = 0;
            }

            // Make sure we have a current
            // region
            if(this.CurrentRegion == null)
            {
                // We do not.
                // Stop executing.
                return;
            }

            // Insert requirement
            this.CurrentRegion.CreditRequirements.Insert(index, req);
        }

        /**
        \brief
            Removes the given requirement from
            the currently selected region

        \param requirement
            Requirement to remove
        */
        public void RemoveRequirement(CreditRequirement requirement)
        {
            // Make sure we have a currently
            // selected region
            if(this.CurrentRegion == null)
            {
                // We do not.
                // Stop executing.
                return;
            }

            // Make sure the given requirement
            // actually exists in the region
            if(this.CurrentRegion.CreditRequirements.Contains(requirement))
            {
                // It does, so remove it
                this.CurrentRegion.CreditRequirements.Remove(requirement);
            }
        }

        /**
        \brief
            Called when the user right-clicks
            on a credit requirement and selects
            "Delete Requirement"

            Confirms the user's intention and
            then removes the requirement
            from the region

        \param sender
            Object that sent command.
            Should be the context menu item
            labeled "Delete Region"

        \param e
            Event arguments
        */
        private void ClickRemoveRequirement(object sender, RoutedEventArgs e)
        {
            // Get the menu item that was
            // clicked
            MenuItem menu = (MenuItem)sender;

            // This will refer to the control
            // that was right-clicked
            RegionCreditRequirementsControl control = null;

            // Make sure menu item exists
            if (menu != null)
            {
                control = (RegionCreditRequirementsControl)(((ContextMenu)menu.Parent).PlacementTarget);
            }

            // Make sure control exists
            if(control == null)
            {
                // It doesn't
                return;
            }

            // Get user confirmation
            MessageBoxResult result;

            // Show confirmation dialog
            result = MessageBox.Show(
                "Are you sure you want to delete this credit requirement?",
                "Delete Credit Requirement",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            // Stop executing if they hit no
            if (result == MessageBoxResult.No)
            {
                return;
            }

            // Get credit requirement
            // for control
            CreditRequirement requirement = control.Requirement;

            // Remove requirement
            this.RemoveRequirement(requirement);
        }

        /**
        \brief
            Called when the user right-clicks
            on a credit transformation and
            selected "Delete Transformation"

            Confirms the user's intention and
            then removes the transformation from
            the region

        \param sender
            Object that sent command.
            Should be the context menu item
            labeled "Delete Transformation"

        \param e
            Event arguments
        */
        private void ClickRemoveTransformation(object sender, RoutedEventArgs e)
        {
            // Get menu item that
            // was clicked
            MenuItem menu = (MenuItem)sender;

            // This will refer to the control
            // that was right-clicked
            RegionCreditTransformationsControl control = null;

            // Make sure menu item
            // exists
            if(menu != null)
            {
                // Get transformation control
                control = (RegionCreditTransformationsControl)(((ContextMenu)menu.Parent).PlacementTarget);
            }

            // Make sure control actually
            // exists
            if(control == null)
            {
                // It does not.
                // Stop executing.
                return;
            }

            // Get user confirmation
            MessageBoxResult result;

            // Show confirmation dialog
            result = MessageBox.Show(
                "Are you sure you want to delete this credit transformation?",
                "Delete Credit Transformation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            // Stop executing if they hit no
            if (result == MessageBoxResult.No)
            {
                return;
            }

            // Get transformation
            CreditTransformation transformation = control.Transformation;

            // Remove transformation
            this.RemoveTransformation(transformation);
        }

        /**
        \brief
            Inserts a transformation
            after the selected transformation
        */
        private void ClickInsertTransformation(object sender, RoutedEventArgs e)
        {
            // Get menu item that
            // was clicked
            MenuItem menu = (MenuItem)sender;

            // This will refer to the control
            // that was right-clicked
            RegionCreditTransformationsControl control = null;

            // Make sure menu item
            // exists
            if (menu != null)
            {
                // Get transformation control
                control = (RegionCreditTransformationsControl)(((ContextMenu)menu.Parent).PlacementTarget);
            }

            // Make sure control actually
            // exists
            if (control == null)
            {
                // It does not.
                // Stop executing.
                return;
            }

            // Get right-clicked transformation
            CreditTransformation transformation = control.Transformation;

            // Refers to the index we will
            // be inserting transformation at
            int index = 0;

            // Make sure transformation exists
            if(this.CurrentRegion.CreditTransformations.Contains(transformation))
            {
                // Get index of transformation
                index = this.CurrentRegion.CreditTransformations.IndexOf(transformation) + 1;
            }

            // Add the transformation
            this.AddTransformation(index);
        }

        private void ClickAddTransformation(object sender, RoutedEventArgs e)
        {
            // Get index
            int index = this.CurrentRegion.CreditTransformations.Count;

            // Add the transformation
            this.AddTransformation(index);
        }

        private void ClickInsertRequirement(object sender, RoutedEventArgs e)
        {
            // Get menu item that
            // was clicked
            MenuItem menu = (MenuItem)sender;

            // This will refer to the control
            // that was right-clicked
            RegionCreditRequirementsControl control = null;

            // Make sure menu item
            // exists
            if (menu != null)
            {
                // Get transformation control
                control = (RegionCreditRequirementsControl)(((ContextMenu)menu.Parent).PlacementTarget);
            }

            // Make sure control actually
            // exists
            if (control == null)
            {
                // It does not.
                // Stop executing.
                return;
            }

            // Get right-clicked transformation
            CreditRequirement requirement = control.Requirement;

            // Refers to the index we will
            // be inserting transformation at
            int index = 0;

            // Make sure transformation exists
            if (this.CurrentRegion.CreditRequirements.Contains(requirement))
            {
                // Get index of transformation
                index = this.CurrentRegion.CreditRequirements.IndexOf(requirement) + 1;
            }

            // Add the transformation
            this.AddRequirement(index);
        }

        /**
        \brief
            Called when the "New Requirement" menu item
            is clicked, but not when another
            requirement is right clicked on.
        */
        private void ClickAddRequirement(object sender, RoutedEventArgs e)
        {
            // Get index
            int index = this.CurrentRegion.CreditRequirements.Count;

            // Add the requirement
            this.AddRequirement(index);
        }

        private void ClickBrowseTemplate(object sender, RoutedEventArgs e)
        {
            // Create an open file dialog
            // that only permits the selection
            // of .DOCX files
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.DefaultExt = ".docx";
            dialog.Filter = "DOCX Files (*.docx)|*.docx";

            // Display dialog and get
            // result
            Nullable<bool> result = dialog.ShowDialog();

            // Refers to the selected file
            string filename = "";


            // See if a file was
            // selected
            if (result == true)
            {
                // One was, so get its
                // filename and assign
                // it to the textbox
                filename = dialog.FileName;
                this.CurrentRegion.CustomTemplatePath = filename;
                tbCustomTemplatePath.Text = filename;
            }
        }
    }
}

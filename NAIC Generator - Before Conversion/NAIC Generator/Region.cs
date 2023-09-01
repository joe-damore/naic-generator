using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naic
{
    /**
    \brief
        Describes a region that an NAIC
        form can be generated for
    */
    public class Region
    {
        /// Region name
        /// Ex. "Pennsylvania"
        public string Name { get; set; }

        /// Region abbreviation
        /// Ex. "PA"
        public string Abbreviation { get; set; }

        /// Provider number for
        /// region
        public string ProviderNumber { get; set; }

        /// File name for NAIC forms
        /// generated for this region
        public string OutputName { get; set; }

        /// String to be appended to the
        /// course titles of all courses
        /// in this region
        public string CourseTitleAppendix { get; set; }

        /// Determines if this course
        /// is timed, and if so will
        /// use the timed word count
        public bool TimedCourse { get; set; }

        /// Provider name to use for courses
        /// in this region.
        /// Ignored if string is empty.
        public string OverrideProviderName { get; set; }

        /// Contact name to use for courses
        /// in this region.
        /// Ignored if string is empty.
        public string OverrideContactName { get; set; }

        /// Contact phone number to use for
        /// courses in this region.
        /// Ignored if string is empty.
        public string OverrideContactPhone { get; set; }

        /// Contact fax number to use for
        /// courses in this region.
        /// Ignored if string is empty.
        public string OverrideContactFax { get; set; }

        /// Contact email address to use
        /// for courses in this region.
        /// Ignored if string is empty.
        public string OverrideContactEmail { get; set; }

        /// Determines whether or not the course
        /// difficulty of courses in this region
        /// should be overridden
        public bool DoesOverrideCourseDifficulty { get; set; }

        /// Course difficulty to use if
        /// DoesOverrideCourseDifficulty is true
        public CourseDifficulty OverrideCourseDifficulty { get; set; }

        /// Describes how credits should
        /// be applied in this region
        public ObservableCollection<CreditTransformation> CreditTransformations { get; set; }

        /// Describes requirements that must
        /// be met in order for an NAIC
        /// form to be generated for this
        /// region
        public ObservableCollection<CreditRequirement> CreditRequirements { get; set; }

        /// Determines how many conditions must be
        /// met in order for requirements to be
        /// satisfied
        public CourseCreditRequirementCondition RequirementConditions { get; set; }

        /// Determines whether or not the
        /// credit transformation list contains
        /// any transformations
        public bool HasTransformations
        {
            get
            {
                return (this.CreditTransformations.Count > 0);
            }
        }

        /// Determines whether or not the
        /// credit requirements list contains
        /// any transformations
        public bool HasRequirements
        {
            get
            {
                return (this.CreditRequirements.Count > 0);
            }
        }

        /// Determines the minimum number
        /// of credits that can be filed
        /// for each line
        public ushort MinimumCreditsPerLine { get; set; }

        /// Determines the maximum number
        /// of credits that can be filed
        /// for each line
        public ushort MaximumCreditsPerLine { get; set; }

        /// Determines the maximum number
        /// of total credits that a course
        /// can be filed for
        public ushort MaximumCredits { get; set; }

        /// Determines whether NAIC files for
        /// this region use a custom template
        public bool DoesUseCustomTemplate { get; set; }

        /// Determines the path to the region's
        /// custom NAIC template if it has one
        public string CustomTemplatePath { get; set; }

        public Region()
        {
            // Create list for credit
            // requirements
            this.CreditRequirements = new ObservableCollection<CreditRequirement>();

            // Create list for credit
            // transformations
            this.CreditTransformations = new ObservableCollection<CreditTransformation>();
        }

        /**
        \brief
            Constructor.

            Creates a copy of the given
            region.

        \param copyRegion
            Reigon to be copied.
        */
        public Region(Region copyRegion)
        {
            // Assign values from copyRegion
            this.Abbreviation                 = copyRegion.Abbreviation;
            this.CourseTitleAppendix          = copyRegion.CourseTitleAppendix;
            this.CreditRequirements           = new ObservableCollection<CreditRequirement>();
            this.CreditTransformations        = new ObservableCollection<CreditTransformation>();
            this.CustomTemplatePath           = copyRegion.CustomTemplatePath;
            this.DoesOverrideCourseDifficulty = copyRegion.DoesOverrideCourseDifficulty;
            this.DoesUseCustomTemplate        = copyRegion.DoesUseCustomTemplate;
            this.MaximumCredits               = copyRegion.MaximumCredits;
            this.MaximumCreditsPerLine        = copyRegion.MaximumCreditsPerLine;
            this.MinimumCreditsPerLine        = copyRegion.MinimumCreditsPerLine;
            this.Name                         = copyRegion.Name;
            this.OutputName                   = copyRegion.OutputName;
            this.OverrideContactEmail         = copyRegion.OverrideContactEmail;
            this.OverrideContactFax           = copyRegion.OverrideContactFax;
            this.OverrideContactName          = copyRegion.OverrideContactName;
            this.OverrideContactPhone         = copyRegion.OverrideContactPhone;
            this.OverrideCourseDifficulty     = copyRegion.OverrideCourseDifficulty;
            this.OverrideProviderName         = copyRegion.OverrideProviderName;
            this.ProviderNumber               = copyRegion.ProviderNumber;
            this.RequirementConditions        = copyRegion.RequirementConditions;
            this.TimedCourse                  = copyRegion.TimedCourse;

            // Manually copy items from
            // credit transformations and
            // requirements array to this
            // new region

            // Iterate through each requirement
            foreach(CreditRequirement requirement in copyRegion.CreditRequirements)
            {
                // Create a new requirement
                CreditRequirement newRequirement = new CreditRequirement();

                // Copy requirement details from
                // old requirement
                newRequirement.Type = requirement.Type;
                newRequirement.Amount = requirement.Amount;
                newRequirement.Condition = requirement.Condition;

                // Add requirement to new region
                this.CreditRequirements.Add(newRequirement);
            }

            // Iterate through each transformation
            foreach(CreditTransformation transformation in copyRegion.CreditTransformations)
            {
                // Create a new transformation
                CreditTransformation newTransformation = new CreditTransformation();

                // Copy transformation details from old
                // transformation
                newTransformation.Action = transformation.Action;
                newTransformation.Amount = transformation.Amount;
                newTransformation.Destination = transformation.Destination;
                newTransformation.DestinationType = transformation.DestinationType;
                newTransformation.SourceType = transformation.SourceType;

                // Add transformation to new region
                this.CreditTransformations.Add(newTransformation);
            }



        }
    }
}

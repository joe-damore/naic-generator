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
        Describes a continuing
        education provider.
    */
    public class Provider
    {
        /// Unique ID for provider
        /// Used when referring to specific
        /// providers, such as when choosing
        /// the default provider
        public Guid ID { get; set; }

        /// Provider name
        public string Name { get; set; }

        /// Provider FEIN
        public string FEIN { get; set; }

        /// Provider contact name
        public string ContactName { get; set; }

        /// Provider contact email address
        public string ContactEmail { get; set; }

        /// Provider contact phone number
        public string ContactPhone { get; set; }

        /// Provider contact fax number
        public string ContactFax { get; set; }

        /// Provider mailing address street
        /// address
        public string MailingStreetAddress { get; set; }

        /// Provider mailing address city
        public string MailingCity { get; set; }

        /// Provider mailing address state
        public string MailingState { get; set; }

        /// Provider mailing address zip
        public string MailingZip { get; set; }

        /// Provider home state abbreviation
        public string HomeState { get; set; }

        /// Provider home state provider number
        public string HomeStateProviderNumber { get; set; }

        /// Determines the default course title
        /// for courses filed by this provider
        public string DefaultCourseTitle { get; set; }

        /// Determines the default course type
        /// for courses filed by this provider
        public CourseType DefaultCourseType { get; set; }

        /// Determines the default course difficulty
        /// for courses filed by this provider
        public CourseDifficulty DefaultCourseDifficulty { get; set; }

        /// Determines whether or not courses filed by
        /// this provider are public or not by default
        public bool DefaultIsPublic { get; set; }

        /// Determines whether or not courses filed by
        /// this provider are for national designations
        /// or not by default
        public bool DefaultIsDesignation { get; set; }

        /// Determines whether or not courses filed by
        /// this provider require an exam by default
        public bool DefaultRequiresExam { get; set; }

        /// List of regions handled by this
        /// provider
        public ObservableCollection<Region> Regions { get; set; }

        public Provider()
        {
            // Create list for regions
            this.Regions = new ObservableCollection<Region>();

            this.ID = Guid.NewGuid();
        }
    }
}

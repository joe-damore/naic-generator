using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naic
{
    /**
    \brief
        Contains information describing
        a course
    */
    public class Course
    {
        /// Course title
        public string Title { get; set; }

        /// Course offering date
        public DateTime? Date { get; set; }

        /// Course type
        public CourseType Type { get; set; }

        /// Course description if course
        /// type is Classroom/Other
        public string OtherDescription { get; set; }

        /// Determines whether this course is a part
        /// of a national designation
        public bool IsDesignation { get; set; }

        /// Designation that this course is for,
        /// if IsDesignation is true
        public string Designation { get; set; }

        /// Determines whether or not this course
        /// is public
        public bool IsPublic { get; set; }

        /// Determines whether or not this course
        /// requires an exam
        public bool DoesRequireExam { get; set; }

        /// Determines the word count for this course
        public uint WordCount { get; set; }

        /// Determines the word count for timed variants
        /// of this course
        public uint WordCountTimed { get; set; }

        /// Determines whether or not the credit amounts differ
        /// for states using a timed outline. If true, the
        /// user is able to specify alternate credit amounts
        /// for timed variations of the course.
        public bool TimedCreditsDiffer { get; set; }

        /// Determines course difficulty
        public CourseDifficulty Difficulty { get; set; }

        public ushort RequestedSalesCreditsLifeHealth { get; set; }

        public ushort RequestedSalesCreditsPropertyCasualty { get; set; }

        public ushort RequestedSalesCreditsEthics { get; set; }

        public ushort RequestedSalesCreditsGeneral { get; set; }

        public ushort RequestedSalesCreditsLaws { get; set; }

        public ushort RequestedSalesCreditsOther { get; set; }

        public ushort RequestedSalesCreditsAdjuster { get; set; }

        public ushort RequestedInsuranceCreditsLifeHealth { get; set; }

        public ushort RequestedInsuranceCreditsPropertyCasualty { get; set; }

        public ushort RequestedInsuranceCreditsEthics { get; set; }

        public ushort RequestedInsuranceCreditsGeneral { get; set; }

        public ushort RequestedInsuranceCreditsLaws { get; set; }

        public ushort RequestedInsuranceCreditsOther { get; set; }

        public ushort RequestedInsuranceCreditsAdjuster { get; set; }

        public ushort RequestedTimedSalesCreditsLifeHealth { get; set; }

        public ushort RequestedTimedSalesCreditsPropertyCasualty { get; set; }

        public ushort RequestedTimedSalesCreditsEthics { get; set; }

        public ushort RequestedTimedSalesCreditsGeneral { get; set; }

        public ushort RequestedTimedSalesCreditsLaws { get; set; }

        public ushort RequestedTimedSalesCreditsOther { get; set; }

        public ushort RequestedTimedSalesCreditsAdjuster { get; set; }

        public ushort RequestedTimedInsuranceCreditsLifeHealth { get; set; }

        public ushort RequestedTimedInsuranceCreditsPropertyCasualty { get; set; }

        public ushort RequestedTimedInsuranceCreditsEthics { get; set; }

        public ushort RequestedTimedInsuranceCreditsGeneral { get; set; }

        public ushort RequestedTimedInsuranceCreditsLaws { get; set; }

        public ushort RequestedTimedInsuranceCreditsOther { get; set; }

        public ushort RequestedTimedInsuranceCreditsAdjuster { get; set; }

        public ushort ApprovedSalesCreditsLifeHealth { get; set; }

        public ushort ApprovedSalesCreditsPropertyCasualty { get; set; }

        public ushort ApprovedSalesCreditsEthics { get; set; }

        public ushort ApprovedSalesCreditsGeneral { get; set; }

        public ushort ApprovedSalesCreditsLaws { get; set; }

        public ushort ApprovedSalesCreditsOther { get; set; }

        public ushort ApprovedSalesCreditsAdjuster { get; set; }

        public ushort ApprovedInsuranceCreditsLifeHealth { get; set; }

        public ushort ApprovedInsuranceCreditsPropertyCasualty { get; set; }

        public ushort ApprovedInsuranceCreditsEthics { get; set; }

        public ushort ApprovedInsuranceCreditsGeneral { get; set; }

        public ushort ApprovedInsuranceCreditsLaws { get; set; }

        public ushort ApprovedInsuranceCreditsOther { get; set; }

        public ushort ApprovedInsuranceCreditsAdjuster { get; set; }

        /// Date that course was approved by
        /// home state
        public DateTime? ApprovalDate { get; set; }

        /// Course approval number
        public string ApprovalNumber { get; set; }

        /// Date that course will expire in
        /// home state
        public DateTime? ExpirationDate { get; set; }

        /// Course number assigned by state
        public string CourseNumber { get; set; }

        public Course()
        {
            this.ApprovalDate = new DateTime();
            this.ExpirationDate = new DateTime();
            this.Date = new DateTime();
        }

        /// Returns true only if Type is
        /// ClassroomOther
        public bool IsOtherType
        {
            get
            {
                // Return true if this course type is
                // Classroom/Other
                return (this.Type == CourseType.ClassroomOther);
            }
        }
    }
}

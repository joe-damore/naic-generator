using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace naic
{
    /**
    \brief
        Stores all the information required to generate
        a single NAIC document, and provides mechanisms
        to generate the document
    */
    public class Job
    {
        // Job metadata

        /// Job meta title
        public string MetaTitle { get; set; } = "";

        /// Job meta region
        public string MetaRegion { get; set; } = "";

        private string metaOutput = "";

        /// Job meta output location
        /// Includes ".docx" extension
        public string MetaOutput
        {
            get
            {
                return this.metaOutput + ".docx";
            }
            set
            {
                this.metaOutput = value;
            }
        }

        /// Job status
        /// Typically either "Pending", "Success",
        /// or "Failure"
        public string MetaStatus
        {
            get
            {
                if(this.Complete == true)
                {
                    if(this.Failure == true)
                    {
                        return "Failure";
                    }
                    return "Complete";
                }
                return "Pending";
            }
        }

        /// Determines whether this job is
        /// enabled in the job selector panel
        public bool Enabled { get; set; }

        /// Determines whether this job should
        /// display as a warning
        public bool Warning { get; set; }

        /// Determines whether or not this job
        /// has been manually edited (ie using
        /// the job editor window)
        public bool Edited { get; set; }

        /// Determines whether or not this job
        /// has been completed
        public bool Complete { get; set; }

        /// Determines whether or not this job
        /// failed to process correctly
        public bool Failure { get; set; }

        /// String describing job warning
        public string WarningString { get; set; }

        /// File path of template file for job
        public string InputPath { get; set; }

        // Generator details

        /// Determines if existing NAIC forms
        /// should be replaced by generator
        public bool ReplaceFiles = true;

        /// Determines if output NAIC file should
        /// still have NAIC forms
        public bool PreserveContentControls = false;

        /// Determines if the provider name should
        /// be rendered with a smaller font
        public bool SmallProviderName = false;

        /// Determines if the provider number should
        /// be rendered with a smaller font
        public bool SmallProviderNumber = false;

        /// Determines if the course title should be
        /// rendered with a smaller font
        public bool SmallCourseTitle = false;

        // Course information

        /// Provider name
        public string ProviderName { get; set; } = "";

        /// Provider FEIN
        public string ProviderFEIN { get; set; } = "";

        /// Provider home state
        public string ProviderHomeState { get; set; } = "";

        /// Provider home state provider number
        public string ProviderHomeStateNumber { get; set; } = "";

        /// Provider reciprocal state
        public string ProviderReciprocalState { get; set; } = "";

        /// Provider reciprocal state provider number
        public string ProviderReciprocalStateNumber { get; set; } = "";

        /// Provider contact person
        public string ProviderContact { get; set; } = "";

        /// Provider contact email address
        public string ProviderEmail { get; set; } = "";

        /// Provier contact phone number
        public string ProviderPhone { get; set; } = "";

        /// Provider contact fax number
        public string ProviderFax { get; set; } = "";

        /// Provider mailing address street address
        public string ProviderMailingStreetAddress { get; set; } = "";

        /// Provider mailing address city
        public string ProviderMailingCity { get; set; } = "";

        /// Provider mailing address state
        public string ProviderMailingState { get; set; } = "";

        /// Provider mailing address zip/postal code
        public string ProviderMailingZip { get; set; } = "";

        public string CourseTitle { get; set; } = "";

        public DateTime? CourseDate { get; set; } = new DateTime();

        public CourseType CourseType { get; set; } = CourseType.None;

        public uint CourseWordCount { get; set; } = 0;

        public CourseDifficulty CourseDifficulty = CourseDifficulty.None;

        public string CourseMethodDescription = "";

        public bool CourseIsDesignation = false;

        public string CourseDesignationType = "";

        public bool CourseRequiresExam = false;

        public bool CourseIsPublic = false;

        public ushort RequestedSalesCreditsLifeHealth { get; set; } = 0;

        public ushort RequestedSalesCreditsPropertyCasualty { get; set; } = 0;

        public ushort RequestedSalesCreditsEthics { get; set; } = 0;

        public ushort RequestedSalesCreditsGeneral { get; set; } = 0;

        public ushort RequestedSalesCreditsLaws { get; set; } = 0;

        public ushort RequestedSalesCreditsOther { get; set; } = 0;

        public ushort RequestedSalesCreditsAdjuster { get; set; } = 0;

        public ushort RequestedInsuranceCreditsLifeHealth { get; set; } = 0;

        public ushort RequestedInsuranceCreditsPropertyCasualty { get; set; } = 0;

        public ushort RequestedInsuranceCreditsEthics { get; set; } = 0;

        public ushort RequestedInsuranceCreditsGeneral { get; set; } = 0;

        public ushort RequestedInsuranceCreditsLaws { get; set; } = 0;

        public ushort RequestedInsuranceCreditsOther { get; set; } = 0;

        public ushort RequestedInsuranceCreditsAdjuster { get; set; } = 0;

        public ushort ApprovedSalesCreditsLifeHealth { get; set; } = 0;

        public ushort ApprovedSalesCreditsPropertyCasualty { get; set; } = 0;

        public ushort ApprovedSalesCreditsEthics { get; set; } = 0;

        public ushort ApprovedSalesCreditsGeneral { get; set; } = 0;

        public ushort ApprovedSalesCreditsLaws { get; set; } = 0;

        public ushort ApprovedSalesCreditsOther { get; set; } = 0;

        public ushort ApprovedSalesCreditsAdjuster { get; set; } = 0;

        public ushort ApprovedInsuranceCreditsLifeHealth { get; set; } = 0;

        public ushort ApprovedInsuranceCreditsPropertyCasualty { get; set; } = 0;

        public ushort ApprovedInsuranceCreditsEthics { get; set; } = 0;

        public ushort ApprovedInsuranceCreditsGeneral { get; set; } = 0;

        public ushort ApprovedInsuranceCreditsLaws { get; set; } = 0;

        public ushort ApprovedInsuranceCreditsOther { get; set; } = 0;

        public ushort ApprovedInsuranceCreditsAdjuster { get; set; } = 0;

        public DateTime? CourseHomeStateApprovalDate { get; set; } = new DateTime();

        public DateTime? CourseHomeStateApprovalExpiration { get; set; } = new DateTime();

        public string CourseHomeStateApprovalNumber = "";


        /**
        \brief
            Escapes string to make it compatible
            as a command line argument.

            See stack oferflow
            https://stackoverflow.com/questions/5510343
            for more information
        */
        private string Escape(string inString)
        {
            if(inString == null)
            {
                inString = "";
            }
            string s = inString;
            s = "\"" + Regex.Replace(s, @"(\\+)$", @"$1$1") + "\"";

            return s;
        }

        /**
        \brief
            Executes the job and returns
            true on success or false
            on failure
        
        \param generatorPath
            Path to NAIC generator file
            (naicgen.exe)

        \param outputPath
            File path for NAIC output

        \return
            True on success, false on
            failure
        */
        public bool Execute(string generatorPath, string outputPath)
        {
            if(this.Enabled == false)
            {
                // This job is not enabled,
                // so it cannot be executed
                // Stop execution
                this.Complete = false;
                this.Failure = false;
                return false;
            }

            // Refers to command line arguments
            string cl = "";

            // Specify input and output first
            cl += " --Input " + this.Escape(this.InputPath);
            cl += " --Output " + this.Escape(outputPath);

            // Provider information
            cl += " -PNM " + this.Escape(this.ProviderName);
            cl += " -PFN " + this.Escape(this.ProviderFEIN);
            cl += " -PHS " + this.Escape(this.ProviderHomeState);
            cl += " -PHN " + this.Escape(this.ProviderHomeStateNumber);
            cl += " -PRS " + this.Escape(this.ProviderReciprocalState);
            cl += " -PRN " + this.Escape(this.ProviderReciprocalStateNumber);

            // Contact information
            cl += " -CN " + this.Escape(this.ProviderContact);
            cl += " -CE " + this.Escape(this.ProviderEmail);
            cl += " -CP " + this.Escape(this.ProviderPhone);
            cl += " -CF " + this.Escape(this.ProviderFax);
            cl += " -MA " + this.Escape(this.ProviderMailingStreetAddress);
            cl += " -MC " + this.Escape(this.ProviderMailingCity);
            cl += " -MS " + this.Escape(this.ProviderMailingState);
            cl += " -MZ " + this.Escape(this.ProviderMailingZip);

            // Course information
            cl += " -CT " + this.Escape(this.CourseTitle);
            if(this.CourseDate.HasValue == true)
            {
                cl += " -CO " + this.Escape(this.CourseDate.Value.ToString("d"));
            }
            cl += " -CM " + (int)this.CourseType;
            cl += " -CWC " + this.CourseWordCount;
            cl += " -CD " + (int)this.CourseDifficulty;
            cl += " -CND " + Convert.ToInt32(this.CourseIsDesignation);
            cl += " -CIP " + Convert.ToInt32(this.CourseIsPublic);
            cl += " -CHE " + Convert.ToInt32(this.CourseRequiresExam);

            // Course approval information
            if (this.CourseHomeStateApprovalDate.HasValue == true)
            {
                cl += " -AD " + this.Escape(this.CourseHomeStateApprovalDate.Value.ToString("d"));
            }
            // Add approval expiration date
            // if value was specified
            if(this.CourseHomeStateApprovalExpiration.HasValue == true)
            {
                cl += " -AE " + this.Escape(this.CourseHomeStateApprovalExpiration.Value.ToString("d"));
            }
            cl += " -AN " + this.Escape(this.CourseHomeStateApprovalNumber);

            // Calculate total credits for
            // sales requested/approved and
            // insurance requested/approved
            int totalRequestedSales, totalRequestedInsurance;
            int totalApprovedSales, totalApprovedInsurance;

            totalRequestedSales = (
                this.RequestedSalesCreditsLifeHealth +
                this.RequestedSalesCreditsPropertyCasualty +
                this.RequestedSalesCreditsEthics +
                this.RequestedSalesCreditsGeneral +
                this.RequestedSalesCreditsLaws +
                this.RequestedSalesCreditsOther
                );

            totalRequestedInsurance = (
                this.RequestedInsuranceCreditsLifeHealth +
                this.RequestedInsuranceCreditsPropertyCasualty +
                this.RequestedInsuranceCreditsEthics +
                this.RequestedInsuranceCreditsGeneral +
                this.RequestedInsuranceCreditsLaws +
                this.RequestedInsuranceCreditsOther
                );

            totalApprovedSales = (
                this.ApprovedSalesCreditsLifeHealth +
                this.ApprovedSalesCreditsPropertyCasualty +
                this.ApprovedSalesCreditsEthics +
                this.ApprovedSalesCreditsGeneral +
                this.ApprovedSalesCreditsLaws +
                this.ApprovedSalesCreditsOther
                );

            totalApprovedInsurance = (
                this.ApprovedInsuranceCreditsLifeHealth +
                this.ApprovedInsuranceCreditsPropertyCasualty +
                this.ApprovedInsuranceCreditsEthics +
                this.ApprovedInsuranceCreditsGeneral +
                this.ApprovedInsuranceCreditsLaws +
                this.ApprovedInsuranceCreditsOther
                );

            // Requested credits
            cl += " -CRSH " + this.RequestedSalesCreditsLifeHealth;
            cl += " -CRSP " + this.RequestedSalesCreditsPropertyCasualty;
            cl += " -CRSE " + this.RequestedSalesCreditsEthics;
            cl += " -CRSG " + this.RequestedSalesCreditsGeneral;
            cl += " -CRSL " + this.RequestedSalesCreditsLaws;
            cl += " -CRSO " + this.RequestedSalesCreditsOther;
            cl += " -CRST " + totalRequestedSales;
            cl += " -CRSA " + this.RequestedSalesCreditsAdjuster;

            cl += " -CRIH " + this.RequestedInsuranceCreditsLifeHealth;
            cl += " -CRIP " + this.RequestedInsuranceCreditsPropertyCasualty;
            cl += " -CRIE " + this.RequestedInsuranceCreditsEthics;
            cl += " -CRIG " + this.RequestedInsuranceCreditsGeneral;
            cl += " -CRIL " + this.RequestedInsuranceCreditsLaws;
            cl += " -CRIO " + this.RequestedInsuranceCreditsOther;
            cl += " -CRIT " + totalRequestedInsurance;
            cl += " -CRIA " + this.RequestedInsuranceCreditsAdjuster;

            cl += " -CASH " + this.ApprovedSalesCreditsLifeHealth;
            cl += " -CASP " + this.ApprovedSalesCreditsPropertyCasualty;
            cl += " -CASE " + this.ApprovedSalesCreditsEthics;
            cl += " -CASG " + this.ApprovedSalesCreditsGeneral;
            cl += " -CASL " + this.ApprovedSalesCreditsLaws;
            cl += " -CASO " + this.ApprovedSalesCreditsOther;
            cl += " -CAST " + totalApprovedSales;
            cl += " -CASA " + this.ApprovedSalesCreditsAdjuster;

            cl += " -CAIH " + this.ApprovedInsuranceCreditsLifeHealth;
            cl += " -CAIP " + this.ApprovedInsuranceCreditsPropertyCasualty;
            cl += " -CAIE " + this.ApprovedInsuranceCreditsEthics;
            cl += " -CAIG " + this.ApprovedInsuranceCreditsGeneral;
            cl += " -CAIL " + this.ApprovedInsuranceCreditsLaws;
            cl += " -CAIO " + this.ApprovedInsuranceCreditsOther;
            cl += " -CAIT " + totalApprovedInsurance;
            cl += " -CAIA " + this.ApprovedInsuranceCreditsAdjuster;


            if (this.CourseIsDesignation == true)
            {
                // Only pass designation type if
                // course designation box is checked
                cl += " -CDT " + this.Escape(this.CourseDesignationType);
            }
            
            if(this.CourseType == CourseType.ClassroomOther)
            {
                // Only pass the course method description
                // if other is selected
                cl += " -CMO " + this.Escape(this.CourseMethodDescription);
            }

            if(this.SmallCourseTitle)
            {
                // Insert small course title flag
                // if requested
                cl += " -SCT";
            }

            if(this.SmallProviderName)
            {
                // Insert small provider name flag
                // if requested
                cl += " -SPN";
            }

            if(this.SmallProviderNumber)
            {
                // Insert small provider number flag
                // if requested
                cl += " -SRPN";
            }

            if(this.PreserveContentControls)
            {
                cl += " -PC";
            }

            if(this.ReplaceFiles)
            {
                cl += " -R";
            }

            if(File.Exists(generatorPath) == false)
            {
                // Return false
                this.Complete = true;
                this.Failure = true;
                // Generator does not exist
                // at given path
                return false;
            }

            this.Complete = true;
            this.Failure = false;

            ProcessStartInfo info = new ProcessStartInfo();

            info.FileName = generatorPath;
            info.Arguments = cl;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;

            Process process;

            process = Process.Start(info);

            return true;
        }
    }
}

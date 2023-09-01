using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace naic
{
    /**
    \brief
        Creates, manages, and executes jobs
        to be processed by the generator.
    */
    public class JobManager
    {
        /// Course to create NAIC forms
        /// for
        public Course Course { get; set; }

        /// Provider to create NAIC forms
        /// for
        public Provider Provider { get; set; }

        /// List containing each job to
        /// be executed
        public ObservableCollection<Job> Jobs { get; set; }

        /// List containing failed jobs
        /// after jobs have been
        /// processed
        public ObservableCollection<Job> FailedJobs { get; set; }

        /// Output directory to generate
        /// NAIC forms to
        public string OutputFolder { get; set; }

        /**
        \brief
            Constructor.
        */
        public JobManager()
        {
            // Create new list containing jobs
            this.Jobs = new ObservableCollection<Job>();

            // Create new list that will contain
            // failed jobs
            this.FailedJobs = new ObservableCollection<Job>();
        }

        /**
        \brief
            Creates jobs and stores them in
            job list for given course
            and provider information
        */
        public void PopulateJobs()
        {
            // Make sure course and provider
            // information are specified
            if (this.Course == null || this.Provider == null)
            {
                // They are not
                // Stop executing
                return;
            }

            foreach(Region reg in this.Provider.Regions)
            {
                Job j = new Job();

                j.MetaTitle = reg.Name;
                j.MetaRegion = reg.Name;
                j.MetaOutput = reg.OutputName;

                j.Enabled = true;
                j.Warning = false;

                // See if we should preserve content
                // controls
                if(Properties.Settings.Default.preserveOutputContentControls == true)
                {
                    // We should
                    j.PreserveContentControls = true;
                }

                j.CourseTitle                       = this.Course.Title;
                j.CourseDate                        = this.Course.Date;
                j.CourseHomeStateApprovalDate       = this.Course.ApprovalDate;
                j.CourseHomeStateApprovalExpiration = this.Course.ExpirationDate;
                j.CourseHomeStateApprovalNumber     = this.Course.ApprovalNumber;
                j.CourseDesignationType             = this.Course.Designation;
                j.CourseDifficulty                  = this.Course.Difficulty;
                j.CourseIsDesignation               = this.Course.IsDesignation;
                j.CourseIsPublic                    = this.Course.IsPublic;
                j.CourseMethodDescription           = this.Course.OtherDescription;
                j.CourseRequiresExam                = this.Course.DoesRequireExam;
                j.CourseType                        = this.Course.Type;
                j.CourseWordCount                   = this.Course.WordCount;
                j.ProviderName                      = this.Provider.Name;
                j.ProviderFEIN                      = this.Provider.FEIN;
                j.ProviderHomeState                 = this.Provider.HomeState;
                j.ProviderHomeStateNumber           = this.Provider.HomeStateProviderNumber;
                j.ProviderContact                   = this.Provider.ContactName;
                j.ProviderEmail                     = this.Provider.ContactEmail;
                j.ProviderPhone                     = this.Provider.ContactPhone;
                j.ProviderFax                       = this.Provider.ContactFax;
                j.ProviderMailingStreetAddress      = this.Provider.MailingStreetAddress;
                j.ProviderMailingCity               = this.Provider.MailingCity;
                j.ProviderMailingState              = this.Provider.MailingState;
                j.ProviderMailingZip                = this.Provider.MailingZip;
                j.ProviderReciprocalState           = reg.Abbreviation;
                j.ProviderReciprocalStateNumber     = reg.ProviderNumber;
                j.InputPath                         = null;

                // Get input path from
                // configuration
                j.InputPath = ConfigurationManager.AppSettings["templateFilePath"].ToString();

                // Calculate credits
                // This will not be pretty
                ushort[] requestedSalesCredits     = new ushort[7];
                ushort[] requestedInsuranceCredits = new ushort[7];
                ushort[] approvedSalesCredits      = new ushort[7];
                ushort[] approvedInsuranceCredits  = new ushort[7];

                // Create a list that will contain
                // each requested credit array
                List<ushort[]> creditArrayList = new List<ushort[]>();

                creditArrayList.Add(requestedSalesCredits);
                creditArrayList.Add(requestedInsuranceCredits);

                // Populate arrays with the specified
                // course credits

                // Check to see if we should be
                // using the regular credits
                // or the timed credits
                if(reg.TimedCourse == true && this.Course.TimedCreditsDiffer == true)
                {
                    // Timed credits
                    requestedSalesCredits[0] = this.Course.RequestedTimedSalesCreditsLifeHealth;
                    requestedSalesCredits[1] = this.Course.RequestedTimedSalesCreditsPropertyCasualty;
                    requestedSalesCredits[2] = this.Course.RequestedTimedSalesCreditsEthics;
                    requestedSalesCredits[3] = this.Course.RequestedTimedSalesCreditsGeneral;
                    requestedSalesCredits[4] = this.Course.RequestedTimedSalesCreditsLaws;
                    requestedSalesCredits[5] = this.Course.RequestedTimedSalesCreditsOther;
                    requestedSalesCredits[6] = this.Course.RequestedTimedSalesCreditsAdjuster;

                    requestedInsuranceCredits[0] = this.Course.RequestedTimedInsuranceCreditsLifeHealth;
                    requestedInsuranceCredits[1] = this.Course.RequestedTimedInsuranceCreditsPropertyCasualty;
                    requestedInsuranceCredits[2] = this.Course.RequestedTimedInsuranceCreditsEthics;
                    requestedInsuranceCredits[3] = this.Course.RequestedTimedInsuranceCreditsGeneral;
                    requestedInsuranceCredits[4] = this.Course.RequestedTimedInsuranceCreditsLaws;
                    requestedInsuranceCredits[5] = this.Course.RequestedTimedInsuranceCreditsOther;
                    requestedInsuranceCredits[6] = this.Course.RequestedTimedInsuranceCreditsAdjuster;
                }
                else
                {
                    // Regular credits
                    requestedSalesCredits[0] = this.Course.RequestedSalesCreditsLifeHealth;
                    requestedSalesCredits[1] = this.Course.RequestedSalesCreditsPropertyCasualty;
                    requestedSalesCredits[2] = this.Course.RequestedSalesCreditsEthics;
                    requestedSalesCredits[3] = this.Course.RequestedSalesCreditsGeneral;
                    requestedSalesCredits[4] = this.Course.RequestedSalesCreditsLaws;
                    requestedSalesCredits[5] = this.Course.RequestedSalesCreditsOther;
                    requestedSalesCredits[6] = this.Course.RequestedSalesCreditsAdjuster;

                    requestedInsuranceCredits[0] = this.Course.RequestedInsuranceCreditsLifeHealth;
                    requestedInsuranceCredits[1] = this.Course.RequestedInsuranceCreditsPropertyCasualty;
                    requestedInsuranceCredits[2] = this.Course.RequestedInsuranceCreditsEthics;
                    requestedInsuranceCredits[3] = this.Course.RequestedInsuranceCreditsGeneral;
                    requestedInsuranceCredits[4] = this.Course.RequestedInsuranceCreditsLaws;
                    requestedInsuranceCredits[5] = this.Course.RequestedInsuranceCreditsOther;
                    requestedInsuranceCredits[6] = this.Course.RequestedInsuranceCreditsAdjuster;
                }

                approvedSalesCredits[0] = this.Course.ApprovedSalesCreditsLifeHealth;
                approvedSalesCredits[1] = this.Course.ApprovedSalesCreditsPropertyCasualty;
                approvedSalesCredits[2] = this.Course.ApprovedSalesCreditsEthics;
                approvedSalesCredits[3] = this.Course.ApprovedSalesCreditsGeneral;
                approvedSalesCredits[4] = this.Course.ApprovedSalesCreditsLaws;
                approvedSalesCredits[5] = this.Course.ApprovedSalesCreditsOther;
                approvedSalesCredits[6] = this.Course.ApprovedSalesCreditsAdjuster;

                approvedInsuranceCredits[0] = this.Course.ApprovedInsuranceCreditsLifeHealth;
                approvedInsuranceCredits[1] = this.Course.ApprovedInsuranceCreditsPropertyCasualty;
                approvedInsuranceCredits[2] = this.Course.ApprovedInsuranceCreditsEthics;
                approvedInsuranceCredits[3] = this.Course.ApprovedInsuranceCreditsGeneral;
                approvedInsuranceCredits[4] = this.Course.ApprovedInsuranceCreditsLaws;
                approvedInsuranceCredits[5] = this.Course.ApprovedInsuranceCreditsOther;
                approvedInsuranceCredits[6] = this.Course.ApprovedInsuranceCreditsAdjuster;

                // Cap each amount if it
                // exceeds the maximum specified
                // credits per line
                if(reg.MaximumCreditsPerLine > 0)
                {
                    // Only proceed if a maximum
                    // is specified

                    // Iterate through each array in the
                    // list
                    foreach(ushort[] array in creditArrayList)
                    {
                        // Iterate through each value
                        // in the array
                        for(int i = 0; i < array.Count(); ++i)
                        {
                            // Check if the stored credit value
                            // exceeds the given maximum
                            if(array[i] > reg.MaximumCreditsPerLine)
                            {
                                // It does.
                                // Cap it.
                                array[i] = reg.MaximumCreditsPerLine;
                            }
                        }
                    }
                }

                // Now process credit transformations
                // Here we go
                foreach (CreditTransformation trans in reg.CreditTransformations)
                {
                    // Iterate through both requested
                    // sales and insurance arrays
                    foreach(ushort[] array in creditArrayList)
                    {
                        // Will contain the newly transformed
                        // credit amounts
                        ushort[] newCredits = new ushort[7];

                        newCredits[0] = array[0];
                        newCredits[1] = array[1];
                        newCredits[2] = array[2];
                        newCredits[3] = array[3];
                        newCredits[4] = array[4];
                        newCredits[5] = array[5];
                        newCredits[6] = array[6];

                        // Determines if we have to
                        // continue after the initial
                        // pass (IE do more than remove
                        // credits)
                        bool twoStepAction = true;

                        // See what kind of 
                        // transformation we're
                        // looking at
                        switch (trans.Action)
                        {
                            case TransformationAction.Move:
                                // Move credits from the given
                                // source type to the given
                                // destination type

                                // Set the source type credits
                                // to 0
                                newCredits[(ushort)trans.SourceType] = 0;

                                twoStepAction = true;

                                break;

                            case TransformationAction.Copy:
                                // Copy credits from the given
                                // source type to the given
                                // destination type

                                // No action required right now,
                                // but continue processing

                                twoStepAction = true;
                                break;

                            case TransformationAction.Remove:
                                // Remove credits from the given
                                // source type

                                // Set the source type credits
                                // to 0
                                switch(trans.Amount)
                                {
                                    case TransformationAmount.All:
                                        newCredits[(ushort)trans.SourceType] = 0;
                                        break;

                                    case TransformationAmount.Half:
                                        newCredits[(ushort)trans.SourceType] = (ushort)Math.Round((double)(array[(ushort)trans.SourceType] / 2));
                                        break;
                                }
                                

                                // No further action required
                                twoStepAction = false;
                                break;
                        }

                        // Proceed only if this is
                        // a two step action
                        if(twoStepAction == true)
                        {
                            // Will determine the number
                            // of credits being moved/copied
                            ushort creditAmount = 0;

                            // Determine how many credits
                            // are being acted on
                            switch (trans.Amount)
                            {
                                case TransformationAmount.All:
                                    // All credits should
                                    // be moved/copied
                                    creditAmount = array[(ushort)trans.SourceType];
                                    break;

                                case TransformationAmount.Half:
                                    // Only half of the credits
                                    // should be moved/copied
                                    // Rounding down
                                    // Change "Math.Floor"
                                    // to "Math.Ceiling" if you
                                    // want it rounded up
                                    creditAmount = (ushort)Math.Round((double)(array[(ushort)trans.SourceType] / 2));
                                    break;
                            }

                            // Determine what should happen to the
                            // existing credits in the destination
                            // line
                            switch(trans.Destination)
                            {
                                case TransformationDestination.Into:
                                    // Credits should be added
                                    // to the existing value
                                    newCredits[(ushort)trans.DestinationType] += creditAmount;
                                    break;

                                case TransformationDestination.Over:
                                    // Credits should overwrite
                                    // the existing value
                                    newCredits[(ushort)trans.DestinationType] = creditAmount;
                                    break;
                            }

                        }

                        // Copy new credit values
                        // into array
                        for (int i = 0; i < newCredits.Count(); ++i)
                        {
                            array[i] = newCredits[i];
                        }
                    }
                }

                // Now assign transformed credit
                // values to the job

                j.RequestedSalesCreditsLifeHealth       = requestedSalesCredits[0];
                j.RequestedSalesCreditsPropertyCasualty = requestedSalesCredits[1];
                j.RequestedSalesCreditsEthics           = requestedSalesCredits[2];
                j.RequestedSalesCreditsGeneral          = requestedSalesCredits[3];
                j.RequestedSalesCreditsLaws             = requestedSalesCredits[4];
                j.RequestedSalesCreditsOther            = requestedSalesCredits[5];
                j.RequestedSalesCreditsAdjuster         = requestedSalesCredits[6];

                j.RequestedInsuranceCreditsLifeHealth       = requestedInsuranceCredits[0];
                j.RequestedInsuranceCreditsPropertyCasualty = requestedInsuranceCredits[1];
                j.RequestedInsuranceCreditsEthics           = requestedInsuranceCredits[2];
                j.RequestedInsuranceCreditsGeneral          = requestedInsuranceCredits[3];
                j.RequestedInsuranceCreditsLaws             = requestedInsuranceCredits[4];
                j.RequestedInsuranceCreditsOther            = requestedInsuranceCredits[5];
                j.RequestedInsuranceCreditsAdjuster         = requestedInsuranceCredits[6];

                j.ApprovedSalesCreditsLifeHealth       = approvedSalesCredits[0];
                j.ApprovedSalesCreditsPropertyCasualty = approvedSalesCredits[1];
                j.ApprovedSalesCreditsEthics           = approvedSalesCredits[2];
                j.ApprovedSalesCreditsGeneral          = approvedSalesCredits[3];
                j.ApprovedSalesCreditsLaws             = approvedSalesCredits[4];
                j.ApprovedSalesCreditsOther            = approvedSalesCredits[5];
                j.ApprovedSalesCreditsAdjuster         = approvedSalesCredits[6];

                j.ApprovedInsuranceCreditsLifeHealth       = approvedInsuranceCredits[0];
                j.ApprovedInsuranceCreditsPropertyCasualty = approvedInsuranceCredits[1];
                j.ApprovedInsuranceCreditsEthics           = approvedInsuranceCredits[2];
                j.ApprovedInsuranceCreditsGeneral          = approvedInsuranceCredits[3];
                j.ApprovedInsuranceCreditsLaws             = approvedInsuranceCredits[4];
                j.ApprovedInsuranceCreditsOther            = approvedInsuranceCredits[5];
                j.ApprovedInsuranceCreditsAdjuster         = approvedInsuranceCredits[6];

                // Now process credit requirements
                // Here we go again

                // Determines if all requirements
                // have been met
                bool allRequirementsSatisfied = true;

                // Determines if any requirements
                // have been met
                bool anyRequirementsSatisfied = false;

                foreach (CreditRequirement req in reg.CreditRequirements)
                {
                    // Contains the sum of all requested
                    // sales and insurance credits
                    ushort[] mergedArray = new ushort[7];

                    // Iterate through both requested
                    // sales and insurance arrays
                    foreach (ushort[] array in creditArrayList)
                    {
                        // Iterate through contents of array
                        // and add value to the same index
                        // of the merged array
                        for(int i = 0; i < array.Count(); ++i)
                        {
                            mergedArray[i] += array[i];
                        }
                    }

                    switch (req.Condition)
                    {
                        case RequirementCondition.EqualTo:
                            if(mergedArray[(ushort)req.Type] == req.Amount)
                            {
                                // Requirement met
                                anyRequirementsSatisfied = true;
                            }
                            else
                            {
                                // Requirement not met
                                allRequirementsSatisfied = false;
                            }
                            break;

                        case RequirementCondition.FewerThan:
                            if(mergedArray[(ushort)req.Type] < req.Amount)
                            {
                                // Requirement met
                                anyRequirementsSatisfied = true;
                            }
                            else
                            {
                                // Requirement not met
                                allRequirementsSatisfied = false;
                            }
                            break;

                        case RequirementCondition.GreaterThan:
                            if(mergedArray[(ushort)req.Type] > req.Amount)
                            {
                                // Requirement met
                                anyRequirementsSatisfied = true;
                            }
                            else
                            {
                                // Requirement not met
                                allRequirementsSatisfied = false;
                            }
                            break;
                    }
                }

                // We have processed requirements
                // Now let's see if we need to
                // meet ALL requirements, or just
                // ONE requirement
                switch (reg.RequirementConditions)
                {
                    case CourseCreditRequirementCondition.AllRequirements:
                        // We have to meet all conditions
                        if(allRequirementsSatisfied != true && reg.CreditRequirements.Count != 0)
                        {
                            // ... And we didn't.
                            // Disable job and add
                            // a warning
                            j.Enabled = false;
                            j.Warning = true;
                            j.WarningString = "Did not meet all credit requirements";
                        }
                        break;

                    case CourseCreditRequirementCondition.OneRequirement:
                        // We only have to meet one condition
                        if(anyRequirementsSatisfied != true && reg.CreditRequirements.Count != 0)
                        {
                            // ... And we didn't.
                            // Disable job and add
                            // a warning
                            j.Enabled = false;
                            j.Warning = true;
                            j.WarningString = "Did not meet all credit requirements";
                        }
                        break;
                }

                // Assign overridden provider name
                // if specified
                if (reg.OverrideProviderName != "" && reg.OverrideProviderName != null)
                {
                    // Custom provider name specified,
                    // so use it instead
                    j.ProviderName = reg.OverrideProviderName;
                }

                // Assign overriden provider contact
                // name if specified
                if (reg.OverrideContactName != "" && reg.OverrideContactName != null)
                {
                    j.ProviderName = reg.OverrideContactName;
                }

                // Assign overridden provider contact
                // email address if specified
                if (reg.OverrideContactEmail != "" && reg.OverrideContactEmail != null)
                {
                    j.ProviderEmail = reg.OverrideContactEmail;
                }

                // Assign overridden provider contact
                // phone number if specified
                if (reg.OverrideContactPhone != "" && reg.OverrideContactPhone != null)
                {
                    j.ProviderPhone = reg.OverrideContactPhone;
                }

                // Assign overridden provider contact
                // fax number if specified
                if (reg.OverrideContactFax != "" && reg.OverrideContactFax != null)
                {
                    j.ProviderFax = reg.OverrideContactFax;
                }

                // Assign overridden course difficulty
                // if specified for region
                if (reg.DoesOverrideCourseDifficulty == true)
                {
                    j.CourseDifficulty = reg.OverrideCourseDifficulty;
                }

                /*
                // TODO Determine if timed courses have unique word count

                This code is currently disabled
                because I am unclear whether or not
                it is necessary. I do not believe it
                is, but will remove when certain.

                // Check if we should use the
                // timed word count
                if (reg.TimedCourse == true)
                {
                    // We should be
                    j.CourseWordCount = this.Course.WordCountTimed;
                }
                */

                // Check to see if we should use
                // a custom template file
                if(reg.DoesUseCustomTemplate == true && reg.CustomTemplatePath != "" && reg.CustomTemplatePath != null)
                {
                    // We should
                    // Let's do it
                    j.InputPath = reg.CustomTemplatePath;     
                }

                // Check to see if we should append
                // anything to the course title
                if(reg.CourseTitleAppendix != null && reg.CourseTitleAppendix != "")
                {
                    // Sure enough
                    // Let's append the appendix
                    j.CourseTitle += reg.CourseTitleAppendix;
                }

                // Check if we should use small
                // course titles
                if(j.CourseTitle != null)
                {
                    if (j.CourseTitle.Length > 41)
                    {
                        // We should
                        j.SmallCourseTitle = true;
                    }
                }

                // Check if we should use the
                // small provider name
                if(j.ProviderName.Length > 36)
                {
                    // We should
                    j.SmallProviderName = true;
                }

                // Check if we should use the
                // small reciprocal provider
                // number
                if(j.ProviderReciprocalStateNumber.Length > 6)
                {
                    // We should
                    j.SmallProviderNumber = true;
                }

                this.Jobs.Add(j);
            }
        }

        public void ProcessAllJobs()
        {
            // Create an output log generator
            OutputLogGenerator outputGenerator = new OutputLogGenerator();

            foreach(Job job in this.Jobs)
            {
                // Process job
                bool success = job.Execute(
                    ConfigurationManager.AppSettings["generatorFilePath"].ToString(),
                    this.OutputFolder + @"\" + job.MetaOutput);

                // Remove job from job
                // list
                // this.Jobs.Remove(job);

                // Check if job was successful
                if(success == false)
                {
                    // It was not
                    // Add to failed job ilst
                    this.FailedJobs.Add(job);
                }

                // Add job to output generator
                outputGenerator.JobList.Add(job);
            }

            // Make sure we should generate
            // output logs by checking user
            // setting
            /*
            if(Properties.Settings.Default.generateOutputLogs == true)
            {
                // We should, proceed
                string dateTime = DateTime.Now.ToString("MM-dd-yyyy hh.mm.ss tt");

                string title = "NAIC " + dateTime + ".docx";
                outputGenerator.Generate(this.OutputFolder + @"\" + title);
            }*/
        }
    }
}

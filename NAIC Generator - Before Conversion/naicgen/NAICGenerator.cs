using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace naic
{

    /**
    \brief
        Generates a new NAIC document from
        the given template.
    */
    public class NAICGenerator
    {
        /// Provider name (ex. CEU)
        public string ProviderName;

        /// Provider FEIN
        public string ProviderFEIN;

        /// Provider contact name
        public string ProviderContact;

        /// Provider contact email address
        public string ProviderEmail;

        /// Provider contact phone number
        public string ProviderPhone;

        /// Provider contact fax number
        public string ProviderFax;

        /// Provider home state (abbreviation)
        public string ProviderHomeState;

        /// Provider home state provider number
        public string ProviderHomeStateNumber;

        /// Provider reciprocal state (abbreviation)
        public string ProviderReciprocalState;

        /// Provider reciprocal state provider number
        public string ProviderReciprocalStateNumber;

        /// Provider mailing street address
        public string ProviderMailingStreetAddress;

        /// Provider mailing address city
        public string ProviderMailingCity;

        /// Provider mailing address state
        public string ProviderMailingState;

        /// Provider mailing address zip
        public string ProviderMailingZip;

        /// Course title
        public string CourseTitle;

        /// Course offering date
        public string CourseOfferingDate;

        /// Course difficulty
        public CourseDifficulty CourseDifficulty;

        /// Course method (ex: self-study correspondence)
        public CourseType CourseMethod;

        /// Describes the course method when
        /// "Classroom: Other" course method is
        /// chosen
        public string CourseMethodDescription;

        /// Determines whether or not course is for
        /// a national designation
        public bool CourseIsDesignation;

        /// Describes the type of designation if
        /// the course is for a national designation
        public string CourseDesignationType;

        /// Determines whether or not course requires
        /// a final exam to be completed
        public bool CourseRequiresExam;

        /// Determines whether or not a course is public
        public bool CourseIsPublic;

        /// Number of words in course content
        public uint CourseWordCount;

        /// Contains requested sales/marketing credit
        /// details
        public CourseCredits RequestedSalesCredits;

        /// Contains requested insurance credit details
        public CourseCredits RequestedInsuranceCredits;

        /// Contains approved sales/marketing credit
        /// details
        public CourseCredits ApprovedSalesCredits;

        /// Contains approved insurance credit details
        public CourseCredits ApprovedInsuranceCredits;

        /// Contains approved reciprocal sales/marketing
        /// credit details
        public CourseCredits ReciprocalSalesCredits;

        /// Contains approved reciprocal insurance credit
        /// details
        public CourseCredits ReciprocalInsuranceCredits;

        /// Home state course approval date
        public string CourseHomeStateApprovalDate;

        /// Home state course approval expiration date
        public string CourseHomeStateApprovalExpiration;

        /// Home state course approval number
        public string CourseHomeStateApprovalNumber;

        /// Determines whether or not the generator will
        /// flatten control controls in the document
        public bool ShouldFlattenControls = true;

        /// Determines whether the generator should use
        /// a smaller font for the provider name, in case
        /// of extra long provider names that may not fit
        /// in the usual space.
        public bool ShouldShrinkProviderName = false;

        /// Determines whether the generator should use
        /// a smaller font for the provider number, in
        /// case of extra longp rovider numbers that may
        /// not fit in the usual space (lookin' at you,
        /// New York)
        public bool ShouldShrinkProviderNumber = false;

        /// Determines whether the generator should use
        /// a smaller font for the course title
        public bool ShouldShrinkCourseTitle = false;

        /// Determines whether or not the generator should
        /// output text while generating documents
        public bool VerboseGeneration = false;

        /// Determines the color that the difficulty
        /// indicator is highlighted in
        public HighlightColorValues HighlightColor = HighlightColorValues.Yellow;

        /**
        \brief
            Constructor.
        */
        public NAICGenerator()
        {
            // Create objects to store
            // course credit information in
            this.RequestedSalesCredits      = new CourseCredits();
            this.RequestedInsuranceCredits  = new CourseCredits();
            this.ApprovedSalesCredits       = new CourseCredits();
            this.ApprovedInsuranceCredits   = new CourseCredits();
            this.ReciprocalSalesCredits     = new CourseCredits();
            this.ReciprocalInsuranceCredits = new CourseCredits();
        }

        /**
        \brief
            Loads properties from given
            command line arguments.

        \param arguments
            CommandLineArguments object
        */
        public void LoadFromArguments(CommandLineArguments arguments)
        {
            // Provider information
            this.ProviderName                  = arguments.GetValue("ProviderName"             , "PNM", "");
            this.ProviderFEIN                  = arguments.GetValue("ProviderFEIN"             , "PFN", "");
            this.ProviderHomeState             = arguments.GetValue("ProviderHomeState"        , "PHS", "");
            this.ProviderHomeStateNumber       = arguments.GetValue("ProviderHomeNumber"       , "PHN", "");
            this.ProviderReciprocalState       = arguments.GetValue("ProviderReciprocalState"  , "PRS", "");
            this.ProviderReciprocalStateNumber = arguments.GetValue("ProviderReciprocalNumber" , "PRN", "");

            // Contact information
            this.ProviderContact              = arguments.GetValue("ContactName"          , "CN", "");
            this.ProviderEmail                = arguments.GetValue("ContactEmail"         , "CE", "");
            this.ProviderPhone                = arguments.GetValue("ContactPhone"         , "CP", "");
            this.ProviderFax                  = arguments.GetValue("ContactFax"           , "CF", "");
            this.ProviderMailingStreetAddress = arguments.GetValue("MailingStreetAddress" , "MA", "");
            this.ProviderMailingCity          = arguments.GetValue("MailingCity"          , "MC", "");
            this.ProviderMailingState         = arguments.GetValue("MailingState"         , "MS", "");
            this.ProviderMailingZip           = arguments.GetValue("MailingZip"           , "MZ", "");

            // Course information
            this.CourseTitle             =     arguments.GetValue("CourseTitle"          , "CT"  , "")  ;
            this.CourseOfferingDate      =     arguments.GetValue("CourseOffering"       , "CO"  , "")  ;
            this.CourseMethod            = (CourseType)       Convert.ToInt32   (
                                               arguments.GetValue("CourseMethod"         , "CM"  , "0"));
            this.CourseWordCount         =                    Convert.ToUInt32  (
                                               arguments.GetValue("CourseWordCount"      , "CWC" , "0"));
            this.CourseDifficulty        = (CourseDifficulty) Convert.ToInt32   (
                                               arguments.GetValue("CourseDifficulty"     , "CD"  , "0"));
            this.CourseMethodDescription =     arguments.GetValue("CourseMethodOther"    , "CMO" , "" ) ;
            this.CourseDesignationType   =     arguments.GetValue("CourseDesignationType", "CDT" , "" ) ;
            this.CourseIsDesignation     =                    (Convert.ToInt32 (
                                               arguments.GetValue("CourseDesignation"    , "CND" , "0")) > 0);
            this.CourseIsPublic          =                    (Convert.ToInt32 (
                                               arguments.GetValue("CoursePublic"         , "CIP" , "1")) > 0);
            this.CourseRequiresExam      =                    (Convert.ToInt32 (
                                               arguments.GetValue("CourseHasExam"        , "CHE" , "1")) > 0);

            // Course approval information
            this.CourseHomeStateApprovalDate       = arguments.GetValue("ApprovalDate"       , "AD" , "");
            this.CourseHomeStateApprovalExpiration = arguments.GetValue("ApprovalExpiration" , "AE" , "");
            this.CourseHomeStateApprovalNumber     = arguments.GetValue("ApprovalNumber"     , "AN" , "");

            // Course credit information
            // This is going to be a doozy
            ushort[] creditsReqSales = new ushort[8]; // Requested sales credits
            ushort[] creditsReqIns   = new ushort[8]; // Requested insurance credits
            ushort[] creditsAppSales = new ushort[8]; // Approved sales credits
            ushort[] creditsAppIns   = new ushort[8]; // Approved insurance credits
            ushort[] creditsRecSales = new ushort[8]; // Reciprocal sales credits
            ushort[] creditsRecIns   = new ushort[8]; // Reciprocal insurance credits

            // Import the command line arguments

            creditsReqSales[0] = Convert.ToUInt16(arguments.GetValue("RequestedSalesLH"     , "CRSH" , "0"));
            creditsReqSales[1] = Convert.ToUInt16(arguments.GetValue("RequestedSalesPC"     , "CRSP" , "0"));
            creditsReqSales[2] = Convert.ToUInt16(arguments.GetValue("RequestedSalesEthics" , "CRSE" , "0"));
            creditsReqSales[3] = Convert.ToUInt16(arguments.GetValue("RequestedSalesGen"    , "CRSG" , "0"));
            creditsReqSales[4] = Convert.ToUInt16(arguments.GetValue("RequestedSalesLaws"   , "CRSL" , "0"));
            creditsReqSales[5] = Convert.ToUInt16(arguments.GetValue("RequestedSalesOther"  , "CRSO" , "0"));
            creditsReqSales[6] = Convert.ToUInt16(arguments.GetValue("RequestedSalesTotal"  , "CRST" , "0"));
            creditsReqSales[7] = Convert.ToUInt16(arguments.GetValue("RequestedSalesAdj"    , "CRSA" , "0"));

            creditsReqIns[0] = Convert.ToUInt16(arguments.GetValue("RequestedInsLH"     , "CRIH" , "0"));
            creditsReqIns[1] = Convert.ToUInt16(arguments.GetValue("RequestedInsPC"     , "CRIP" , "0"));
            creditsReqIns[2] = Convert.ToUInt16(arguments.GetValue("RequestedInsEthics" , "CRIE" , "0"));
            creditsReqIns[3] = Convert.ToUInt16(arguments.GetValue("RequestedInsGen"    , "CRIG" , "0"));
            creditsReqIns[4] = Convert.ToUInt16(arguments.GetValue("RequestedInsLaws"   , "CRIL" , "0"));
            creditsReqIns[5] = Convert.ToUInt16(arguments.GetValue("RequestedInsOther"  , "CRIO" , "0"));
            creditsReqIns[6] = Convert.ToUInt16(arguments.GetValue("RequestedInsTotal"  , "CRIT" , "0"));
            creditsReqIns[7] = Convert.ToUInt16(arguments.GetValue("RequestedInsAdj"    , "CRIA" , "0"));

            creditsAppSales[0] = Convert.ToUInt16(arguments.GetValue("ApprovedSalesLH"     , "CASH" , "0"));
            creditsAppSales[1] = Convert.ToUInt16(arguments.GetValue("ApprovedSalesPC"     , "CASP" , "0"));
            creditsAppSales[2] = Convert.ToUInt16(arguments.GetValue("ApprovedSalesEthics" , "CASE" , "0"));
            creditsAppSales[3] = Convert.ToUInt16(arguments.GetValue("ApprovedSalesGen"    , "CASG" , "0"));
            creditsAppSales[4] = Convert.ToUInt16(arguments.GetValue("ApprovedSalesLaws"   , "CASL" , "0"));
            creditsAppSales[5] = Convert.ToUInt16(arguments.GetValue("ApprovedSalesOther"  , "CASO" , "0"));
            creditsAppSales[6] = Convert.ToUInt16(arguments.GetValue("ApprovedSalesTotal"  , "CAST" , "0"));
            creditsAppSales[7] = Convert.ToUInt16(arguments.GetValue("ApprovedSalesAdj"    , "CASA" , "0"));

            creditsAppIns[0] = Convert.ToUInt16(arguments.GetValue("ApprovedInsLH"     , "CAIH" , "0"));
            creditsAppIns[1] = Convert.ToUInt16(arguments.GetValue("ApprovedInsPC"     , "CAIP" , "0"));
            creditsAppIns[2] = Convert.ToUInt16(arguments.GetValue("ApprovedInsEthics" , "CAIE" , "0"));
            creditsAppIns[3] = Convert.ToUInt16(arguments.GetValue("ApprovedInsGen"    , "CAIG" , "0"));
            creditsAppIns[4] = Convert.ToUInt16(arguments.GetValue("ApprovedInsLaws"   , "CAIL" , "0"));
            creditsAppIns[5] = Convert.ToUInt16(arguments.GetValue("ApprovedInsOther"  , "CAIO" , "0"));
            creditsAppIns[6] = Convert.ToUInt16(arguments.GetValue("ApprovedInsTotal"  , "CAIT" , "0"));
            creditsAppIns[7] = Convert.ToUInt16(arguments.GetValue("ApprovedSalesAdj"  , "CAIA" , "0"));

            creditsRecSales[0] = Convert.ToUInt16(arguments.GetValue("ReciprocalSalesLH"     , "COSH" , "0"));
            creditsRecSales[1] = Convert.ToUInt16(arguments.GetValue("ReciprocalSalesPC"     , "COSP" , "0"));
            creditsRecSales[2] = Convert.ToUInt16(arguments.GetValue("ReciprocalSalesEthics" , "COSE" , "0"));
            creditsRecSales[3] = Convert.ToUInt16(arguments.GetValue("ReciprocalSalesGen"    , "COSG" , "0"));
            creditsRecSales[4] = Convert.ToUInt16(arguments.GetValue("ReciprocalSalesLaws"   , "COSL" , "0"));
            creditsRecSales[5] = Convert.ToUInt16(arguments.GetValue("ReciprocalSalesOther"  , "COSO" , "0"));
            creditsRecSales[6] = Convert.ToUInt16(arguments.GetValue("ReciprocalSalesTotal"  , "COST" , "0"));
            creditsRecSales[7] = Convert.ToUInt16(arguments.GetValue("ReciprocalSalesAdj"    , "COSA" , "0"));

            creditsRecIns[0] = Convert.ToUInt16(arguments.GetValue("ReciprocalInsLH"     , "COIH" , "0"));
            creditsRecIns[1] = Convert.ToUInt16(arguments.GetValue("ReciprocalInsPC"     , "COIP" , "0"));
            creditsRecIns[2] = Convert.ToUInt16(arguments.GetValue("ReciprocalInsEthics" , "COIE" , "0"));
            creditsRecIns[3] = Convert.ToUInt16(arguments.GetValue("ReciprocalInsGen"    , "COIG" , "0"));
            creditsRecIns[4] = Convert.ToUInt16(arguments.GetValue("ReciprocalInsLaws"   , "COIL" , "0"));
            creditsRecIns[5] = Convert.ToUInt16(arguments.GetValue("ReciprocalInsOther"  , "COIO" , "0"));
            creditsRecIns[6] = Convert.ToUInt16(arguments.GetValue("ReciprocalInsTotal"  , "COIT" , "0"));
            creditsRecIns[7] = Convert.ToUInt16(arguments.GetValue("ReciprocalInsAdj"    , "COIA" , "0"));

            // Add that data into the credit objects
            // contained by this generator

            this.RequestedSalesCredits.LifeHealthCredits       = creditsReqSales[0];
            this.RequestedSalesCredits.PropertyCasualtyCredits = creditsReqSales[1];
            this.RequestedSalesCredits.EthicsCredits           = creditsReqSales[2];
            this.RequestedSalesCredits.GeneralCredits          = creditsReqSales[3];
            this.RequestedSalesCredits.LawCredits              = creditsReqSales[4];
            this.RequestedSalesCredits.OtherCredits            = creditsReqSales[5];
            this.RequestedSalesCredits.TotalCredits            = creditsReqSales[6];
            this.RequestedSalesCredits.AdjusterCredits         = creditsReqSales[7];

            this.RequestedInsuranceCredits.LifeHealthCredits       = creditsReqIns[0];
            this.RequestedInsuranceCredits.PropertyCasualtyCredits = creditsReqIns[1];
            this.RequestedInsuranceCredits.EthicsCredits           = creditsReqIns[2];
            this.RequestedInsuranceCredits.GeneralCredits          = creditsReqIns[3];
            this.RequestedInsuranceCredits.LawCredits              = creditsReqIns[4];
            this.RequestedInsuranceCredits.OtherCredits            = creditsReqIns[5];
            this.RequestedInsuranceCredits.TotalCredits            = creditsReqIns[6];
            this.RequestedInsuranceCredits.AdjusterCredits         = creditsReqIns[7];

            this.ApprovedSalesCredits.LifeHealthCredits       = creditsAppSales[0];
            this.ApprovedSalesCredits.PropertyCasualtyCredits = creditsAppSales[1];
            this.ApprovedSalesCredits.EthicsCredits           = creditsAppSales[2];
            this.ApprovedSalesCredits.GeneralCredits          = creditsAppSales[3];
            this.ApprovedSalesCredits.LawCredits              = creditsAppSales[4];
            this.ApprovedSalesCredits.OtherCredits            = creditsAppSales[5];
            this.ApprovedSalesCredits.TotalCredits            = creditsAppSales[6];
            this.ApprovedSalesCredits.AdjusterCredits         = creditsAppSales[7];

            this.ApprovedInsuranceCredits.LifeHealthCredits       = creditsAppIns[0];
            this.ApprovedInsuranceCredits.PropertyCasualtyCredits = creditsAppIns[1];
            this.ApprovedInsuranceCredits.EthicsCredits           = creditsAppIns[2];
            this.ApprovedInsuranceCredits.GeneralCredits          = creditsAppIns[3];
            this.ApprovedInsuranceCredits.LawCredits              = creditsAppIns[4];
            this.ApprovedInsuranceCredits.OtherCredits            = creditsAppIns[5];
            this.ApprovedInsuranceCredits.TotalCredits            = creditsAppIns[6];
            this.ApprovedInsuranceCredits.AdjusterCredits         = creditsAppIns[7];

            this.ReciprocalSalesCredits.LifeHealthCredits       = creditsRecSales[0];
            this.ReciprocalSalesCredits.PropertyCasualtyCredits = creditsRecSales[1];
            this.ReciprocalSalesCredits.EthicsCredits           = creditsRecSales[2];
            this.ReciprocalSalesCredits.GeneralCredits          = creditsRecSales[3];
            this.ReciprocalSalesCredits.LawCredits              = creditsRecSales[4];
            this.ReciprocalSalesCredits.OtherCredits            = creditsRecSales[5];
            this.ReciprocalSalesCredits.TotalCredits            = creditsRecSales[6];
            this.ReciprocalSalesCredits.AdjusterCredits         = creditsRecSales[7];

            this.ReciprocalInsuranceCredits.LifeHealthCredits       = creditsRecIns[0];
            this.ReciprocalInsuranceCredits.PropertyCasualtyCredits = creditsRecIns[1];
            this.ReciprocalInsuranceCredits.EthicsCredits           = creditsRecIns[2];
            this.ReciprocalInsuranceCredits.GeneralCredits          = creditsRecIns[3];
            this.ReciprocalInsuranceCredits.LawCredits              = creditsRecIns[4];
            this.ReciprocalInsuranceCredits.OtherCredits            = creditsRecIns[5];
            this.ReciprocalInsuranceCredits.TotalCredits            = creditsRecIns[6];
            this.ReciprocalInsuranceCredits.AdjusterCredits         = creditsRecIns[7];
        }

        /**
        \brief
            Generates an NAIC form and
            outputs it to outputPath.

        \param templatePath
            File path of NAIC template

        \param outputPath
            File path for new NAIC document

        \param replaceFile
            If a file already exists at
            outputPath, this argument determines
            whether or not it should be replaced

        \return
            A Result object describing the output
            of the operation.
        */
        public Result Generate(
            string templatePath,
            string outputPath,
            bool replaceFile = false)
        {
            // Create a new Result object.
            Result output = new Result();

            // Describe each possible error.
            output.ErrorDescriptions[0] = "No error";
            output.ErrorDescriptions[1] = "File already exists at NAIC output path";
            output.ErrorDescriptions[2] = "Template file does not exist at given path";
            output.ErrorDescriptions[3] = "Failed to write output file";
            output.ErrorDescriptions[4] = "Invalid input or output path";


            this.Log("Verifying template and output paths...");

            // Make sure we were passed correct template
            // and output paths. Null paths indicate
            // no template/output path were specified
            // at runtime
            if(templatePath == null || outputPath == null)
            {
                this.Log("Failed");

                // Either no input or output
                // path specified. Return
                // result
                output.Success = false;
                output.ErrorCode = 4;

                return output;
            }

            this.Log("Checking output path...");

            // Check if the file at outputPath already
            // exists and, if so, whether or not we
            // should replace it.
            if(File.Exists(outputPath) && replaceFile == false)
            {
                this.Log("Failed");

                // One exists and we're not allowed to
                // replace it. Return the result.
                output.Success = false;
                output.ErrorCode = 1;

                return output;
            }

            this.Log("Checking template path...");

            // Make sure that the template file exists
            if(File.Exists(templatePath) == false)
            {
                // It does not. Return the result.
                output.Success = false;
                output.ErrorCode = 2;

                return output;
            }

            this.Log("Opening template document and creating output document...");

            // Open the NAIC template and create a new wordprocessing document
            try
            {
                using (WordprocessingDocument naicDoc = WordprocessingDocument.Open(templatePath, false))
                using (WordprocessingDocument outDoc = WordprocessingDocument.Create(outputPath, WordprocessingDocumentType.Document))
                {
                    this.Log("Copying template content into output document...");
                    // Insert each part of the NAIC template into
                    // the output document
                    foreach (IdPartPair part in naicDoc.Parts)
                    {
                        outDoc.AddPart(part.OpenXmlPart, part.RelationshipId);
                    }

                    this.Log("Replacing output document content...");
                    // Begin replacing content
                    ContentControlProcessor parser = new ContentControlProcessor(outDoc);

                    // Basic provider and contact
                    // information
                    parser.ReplaceContentAtTag("idProviderName"                  , this.ProviderName);
                    parser.ReplaceContentAtTag("idFEIN"                          , this.ProviderFEIN);
                    parser.ReplaceContentAtTag("idContactPerson"                 , this.ProviderContact);
                    parser.ReplaceContentAtTag("idContactEmail"                  , this.ProviderEmail);
                    parser.ReplaceContentAtTag("idContactPhone"                  , this.ProviderPhone);
                    parser.ReplaceContentAtTag("idContactFax"                    , this.ProviderFax);
                    parser.ReplaceContentAtTag("idHomeState"                     , this.ProviderHomeState);
                    parser.ReplaceContentAtTag("idHomeStateProviderNumber"       , this.ProviderHomeStateNumber);
                    parser.ReplaceContentAtTag("idReciprocalState"               , this.ProviderReciprocalState);
                    parser.ReplaceContentAtTag("idReciprocalStateProviderNumber" , this.ProviderReciprocalStateNumber);

                    // Check to see if we should use a 
                    // smaller font for the provider name
                    if(this.ShouldShrinkProviderName == true)
                    {
                        // We should
                        parser.SetContentFontSizeAtTag("idProviderName", 18);
                    }

                    // Check to see if we should use a
                    // smaller font for the provider number
                    if(this.ShouldShrinkProviderNumber == true)
                    {
                        // We should indeed
                        parser.SetContentFontSizeAtTag("idReciprocalStateProviderNumber", 16);
                    }

                    // Provider address
                    parser.ReplaceContentAtTag("idMailingAddressStreetAddress", this.ProviderMailingStreetAddress);
                    parser.ReplaceContentAtTag("idMailingAddressCity", this.ProviderMailingCity);
                    parser.ReplaceContentAtTag("idMailingAddressState", this.ProviderMailingState);
                    parser.ReplaceContentAtTag("idMailingAddressZip", this.ProviderMailingZip);

                    // Course information
                    parser.ReplaceContentAtTag("idCourseTitle", this.CourseTitle);
                    parser.ReplaceContentAtTag("idCourseOfferingDate", this.CourseOfferingDate);
                    parser.ReplaceContentAtTag("idCourseWordCount", this.CourseWordCount.ToString());

                    if(this.ShouldShrinkCourseTitle == true)
                    {
                        parser.SetContentFontSizeAtTag("idCourseTitle", 20);
                    }

                    // Course method
                    parser.SetCheckBoxStateAtTag("idCourseMethodSelfStudyCorrespondence" , (this.CourseMethod == CourseType.SelfStudyCorrespondence  ));
                    parser.SetCheckBoxStateAtTag("idCourseMethodSelfStudyOnlineTraining" , (this.CourseMethod == CourseType.SelfStudyOnlineTraining  ));
                    parser.SetCheckBoxStateAtTag("idCourseMethodSelfStudyVideoAudio"     , (this.CourseMethod == CourseType.SelfStudyVideoAudioCDDVD ));
                    parser.SetCheckBoxStateAtTag("idCourseMethodClassroomSeminar"        , (this.CourseMethod == CourseType.ClassroomSeminarWorkshop ));
                    parser.SetCheckBoxStateAtTag("idCourseMethodClassroomWebinar"        , (this.CourseMethod == CourseType.ClassroomWebinar         ));
                    parser.SetCheckBoxStateAtTag("idCourseMethodClassroomTeleconference" , (this.CourseMethod == CourseType.ClassroomTeleconference  ));
                    parser.SetCheckBoxStateAtTag("idCourseMethodClassroomOther"          , (this.CourseMethod == CourseType.ClassroomOther           ));
                    parser.ReplaceContentAtTag("idCourseMethodClassroomOtherDescription" , this.CourseMethodDescription);

                    // Course difficulty
                    parser.SetContentHighlightAtTag( "idCourseDifficultyBasic"        , (this.CourseDifficulty == CourseDifficulty.Basic)        , this.HighlightColor);
                    parser.SetContentBoldAtTag(      "idCourseDifficultyBasic"        , (this.CourseDifficulty == CourseDifficulty.Basic))                             ;
                    parser.SetContentHighlightAtTag( "idCourseDifficultyIntermediate" , (this.CourseDifficulty == CourseDifficulty.Intermediate) , this.HighlightColor);
                    parser.SetContentBoldAtTag(      "idCourseDifficultyIntermediate" , (this.CourseDifficulty == CourseDifficulty.Intermediate))                      ;
                    parser.SetContentHighlightAtTag( "idCourseDifficultyAdvanced"     , (this.CourseDifficulty == CourseDifficulty.Advanced)     , this.HighlightColor);
                    parser.SetContentBoldAtTag(      "idCourseDifficultyAdvanced"     , (this.CourseDifficulty == CourseDifficulty.Advanced))                          ;

                    // Course public
                    parser.SetCheckBoxStateAtTag("idCoursePublicYes" , (this.CourseIsPublic == true));
                    parser.SetCheckBoxStateAtTag("idCoursePublicNo"  , (this.CourseIsPublic != true));

                    // Course designation
                    parser.SetCheckBoxStateAtTag("idCourseNationalInsuranceDesignationYes" , (this.CourseIsDesignation == true));
                    parser.SetCheckBoxStateAtTag("idCourseNationalInsuranceDesignationNo"  , (this.CourseIsDesignation != true));
                    parser.ReplaceContentAtTag(  "idNationalCourseDesignationType"         , (this.CourseDesignationType));

                    // Course exam requirement
                    parser.SetCheckBoxStateAtTag("idCourseExamYes" , (this.CourseRequiresExam == true));
                    parser.SetCheckBoxStateAtTag("idCourseExamNo"  , (this.CourseRequiresExam != true));

                    // Course credits
                    // This is going to be a doozy.

                    // Credits requested, sales:
                    parser.ReplaceContentAtTag("idCreditsRequestedSalesLifeHealth",
                        CourseCredits.CreditsToString(this.RequestedSalesCredits.LifeHealthCredits));
                    parser.ReplaceContentAtTag("idCreditsRequestedSalesPropertyCasualty",
                        CourseCredits.CreditsToString(this.RequestedSalesCredits.PropertyCasualtyCredits));
                    parser.ReplaceContentAtTag("idCreditsRequestedSalesEthics",
                        CourseCredits.CreditsToString(this.RequestedSalesCredits.EthicsCredits));
                    parser.ReplaceContentAtTag("idCreditsRequestedSalesGeneral",
                        CourseCredits.CreditsToString(this.RequestedSalesCredits.GeneralCredits));
                    parser.ReplaceContentAtTag("idCreditsRequestedSalesLaws",
                        CourseCredits.CreditsToString(this.RequestedSalesCredits.LawCredits));
                    parser.ReplaceContentAtTag("idCreditsRequestedSalesOther",
                        CourseCredits.CreditsToString(this.RequestedSalesCredits.OtherCredits));
                    parser.ReplaceContentAtTag("idCreditsRequestedSalesTotal",
                        CourseCredits.CreditsToString(this.RequestedSalesCredits.TotalCredits));
                    parser.ReplaceContentAtTag("idCreditsAdjusterRequestedSales",
                        this.RequestedSalesCredits.AdjusterCreditsToString());

                    // Credits requested, insurance:
                    parser.ReplaceContentAtTag("idCreditsRequestedInsuranceLifeHealth",
                        CourseCredits.CreditsToString(this.RequestedInsuranceCredits.LifeHealthCredits));
                    parser.ReplaceContentAtTag("idCreditsRequestedInsurancePropertyCasualty",
                        CourseCredits.CreditsToString(this.RequestedInsuranceCredits.PropertyCasualtyCredits));
                    parser.ReplaceContentAtTag("idCreditsRequestedInsuranceEthics",
                        CourseCredits.CreditsToString(this.RequestedInsuranceCredits.EthicsCredits));
                    parser.ReplaceContentAtTag("idCreditsRequestedInsuranceGeneral",
                        CourseCredits.CreditsToString(this.RequestedInsuranceCredits.GeneralCredits));
                    parser.ReplaceContentAtTag("idCreditsRequestedInsuranceLaws",
                        CourseCredits.CreditsToString(this.RequestedInsuranceCredits.LawCredits));
                    parser.ReplaceContentAtTag("idCreditsRequestedInsuranceOther",
                        CourseCredits.CreditsToString(this.RequestedInsuranceCredits.OtherCredits));
                    parser.ReplaceContentAtTag("idCreditsRequestedInsuranceTotal",
                        CourseCredits.CreditsToString(this.RequestedInsuranceCredits.TotalCredits));
                    parser.ReplaceContentAtTag("idCreditsAdjusterRequestedInsurance",
                        this.RequestedInsuranceCredits.AdjusterCreditsToString());

                    // Credits approved, sales:
                    parser.ReplaceContentAtTag("idCreditsHomeStateSalesLifeHealth",
                        CourseCredits.CreditsToString(this.ApprovedSalesCredits.LifeHealthCredits));
                    parser.ReplaceContentAtTag("idCreditsHomeStateSalesPropertyCasualty",
                        CourseCredits.CreditsToString(this.ApprovedSalesCredits.PropertyCasualtyCredits));
                    parser.ReplaceContentAtTag("idCreditsHomeStateSalesEthics",
                        CourseCredits.CreditsToString(this.ApprovedSalesCredits.EthicsCredits));
                    parser.ReplaceContentAtTag("idCreditsHomeStateSalesGeneral",
                        CourseCredits.CreditsToString(this.ApprovedSalesCredits.GeneralCredits));
                    parser.ReplaceContentAtTag("idCreditsHomeStateSalesLaws",
                        CourseCredits.CreditsToString(this.ApprovedSalesCredits.LawCredits));
                    parser.ReplaceContentAtTag("idCreditsHomeStateSalesOther",
                        CourseCredits.CreditsToString(this.ApprovedSalesCredits.OtherCredits));
                    parser.ReplaceContentAtTag("idCreditsHomeStateSalesTotal",
                        CourseCredits.CreditsToString(this.ApprovedSalesCredits.TotalCredits));
                    parser.ReplaceContentAtTag("idCreditsAdjusterHomeStateSales",
                        this.ApprovedSalesCredits.AdjusterCreditsToString());

                    // Credits approved, insurance:
                    parser.ReplaceContentAtTag("idCreditsHomeStateInsuranceLifeHealth",
                        CourseCredits.CreditsToString(this.ApprovedInsuranceCredits.LifeHealthCredits));
                    parser.ReplaceContentAtTag("idCreditsHomeStateInsurancePropertyCasualty",
                        CourseCredits.CreditsToString(this.ApprovedInsuranceCredits.PropertyCasualtyCredits));
                    parser.ReplaceContentAtTag("idCreditsHomeStateInsuranceEthics",
                        CourseCredits.CreditsToString(this.ApprovedInsuranceCredits.EthicsCredits));
                    parser.ReplaceContentAtTag("idCreditsHomeStateInsuranceGeneral",
                        CourseCredits.CreditsToString(this.ApprovedInsuranceCredits.GeneralCredits));
                    parser.ReplaceContentAtTag("idCreditsHomeStateInsuranceLaws",
                        CourseCredits.CreditsToString(this.ApprovedInsuranceCredits.LawCredits));
                    parser.ReplaceContentAtTag("idCreditsHomeStateInsuranceOther",
                        CourseCredits.CreditsToString(this.ApprovedInsuranceCredits.OtherCredits));
                    parser.ReplaceContentAtTag("idCreditsHomeStateInsuranceTotal",
                        CourseCredits.CreditsToString(this.ApprovedInsuranceCredits.TotalCredits));
                    parser.ReplaceContentAtTag("idCreditsAdjusterHomeStateInsurance",
                        this.ApprovedInsuranceCredits.AdjusterCreditsToString());

                    // Credits reciprocal, sales:
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateSalesLifeHealth",
                        CourseCredits.CreditsToString(this.ReciprocalSalesCredits.LifeHealthCredits));
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateSalesPropertyCasualty",
                        CourseCredits.CreditsToString(this.ReciprocalSalesCredits.PropertyCasualtyCredits));
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateSalesEthics",
                        CourseCredits.CreditsToString(this.ReciprocalSalesCredits.EthicsCredits));
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateSalesGeneral",
                        CourseCredits.CreditsToString(this.ReciprocalSalesCredits.GeneralCredits));
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateSalesLaws",
                        CourseCredits.CreditsToString(this.ReciprocalSalesCredits.LawCredits));
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateSalesOther",
                        CourseCredits.CreditsToString(this.ReciprocalSalesCredits.OtherCredits));
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateSalesTotal",
                        CourseCredits.CreditsToString(this.ReciprocalSalesCredits.TotalCredits));
                    parser.ReplaceContentAtTag("idCreditsAdjusterReciprocalStateSales",
                        this.ReciprocalSalesCredits.AdjusterCreditsToString());

                    // Credits reciprocal, insurance:
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateInsuranceLifeHealth",
                        CourseCredits.CreditsToString(this.ReciprocalInsuranceCredits.LifeHealthCredits));
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateInsurancePropertyCasualty",
                        CourseCredits.CreditsToString(this.ReciprocalInsuranceCredits.PropertyCasualtyCredits));
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateInsuranceEthics",
                        CourseCredits.CreditsToString(this.ReciprocalInsuranceCredits.EthicsCredits));
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateInsuranceGeneral",
                        CourseCredits.CreditsToString(this.ReciprocalInsuranceCredits.GeneralCredits));
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateInsuranceLaws",
                        CourseCredits.CreditsToString(this.ReciprocalInsuranceCredits.LawCredits));
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateInsuranceOther",
                        CourseCredits.CreditsToString(this.ReciprocalInsuranceCredits.OtherCredits));
                    parser.ReplaceContentAtTag("idCreditsReciprocalStateInsuranceTotal",
                        CourseCredits.CreditsToString(this.ReciprocalInsuranceCredits.TotalCredits));
                    parser.ReplaceContentAtTag("idCreditsAdjusterReciprocalStateInsurance",
                        this.ReciprocalInsuranceCredits.AdjusterCreditsToString());

                    // Course approval information
                    parser.ReplaceContentAtTag("idHomeStateApprovalDate"           , this.CourseHomeStateApprovalDate       );
                    parser.ReplaceContentAtTag("idHomeStateApprovalExpirationDate" , this.CourseHomeStateApprovalExpiration );
                    parser.ReplaceContentAtTag("idHomeStateCourseApprovalNumber"   , this.CourseHomeStateApprovalNumber     );

                    /// If the generator is set to flatten controls,
                    /// then flatten each content control
                    if (this.ShouldFlattenControls == true)
                    {
                        this.Log("Flattening output document content controls...");
                        parser.FlattenAllContent();
                    }

                    this.Log("Closing output document...");
                    outDoc.Close();
                }
            }
            catch(IOException exception)
            {
                // Get exception HResult
                int HResult = exception.HResult;

                // Failed to create output file.
                // Later on we should find the specific
                // reason why using the exception
                // HResult, but for now return
                // a generic error.
                output.Success = false;
                output.ErrorCode = 3;

                return output;
            }

            // Success!
            // Return the result.
            output.Success = true;
            output.ErrorCode = 0;

            return output;
        }

        /**
        \brief
            Writes information to the console.

            Only performed if this generator
            is configured with verbose generation.

        \param output
            String to output to console.
        */
        private void Log(string output)
        {
            // Only output string if
            // verbose output is set
            // to true
            if(this.VerboseGeneration == true)
            {
                // It is. Output.
                Console.WriteLine(output);
            }
        }
    }
}

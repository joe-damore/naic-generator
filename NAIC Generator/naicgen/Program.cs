using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naic
{
    /**
    \brief
        Contains possible exit codes
        produced by this application.
    */
    public enum ExitCode : int
    {
        Success             = 0, // Successfully generated NAIC
        FailureBadPath      = 1, // Failed, given bad output path
        OutputAlreadyExists = 2, // Failed, output file already exists
        TemplateDoesNotExit = 3, // Failed, template file does not exist
        OutputFailure       = 4  // Failed, generic output failure

    }

    public class Program
    {
        /**
        \brief
            Main program code
        */
        public static void Main(string[] args)
        {
            const string programTitle        = "naicgen" ; // NAIC Generator title
            const ushort programVersionMajor = 0         ; // NAIC Generator version, major
            const ushort programVersionMinor = 2         ; // NAIC Generator version, minor
            const ushort programVersionPatch = 0         ; // NAIC Generator version, patch

            // Output program information
            Console.WriteLine(programTitle);
            Console.WriteLine(
                "Version {0}.{1}.{2}",
                programVersionMajor,
                programVersionMinor,
                programVersionPatch);

            // Create a CommandLineArguments object from the
            // given args array
            CommandLineArguments arguments = new CommandLineArguments(args);

            // Create a new NAIC generator instance
            NAICGenerator generator = new NAICGenerator();

            // Get basic program parameters from
            // command line arguments
            string inputPath  = arguments.GetValue("Input"  , "I" , null);
            string outputPath = arguments.GetValue("Output" , "O" , null);

            bool replaceOutput         =  arguments.HasArgument("Replace"                       , "R"   );
            bool verbose               =  arguments.HasArgument("Verbose"                       , "V"   );
            bool help                  =  arguments.HasArgument("Help"                          , "?"   );
            bool shouldFlattenControls = (arguments.HasArgument("PreserveControls"              , "PC"  ) != true);
            bool shrinkProviderName    =  arguments.HasArgument("SmallProviderName"             , "SPN" );
            bool shrinkProviderNumber  =  arguments.HasArgument("SmallReciprocalProviderNumber" , "SRPN");
            bool shrinkCourseTitle     =  arguments.HasArgument("SmallCourseTitle"              , "SCT" );

            // Load provider/course details from command
            // line arguments using generator
            generator.LoadFromArguments(arguments);

            // Configure advanced generation options
            generator.ShouldFlattenControls      = shouldFlattenControls ;
            generator.ShouldShrinkProviderName   = shrinkProviderName    ;
            generator.ShouldShrinkCourseTitle    = shrinkCourseTitle     ;
            generator.ShouldShrinkProviderNumber = shrinkProviderNumber  ;
            generator.VerboseGeneration          = verbose               ;

            // Generate NAIC document
            Result result = generator.Generate(
                inputPath,
                outputPath,
                replaceOutput);

            // Check error code
            switch(result.ErrorCode)
            {
                case 0:
                    // Successful generation.
                    Environment.ExitCode = (int)ExitCode.Success;
                    break;

                case 1:
                    // Output file already exists and no
                    // replace flag was provided
                    Environment.ExitCode = (int)ExitCode.OutputAlreadyExists;
                    break;

                case 2:
                    // Specified template file does not
                    // exist
                    Environment.ExitCode = (int)ExitCode.TemplateDoesNotExit;
                    break;

                case 3:
                    // Failed to create output file.
                    // This could be because the file is
                    // being used by another program, because
                    // the hard drive is out of space, etc.
                    Environment.ExitCode = (int)ExitCode.OutputFailure;
                    break;

                case 4:
                    // No input or output path was
                    // specified.
                    Environment.ExitCode = (int)ExitCode.FailureBadPath;
                    break;

            }

            // Show output result
            if(result.Success == true)
            {
                Environment.ExitCode = (int)ExitCode.Success;
                Console.WriteLine("Successfully generated NAIC document!");

                return;
            }

            Console.WriteLine("Failed to generate NAIC document!");
            Console.WriteLine("Ending execution with exit code " + Environment.ExitCode);
        }
    }
}

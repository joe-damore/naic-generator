using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naic
{
    public class CommandLineArguments
    {
        /// Array of command line arguments
        protected string[] arguments;

        /**
        \brief
            Constructor.

        \param argArray
            Array of command line arguments.
        */
        public CommandLineArguments(string[] argArray)
        {
            // Assign arguments array
            this.arguments = argArray;
        }

        /**
        \brief
            Determines whether or not the given
            argument exists.

            The argument does not need to have
            been given a value for it to return
            true.
            
        \param argumentLong
            Long argument name

        \param argumentShort
            Short argument name

        \return
            True if the argument exists, false
            otherwise.
        */
        public bool HasArgument(
            string argumentLong,
            string argumentShort)
        {
            // Iterate through each argument
            for(uint i = 0; i < this.arguments.Count(); ++i)
            {
                // Check if the argument matches argumentLong
                // or argumentShort
                if ((this.arguments[i] == ("--" + argumentLong)) ||
                   (this.arguments[i] == ("-" + argumentShort)))
                {
                    // Argument exists!
                    // Return true.
                    return true;
                }
            }

            // Argument does not exist
            // Return false.
            return false;
        }

        /**
        \brief
            Gets the value for the given command
            line argument.

        \param argumentLong
            Long argument name

        \param argumentShort
            Argument abbreviation. Use null if no
            short command is supported.

        \param defaultValue
            Default string to be returned, or null

        \return
            The value for the given argument, or
            defaultValue if the argument is not
            specified.
        */
        public string GetValue(
            string argumentLong,
            string argumentShort,
            string defaultValue = "")
        {
            // Iterate through each argument
            for(uint i = 0; i < this.arguments.Count(); ++i)
            {
                // Check if the argument matches argumentLong
                // or argumentShort
                if((this.arguments[i] == ("--" + argumentLong)) ||
                    (this.arguments[i] == ("-" + argumentShort)))
                {
                    // We have a match
                    // See if there is another item
                    // at index (i + 1)
                    if(i + 1 > (this.arguments.Count() - 1))
                    {
                        // No item exists at (i + 1)
                        // Return defaultValue
                        return defaultValue;
                    }

                    // Return the next item
                    return this.arguments[i + 1];
                }
            }

            // No matching argument was found
            // Return defaultValue
            return defaultValue;
        }
    }
}

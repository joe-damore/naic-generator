using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naic
{
    /**
    \brief
        Describes the result of any given
        action.
    */
    public class Result
    {
        /// Determines whether or not
        /// the action was successful
        public bool Success = true;

        /// Determines the specific error
        /// code for non-successful actions.
        public uint ErrorCode = 0;

        /// Array containing descriptions for
        /// each error code.
        public Dictionary<uint, string> ErrorDescriptions;

        /**
        \brief
            Constructor.

            Optionally determines whether the
            result is successful and, if not,
            the error code.

        \param success
            Boolean determining if action was
            successful

        \param errorCode
            Error code describing error if
            action was unsuccessful
        */
        public Result(
            bool success = true,
            uint errorCode = 0)
        {
            // Assign properties using
            // parameters
            this.Success = success;
            this.ErrorCode = errorCode;
            this.ErrorDescriptions = new Dictionary<uint, string>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naic
{
    /**
    \brief
        Represents the amount and type of
        credits being requested or approved
        for a course.
    */
    public class CourseCredits
    {
        /// Number of L/H credits for course
        public ushort LifeHealthCredits;

        /// Number of P&C credits for course
        public ushort PropertyCasualtyCredits;

        /// Number of ethics redits for course
        public ushort EthicsCredits;

        /// Number of general credits for course
        public ushort GeneralCredits;

        /// Number of law/reg credits for course
        public ushort LawCredits;

        /// Number of other credits for course
        public ushort OtherCredits;

        /// Total number of insurance credits
        /// (Must be assigned manually)
        public ushort TotalCredits;

        /// Total number of adjuster credits
        public ushort AdjusterCredits;

        /**
        \brief
            Determines whether or not this object
            is storing any producer credits.
        */
        public bool HasProducerCredits()
        {
            // Return true if the total number of
            // LH, P&C, Ethics, Gen, Law, Other,
            // and total credits is larger than 0.
            return (
                (this.LifeHealthCredits +
                 this.PropertyCasualtyCredits +
                 this.EthicsCredits +
                 this.GeneralCredits +
                 this.LawCredits +
                 this.OtherCredits +
                 this.TotalCredits) > 0);
        }

        /**
        \brief
            Returns the given number of
            credit hours as a string.

            If the credits are 0, a blank
            string is returned.

        \param credits
            Number of credits to display as
            string.
        */
        public static string CreditsToString(ushort credits)
        {
            // Return a blank string if
            // the number of credits is
            // zero.
            if(credits == 0)
            {
                return "";
            }

            // Return the int as a string
            // otherwise.
            return credits.ToString();
        }

        /**
        \brief
            Returns this object's adjuster credits
            as a string.

            The string will be blank if the course
            has 0 adjuster credits and has no
            producer credits.

        \return
            Adjuster credits as string
        */
        public string AdjusterCreditsToString()
        {
            // See if this course has any producer credits
            // and see if it has any adjuster credits
            if(
                this.AdjusterCredits      == 0     &&
                this.HasProducerCredits() == false    )
            {
                // It has neither, so return a blank
                // string
                return "";
            }

            // Return the adjuster credits as a
            // string.
            return this.AdjusterCredits.ToString();
        }
    }
}

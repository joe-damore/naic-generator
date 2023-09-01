using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naic
{
    /**
    \brief
        Describes a requirement that
        a course must meet in order
        for an NAIC document to be
        generated
    */
    public class CreditRequirement
    {
        /// Type of credits that must
        /// meet requirement
        public CreditType Type { get; set; }

        /// Condition type to be met for
        /// requirement
        public RequirementCondition Condition { get; set; }

        /// Number of credits for condition
        /// that must be met
        public ushort Amount { get; set; }
    }
}

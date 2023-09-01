using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace naic
{
    /**
    \brief
        Describes how many credit requirements
        must be met by a course
    */
    public enum CourseCreditRequirementCondition : int
    {
        [Description("At Least One Requirement")]
        [XmlEnum(Name = "0")]
        OneRequirement = 0,

        [Description("All Requirements")]
        [XmlEnum(Name = "1")]
        AllRequirements = 1
    }
}

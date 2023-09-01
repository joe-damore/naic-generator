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
        Describes the type of
        condition that must be
        met for a CreditRequirement
    */
    public enum RequirementCondition
    {
        [Description("More Than")]
        [XmlEnum(Name = "0")]
        GreaterThan = 0,
        [Description("Exactly")]
        [XmlEnum(Name = "1")]
        EqualTo     = 1,
        [Description("Fewer Than")]
        [XmlEnum(Name = "2")]
        FewerThan   = 2
    }
}

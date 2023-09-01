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
        Describes different
        types of continuing
        education credits
    */
    public enum CreditType
    {
        [Description("Life/Health")]
        [XmlEnum(Name = "0")]
        LifeHealth       = 0,

        [Description("Property/Casualty")]
        [XmlEnum(Name = "1")]
        PropertyCasualty = 1,

        [Description("Ethics")]
        [XmlEnum(Name = "2")]
        Ethics           = 2,

        [Description("General")]
        [XmlEnum(Name = "3")]
        General          = 3,

        [Description("Laws")]
        [XmlEnum(Name = "4")]
        Laws             = 4,

        [Description("Other")]
        [XmlEnum(Name = "5")]
        Other            = 5,
        
        [Description("Adjuster")]
        [XmlEnum(Name = "6")]
        Adjuster         = 6
    }
}

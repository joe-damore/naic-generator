using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace naic
{
    /**
    \brief
        Describes the number of
        credits affected by a
        CreditTransformation.
    */
    public enum TransformationAmount
    {
        [XmlEnum(Name = "0")]
        All  = 0,
        [XmlEnum(Name = "1")]
        Half = 1
    }
}

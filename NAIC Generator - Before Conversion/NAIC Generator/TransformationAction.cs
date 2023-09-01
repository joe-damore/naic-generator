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
        Describes the action performed
        by a CreditTransformation
        object
    */
    public enum TransformationAction
    {
        [XmlEnum(Name = "0")]
        Move = 0,
        [XmlEnum(Name = "1")]
        Copy = 1,
        [XmlEnum(Name = "2")]
        Remove = 2
    }
}

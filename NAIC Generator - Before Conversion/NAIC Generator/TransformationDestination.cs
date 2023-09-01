using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace naic
{
    /**
    \brief
        Describes what happens to
        a CreditTransformation
        destination
    */
    public enum TransformationDestination
    {
        [XmlEnum(Name = "0")]
        Into = 0, // New credits are added to
                  // existing credits
        
        [XmlEnum(Name = "1")]
        Over = 1  // New credits overwrite
                  // existing credits
    }
}

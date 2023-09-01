using System.ComponentModel;
using System.Xml.Serialization;

namespace naic
{
    /**
    \brief
        Describes the difficulty
        of a course.
    */
    public enum CourseDifficulty : int
    {
        [Description("None")]
        [XmlEnum(Name = "0")]
        None         = 0,

        [Description("Basic")]
        [XmlEnum(Name = "1")]
        Basic        = 1,

        [Description("Intermediate")]
        [XmlEnum(Name = "2")]
        Intermediate = 2,

        [Description("Advanced")]
        [XmlEnum(Name = "3")]
        Advanced     = 3
    }
}

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
        Describes a type of
        course
    */
    public enum CourseType : int
    {
        [Description("None")]
        [XmlEnum(Name = "0")]
        None                     = 0, // None selected

        [Description("Self-Study: Correspondence")]
        [XmlEnum(Name = "1")]
        SelfStudyCorrespondence  = 1, // Self-Study Correspondence

        [Description("Self-Study: On-Line Training")]
        [XmlEnum(Name = "2")]
        SelfStudyOnlineTraining  = 2, // Self-Study On-Line Training

        [Description("Self-Study: Video/Audio/CD/DVDs")]
        [XmlEnum(Name = "3")]
        SelfStudyVideoAudioCDDVD = 3, // Self-Study Video/Audio/CD/DVDs

        [Description("Classroom: Seminar/Workshop")]
        [XmlEnum(Name = "4")]
        ClassroomSeminarWorkshop = 4, // Classroom Seminar/Workshop

        [Description("Classroom: Webinar")]
        [XmlEnum(Name = "5")]
        ClassroomWebinar         = 5, // Classroom Webinar

        [Description("Classroom: Teleconference")]
        [XmlEnum(Name = "6")]
        ClassroomTeleconference  = 6, // Classroom Teleconference

        [Description("Classroom: Other")]
        [XmlEnum(Name = "7")]
        ClassroomOther           = 7   // Classroom (Other)
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naic
{
    public enum CourseType : int
    {
        None                     = 0 , // None selected
        SelfStudyCorrespondence  = 1 , // Self-Study Correspondence
        SelfStudyOnlineTraining  = 2 , // Self-Study On-Line Training
        SelfStudyVideoAudioCDDVD = 3 , // Self-Study Video/Audio/CD/DVDs
        ClassroomSeminarWorkshop = 4 , // Classroom Seminar/Workshop
        ClassroomWebinar         = 5 , // Classroom Webinar
        ClassroomTeleconference  = 6 , // Classroom Teleconference
        ClassroomOther           = 7   // Classroom (Other)
    }
}

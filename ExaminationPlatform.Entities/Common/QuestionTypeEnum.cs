using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationPlatform.Entities.Common
{
    public enum QuestionTypeEnum
    {
        Invalid,
        RadioUnit = 1,
        MultiSelectUnit = 2,
        SubjectiveUnit = 3,
        ScoreUnit = 4,
        CumulationUnit = 5,
        StatisticsUnit = 6
    }
}

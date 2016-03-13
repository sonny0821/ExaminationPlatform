using ExaminationPlatform.Center.BaseClass;
using ExaminationPlatform.Entities.Common;
using ExaminationPlatform.Entities.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionUnit.Extend
{
    public class StatisticsUnit : BaseUnit
    {
        public StatisticsUnit(string name)
            : base(name)
        {
            _name = name;
        }

        public override Result Calculate(QuestionInstanceEx question)
        {
            throw new NotImplementedException();
        }
    }
}

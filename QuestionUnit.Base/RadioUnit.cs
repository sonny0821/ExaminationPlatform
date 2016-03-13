using ExaminationPlatform.Center.BaseClass;
using ExaminationPlatform.Entities.Common;
using ExaminationPlatform.Entities.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionUnit.Base
{
    public class RadioUnit : BaseUnit
    {
        public RadioUnit(string name)
            : base(name)
        {
            _name = name;
        }

        public override Result Calculate(QuestionInstanceEx question)
        {
            foreach (var option in question.Options)
            {
                if (option.IsSelected != option.Answer)
                {
                    question.Score = 0;
                    break;
                }
            }
            return new Result() { Data = question, IsSuccess = true };
        }
    }
}

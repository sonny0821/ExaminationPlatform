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
    public class MultiSelectUnit : BaseUnit
    {
        public MultiSelectUnit(string name)
            : base(name)
        {
            _name = name;
        }

        public override Result Calculate(QuestionInstanceEx question)
        {
            int unSelectedCount = 0;
            int optionCount = 0;
            float score = question.Score;
            foreach (var option in question.Options)
            {
                if (!option.Answer && option.IsSelected)
                {
                    question.Score = 0;
                }
                if (option.Answer && !option.IsSelected && question.Score == score)
                {
                    question.Score = score / 2;
                }
                if (!option.IsSelected)
                {
                    unSelectedCount++;
                }
                optionCount++;
            }
            if (optionCount == unSelectedCount)
            {
                question.Score = 0;
            }
            return new Result() { Data = question, IsSuccess = true };
        }
    }
}

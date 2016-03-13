using ExaminationPlatform.Center.BaseClass;
using ExaminationPlatform.Entities;
using ExaminationPlatform.Entities.Common;
using ExaminationPlatform.Entities.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionUnit.Base
{
    public class SubjectiveUnit : BaseUnit
    {
        public SubjectiveUnit(string name)
            : base(name)
        {
            _name = name;
        }

        public override QuestionEx ConvertToQuestion(IList<string> info, Guid poolId, Guid userId)
        {
            QuestionEx question = new QuestionEx()
            {
                Id = Guid.NewGuid(),
                Content = info[0],
                Type = int.Parse(info[1]),
                ParentId = Guid.Empty,
                Tag = info[3],
                PoolId = poolId,
                UpdaterId = userId,
                Options = new List<Option>()
            };
            question.Options.Add(new Option()
            {
                Id = Guid.NewGuid(),
                Content = string.Empty,
                Answer = false,
                HasText = false,
                SortIndex = 1,
                QuestionId = question.Id,
                UpdaterId = userId
            });
            return question;
        }
        public override Result Calculate(QuestionInstanceEx question)
        {
            return new Result() { IsSuccess = true };
        }
    }
}

using ExaminationPlatform.Center.BaseClass;
using ExaminationPlatform.Entities;
using ExaminationPlatform.Entities.Common;
using ExaminationPlatform.Entities.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionUnit.Extend
{
    public class CumulationUnit : BaseUnit
    {
        public CumulationUnit(string name)
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
                Tag = info[2],
                PoolId = poolId,
                UpdaterId = userId,
                Options = new List<Option>()
            };
            for (int j = 3; j < info.Count; j = j + 2)
            {
                if (string.IsNullOrEmpty(info[j]))
                {
                    break;
                }
                question.Options.Add(new Option()
                {
                    Id = Guid.NewGuid(),
                    Content = info[j],
                    Answer = false,
                    HasText = false,
                    SortIndex = j - 2,
                    QuestionId = question.Id,
                    Tag = info[j + 1],
                    UpdaterId = userId
                });
            }
            return question;
        }
        public override Result Calculate(QuestionInstanceEx question)
        {
            question.Score = 0;
            foreach (var option in question.Options)
            {
                if (option.IsSelected)
                {
                    question.Score = float.Parse(option.Tag);
                }
            }

            return new Result() { Data = question, IsSuccess = true };
        }
    }
}

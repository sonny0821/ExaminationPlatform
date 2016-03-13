using ExaminationPlatform.Entities;
using ExaminationPlatform.Entities.Common;
using ExaminationPlatform.Entities.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCADB;

namespace ExaminationPlatform.Center.BaseClass
{
    public abstract class BaseUnit
    {
        protected string _name;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        protected TableOperator opTo = new TableOperator();

        public BaseUnit(string name)
        {
            _name = name;
        }

        /// <summary>
        /// 创建题
        /// </summary>
        public virtual Result CreateQuestion(QuestionEx question)
        {
            Result result = new Result() { IsSuccess = false };
            if (opTo.ExecuteNonQuery(false, ConvertToSql(question), "ExamPlatform"))
            {
                result.IsSuccess = true;
            }
            else
            {
                result.SetException(opTo.ex);
            }
            return result;
        }

        /// <summary>
        /// 编辑题
        /// </summary>
        public virtual Result EditQuestion(QuestionEx question)
        {
            Result result = new Result() { IsSuccess = false };
            if (opTo.ExecuteNonQuery(false, ConvertToSql(question), "ExamPlatform"))
            {
                result.IsSuccess = true;
            }
            else
            {
                result.SetException(opTo.ex);
            }
            return result;
        }

        /// <summary>
        /// 删除题
        /// </summary>
        /// <param name="questionId"></param>
        public virtual Result DeleteQuestion(Guid questionId)
        {
            Result result = new Result() { IsSuccess = false };
            string strSql = @"SET XACT_ABORT ON
            BEGIN TRANSACTION
            DELETE FROM T_Option WHERE QuestionId = [@QuestionId]
            DELETE FROM T_Question WHERE Id = [@QuestionId]
            COMMIT";
            if (opTo.ExecuteNonQuery(false, strSql, new object[] { questionId }, "ExamPlatform"))
            {
                result.IsSuccess = true;
            }
            else
            {
                result.SetException(opTo.ex);
            }
            return result;
        }

        /// <summary>
        /// 将题信息转化题对象
        /// </summary>
        /// <param name="info"></param>
        /// <param name="poolId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual QuestionEx ConvertToQuestion(IList<string> info, Guid poolId, Guid userId)
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
            string strAnswer = info[4];
            List<int> answers = new List<int>();
            for (int i = 0; i < strAnswer.Length; i++)
            {
                answers.Add(Convert.ToInt32(strAnswer[i]) - 64);
            }
            for (int j = 5; j < info.Count; j++)
            {
                if (string.IsNullOrEmpty(info[j]))
                {
                    break;
                }
                question.Options.Add(new Option()
                {
                    Id = Guid.NewGuid(),
                    Content = info[j],
                    Answer = answers.Contains(j - 4),
                    HasText = false,
                    SortIndex = j - 4,
                    QuestionId = question.Id,
                    UpdaterId = userId
                });
            }
            return question;
        }

        /// <summary>
        /// 将题对象转化Sql语句
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public virtual string ConvertToSql(QuestionEx question)
        {
            List<string> strInsertOptions = new List<string>();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("SET XACT_ABORT ON");
            strSql.AppendLine("BEGIN TRANSACTION");
            if (Guid.Equals(Guid.Empty, question.Id))
            {
                question.Id = Guid.NewGuid();
                strSql.AppendLine(string.Format(
                "INSERT INTO T_Question (Id,Content,Type,Tag,ParentId,PoolId,UpdaterId) VALUES (N'{0}',N'{1}',{2},N'{3}',N'{4}',N'{5}',N'{6}')",
                question.Id, question.Content, question.Type, question.Tag, question.ParentId, question.PoolId, question.UpdaterId));
                for (int i = 0; i < question.Options.Count; i++)
                {
                    var option = question.Options[i];
                    if (Guid.Equals(Guid.Empty, option.Id))
                    {
                        option.Id = Guid.NewGuid();
                    }
                    strInsertOptions.Add(string.Format("(N'{0}',N'{1}',{2},{3},{4},N'{5}',N'{6}',N'{7}')"
                        , option.Id, option.Content, option.Answer ? 1 : 0, option.HasText ? 1 : 0, i, option.Tag, question.Id, option.UpdaterId));
                }
                if (strInsertOptions.Count > 0)
                {
                    strSql.AppendLine(string.Format("INSERT INTO T_Option (Id,Content,Answer,HasText,SortIndex,Tag,QuestionId,UpdaterId) VALUES {0}", string.Join(",", strInsertOptions)));
                }
            }
            else
            {
                strSql.AppendLine(string.Format("UPDATE T_Question SET Content=N'{0}',Tag=N'{1}',Type={2} WHERE Id=N'{3}'", question.Content, question.Tag, question.Type, question.Id));
                strSql.AppendLine(string.Format("DELETE T_Option WHERE QuestionId = N'{0}'", question.Id));
                for (int i = 0; i < question.Options.Count; i++)
                {
                    var option = question.Options[i];
                    if (Guid.Equals(Guid.Empty, option.Id))
                    {
                        option.Id = Guid.NewGuid();
                    }
                    strInsertOptions.Add(string.Format("(N'{0}',N'{1}',{2},{3},{4},N'{5}',N'{6}',N'{7}')"
                        , option.Id, option.Content, option.Answer ? 1 : 0, option.HasText ? 1 : 0, i, option.Tag, question.Id, option.UpdaterId));
                }
                if (strInsertOptions.Count > 0)
                {
                    strSql.AppendLine(string.Format("INSERT INTO T_Option (Id,Content,Answer,HasText,SortIndex,Tag,QuestionId,UpdaterId) VALUES {0}", string.Join(",", strInsertOptions)));
                }
            }
            strSql.AppendLine("COMMIT");
            return strSql.ToString();
        }

        /// <summary>
        /// 计算题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public abstract Result Calculate(QuestionInstanceEx question);
    }
}

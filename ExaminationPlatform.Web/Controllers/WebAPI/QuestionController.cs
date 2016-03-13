using ExaminationPlatform.Center;
using ExaminationPlatform.Entities;
using ExaminationPlatform.Entities.Common;
using ExaminationPlatform.Entities.Extend;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExaminationPlatform.Web.Controllers.WebAPI
{
    public class QuestionController : BaseAPIController
    {
        [HttpGet]
        public async Task<Result> GetQuestionList(string name, string poolIds, int pageIndex, int rowCount)
        {
            return await Task.Run<Result>(() =>
                {
                    Result result = new Result();
                    List<object> datas = new List<object>();
                    int questionCount = 0;
                    string poolIdCode = string.Empty;
                    List<object> param = new List<object>() { string.Concat("%", name, "%") };

                    if (!string.IsNullOrEmpty(poolIds))
                    {
                        var pools = poolIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (pools.Length > 0)
                        {
                            List<string> poolCodes = new List<string>();
                            for (var i = 0; i < pools.Length; i++)
                            {
                                poolCodes.Add(string.Format("N'{0}'", pools[i]));
                            }
                            poolIdCode = string.Format(" AND PoolId IN ({0})", string.Join(",", poolCodes));
                        }
                    }
                    param.Add((pageIndex - 1) * rowCount);
                    param.Add(pageIndex * rowCount);
                    string strSql = string.Format(@"SELECT 
                    ROW_NUMBER() OVER(ORDER BY A.CreateTime DESC) RowId
                    ,A.Id
                    ,A.Content
                    ,A.[Type]
                    ,B.Name QuestionPoolName
                    ,A.Tag
                    ,A.CreateTime 
                    INTO #TempData
                    FROM T_Question A
                    INNER JOIN T_QuestionPool B ON A.PoolId = B.Id
                    WHERE A.Content LIKE [@Content]{0} AND A.IsDelete = 0

                    SELECT * FROM #TempData T
                    WHERE RowId BETWEEN [@Start] AND [@End]

                    SELECT COUNT(1) QuestionCount FROM #TempData
                    DROP TABLE #TempData", poolIdCode);
                    if (opTo.ReturnDS(false, strSql, param.ToArray(), "ExamPlatform") && opTo.Ds.Tables.Count == 2)
                    {
                        foreach (DataRow data in opTo.Ds.Tables[0].Rows)
                        {
                            var type = (QuestionTypeEnum)int.Parse(data["Type"].ToString());
                            datas.Add(new
                            {
                                Id = data["Id"].ToString(),
                                Content = data["Content"].ToString(),
                                Type = type,
                                TypeValue = ControlCenter.Instance.GetUnitName(type),
                                QuestionPoolName = data["QuestionPoolName"].ToString(),
                                CreateTime = data["CreateTime"].ToString(),
                                Tag = data["Tag"].ToString()
                            });
                        }
                        questionCount = int.Parse(opTo.Ds.Tables[1].Rows[0]["QuestionCount"].ToString());
                    }
                    else
                    {
                        result.SetException(opTo.ex);
                    }
                    return new Result
                    {
                        IsSuccess = true,
                        Data = new
                        {
                            Questions = datas,
                            QuestionCount = questionCount
                        }
                    };
                });
        }

        [HttpGet]
        public async Task<Result> GetQuestion(Guid id)
        {
            return await Task.Run<Result>(() =>
                {
                    Result result = new Result();
                    string strSql = @"SELECT A.Id QuestionId,A.Content QuestionContent,A.[Type],A.Tag QuestionTag,
                    B.Id OptionId,B.Content OptionContent,B.Answer,B.HasText,B.SortIndex,B.Tag OptionTag 
                    FROM T_Question A 
                    LEFT JOIN T_Option B ON A.Id = B.QuestionId
                    WHERE A.Id = [@QuestionId]
                    ORDER BY B.SortIndex";

                    if (opTo.ReturnDS(false, strSql, new object[] { id }, "ExamPlatform") && opTo.Ds.Tables.Count > 0)
                    {
                        var datas = opTo.Ds.Tables[0].Rows;
                        var options = new List<object>();
                        object question = new
                        {
                            Id = Guid.Parse(datas[0]["QuestionId"].ToString()),
                            Content = datas[0]["QuestionContent"].ToString(),
                            Type = int.Parse(datas[0]["Type"].ToString()),
                            Tag = datas[0]["QuestionTag"].ToString(),
                            Options = options
                        };
                        foreach (DataRow data in datas)
                        {
                            if (data["OptionId"] != DBNull.Value)
                            {
                                options.Add(new
                                {
                                    Id = Guid.Parse(data["OptionId"].ToString()),
                                    Content = data["OptionContent"].ToString(),
                                    Answer = bool.Parse(data["Answer"].ToString()),
                                    HasText = bool.Parse(data["HasText"].ToString()),
                                    SortIndex = int.Parse(data["SortIndex"].ToString()),
                                    Tag = data["OptionTag"].ToString()
                                });
                            }
                        }
                        result.Data = question;
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.SetException(opTo.ex);
                    }
                    return result;
                });
        }

        [HttpPost]
        public async Task<Result> CreateQuestion([FromBody]QuestionEx question)
        {
            return await ControlCenter.Instance.CreateQuestionAsync(question);
        }

        [HttpPut]
        public async Task<Result> EditQuestionPool([FromBody]QuestionEx question)
        {
            return await ControlCenter.Instance.EditQuestionAsync(question);
        }

        [HttpDelete]
        public async Task<Result> DeleteQuestionPool(Guid id, int type)
        {
            return await ControlCenter.Instance.DeleteQuestionAsync(id, (QuestionTypeEnum)type);
        }
    }
}

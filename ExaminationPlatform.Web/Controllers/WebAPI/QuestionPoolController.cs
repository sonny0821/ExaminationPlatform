using ExaminationPlatform.Center;
using ExaminationPlatform.Entities;
using ExaminationPlatform.Entities.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExaminationPlatform.Web.Controllers.WebAPI
{
    public class QuestionPoolController : BaseAPIController
    {
        [HttpGet]
        public async Task<Result> GetQuestionPoolList(string name)
        {
            return await Task<Result>.Run(() =>
            {
                Result result = new Result();
                List<object> pools = new List<object>();
                string strSql = @"SELECT Id,Name,Tag,Summary,ISNULL(QCount,0) QCount
                FROM T_QuestionPool A 
                LEFT JOIN (SELECT PoolId,COUNT(1) QCount FROM T_Question GROUP BY PoolId) B ON A.Id = B.PoolId
                WHERE Name LIKE [@Name] AND IsDelete = 0
	            ORDER BY CreateTime DESC";
                if (opTo.ReturnDS(false, strSql, new object[] { string.Format("%{0}%", name) }, "ExamPlatform") && opTo.Ds.Tables.Count > 0)
                {
                    foreach (DataRow data in opTo.Ds.Tables[0].Rows)
                    {
                        pools.Add(new
                        {
                            Id = data["Id"].ToString(),
                            Name = data["Name"].ToString(),
                            Tag = data["Tag"].ToString(),
                            Summary = data["Summary"].ToString(),
                            QCount = data["QCount"].ToString()
                        });
                    }
                    result.IsSuccess = true;
                    result.Data = pools;
                }
                else
                {
                    result.SetException(opTo.ex);
                }
                return result;
            });
        }

        [HttpPost]
        public async Task<Result> CreateQuestionPool([FromBody]QuestionPool pool)
        {
            return await ControlCenter.Instance.CreateQuestionPoolAsync(pool);
        }

        [HttpPut]
        public async Task<Result> EditQuestionPool([FromBody]QuestionPool pool)
        {
            return await ControlCenter.Instance.EditQuestionPoolAsync(pool);
        }

        [HttpDelete]
        public async Task<Result> DeleteQuestionPool(Guid id)
        {
            return await ControlCenter.Instance.DeleteQuestionPoolAsync(id);
        }
    }
}

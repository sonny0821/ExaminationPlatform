using ExaminationPlatform.Center;
using ExaminationPlatform.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExaminationPlatform.Web.Controllers.WebAPI
{
    public class QuestionTypeController : BaseAPIController
    {
        public async Task<Result> GetQuestionTypeList()
        {
            return await Task.Run<Result>(() =>
            {
                var units = ControlCenter.Instance.GetUnitList();
                return new Result
                {
                    IsSuccess = units.Count > 0,
                    Data = units.Select(u =>
                    {
                        return new
                        {
                            Value = u.Key,
                            Name = u.Value
                        };
                    })
                };
            });
        }
    }
}

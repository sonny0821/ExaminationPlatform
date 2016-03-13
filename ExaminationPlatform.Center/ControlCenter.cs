using ExaminationPlatform.Center.BaseClass;
using ExaminationPlatform.Entities;
using ExaminationPlatform.Entities.Common;
using ExaminationPlatform.Entities.Extend;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UCADB;

namespace ExaminationPlatform.Center
{
    public class ControlCenter
    {
        private TableOperator opTo = new TableOperator();
        private Dictionary<QuestionTypeEnum, BaseUnit> unitDic = new Dictionary<QuestionTypeEnum, BaseUnit>();
        private static ControlCenter _instance;
        private static string locker = string.Empty;

        /// <summary>
        /// 中央处理单例
        /// </summary>
        public static ControlCenter Instance
        {
            get
            {
                lock (locker)
                {
                    if (_instance == null)
                    {
                        _instance = new ControlCenter();
                    }
                    return _instance;
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private ControlCenter()
        {
            InitControlSettings();
        }

        /// <summary>
        /// 初始化配置文件以及题单元
        /// </summary>
        public void InitControlSettings()
        {
            try
            {
                lock (locker)
                {
                    XmlDocument config = new XmlDocument();
                    string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\Assemblies.xml");
                    if (File.Exists(configPath))
                    {
                        config.Load(configPath);
                        var questions = config.GetElementsByTagName("Question");
                        foreach (XmlNode question in questions)
                        {
                            string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, question.Attributes["path"].Value);
                            if (File.Exists(dllPath))
                            {
                                Assembly assembly = Assembly.LoadFrom(dllPath);
                                foreach (XmlNode unit in question.ChildNodes)
                                {
                                    var type = (QuestionTypeEnum)int.Parse(unit.Attributes["questionType"].Value);
                                    if (!unitDic.ContainsKey(type))
                                    {
                                        var instance = (BaseUnit)assembly.CreateInstance(unit.InnerText, true
                                            , BindingFlags.CreateInstance, null, new object[] { unit.Attributes["questionName"].Value }, null, null);
                                        if (instance != null)
                                        {
                                            unitDic.Add(type, instance);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new FileLoadException("配置文件路径异常。");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取题型名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public string GetUnitName(QuestionTypeEnum type)
        {
            if (unitDic.ContainsKey(type))
            {
                return unitDic[type].Name;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取题型列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<QuestionTypeEnum, string> GetUnitList()
        {
            var units = new Dictionary<QuestionTypeEnum, string>();
            foreach (var unit in unitDic)
            {
                units.Add(unit.Key, unit.Value.Name);
            }
            return units;
        }

        /// <summary>
        /// 创建试题库
        /// </summary>
        /// <param name="pool"></param>
        public async Task<Result> CreateQuestionPoolAsync(QuestionPool pool)
        {
            return await Task.Run<Result>(() =>
                {
                    Result result = new Result();
                    if (Guid.Equals(Guid.Empty, pool.Id))
                    {
                        pool.Id = Guid.NewGuid();
                    }
                    string strSql = @"INSERT INTO [dbo].[T_QuestionPool] (Id,Name,Summary,Tag,UpdaterId) VALUES ([@Id],[@Name],[@Summary],[@Tag],[@UpdaterId])";
                    if (opTo.ExecuteNonQuery(false, strSql, new object[] { pool.Id, pool.Name, string.IsNullOrEmpty(pool.Summary) ? string.Empty : pool.Summary, string.IsNullOrEmpty(pool.Tag) ? string.Empty : pool.Tag, pool.UpdaterId }, "ExamPlatform"))
                    {
                        result.IsSuccess = true;
                        result.Data = pool;
                    }
                    else
                    {
                        result.SetException(opTo.ex);
                    }
                    return result;
                });
        }

        /// <summary>
        /// 编辑试题库
        /// </summary>
        /// <param name="pool"></param>
        /// <returns></returns>
        public async Task<Result> EditQuestionPoolAsync(QuestionPool pool)
        {
            return await Task.Run<Result>(() =>
                {
                    Result result = new Result();
                    string strSql = @"UPDATE [dbo].[T_QuestionPool] SET Name=[@Name],Summary=[@Summary],Tag=[@Tag],UpdaterId=[@UpdaterId] WHERE Id = [@Id]";
                    if (opTo.ExecuteNonQuery(false, strSql, new object[] { pool.Name, string.IsNullOrEmpty(pool.Summary) ? string.Empty : pool.Summary, string.IsNullOrEmpty(pool.Tag) ? string.Empty : pool.Tag, pool.UpdaterId, pool.Id }, "ExamPlatform"))
                    {
                        result.IsSuccess = true;
                        result.Data = pool;
                    }
                    else
                    {
                        result.SetException(opTo.ex);
                    }
                    return result;
                });
        }

        /// <summary>
        /// 删除试题库
        /// </summary>
        /// <param name="pool"></param>
        public async Task<Result> DeleteQuestionPoolAsync(Guid poolId)
        {
            return await Task.Run<Result>(() =>
                {
                    Result result = new Result();
                    string strSql = @"SET XACT_ABORT ON
                    BEGIN TRANSACTION
                    UPDATE [dbo].[T_QuestionPool] SET IsDelete=1 WHERE Id=[@PoolId]
                    UPDATE [dbo].[T_Question] SET IsDelete=1 WHERE PoolId=[@PoolId]
                    COMMIT";
                    if (opTo.ExecuteNonQuery(false, strSql, new object[] { poolId }, "ExamPlatform"))
                    {
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.SetException(opTo.ex);
                    }
                    return result;
                });
        }

        /// <summary>
        /// 导入题目
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="poolId"></param>
        /// <param name="userId"></param>
        public Result ImportQuestions(Stream fileStream, Guid poolId, Guid userId)
        {
            Result result = new Result();
            XSSFWorkbook workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = workbook.GetSheetAt(0);
            IRow rowTag = sheet.GetRow(0);
            IRow rowHeader = sheet.GetRow(1);
            string type = rowTag.GetCell(rowTag.FirstCellNum).ToString();
            StringBuilder strSql = new StringBuilder();
            for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row.GetCell(0) == null)
                {
                    continue;
                }
                else
                {
                    if (string.IsNullOrEmpty(row.GetCell(0).ToString().Trim()))
                    {
                        continue;
                    }
                }
                int qType;
                if (int.TryParse(row.GetCell(1).ToString(), out qType))
                {
                    List<string> info = new List<string>();
                    for (int j = 0; j < row.PhysicalNumberOfCells; j++)
                    {
                        info.Add(row.GetCell(j, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString());
                    }
                    strSql.AppendLine(unitDic[(QuestionTypeEnum)qType].ConvertToSql(unitDic[(QuestionTypeEnum)qType].ConvertToQuestion(info, poolId, userId)));
                }
                else 
                {

                }
            }
            opTo.ExecuteNonQuery(false, strSql.ToString(), "ExamPlatform");
            return result;
        }

        /// <summary>
        /// 创建试题
        /// </summary>
        /// <param name="question"></param>
        public async Task<Result> CreateQuestionAsync(QuestionEx question)
        {
            return await Task.Run<Result>(() =>
                {
                    return unitDic[(QuestionTypeEnum)question.Type].CreateQuestion(question);
                });
        }

        /// <summary>
        /// 编辑试题
        /// </summary>
        /// <param name="question"></param>
        public async Task<Result> EditQuestionAsync(QuestionEx question)
        {
            return await Task.Run<Result>(() =>
                {
                    return unitDic[(QuestionTypeEnum)question.Type].EditQuestion(question);
                });
        }

        /// <summary>
        /// 删除试题
        /// </summary>
        /// <param name="question"></param>
        public async Task<Result> DeleteQuestionAsync(Guid id, QuestionTypeEnum type)
        {
            return await Task.Run<Result>(() =>
                {
                    return unitDic[type].DeleteQuestion(id);
                });
        }

        ///// <summary>
        ///// 计算试卷结果
        ///// </summary>
        ///// <param name="paperId">试卷Id</param>
        ///// <param name="examType">试卷类型</param>
        ///// <returns>试卷结果</returns>
        //public Result Calculate(Guid paperId, int examType)
        //{
        //    var exam = ExaminationFactory.ExaminationCreator((ExaminationTypeEnum)examType);
        //    exam.GetResult(paperId, unitDic);
        //    return new Result();
        //}

        /// <summary>
        /// 创建试卷模板
        /// </summary>
        /// <param name="exam">试卷模板实体</param>
        public void CreateExaminationModel(ExamEx exam)
        {
            if (Guid.Equals(Guid.Empty, exam.Id))
            {
                exam.Id = Guid.NewGuid();
            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine(string.Format(@"SET XACT_ABORT ON
            BEGIN TRANSACTION
            INSERT INTO T_Exam (Id,Title,Summary,IsDisorder,UpdaterId) VALUES (N'{0}',N'{1}',N'{2}',{3},N'{4}')", exam.Id, exam.Title, exam.Summary, exam.IsDisorder, exam.UpdaterId));

            List<string> strInsertGroups = new List<string>();
            List<string> strInsertGQRs = new List<string>();
            foreach (var group in exam.Groups)
            {
                if (Guid.Equals(Guid.Empty, group.Id))
                {
                    group.Id = Guid.NewGuid();
                }
                strInsertGroups.Add(string.Format("(N'{0}',N'{1}',N'{2}',{3},{4},N'{5}',N'{6}',N'{7}')"
                    , group.Id, group.Name, group.Summary, group.IsRadom ? 1 : 0, group.SortIndex, group.ParentId, exam.Id, group.UpdaterId));
                foreach (var question in group.Questions)
                {
                    if (Guid.Equals(Guid.Empty, question.Id))
                    {
                        question.Id = Guid.NewGuid();
                    }
                    strInsertGQRs.Add(string.Format("(N'{0}',N'{1}',N'{2}',N'{3}',{4},{5},{6},N'{7}')"
                        , question.Id, group.Id, question.Id, question.DimensionGroupId, question.IsRequired ? 1 : 0, question.Score, question.SortIndex, question.UpdaterId, DateTime.Now));
                }
            }
            if (strInsertGroups.Count > 0)
            {
                strSql.AppendLine(string.Format("INSERT INTO T_Group (Id,Name,Summary,IsRadom,SortIndex,ParentId,ExamId,UpdaterId) VALUES {0}", string.Join(",", strInsertGroups)));
            }
            if (strInsertGQRs.Count > 0)
            {
                strSql.AppendLine(string.Format("INSERT INTO T_GroupQuestion (Id,GroupId,QuestionId,DimensionGroupId,IsRequired,Score,SortIndex,UpdaterId) VALUES {0}", string.Join(",", strInsertGQRs)));
            }
            strSql.AppendLine("COMMIT");
            opTo.ExecuteNonQuery(false, strSql.ToString(), "ExamPlatform");
        }

        /// <summary>
        /// 编辑试卷模板
        /// </summary>
        /// <param name="exam">试卷模板实体</param>
        public void EditExaminationModel(ExamEx exam)
        {
            if (Guid.Equals(Guid.Empty, exam.Id))
            {
                exam.Id = Guid.NewGuid();
            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine(string.Format(@"SET XACT_ABORT ON
            BEGIN TRANSACTION
            INSERT INTO T_Exam (Id,Title,Summary,IsDisorder,UpdaterId) VALUES (N'{0}',N'{1}',N'{2}',{3},N'{4}')", exam.Id, exam.Title, exam.Summary, exam.IsDisorder, exam.UpdaterId));
            foreach (var group in exam.Groups)
            {
                if (Guid.Equals(Guid.Empty, group.Id))
                {
                    group.Id = Guid.NewGuid();
                }
                strSql.AppendLine(string.Format("INSERT INTO T_Group (Id,Name,Summary,IsRadom,SortIndex,ParentId,ExamId,UpdaterId) VALUES ('{0}','{1}',{2},{3},{4},'{5}','{6}','{7}','{8}')",
                    group.Id, group.Name, group.Summary, group.IsRadom ? 1 : 0, group.SortIndex, group.ParentId, exam.Id, group.UpdaterId));
                foreach (var question in group.Questions)
                {
                    if (Guid.Equals(Guid.Empty, question.Id))
                    {
                        question.Id = Guid.NewGuid();
                    }
                    strSql.AppendLine(string.Format("INSERT INTO T_GroupQuestion (Id,GroupId,QuestionId,DimensionGroupId,IsOrder,Score,DispOrder,UpdaterId,UpdateTime) VALUES ('{0}','{1}','{2}','{3}',{4},{5},{6},'{7}','{8}')",
                        question.Id, group.Id, question.Id, question.DimensionGroupId, question.IsRequired ? 1 : 0, question.Score, question.SortIndex, question.UpdaterId, DateTime.Now));
                }
            }
            strSql.AppendLine("COMMIT");
            opTo.ExecuteNonQuery(false, strSql.ToString(), "ExamPlatform");
        }

        /// <summary>
        /// 删除试卷模板
        /// </summary>
        /// <param name="examId">试卷模板Id</param>
        public void DeleteExaminationModel(Guid examId)
        {
            string strSql = @"SET XACT_ABORT ON
                    BEGIN TRANSACTION
                    DELETE FROM T_GroupQuestion WHERE GroupId IN (SELECT Id FROM T_Group WHERE ExamId = [@Id])
                    DELETE FROM T_Group WHERE ExamId = [@Id]
                    DELETE FROM T_Exam WHERE Id = [@Id]
                    COMMIT";
            opTo.ExecuteNonQuery(false, strSql, new object[] { examId }, "ExamPlatform");
        }

        //        /// <summary>
        //        /// 创建考试安排
        //        /// </summary>
        //        /// <param name="plan">安排信息</param>
        //        public void CreateExaminationPlan(ExamBatch plan)
        //        {
        //            opTo.InsertER(plan, "T_ExamBatch", "ExamPlatform", new string[] { "CreateTime" }, new string[] { "CreateTime" });
        //        }

        //        /// <summary>
        //        /// 生产试卷
        //        /// </summary>
        //        /// <param name="plan">考试安排信息</param>
        //        /// <param name="papersCount">诗卷数</param>
        //        public void GeneratePapers(Guid planId, Guid updaterId, int papersCount)
        //        {
        //            opTo.ExecuteNonQuery(true, "GeneratePapers", new object[] { planId, papersCount, updaterId }, "ExamPlatform");
        //        }

        //        /// <summary>
        //        /// 删除考试安排
        //        /// </summary>
        //        /// <param name="planId">安排Id</param>
        //        public Result DeleteExaminationPlan(Guid planId)
        //        {
        //            Result result = new Result() { IsSuccess = false };
        //            string strSql = @"
        //            SELECT A.BatchId,A.Id ExamId,B.Id GroupId,C.Id QuestionId,D.Id OptionGroupId,E.Id OptionId 
        //            INTO #PaperId
        //            FROM T_ExamInstance A
        //            INNER JOIN T_GroupInstance B ON A.Id = B.ExamInstanceId
        //            INNER JOIN T_QuestionInstance C ON B.Id = C.GroupInstanceId
        //            INNER JOIN T_OptionGroupInstance D ON C.Id = D.QuestionInstanceId
        //            INNER JOIN T_OptionInstance E ON D.Id = E.GroupInstanceId
        //            WHERE A.BatchId = [@BatchId]
        //
        //            SET XACT_ABORT ON
        //            BEGIN TRANSACTION
        //                DELETE FROM T_OptionInstance WHERE Id IN (SELECT OptionId FROM #PaperId)
        //                DELETE FROM T_OptionGroupInstance WHERE Id IN (SELECT OptionGroupId FROM #PaperId)
        //                DELETE FROM T_QuestionInstance WHERE Id IN (SELECT QuestionId FROM #PaperId)
        //                DELETE FROM T_GroupInstance WHERE Id IN (SELECT GroupId FROM #PaperId)
        //                DELETE FROM T_ExamInstance WHERE Id IN (SELECT ExamId FROM #PaperId)
        //                DELETE FROM T_ExamBatch WHERE Id IN (SELECT BatchId FROM #PaperId)
        //            COMMIT";
        //            if (opTo.ExecuteNonQuery(false, strSql, new object[] { planId }, "ExamPlatform"))
        //            {
        //                result.IsSuccess = true;
        //            }
        //            return result;
        //        }

        //        /// <summary>
        //        /// 重置试卷
        //        /// </summary>
        //        /// <param name="paperId">试卷Id</param>
        //        public void ResetPaper(Guid paperId)
        //        {
        //            opTo.ExecuteNonQuery(true, "ResetPaper", new object[] { paperId, Guid.Empty }, "ExamPlatform");
        //        }

        //        /// <summary>
        //        /// 备份试卷
        //        /// </summary>
        //        /// <param name="id">考试安排Id或者试卷Id</param>
        //        public void BackupPapers(Guid id)
        //        {
        //            opTo.ExecuteNonQuery(true, "BackupPapers", new object[] { id }, "ExamPlatform");
        //        }
    }
}

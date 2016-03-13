using System;

namespace ExaminationPlatform.Entities
{
    public class ExamBatch
    {
        private Guid id;

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }        
        private int examType;

        public int ExamType
        {
            get { return examType; }
            set { examType = value; }
        }
        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private DateTime endTime;

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }        
        private bool isTimeLimit;

        public bool IsTimeLimit
        {
            get { return isTimeLimit; }
            set { isTimeLimit = value; }
        }
        private int timeLimit;

        public int TimeLimit
        {
            get { return timeLimit; }
            set { timeLimit = value; }
        }

        private string projectCode;

        public string ProjectCode
        {
            get { return projectCode; }
            set { projectCode = value; }
        }

        private Guid examId;

        public Guid ExamId
        {
            get { return examId; }
            set { examId = value; }
        }
        private string resvField1;

        public string ResvField1
        {
            get { return resvField1; }
            set { resvField1 = value; }
        }
        private Guid updaterId;

        public Guid UpdaterId
        {
            get { return updaterId; }
            set { updaterId = value; }
        }
        private DateTime createTime;

        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
        private DateTime updateTime;

        public DateTime UpdateTime
        {
            get { return updateTime; }
            set { updateTime = value; }
        }
    }
}
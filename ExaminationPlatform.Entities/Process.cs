using System;

namespace ExaminationPlatform.Entities
{
    public class Process
    {
        private Guid id;

        public Guid Id
        {
            get
            { return id; }
            set
            { id = value; }
        }
        private string examId;

        public string ExamId
        {
            get
            { return examId; }
            set
            { examId = value; }
        }
        private string questionId;

        public string QuestionId
        {
            get
            { return questionId; }
            set
            { questionId = value; }
        }
        private string behavior;

        public string Behavior
        {
            get
            { return behavior; }
            set
            { behavior = value; }
        }
        private Guid updaterId;

        public Guid UpdaterId
        {
            get
            { return updaterId; }
            set
            { updaterId = value; }
        }
        private DateTime createTime;

        public DateTime CreateTime
        {
            get
            { return createTime; }
            set
            { createTime = value; }
        }
        private DateTime updateTime;

        public DateTime UpdateTime
        {
            get
            { return updateTime; }
            set
            { updateTime = value; }
        }
    }
}
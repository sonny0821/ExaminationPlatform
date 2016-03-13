using System;

namespace ExaminationPlatform.Entities
{
    public class Exam
    {
        private Guid id;
        public Guid Id
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private string title;
        public string Title
        {
            get
            { return title; }
            set
            { title = value; }
        }

        private string summary;
        public string Summary
        {
            get
            { return summary; }
            set
            { summary = value; }
        }

        private bool isDisorder;
        public bool IsDisorder
        {
            get
            { return isDisorder; }
            set
            { isDisorder = value; }
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
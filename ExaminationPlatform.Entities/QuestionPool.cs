using System;

namespace ExaminationPlatform.Entities
{
    public class QuestionPool
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
        private string summary;

        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }        
        private string tag;

        public string Tag
        {
            get { return tag; }
            set { tag = value; }
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
        private bool isDelete;

        public bool IsDelete
        {
            get { return isDelete; }
            set { isDelete = value; }
        }
    }
}
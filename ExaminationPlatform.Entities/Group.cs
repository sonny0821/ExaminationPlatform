using System;

namespace ExaminationPlatform.Entities
{
    public class Group
    {
        private Guid id;
        public Guid Id
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private string name;
        public string Name
        {
            get
            { return name; }
            set
            { name = value; }
        }

        private string summary;
        public string Summary
        {
            get
            { return summary; }
            set
            { summary = value; }
        }

        private bool isRadom;
        public bool IsRadom
        {
            get
            { return isRadom; }
            set
            { isRadom = value; }
        }

        private int sortIndex;
        public int SortIndex
        {
            get
            { return sortIndex; }
            set
            { sortIndex = value; }
        }

        private Guid parentId;
        public Guid ParentId
        {
            get
            { return parentId; }
            set
            { parentId = value; }
        }

        private Guid examId;
        public Guid ExamId
        {
            get
            { return examId; }
            set
            { examId = value; }
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
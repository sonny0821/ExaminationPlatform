using System;

namespace ExaminationPlatform.Entities
{
    public class DimensionGroup
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
using System;

namespace ExaminationPlatform.Entities
{
    public class Dimension
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
        private int dispOrder;

        public int DispOrder
        {
            get
            { return dispOrder; }
            set
            { dispOrder = value; }
        }
        private Guid groupId;

        public Guid GroupId
        {
            get
            { return groupId; }
            set
            { groupId = value; }
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
using ExaminationPlatform.Entities.Common;
using System;

namespace ExaminationPlatform.Entities
{
    public class Question
    {
        private Guid id;

        public Guid Id
        {
            get
            { return id; }
            set
            { id = value; }
        }
        private string content;

        public string Content
        {
            get
            { return content; }
            set
            { content = value; }
        }

        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        private string tag;

        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        private Guid parentId;

        public Guid ParentId
        {
            get
            { return parentId; }
            set
            { parentId = value; }
        }
        private Guid poolId;

        public Guid PoolId
        {
            get
            { return poolId; }
            set
            { poolId = value; }
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
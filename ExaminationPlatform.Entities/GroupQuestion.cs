using System;

namespace ExaminationPlatform.Entities
{
    public class GroupQuestion
    {
        private Guid id;
        public Guid Id
        {
            get
            { return id; }
            set
            { id = value; }
        }

        private Guid groupId;
        public Guid GroupId
        {
            get
            { return groupId; }
            set
            { groupId = value; }
        }

        private Guid questionId;
        public Guid QuestionId
        {
            get
            { return questionId; }
            set
            { questionId = value; }
        }

        private Guid dimensionGroupId;
        public Guid DimensionGroupId
        {
            get
            { return dimensionGroupId; }
            set
            { dimensionGroupId = value; }
        }

        private bool isRequired;
        public bool IsRequired
        {
            get { return isRequired; }
            set { isRequired = value; }
        }

        private float score;
        public float Score
        {
            get
            { return score; }
            set
            { score = value; }
        }

        private int sortIndex;
        public int SortIndex
        {
            get
            { return sortIndex; }
            set
            { sortIndex = value; }
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
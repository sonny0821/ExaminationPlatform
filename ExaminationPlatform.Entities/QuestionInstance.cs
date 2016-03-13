using System;

namespace ExaminationPlatform.Entities
{
    public class QuestionInstance
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
        private Guid questionId;

        public Guid QuestionId
        {
            get
            { return questionId; }
            set
            { questionId = value; }
        }
        private float score;

        public float Score
        {
            get
            { return score; }
            set
            { score = value; }
        }
        private int dispOrder;

        public int DispOrder
        {
            get
            { return dispOrder; }
            set
            { dispOrder = value; }
        }
        private string dimensionName;

        public string DimensionName
        {
            get
            { return dimensionName; }
            set
            { dimensionName = value; }
        }
        private Guid parentId;

        public Guid ParentId
        {
            get
            { return parentId; }
            set
            { parentId = value; }
        }
        private string imagePath;

        public string ImagePath
        {
            get
            { return imagePath; }
            set
            { imagePath = value; }
        }
        private string tag;

        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        private Guid groupInstanceId;

        public Guid GroupInstanceId
        {
            get
            { return groupInstanceId; }
            set
            { groupInstanceId = value; }
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
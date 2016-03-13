using System;

namespace ExaminationPlatform.Entities
{
    public class Option
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
        private bool answer;
        public bool Answer
        {
            get
            { return answer; }
            set
            { answer = value; }
        }
        private bool hasText;

        public bool HasText
        {
            get { return hasText; }
            set { hasText = value; }
        }
        private int sortIndex;

        public int SortIndex
        {
            get
            { return sortIndex; }
            set
            { sortIndex = value; }
        }        
        private string tag;

        public string Tag
        {
            get
            { return tag; }
            set
            { tag = value; }
        }
        private Guid questionId;

        public Guid QuestionId
        {
            get
            { return questionId; }
            set
            { questionId = value; }
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
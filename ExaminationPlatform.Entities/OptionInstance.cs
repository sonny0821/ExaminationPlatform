using System;

namespace ExaminationPlatform.Entities
{
    public class OptionInstance
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
        private string text;

        public string Text
        {
            get
            { return text; }
            set
            { text = value; }
        }
        private bool answer;

        public bool Answer
        {
            get
            { return answer; }
            set
            { answer = value; }
        }
        private bool isSelected;

        public bool IsSelected
        {
            get
            { return isSelected; }
            set
            { isSelected = value; }
        }
        private int dispOrder;

        public int DispOrder
        {
            get
            { return dispOrder; }
            set
            { dispOrder = value; }
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
            get
            { return tag; }
            set
            { tag = value; }
        }
        private bool showText;

        public bool ShowText
        {
            get { return showText; }
            set { showText = value; }
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
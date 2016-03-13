using System;

namespace ExaminationPlatform.Entities
{
    public class ExamInstance
    {
        private Guid id;

        public Guid Id
        {
            get
            { return id; }
            set
            { id = value; }
        }
        private Guid batchId;

        public Guid BatchId
        {
            get
            { return batchId; }
            set
            { batchId = value; }
        }
        private string title;

        public string Title
        {
            get
            { return title; }
            set
            { title = value; }
        }
        private Guid userId;

        public Guid UserId
        {
            get
            { return userId; }
            set
            { userId = value; }
        }
        private int state;

        public int State
        {
            get { return state; }
            set { state = value; }
        }       
        
        private DateTime deliveryTime;

        public DateTime DeliveryTime
        {
            get
            { return deliveryTime; }
            set
            { deliveryTime = value; }
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
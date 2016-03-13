using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationPlatform.Entities.Extend
{
    public class GroupEx : Group
    {
        private List<GroupQuestion> questions;

        public List<GroupQuestion> Questions
        {
            get { return questions; }
            set { questions = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationPlatform.Entities.Extend
{
    public class ExamEx : Exam
    {
        private List<GroupEx> groups;

        public List<GroupEx> Groups
        {
            get { return groups; }
            set { groups = value; }
        }
    }
}

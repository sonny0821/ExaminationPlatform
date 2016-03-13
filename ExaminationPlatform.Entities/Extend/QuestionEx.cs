using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationPlatform.Entities.Extend
{
    public class QuestionEx : Question
    {
        private List<Option> options;

        public List<Option> Options
        {
            get { return options; }
            set { options = value; }
        }
    }
}

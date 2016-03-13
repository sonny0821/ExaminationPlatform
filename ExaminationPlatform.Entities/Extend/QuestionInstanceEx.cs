using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationPlatform.Entities.Extend
{
    public class QuestionInstanceEx : QuestionInstance
    {
        private List<OptionInstance> options;

        public List<OptionInstance> Options
        {
            get { return options; }
            set { options = value; }
        }
    }
}

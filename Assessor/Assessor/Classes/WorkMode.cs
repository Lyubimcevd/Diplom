using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessor.Classes
{
    class WorkMode
    {
        static bool is_expert;

        public static bool IsExpert
        {
            get
            {
                return is_expert;
            }
            set
            {
                is_expert = value;
            }
        }  
    }
}

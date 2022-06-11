using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRModCore
{
    public interface SRLogger
    {
        void Msg(string message);
        void Debug(string message);
        void Error(string message);
        void Error(string message, Exception e);
    }
}

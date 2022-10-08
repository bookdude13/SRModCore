using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRModCore
{
    public class MelonLoggerWrapper : SRLogger
    {
        private readonly bool isDebug = false;

        private readonly MelonLogger.Instance melonLogger;

        public MelonLoggerWrapper(MelonLogger.Instance melonLogger, bool isDebug = false)
        {
            this.melonLogger = melonLogger;
            this.isDebug = isDebug;
        }

        public void Msg(string message)
        {
            melonLogger.Msg(message);
        }

        public void Debug(string message)
        {
            if (isDebug)
            {
                melonLogger.Msg(message);
            }
        }

        public void Error(string message)
        {
            melonLogger.Error(message);
        }
        public void Error(string message, Exception e)
        {
            melonLogger.Error(message, e);
        }

    }
}

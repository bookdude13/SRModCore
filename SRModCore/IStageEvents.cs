using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRModCore
{
    public interface IStageEvents
    {
        void OnSongStart();

        void OnSongEnd();

        void OnNoteHit();

        void OnNoteFail();

        void OnEnterSpecial();

        void OnCompleteSpecial();

        void OnFailSpecial();
    }
}

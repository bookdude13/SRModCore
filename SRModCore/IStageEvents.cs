using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

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

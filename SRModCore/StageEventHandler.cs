using Synth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace SRModCore
{
    public class StageEventHandler
    {
        private readonly SRLogger logger;
        private readonly IStageEvents listener;
        private readonly StageEvents stageEvents;

        public StageEventHandler(SRLogger logger, IStageEvents eventListener)
        {
            this.logger = logger;
            this.listener = eventListener;

            stageEvents = new StageEvents
            {
                OnSongStart = new UnityEvent(),
                OnSongEnd = new UnityEvent(),
                OnNoteHit = new UnityEvent(),
                OnNoteFail = new UnityEvent(),
                OnEnterSpecial = new UnityEvent(),
                OnCompleteSpecial = new UnityEvent(),
                OnFailSpecial = new UnityEvent()
            };
        }

        public void RemoveListeners()
        {
            if (stageEvents != null)
            {
                stageEvents.OnSongStart.RemoveListener(listener.OnSongStart);
                stageEvents.OnSongEnd.RemoveListener(listener.OnSongEnd);
                stageEvents.OnNoteHit.RemoveListener(listener.OnNoteHit);
                stageEvents.OnNoteFail.RemoveListener(listener.OnNoteFail);
                stageEvents.OnEnterSpecial.RemoveListener(listener.OnEnterSpecial);
                stageEvents.OnCompleteSpecial.RemoveListener(listener.OnCompleteSpecial);
                stageEvents.OnFailSpecial.RemoveListener(listener.OnFailSpecial);
            }
        }

        private void AddListeners()
        {
            if (stageEvents != null)
            {
                stageEvents.OnSongStart.AddListener(listener.OnSongStart);
                stageEvents.OnSongEnd.AddListener(listener.OnSongEnd);
                stageEvents.OnNoteHit.AddListener(listener.OnNoteHit);
                stageEvents.OnNoteFail.AddListener(listener.OnNoteFail);
                stageEvents.OnEnterSpecial.AddListener(listener.OnEnterSpecial);
                stageEvents.OnCompleteSpecial.AddListener(listener.OnCompleteSpecial);
                stageEvents.OnFailSpecial.AddListener(listener.OnFailSpecial);
            }
        }

        public void SetupEvents()
        {
            try
            {
                logger.Msg("Adding stage events!");
                RemoveListeners();
                AddListeners();
                GameControlManager.UpdateStageEventList(stageEvents);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
            }
        }
    }
}

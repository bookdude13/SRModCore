using Il2Cpp;
using Il2CppSynth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using HarmonyLib;

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
                stageEvents.OnSongStart.RemoveListener((UnityAction)listener.OnSongStart);
                stageEvents.OnSongEnd.RemoveListener((UnityAction)listener.OnSongEnd);
                stageEvents.OnNoteHit.RemoveListener((UnityAction)listener.OnNoteHit);
                stageEvents.OnNoteFail.RemoveListener((UnityAction)listener.OnNoteFail);
                stageEvents.OnEnterSpecial.RemoveListener((UnityAction)listener.OnEnterSpecial);
                stageEvents.OnCompleteSpecial.RemoveListener((UnityAction)listener.OnCompleteSpecial);
                stageEvents.OnFailSpecial.RemoveListener((UnityAction)listener.OnFailSpecial);
            }
        }

        private void AddListeners()
        {
            if (stageEvents != null)
            {
                stageEvents.OnSongStart.AddListener((UnityAction)listener.OnSongStart);
                stageEvents.OnSongEnd.AddListener((UnityAction)listener.OnSongEnd);
                stageEvents.OnNoteHit.AddListener((UnityAction)listener.OnNoteHit);
                stageEvents.OnNoteFail.AddListener((UnityAction)listener.OnNoteFail);
                stageEvents.OnEnterSpecial.AddListener((UnityAction)listener.OnEnterSpecial);
                stageEvents.OnCompleteSpecial.AddListener((UnityAction)listener.OnCompleteSpecial);
                stageEvents.OnFailSpecial.AddListener((UnityAction)listener.OnFailSpecial);
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

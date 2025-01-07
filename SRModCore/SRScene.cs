﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRModCore
{
    public class SRScene
    {
        private static readonly string SCENE_NAME_GAME_START = "GameStart";
        private static readonly string SCENE_NAME_WARNING = "0.AWarning";
        private static readonly string SCENE_NAME_GAME_END = "3.GameEnd";
        private static readonly string SCENE_NAME_CUSTOM_STAGES= "CustomStages";
        private static readonly string SCENE_NAME_EXPERIENCE= "Experience Scene";

        //TODO experience?
        //TODO multiplayer scenes?
        public enum SRSceneType
        {
            GAME_START,
            WARNING,
            MAIN_MENU,
            NORMAL_STAGE,
            SPIN_STAGE,
            SPIRAL_STAGE,
            GAME_END,
            OTHER,
            CUSTOM_STAGE,
            EXPERIENCE_STAGE
        }

        public string SceneName { get; private set; }
        public SRSceneType SceneType { get; private set; }

        public SRScene(string sceneName)
        {
            SceneName = sceneName;
            SceneType = GetSceneType(sceneName);
        }

        public bool IsStage()
        {
            return SceneType == SRSceneType.NORMAL_STAGE
                || SceneType == SRSceneType.SPIN_STAGE
                || SceneType == SRSceneType.SPIRAL_STAGE
                || SceneType == SRSceneType.CUSTOM_STAGE
                || SceneType == SRSceneType.EXPERIENCE_STAGE;
        }

        private SRSceneType GetSceneType(string sceneName)
        {
            if (sceneName == null)
            {
                return SRSceneType.OTHER;
            }

            if (sceneName == SCENE_NAME_GAME_START)
            {
                return SRSceneType.GAME_START;
            }

            if (sceneName == SCENE_NAME_WARNING)
            {
                return SRSceneType.WARNING;
            }

            var mainMenuScenes = new List<string>()
            {
                "01.The Room",
                "02.The Void",
                "03.Roof Top",
                "04.The Planet",
                "SongSelection"
            };
            if (mainMenuScenes.Contains(sceneName))
            {
                return SRSceneType.MAIN_MENU;
            }

            string subName = "";
            if (sceneName.Contains("."))
            {
                subName = sceneName.Split('.')[1];
            }

            bool isNormalStage = subName.StartsWith("Stage");
            if (isNormalStage)
            {
                return SRSceneType.NORMAL_STAGE;
            }

            bool isSpinStage = subName.StartsWith("Static Stage");
            if (isSpinStage)
            {
                return SRSceneType.SPIN_STAGE;
            }

            bool isSpiralStage = subName.StartsWith("Spiral Stage");
            if (isSpiralStage)
            {
                return SRSceneType.SPIRAL_STAGE;
            }

            if (sceneName == SCENE_NAME_CUSTOM_STAGES)
            {
                return SRSceneType.CUSTOM_STAGE;
            }

            if (sceneName == SCENE_NAME_GAME_END)
            {
                return SRSceneType.GAME_END;
            }

            if (sceneName.Contains(SCENE_NAME_EXPERIENCE))
            {
                return SRSceneType.EXPERIENCE_STAGE;
            }

            return SRSceneType.OTHER;
        }
    }
}

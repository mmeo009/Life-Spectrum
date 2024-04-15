using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LifeSpectrum
{
    public class Enums
    {
        public enum PlayerStats
        {
            life,
            fullness,
            fell,
            money
        }
        public enum AdditionalEvent
        {
            foodPoison,
            cold,
            hurt
        }
        public enum ActonType
        {
            SceneMove,
            ExitGame,
            PauseGame,
            SaveGame,
            LoadGame,
            DefaultAction
        }
        public enum StoryStangeDegree
        {
            Ordinary,
            Strange,
            Terrible,
            Morbid,
            Disaster
        }
    }
}


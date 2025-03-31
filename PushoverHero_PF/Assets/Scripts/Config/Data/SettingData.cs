using System;
using Config.Enums;

namespace Config.Data
{
    [Serializable]
    public class SettingData
    {
        public eLanguageType Language;

        public float MasterVolume;
        public float EffectVolume;
        public float BGMVolume;

        public bool[] TutorialFlagArr;
    }
}

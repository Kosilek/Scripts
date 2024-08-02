using UnityEngine;

namespace Kosilek.SaveAndLoad
{
    public static class Path
    {
        public static string DefaultPath
        {
#if UNITY_EDITOR
            get { return Application.dataPath + "\\"; }
#else
                get { return Application.persistentDataPath + "\\"; }
#endif
        }

        public static string Volume
        {
            get { return "Volume"; }
        }

        public static string Music
        {
            get { return Volume + "\\" + "Music.txt"; }
        }

        public static string Sound
        {
            get { return Volume + "\\" + "Sound.txt"; }
        }

        public static string Vibration
        {
            get { return Volume + "\\" + "Vibration.txt"; }
        }

        public static string SaveCells
        {
            get { return "SaveCells"; }
        }

        public static string Cell
        {
            get { return SaveCells + "\\" + "cell"; }
        }

        public static string Txt
        {
            get { return ".txt"; }
        }

        public static string Data
        {
            get { return "Data"; }
        }

        public static string NameData
        {
            get { return Data + "\\" + "Name.txt"; }
        }

        public static string DateTime
        {
            get { return Data + "\\" + "DateTime.txt"; }
        }

        public static string Score
        {
            get { return Data + "\\" + "Score.txt"; }
        }

        public static string BustVolume
        {
            get { return Data + "\\" + "BustVolume.txt"; }
        }

        public static string MaxMonstrNumber
        {
            get { return Data + "\\" + "MaxMonstrNumber.txt"; }
        }

        public static string BestScore
        {
            get { return Data + "\\" + "BestScore.txt"; }
        }

        public static string Money
        {
            get { return Data + "\\" + "Money.txt"; }
        }
    }
}
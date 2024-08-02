using System.IO;
using UnityEngine;

namespace Kosilek.SaveAndLoad
{
    public class CreateDirectory : MonoBehaviour
    {
        #region const
        private const int COUNTCELL = 40;
        #endregion

        public void InitializeFile()
        {
            CreateFile(Path.Volume);
            CreateFile(Path.SaveCells);
            CreateTxtFile(Path.Music);
            CreateTxtFile(Path.Sound);
            CreateTxtFile(Path.Vibration);
            for (int i = 0; i < COUNTCELL; i++)
            {
                CreateTxtFile(Path.Cell + i + Path.Txt);
            }
            CreateFile(Path.Data);
            CreateTxtFile(Path.NameData);
            CreateTxtFile(Path.Score);
            CreateTxtFile(Path.BustVolume);
            CreateTxtFile(Path.MaxMonstrNumber);
            CreateTxtFile(Path.Money);
            CreateTxtFile(Path.BestScore);
            CreateTxtFile(Path.DateTime);
        }

        private void CreateFile(string fileName)
        {
            var path = Path.DefaultPath + fileName;

            if (Directory.Exists(path))
                return;
            Directory.CreateDirectory(path);
        }

        private void CreateTxtFile(string fileName)
        {
            var path = Path.DefaultPath + fileName;

            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
            }
        }
    }
}
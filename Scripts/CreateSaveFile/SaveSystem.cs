using System;
using System.IO;
using UnityEngine;

namespace Kosilek.SaveAndLoad
{
    public class SaveSystem : MonoBehaviour
    {
        public void WritingDataToADocument(bool isNewData, string path, string fileData)
        {
            Action<string, string> action = isNewData ? WritingDataToADocumentNew : WritingDataToADocumentOld;

            action?.Invoke(Path.DefaultPath + path, fileData);
        }

        private void WritingDataToADocumentNew(string path, string fileData)
        {
            File.WriteAllText(path, fileData);
        }

        private void WritingDataToADocumentOld(string path, string fileData)
        {
            using (StreamWriter writer = File.AppendText(path))
            {
                writer.WriteLine("\n" + fileData);
            }
        }
    }
}
using UnityEngine;
using System.IO;

namespace Kosilek.SaveAndLoad
{
    public class LoadSystem : MonoBehaviour
    {
        public string[] LoadData(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}
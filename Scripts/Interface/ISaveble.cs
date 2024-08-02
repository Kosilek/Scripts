namespace Kosilek.SaveAndLoad
{
    interface ISaveble
    {
        public void Save(string path);

        public void Save(bool isNewData, string fileName, string fileData);

        public string[] Load(string path);
    }
}
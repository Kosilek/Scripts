using Kosilek.SaveAndLoad;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Kosilek.UI
{
    public class BestScore : MonoBehaviour, ISaveble
    {
        #region Values Score
        private ulong bestScore;
        [SerializeField] private TextMeshProUGUI bestScoreText;
        #endregion end Values Score
        #region InitializeLoadData
        public void InitializeLoadData()
        {
            string[] bestScoreData = Load(Path.DefaultPath + Path.BestScore);
            InitializeBestScore(bestScoreData);
        }

        private void InitializeBestScore(string[] bestScoreData)
        {
            try
            {
                var hasNameData = bestScoreData.Length > 0;
                Action<string[]> action = hasNameData ? InitializeBestScoreLoad : InitializeBestScoreFirst;
                action.Invoke(bestScoreData);
            }
            catch
            {
                Debug.LogError("Error: Data could not be watered, InitializeMaxNumberMonstr");
            }
        }

        private void InitializeBestScoreLoad(string[] bestScoreData)
        {
            bestScore = Convert.ToUInt32(bestScoreData[0]);
            UpdateUIBestScoreText();
        }

        private void InitializeBestScoreFirst(string[] bestScoreData)
        {
            bestScore = 0;
            UpdateUIBestScoreText();
        }
        #endregion end InitializeLoadData

        #region UpdateUI
        private void UpdateUIBestScoreText()
        {
            bestScoreText.text = bestScore.ToString();
            Save(true, Path.BestScore, bestScore.ToString());
        }
        #endregion end UpdateUI

        #region Instance
        public void SetBestScore(ulong score)
        {
            if (score < bestScore)
                return;

            bestScore = score;
            UpdateUIBestScoreText();
        }
        #endregion end Instance

        #region Save
        public string[] Load(string path)
        {
            try
            {
                return SaveManager.Instance.loadSystem.LoadData(path);
            }
            catch
            {
                Debug.LogError($"Error: Data could not be downloaded from the file. File path: {path}");
                return null;
            }
        }

        public void Save(string path)
        {
            throw new System.NotImplementedException();
        }

        public void Save(bool isNewData, string fileName, string fileData)
        {
            SaveManager.Instance.multiThread.SaveData(SaveManager.Instance.saveSystem.WritingDataToADocument, SaveTaskType.OtherData, isNewData, fileName, fileData);
        }
        #endregion end Save
    }
}

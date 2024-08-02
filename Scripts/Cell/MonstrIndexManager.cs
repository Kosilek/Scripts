using DredPack;
using Kosilek.Ad;
using Kosilek.MonstrPack;
using Kosilek.SaveAndLoad;
using Kosilek.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosilek.Manager
{
    public class MonstrIndexManager : SimpleSingleton<MonstrIndexManager>, ISaveble
    {
        [HideInInspector]
        public int maxNumberMonstOpen;

        [HideInInspector]
        public int oldNumberMonstOpen;

        //[HideInInspector]
        public int stepIndexMonstrFirst = 0;
        //[HideInInspector]
        public int stepIndexMonstr = 0;

        //[HideInInspector]
        public bool isFirstLoad = true;

        #region Image
        [SerializeField] private Image maxMonstrImage;
        #endregion

        #region InitializeLoadData
        public void InitializeLoadData()
        {
            string[] monstrIndex = Load(Path.DefaultPath + Path.MaxMonstrNumber);
            InitializeMaxNumberMonstr(monstrIndex);
            CanvasManager.Instance.seeMonstrPrize.Initialize();
        }

        private void InitializeMaxNumberMonstr(string[] monstrIndex)
        {
            try
            {
                var hasNameData = monstrIndex.Length > 0;
                Action<string[]> action = hasNameData ? InitializeMaxNumberMonstrLoad : InitializeMaxNumberMonstrFirst;
                action.Invoke(monstrIndex);
            }
            catch
            {
                Debug.LogError("Error: Data could not be watered, InitializeMaxNumberMonstr");
            }
        }

        private void InitializeMaxNumberMonstrLoad(string[] monstrIndex)
        {
            maxNumberMonstOpen = Convert.ToInt32(monstrIndex[0]);
            UpdateUIIcon(maxNumberMonstOpen);
        }

        private void InitializeMaxNumberMonstrFirst(string[] monstrIndex)
        {
            maxNumberMonstOpen = 0;
            UpdateUIIcon(maxNumberMonstOpen);
            isFirstLoad = false;
        }
        #endregion end InitializeLoadData

        #region UpdateUI
        public void UpdateUIIcon(int index)
        {
            maxMonstrImage.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[index].iconMonstr;
            UpdateStep();
            Save(true, Path.MaxMonstrNumber, maxNumberMonstOpen.ToString());
            oldNumberMonstOpen = maxNumberMonstOpen;
        }
        #endregion end UpdateUI

        #region UpdateStep
        public void UpdateStep()
        {
            if (maxNumberMonstOpen > 9) //9
            {
                if (stepIndexMonstr != maxNumberMonstOpen - 9)
                {
                    stepIndexMonstr = maxNumberMonstOpen - 9;

                    if (stepIndexMonstr >= 1)
                    {
                        stepIndexMonstrFirst = 0;

                        stepIndexMonstrFirst = 1;

                        var counter = 0;
                        for (int i = 2; i <= stepIndexMonstr; i++)
                        {
                            counter++;

                            if (counter == 3)//3
                            {
                                stepIndexMonstrFirst++;
                                counter = 0;
                            }
                        }
                    }
                  

                    if (isFirstLoad)
                    {
                        isFirstLoad = false;
                    }
                    else
                    {
                        StartCoroutine(IE());
                    }

                    IEnumerator IE()
                    {
                        yield return new WaitForSeconds(CellManager.DELAY_DESTROY);
                        CellManager.Instance.indexCell.Clear();
                        CellManager.Instance.columnCell.Clear();
                        for (int i = 0; i < stepIndexMonstrFirst + 1; i++)
                        {
                            CellManager.Instance.DestroyCellOld(Convert.ToUInt32(MathF.Pow(2, i)));
                        }
                        CellManager.Instance.DestroyCell();
                        AdManager.Instance.ShowInterstitial();
                    }
                }
            }
        }
        #endregion

        #region SaveAndLoad
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
        #endregion end SaveAndLoad

        public void ResetData()
        {
            maxNumberMonstOpen = 0;
            oldNumberMonstOpen = 0;
            stepIndexMonstrFirst = 0;
            CanvasManager.Instance.seeMonstrPrize.Initialize();
            UpdateUIIcon(maxNumberMonstOpen);
            stepIndexMonstr = 0;
        }
    }
}
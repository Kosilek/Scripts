using DredPack;
using Kosilek.SaveAndLoad;
using Kosilek.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Kosilek.Manager
{
    public class MoneyManager : SimpleSingleton<MoneyManager>, ISaveble
    {
        #region Values Money
        public int money;
        public int realMoney;
        #endregion end Values Money

        #region TextMeshPro
        [SerializeField] private TextMeshProUGUI textMoney;
        #endregion end TextMeshPro

        #region Event
        [HideInInspector] public UnityEvent updateMoneyUI = new UnityEvent();
        [HideInInspector] public UnityEvent updateRealMoneyUI = new UnityEvent();
        #endregion nend Event

        #region InitializeLoadData
        public void InitializeLoadData()
        {
            string[] moneyData = Load(Path.DefaultPath + Path.Money);
            InitializeMoney(moneyData);
        }

        private void InitializeMoney(string[] moneyData)
        {
            try
            {
                var hasNameData = moneyData.Length > 0;
                Action<string[]> action = hasNameData ? InitializeMoneyLoad : InitializeMoneyFirst;
                action.Invoke(moneyData);
            }
            catch
            {
                Debug.LogError("Error: Data could not be watered, InitializeMaxNumberMonstr");
            }
        }

        private void InitializeMoneyLoad(string[] moneyData)
        {
            money = Convert.ToInt32(moneyData[0]);
            UpdateUIMoneyText();
        }

        private void InitializeMoneyFirst(string[] moneyData)
        {
            money = 0;
            UpdateUIMoneyText();
        }
        #endregion end InitializeLoadData

        #region UpdateUI
        private void UpdateUIMoneyText()
        {
            textMoney.text = money.ToString();
            Save(true, Path.Money, money.ToString());
        }
        #endregion end UpdateUI

        #region Set
        public void SetMoney(int money)
        {
            this.money += money;
            updateMoneyUI?.Invoke();
            UpdateUIMoneyText();
        }

        public void SetRealMoney(int realMoney)
        {
            this.realMoney += realMoney;
            updateRealMoneyUI?.Invoke();
            
        }
        #endregion end Set

        #region Get
        public int GetMoney()
        {
            return money;
        }

        public int GetRealMoney()
        {
            return realMoney;
        }
        #endregion end Get

        //Update
        #region Spend 
        public bool SpendMoney(int money)
        {
            if (money > this.money)
                return false;
            else
            {
                this.money -= money;
                updateMoneyUI?.Invoke();
                UpdateUIMoneyText();
                return true;
            }
        }

        public bool SpendRealMoney(int realMoney)
        {
            if (realMoney > this.realMoney)
                return false;
            else
            {
                this.realMoney -= realMoney;
                updateRealMoneyUI?.Invoke();

                return true;
            }

        }
        #endregion end Spend
        #region Save
        public void Save(string path)
        {
            throw new NotImplementedException();
        }

        public void Save(bool isNewData, string fileName, string fileData)
        {
            SaveManager.Instance.multiThread.SaveData(SaveManager.Instance.saveSystem.WritingDataToADocument, SaveTaskType.OtherData, isNewData, fileName, fileData);
        }

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
        #endregion end Save
    }
}
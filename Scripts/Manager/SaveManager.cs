using DredPack;
using DredPack.UI.WindowAnimations;
using Kosilek.Manager;
using Kosilek.SaveAndLoad;
using Kosilek.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosilek.SaveAndLoad
{
    public class SaveManager : SimpleSingleton<SaveManager>
    {
        #region Internal classes
        private InitializeLoadData initializeLoadData;
        #endregion end Internal classes

        #region ScriptComonent
        public CreateDirectory createDirectory;
        public MultiThread multiThread;
        public LoadSystem loadSystem;
        public SaveSystem saveSystem;
        #endregion

        #region Awake
        private void Awake()
        {
            SetValues();
            InitializeLoadAwake();
        }

        private void SetValues()
        {
            initializeLoadData = new InitializeLoadData();
        }

        private void InitializeLoadAwake()
        {
            initializeLoadData.CreateFile(createDirectory);
            initializeLoadData.LoadSettingsData(CanvasManager.Instance);
            CanvasManager.Instance.canvasSettings.GetComponent<CanvasSettings>().SettingInitialValues();
            initializeLoadData.LoadNameData(CanvasManager.Instance);
        }
        #endregion endAwake

        #region Start
        private void Start()
        {
            InitializeLoadStart();
        }

        private void InitializeLoadStart()
        {
            initializeLoadData.LoadCellData(CellManager.Instance);
            initializeLoadData.LoadScore(CanvasManager.Instance.canvasGameScr);
            initializeLoadData.LoadBustVolume(BustManager.Instance);
            initializeLoadData.LoadMaxNumberMonstr(MonstrIndexManager.Instance);
            initializeLoadData.LoadMoney(MoneyManager.Instance);
            initializeLoadData.LoadBestScore(CanvasManager.Instance.bestScore);
            initializeLoadData.LoadDateTime(TimeManager.Instance);
        }
        #endregion endStart
    }

    internal class InitializeLoadData
    {
        internal void CreateFile(CreateDirectory createDirectory)
        {
            createDirectory.InitializeFile();
        }

        internal void LoadSettingsData(CanvasManager canvasManager)
        {
            canvasManager.canvasSettings.GetComponent<CanvasSettings>().InitializeLoadData();
        }

        internal void LoadNameData(CanvasManager canvasManager)
        {
            canvasManager.canvasProfile.GetComponent<CanvasProfile>().InitializeLoadData();
        }

        internal void LoadCellData(CellManager cellManager)
        {
            cellManager.InitializingCell();
        }

        internal void LoadScore(CanvasGame canvasGame)
        {
            canvasGame.InitializeLoadData();
        }

        internal void LoadBustVolume(BustManager bustManager)
        {
            bustManager.InitializeLoadData();
        }

        internal void LoadMaxNumberMonstr(MonstrIndexManager monstrIndexManager)
        {
            monstrIndexManager.InitializeLoadData();
        }

        internal void LoadMoney(MoneyManager moneyManager)
        {
            moneyManager.InitializeLoadData();
        }

        internal void LoadBestScore(BestScore bestScore)
        {
            bestScore.InitializeLoadData();
        }

        internal void LoadDateTime(TimeManager time)
        {
            time.InitializeLoadData();
        }
    }
}

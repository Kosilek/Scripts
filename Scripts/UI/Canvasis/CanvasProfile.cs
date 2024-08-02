using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kosilek.SaveAndLoad;
using TMPro;
using System;
using UnityEngine.UI;

namespace Kosilek.UI
{
    public class CanvasProfile : MonoBehaviour, ISaveble
    {
        private string nickName;
        public TextMeshProUGUI textNickName;
        #region Button
        [SerializeField]
        private Button buttonBack;
        [SerializeField]
        private TextMeshProUGUI nickFieldText;
        [SerializeField]
        private TMP_InputField inputField;
        #endregion endButton

        #region Start
        private void Start()
        {
            AddListener();
        }

        private void AddListener()
        {
            buttonBack.onClick.AddListener(ActionButton);
        }

        private void ActionButton()
        {
            CanvasManager.Instance.actionWindow.OpenWindow(false, CanvasManager.Instance.canvasProfile, "canvasProfile");
        }
        #endregion end Start

        #region InitializeName
        public void InitializeLoadData()
        {
            string[] nameData = Load(Path.DefaultPath + Path.NameData);
            InitializeName(nameData);
        }

        private void InitializeName(string[] nameData)
        {
            try
            {
                var hasNameData = nameData.Length > 0;
                Action<string[]> action = hasNameData ? InitializeNameLoad : InitializeNameFirst;
                action.Invoke(nameData);
            }
            catch
            {
                Debug.LogError("Error: Data could not be watered, InitializeName");
            }
        }

        private void InitializeNameFirst(string[] nameData)
        {
            var name = "guest";
            Save(true, Path.NameData, name);
            textNickName.text = name;
        }

        private void InitializeNameLoad(string[] nameData)
        {
            var name = nameData[0];
            textNickName.text = name;
        }
        #endregion endInitializeName
        #region ActionEventInputTxt
        public void ActionInputStart()
        {

        }

        public void ActionInputEnd()
        {
            var name = nickFieldText.text;
            textNickName.text = name;
            inputField.textComponent = null;
            nickFieldText.text = "";
            StartCoroutine(IE());

            IEnumerator IE()
            {
                yield return new WaitForSeconds(.1f);
                inputField.textComponent = nickFieldText;
                Save(true, Path.NameData, name);
            }
        }
        #endregion end ActionEvent

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
        //    SaveManager.Instance.multiThread.SaveData(SaveManager.Instance.saveSystem.WritingDataToADocument, SaveTaskType.);
        }

        public void Save(bool isNewData, string fileName, string fileData)
        {
            SaveManager.Instance.multiThread.SaveData(SaveManager.Instance.saveSystem.WritingDataToADocument, SaveTaskType.NameData, isNewData, fileName, fileData);
        }
        #endregion end SaveAndLoad
    }
}
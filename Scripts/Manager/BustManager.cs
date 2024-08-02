using DredPack;
using Kosilek.SaveAndLoad;
using Kosilek.UI;
using NaughtyAttributes;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kosilek.Manager
{
    public class BustManager : SimpleSingleton<BustManager>, ISaveble
    {
       // [HideInInspector]
        public bool isHummer = false;
        //[HideInInspector]
        public bool isExchange = false;

       // [HideInInspector]
        public bool isActiveHammer = false;
       // [HideInInspector]
        public bool isActiveExchange = false;

        [HideInInspector]
        public int hammerCount;
        [HideInInspector]
        public int exchangeCount;

        #region ScriptsComponent
        [Foldout("ScriptsComponent")]
        [SerializeField] private ButtonBustRe buttonBustRe;
        [Foldout("ScriptsComponent")]
        [SerializeField] private ButtonBustHummer buttonBustHummer;
        #endregion end ScriptsComponent

        #region TextMeshPro
        [Foldout("TextMeshPro")]
        [SerializeField] private TextMeshProUGUI hummerCountText;
        [Foldout("TextMeshPro")]
        [SerializeField] private TextMeshProUGUI exchangeCountText;
        #endregion end TextMeshPro

        #region Animator
        [Foldout("Animator")]
        [SerializeField] private Animator animHummer;
        [Foldout("Animator")]
        [SerializeField] private Animator animExchange;
        #endregion end Animator
        #region ButtonComponent
        [Foldout("ButtonComponent")]
        [SerializeField] private Color buttonActive;
        [Foldout("ButtonComponent")]
        [SerializeField] private Color buttonInActive;
        [HideInInspector]
        public Image imageHummer;
        [HideInInspector]
        public Image imageExchange;
        #endregion end ButtonComponent
        #region Const
        private const int INDEX_HUMMER_BUST = 0;
        private const int INDEX_ECHANGE_BUST = 1;

        private const string START_ANIM = "Start";
        #endregion end Const

        #region Start
        private void Start()
        {
            SetValues();
        }

        private void SetValues()
        {
            imageHummer = animHummer.GetComponent<Image>();
            imageExchange = animExchange.GetComponent<Image>();
        }
        #endregion end Start

        #region InitializeLoadData
        public void InitializeLoadData()
        {
            string[] bust = Load(Path.DefaultPath + Path.BustVolume);
            InitializeBust(bust);
        }

        private void InitializeBust(string[] bust)
        {
            try
            {
                var hasNameData = bust.Length > 0;
                Action<string[]> action = hasNameData ? InitializeBustLoad : InitializeBustFirst;
                action.Invoke(bust);
            }
            catch
            {
                Debug.LogError("Error: Data could not be watered, InitializeBust");
            }
        }

        private void InitializeBustLoad(string[] bust)
        {
            UpdateUITextHummer(Convert.ToInt32(bust[INDEX_HUMMER_BUST]));
            UpdateUITextEchange(Convert.ToInt32(bust[INDEX_ECHANGE_BUST]));
        }

        private void InitializeBustFirst(string[] bust)
        {
            UpdateUITextHummer(3);
            UpdateUITextEchange(3);
        }
        #endregion end InitializeLoadData
        #region UpdateUI
        public void UpdateUITextHummer(int count)
        {
            hammerCount += count;
            hummerCountText.text = hammerCount.ToString();
            Save(true, Path.BustVolume, hammerCount.ToString() + "\n" + exchangeCount.ToString());
        }

        public void UpdateUITextEchange(int count)
        {
            exchangeCount += count;
            exchangeCountText.text = exchangeCount.ToString();
            Save(true, Path.BustVolume, hammerCount.ToString() + "\n" + exchangeCount.ToString());
        }
        #endregion end UpdateUI
        #region Animation
        public void StartAnimHummer(bool isActive, Image image) => StartAnimation(animHummer, START_ANIM);
        public void StartAnimExchange(bool isActive, Image image) => StartAnimation(animExchange, START_ANIM);

        private void StartAnimation(Animator anim, string name)
        {
            anim.SetTrigger(name);
        }
        #endregion end Animation
        #region Instance
        public void ActiveBusterHammer()
        {
            Action<int, bool, Image, bool> action = isActiveHammer ? DeActiveBustButton : ActionButtonBust;
            action.Invoke(hammerCount, true, imageHummer, isActiveHammer);
        }

        public void ActiveBusterExchange()
        {
            Action<int, bool, Image, bool> action = isActiveExchange ? DeActiveBustButton : ActionButtonBust;
            action.Invoke(exchangeCount, false, imageExchange, isActiveExchange);
        }

        private void ActionButtonBust(int count, bool isHummer, Image image, bool isInActive)
        {
            Action<bool, Image> action = count > 0 ? ActionBustButton : StartAnim;
            action.Invoke(isHummer, image);
        }

        private void StartAnim(bool isHummer, Image image)
        {
            Action<bool, Image> action = isHummer ? StartAnimHummer : StartAnimExchange;
            action.Invoke(isHummer, image);
            var boolean = isHummer ? this.isHummer = false : isExchange = false;
        }

        private void ActionBustButton(bool isHummer, Image image)
        {
            CellManager.Instance.RaycastIsActive = false;
            var boolean = isHummer ? isActiveHammer = true : isActiveExchange = true;
            Action<int> action = isHummer ? UpdateUITextHummer : UpdateUITextEchange;
            action.Invoke(-1);
            image.color = buttonActive;
        }

        public void DeActiveBustButton(int count, bool isHummer, Image image, bool isInActive)
        {
            Action<bool> action = isInActive ? InActiveClickButton : null;
            action?.Invoke(isHummer);
            var boolean = isHummer ? isActiveHammer = false : isActiveExchange = false;
            boolean = isHummer ? this.isHummer = false : isExchange = false;
            image.color = buttonInActive;

            Action call = isHummer ? DeActionHummer : DeActionExchange;
            call.Invoke();
        }

        private void DeActionHummer()
        {
            CellManager.Instance.RaycastIsActive = true;
        }

        private void DeActionExchange()
        {
            StartCoroutine(IE());

            IEnumerator IE()
            {
                var delay = CellManager.DELAY_DESTROY;
                yield return new WaitForSeconds(delay);
                CellManager.Instance.RaycastIsActive = true;
            }
        }

        public void DeActiveBustButtonEndGame(int count, bool isHummer, Image image, bool isInActive)
        {
            Action<bool> action = isInActive ? InActiveClickButton : null;
            action?.Invoke(isHummer);
            var boolean = isHummer ? isActiveHammer = false : isActiveExchange = false;
            boolean = isHummer ? this.isHummer = false : isExchange = false;
            image.color = buttonInActive;
        }

        public void DeAcriveAllBuster()
        {
            DeActiveBustHummer();
            DeActiveBustButtonEndGame(hammerCount, true, imageHummer, isActiveHammer);
            DeActiveBustButtonEndGame(exchangeCount, false, imageExchange, isActiveExchange);
            UpdateUITextHummer(0);
            UpdateUITextEchange(0);
        }

        public void DeActiveBustHummer()
        {
            buttonBustHummer.isAction = false;
            buttonBustHummer.cellGame = null;
            isActiveHammer = false;
            isHummer = false;
            CellManager.Instance.indexCell.Clear();
            CellManager.Instance.columnCell.Clear();
        }

        public void InActiveClickButton(bool isHummer)
        {
            Action<int> action = isHummer ? UpdateUITextHummer : UpdateUITextEchange;
            action.Invoke(1);
            Action callButtonRe = isHummer ? null : buttonBustRe.ResetData;
            callButtonRe?.Invoke();
            var boolean = isHummer ? this.isHummer = false : isExchange = false;
        }
        #endregion end Instance
        #region SaveAndLoad
        public void Save(string path)
        {
            throw new System.NotImplementedException();
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
        #endregion

        #region Test
        public void AddBustTest()
        {
            UpdateUITextHummer(5);
            UpdateUITextEchange(5);
        }
        #endregion endTest
    }
}
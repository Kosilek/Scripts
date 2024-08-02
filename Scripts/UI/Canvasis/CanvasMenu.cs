using DredPack.UI;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Kosilek.Manager;
using Kosilek.Ad;
using System;
using Kosilek.Data;

namespace Kosilek.UI
{
    public class CanvasMenu : MonoBehaviour
    {
        #region ScriptsComponent 
        private Window window;
        #endregion endScriptsComponent

        #region StarGame
        [Foldout("StarGame")]
        public Button buttonStart;
        private Animator animStart;

        [Foldout("StarGame")]
        public float moveSpeedAnimStartGame = 1.25f;
        [Foldout("StarGame")]
        public AnimationCurve animCurveStartGame;
        private Coroutine coroutineStartGame;
        #endregion endStartGame

        #region Top Button
        [Foldout("Top Button")]
        [SerializeField]
        private Button buttonBonus;
        [Foldout("Top Button")]
        [SerializeField]
        private Button buttonSetting;
        [Foldout("Top Button")]
        [SerializeField]
        private Button buttonMessage;
        [Foldout("Top Button")]
        [SerializeField]
        private Button buttonProfile;
        #endregion

        #region Middle Button
        [Foldout("Middle Button")]
        [SerializeField]
        private Button buttonTheBranchOfEvolution;
        [Foldout("Middle Button")]
        [SerializeField]
        private Button buttonShop;
        [Foldout("Middle Button")]
        [SerializeField]
        private Button buttonRating;
        #endregion

        public GameObject canvasGameObject;
        private bool isReadyStart = true;

        private bool isOneStart = true;

        #region Const
        private const string IDLE = "Idle";
        private const string START = "Start";
        private const string CLOSE = "Close";

        public const float MIN_SCALE_LEVEL = 0.01f;
        public const float MAX_SCALE_LEVEL = 1f;

        public const string IS_FIRST_START_MENU = "IsFirstStartMenu";
        #endregion

        [NonSerialized]
        private int _isFirstStartMenu = -1;
        public bool IsFirstStartMenu
        {
            get
            {
                if (_isFirstStartMenu == -1)
                    _isFirstStartMenu = PlayerPrefs.GetInt(IS_FIRST_START_MENU);
                return _isFirstStartMenu == 1;
            }
            set
            {

                _isFirstStartMenu = value ? 1 : 0;
                PlayerPrefs.SetInt(IS_FIRST_START_MENU, _isFirstStartMenu);
            }
        }

        #region Start
        private void Start()
        {
            InitializeButtons();
            SetValues();
        }

        private void SetValues()
        {
            window = GetComponent<Window>();
            animStart = GetComponent<Animator>();
            AudioManager2248.Instance.StartClipMenuStartGame();
        }

        private void InitializeButtons()
        {
            buttonStart.onClick.AddListener(ActionStart);
            buttonBonus.onClick.AddListener(ActionBonus);
            buttonSetting.onClick.AddListener(ActionSetting);
            buttonMessage.onClick.AddListener(ActionMessage);
            buttonProfile.onClick.AddListener(ActionProfile);
            buttonTheBranchOfEvolution.onClick.AddListener(ActionTheBranchOfEvolution);
            buttonShop.onClick.AddListener(ActionShop);
            buttonRating.onClick.AddListener(ActionRating);
        }

        #region InitializeButtons
        private void ActionStart()
        {
            if (GameManager.Instance.isGame)
                return;
            if (!isReadyStart)
                return;

            if (!isOneStart)
                AdManager.Instance.ShowInterstitial();
            else
                isOneStart = false;

            if (!IsFirstStartMenu)
            {
                IsFirstStartMenu = true;
                CanvasManager.Instance.tutoritalLevel.ActiveStage1();
            }
            else
            {
                for (int i = 0; i < CellManager.Instance.cellData.Count;  i++)
                {
                    CellManager.Instance.cellData[i].isReady = true;
                }
            }

            AudioManager2248.Instance.StartMainClip(false);
            GameManager.Instance.isGame = true;
            isReadyStart = false;
            animStart.SetTrigger(START);
        }

        private void ActionBonus()
        {

        }

        private void ActionSetting()
        {
            CanvasManager.Instance.actionWindow.OpenWindow(true, CanvasManager.Instance.canvasSettings, "canvasSettings");
        }

        private void ActionMessage()
        {

        }

        private void ActionProfile()
        {
            CanvasManager.Instance.actionWindow.OpenWindow(true, CanvasManager.Instance.canvasProfile, "CanvasProfile");
        }

        private void ActionTheBranchOfEvolution()
        {
            CanvasManager.Instance.actionWindow.OpenWindow(true, CanvasManager.Instance.canvasMonstrThreePack, "canvasMonstrThreePack");
        }

        private void ActionShop()
        {
            CanvasManager.Instance.canvasShop.GetComponent<CanvasShop>().AddListener(true);
            CanvasManager.Instance.actionWindow.OpenWindow(true, CanvasManager.Instance.canvasShop, "canvasShop");

        }

        private void ActionRating()
        {

        }
        #endregion
        #endregion

        #region Animation Event
        public void EventStartGame()
        {
            GameManager.Instance.isGame = true;
            LevelManager.Instance.InitializingLevel();
        }

        public void EventEndGame()
        {
            GameManager.Instance.isGame = false;
            LevelManager.Instance.InitializingEndLevel();
        }

        public void EventEndGameEnd()
        {
            isReadyStart = true;
        }

        public void EventEndGameFirst()
        {
            GameManager.Instance.isGame = false;
            CellManager.Instance.RaycastIsActive = false;
        }


        public void EventCanvasStartGame()
        {
            canvasGameObject.SetActive(true);
        }

        public void EventCanvasEndGame()
        {
            canvasGameObject.SetActive(false);
        }

        public void EventStartClipOpenDoor()
        {
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.openDoor);
        }

        public void EventStartClipCloseDoor()
        {
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.closeDoor);
        }   

        public void EventSwitchMusicGeneral()
        {
            AudioManager2248.Instance.StartMainClip(true);
        }
        #endregion end Animation Event

        #region Insctance
        public void ActionEnd()
        {
            animStart.SetTrigger(CLOSE);
        }
        #endregion end Insttance
    }
}

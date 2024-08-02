using Kosilek.Manager;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosilek.UI
{
    public class CanvasTrainingLevel : MonoBehaviour
    {
        #region Touch
        private Touch touch;
        #endregion

        #region Animator
        public Animator anim;
        public bool isActiveAnim = false;
        public bool isActivePauseAnim = false;
        #endregion

        #region Stage GameObject
        [Foldout("StageGameObjecct")]
        [SerializeField]
        private GameObject stage1;
        [Foldout("StageGameObjecct")]
        [SerializeField]
        private GameObject stage2;
        #endregion Stage GameObject

        public static bool isActiveTutorital;
        public static bool isActiveFIrstBust;
        public static bool isActiveSecondBust;
        // 4 9

        #region Const
        private const string STATE_STAGE_1 = "STATE_STAGE_1";
        private const string STATE_STAGE_2 = "STATE_STAGE_2";
        private const string STATE_STAGE_3 = "STATE_STAGE_3";
        private const string STATE_STAGE_4 = "STATE_STAGE_4";
        private const string STATE_STAGE_5 = "STATE_STAGE_5";
        private const string STATE_STAGE_6 = "STATE_STAGE_6";

        private const string ANIM_STATE_1 = "ANIM_STATE_1";
        private const string ANIM_STATE_END_1 = "ANIM_STATE_END_1";
        private const string ANIM_STATE_2 = "ANIM_STATE_2";
        private const string ANIM_STATE_END_2 = "ANIM_STATE_END_2";
        private const string ANIM_STATE_3 = "ANIM_STATE_3";
        private const string ANIM_STATE_END_3 = "ANIM_STATE_END_3";
        private const string ANIM_STATE_4 = "ANIM_STATE_4";
        private const string ANIM_STATE_END_4 = "ANIM_STATE_END_4";
        private const string ANIM_STATE_5 = "ANIM_STATE_5";
        private const string ANIM_STATE_END_5 = "ANIM_STATE_END_5";
        private const string ANIM_STATE_6 = "ANIM_STATE_6";
        private const string ANIM_STATE_END_6 = "ANIM_STATE_END_6";
        #endregion

        #region State Stage
        [NonSerialized]
        private static int _stateStage1 = -1;
        public static bool StateStage1
        {
            get
            {
                if (_stateStage1 == -1)
                    _stateStage1 = PlayerPrefs.GetInt(STATE_STAGE_1);
                return _stateStage1 == 1;
            }
            set
            {

                _stateStage1 = value ? 1 : 0;
                PlayerPrefs.SetInt(STATE_STAGE_1, _stateStage1);
            }
        }
        [NonSerialized]
        private static int _stateStage2 = -1;
        public static bool StateStage2
        {
            get
            {
                if (_stateStage2 == -1)
                    _stateStage2 = PlayerPrefs.GetInt(STATE_STAGE_2);
                return _stateStage2 == 1;
            }
            set
            {

                _stateStage2 = value ? 1 : 0;
                PlayerPrefs.SetInt(STATE_STAGE_2, _stateStage2);
            }
        }
        [NonSerialized]
        private static int _stateStage3 = -1;
        public static bool StateStage3
        {
            get
            {
                if (_stateStage3 == -1)
                    _stateStage3 = PlayerPrefs.GetInt(STATE_STAGE_3);
                return _stateStage3 == 1;
            }
            set
            {

                _stateStage3 = value ? 1 : 0;
                PlayerPrefs.SetInt(STATE_STAGE_3, _stateStage3);
            }
        }
        [NonSerialized]
        private static int _stateStage4 = -1;
        public static bool StateStage4
        {
            get
            {
                if (_stateStage4 == -1)
                    _stateStage4 = PlayerPrefs.GetInt(STATE_STAGE_4);
                return _stateStage4 == 1;
            }
            set
            {

                _stateStage4 = value ? 1 : 0;
                PlayerPrefs.SetInt(STATE_STAGE_4, _stateStage4);
            }
        }
        [NonSerialized]
        private static int _stateStage5 = -1;
        public static bool StateStage5
        {
            get
            {
                if (_stateStage5 == -1)
                    _stateStage5 = PlayerPrefs.GetInt(STATE_STAGE_5);
                return _stateStage5 == 1;
            }
            set
            {

                _stateStage5 = value ? 1 : 0;
                PlayerPrefs.SetInt(STATE_STAGE_5, _stateStage5);
            }
        }
        [NonSerialized]
        private static int _stateStage6 = -1;
        public static bool StateStage6
        {
            get
            {
                if (_stateStage6 == -1)
                    _stateStage6 = PlayerPrefs.GetInt(STATE_STAGE_6);
                return _stateStage6 == 1;
            }
            set
            {

                _stateStage6 = value ? 1 : 0;
                PlayerPrefs.SetInt(STATE_STAGE_6, _stateStage6);
            }
        }
        #endregion

        public void ActiveStage1()
        {
            CanvasManager.Instance.canvasTutoritalLevel.Open();
            isActiveTutorital = true;
            isActiveFIrstBust = false;
            isActiveSecondBust = false;
        }

        public void ActiveStage2()
        {
            StateStage1 = true;
            stage1.SetActive(false);
            stage2.SetActive(true);
            CellManagerActiveStage2();
        }

        public void ActiveStage5()
        {
            stage2.SetActive(false);
        }

        private void CellManagerActiveStage2()
        {
            CellManager.Instance.cellData[4].isReady = true;
            CellManager.Instance.cellData[9].isReady = true;
        }

        public void CellManagerActiveStage3()
        {
            CellManager.Instance.cellData[17].isReady = true;
            CellManager.Instance.cellData[22].isReady = true;
            CellManager.Instance.cellData[27].isReady = true;
            CellManager.Instance.cellData[32].isReady = true;
            CellManager.Instance.cellData[37].isReady = true;
        }

        public void CellManagerActiveStage4()
        {
            CellManager.Instance.cellData[29].isReady = true;
            CellManager.Instance.cellData[23].isReady = true;
            CellManager.Instance.cellData[19].isReady = true;
            CellManager.Instance.cellData[13].isReady = true;
            CellManager.Instance.cellData[7].isReady = true;
        }

        #region Update
        private void Update()
        {
            if (isActiveTutorital)
            {
                if (!StateStage2 && StateStage1)
                {
                    ActionUpdate(ANIM_STATE_1, ANIM_STATE_END_1);
                }
                else if (!StateStage3 && StateStage2)
                {
                    ActionUpdate(ANIM_STATE_2, ANIM_STATE_END_2);
                }
                else if (!StateStage4 && StateStage3)
                {
                    ActionUpdate(ANIM_STATE_3, ANIM_STATE_END_3);
                }
            }
            #endregion
        }

        private void ActionUpdate(string start, string end)
        {
            if (!isActiveAnim)
            {
                isActiveAnim = true;
                anim.SetTrigger(start);
            }
            else
            {
                //       #if UNITY_EDITOR
                if (Input.GetMouseButton(0))
                {
                    if (!isActivePauseAnim)
                    {
                        isActivePauseAnim = true;
                        anim.SetTrigger(end);
                    }

                }
                else if (Input.GetMouseButtonUp(0))
                {
                    isActiveAnim = false;
                    isActivePauseAnim = false;
                }
                //#else
                if (Input.touchCount > 0)
                {
                    touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                    {
                        if (!isActivePauseAnim)
                        {
                            isActivePauseAnim = true;
                            anim.SetTrigger(end);
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        isActiveAnim = false;
                        isActivePauseAnim = false;
                    }
                }
            }
        }
    }
}
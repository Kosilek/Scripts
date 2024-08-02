using DredPack;
using Kosilek.Data;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using Kosilek.MonstrPack;
using Kosilek.SaveAndLoad;
using System;
using Kosilek.UI;
using System.Collections;

namespace Kosilek.Manager
{
    public class CellManager : SimpleSingleton<CellManager>, ISaveble
    {
        #region Touch
        private Touch touch;
        #endregion
        #region ScriptsComponent
        [Foldout("ScriptsComponent")]
        public SeeCell seeCell;
        #endregion end ScriptsComponent
        #region CellData
        [Foldout("CellData")]
        public List<GameObject> cellOutLine;
         [Foldout("CellData")]
      //  [HideInInspector]
        public List<CellGame> cellData;
        [Foldout("CellData")]
        public List<CellManagerData> cellManagerData;
        [Foldout("CellData")]
        public List<Transform> allCellTransform;
        #endregion

        public bool RaycastIsActive = false;
        public Camera _camera;
        public List<int> indexCell;
        public List<int> columnCell;
        private bool isFirstCell = false;
        public List<Vector3> middlePos = new();

        public Transform cellLine;
        public Transform endCellLine;

        public Vector3 startPos;
        public Vector3 endPos;

        public ulong indexFirstCell = 0;

        #region spawn
        [Foldout("Spawn")]
        public Transform parentTransform;
        [Foldout("Spawn")]
        public GameObject cellPrefabe;
        #endregion

        public Animator anim;

        #region const
        private const float posXFirst = -423f;
        private const float posXSecond = -211.5f;
        private const float posXThird = 0f;
        private const float posXFourth = 211.5f;
        private const float posXFifth = 423f;
        private const int startCountList = 8;

        public const int MAX_RAND_ICON = 6;

        public const float DELAY_DESTROY = 0.5f;

        private const string EFFECT_TEXT = "Effect";
        private const string IS_FIRST_START = "IsFirstStart";
        #endregion

        private int valueStep = 1;

        private CellGame saveLastCell;

        private CellGame saveCellLat;

        [NonSerialized]
        private int _isFirstStart = -1;
        public bool IsFirstStart
        {
            get
            {
                if (_isFirstStart == -1)
                    _isFirstStart = PlayerPrefs.GetInt(IS_FIRST_START);
                return _isFirstStart == 1;
            }
            set
            {

                _isFirstStart = value ? 1 : 0;
                PlayerPrefs.SetInt(IS_FIRST_START, _isFirstStart);
            }
        }

        public List<int> cellDataFirstStart;

        public int counter;

        private bool isActiveState1 = false;
        private bool isActiveState2 = false;

        private void Start()
        {
            FillingInTheListAll();
            SetValues();
            SetValuesList();
            AddListIndex();
        }

        #region Start
        #region FillingInTheListAll
        private void FillingInTheListAll()
        {
            FillingInTheListCellgame();

            for (int i = 0; i < cellManagerData.Count; i++)
            {
                FillingInTheListCellManagerData(i);
            }
        }

        private void FillingInTheListCellgame()
        {
            for (int i = 0; i < cellOutLine.Count; i++)
            {
                cellData.Add(cellOutLine[i].transform.GetChild(0).GetComponent<CellGame>());
            }
        }

        private void FillingInTheListCellManagerData(int startIndex)
        {
            FillingInTheListCellManagerDataCellTransfrom(startIndex);

            FillingInTheListCellManagerDataColumn(startIndex);
        }

        #region FillingInTheListCellManagerData
        private void FillingInTheListCellManagerDataCellTransfrom(int startIndex)
        {
            for (int i = startIndex; i < allCellTransform.Count; i += 5)
            {
                cellManagerData[startIndex].cellTransform.Add(allCellTransform[i]);
            }
        }

        private void FillingInTheListCellManagerDataColumn(int startIndex)
        {
            for (int i = startIndex; i < cellData.Count; i += 5)
            {
                cellManagerData[startIndex].column.Add(cellData[i]);
            }
        }
        #endregion end FillingInTheListCellManagerData
        #endregion end FillingInTheListAll
        private void SetValues()
        {
            var indexColumn = 0;
            var column = 0;
            for (int i = 0; i < cellData.Count; i++)
            {
                cellData[i].index = i;
                cellData[i].column = column;
                cellData[i].indexColumn = indexColumn;
                column++;
                if (column == 5)
                {
                    column = 0;
                    indexColumn++;
                }
            }
        }

        private void SetValuesList()
        {
            for (int i = 0; i < cellManagerData.Count; i++)
            {
                CompletionList(cellManagerData[i].cellTransform, ref cellManagerData[i].cellDataTransform);
            }
        }

        private void CompletionList(List<Transform> cellTransform, ref List<CellDataTransform> cellDataTransform)
        {
            for (int i = 0; i < cellTransform.Count; i++)
            {
                cellDataTransform.Add(new CellDataTransform(i, cellTransform[i]));
            }
        }

        private void AddListIndex()
        {
            for (int i = 0; i < cellManagerData[0].column.Count; i++)
            {
                cellManagerData[0].indexColumn.Add(cellManagerData[0].column[i].index);
            }

            for (int i = 0; i < cellManagerData[1].column.Count; i++)
            {
                cellManagerData[1].indexColumn.Add(cellManagerData[1].column[i].index);
            }

            for (int i = 0; i < cellManagerData[2].column.Count; i++)
            {
                cellManagerData[2].indexColumn.Add(cellManagerData[2].column[i].index);
            }

            for (int i = 0; i < cellManagerData[3].column.Count; i++)
            {
                cellManagerData[3].indexColumn.Add(cellManagerData[3].column[i].index);
            }

            for (int i = 0; i < cellManagerData[4].column.Count; i++)
            {
                cellManagerData[4].indexColumn.Add(cellManagerData[4].column[i].index);
            }
        }

        #region InitializingCell
        public void InitializingCell()
        {
            try
            {
                string[] cellData = Load(Path.DefaultPath + Path.Cell + "0" + ".txt");
                Action action = cellData.Length > 0 ? InitializingCellLoadData : InitializingCellNewGame;
                action?.Invoke();
            }
            catch
            {
                Debug.LogError("Error: Failed initialization of cell data, InitializingCell");
            }
        }

        public void InitializingCellNewGame()
        {
            try
            {
                var maxRandIcon = ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count > MAX_RAND_ICON ? MAX_RAND_ICON : ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count;
                if (!IsFirstStart)
                {
                    for (int i = 0; i < cellData.Count; i++)
                    {
                       // var random = UnityEngine.Random.Range(0, maxRandIcon);
                        cellData[i].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[cellDataFirstStart[i]].iconMonstr;
                        cellData[i].indexMonstr = ManagerMonstrPack.Instance.monsterCollection.monstrPack[cellDataFirstStart[i]].indexMonstr;
                        cellData[i].numberMonstr = ManagerMonstrPack.Instance.monsterCollection.monstrPack[cellDataFirstStart[i]].numberMonstr;
                        cellData[i].textIndexMonstr.text = cellData[i].indexMonstr.ToString();
                    }
                    IsFirstStart = true;
                }
                else
                {
                    for (int i = 0; i < cellData.Count; i++)
                    {
                        var random = UnityEngine.Random.Range(0, maxRandIcon);
                        cellData[i].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[random].iconMonstr;
                        cellData[i].indexMonstr = ManagerMonstrPack.Instance.monsterCollection.monstrPack[random].indexMonstr;
                        cellData[i].numberMonstr = ManagerMonstrPack.Instance.monsterCollection.monstrPack[random].numberMonstr;
                        cellData[i].textIndexMonstr.text = cellData[i].indexMonstr.ToString();
                    }
                }
               
                for (int i = 0; i < cellData.Count; i++)
                {
                    Save(true, Path.Cell + i + ".txt", cellData[i].indexMonstr.ToString());
                }
                if (!IsFirstStart)
                {
                    IsFirstStart = true;
                }
            }
            catch
            {
                Debug.LogError("Error: The level manager gave a fuck-up");
            }
        }

        private void InitializingCellLoadData()
        {
            for (int i = 0; i < cellData.Count; i++)
            {
                var indexString = Load(Path.DefaultPath + Path.Cell + i + ".txt");
                ulong index = Convert.ToUInt32(indexString[0]);
                cellData[i].indexMonstr = index;
                for (int j = 0; j < ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count; j++)
                {
                    if (ManagerMonstrPack.Instance.monsterCollection.monstrPack[j].indexMonstr == index)
                    {
                        cellData[i].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[j].iconMonstr;
                        cellData[i].numberMonstr = ManagerMonstrPack.Instance.monsterCollection.monstrPack[j].numberMonstr;
                        break;
                    }
                }
                //cellData[i].textIndexMonstr.text = cellData[i].indexMonstr.ToString();
                cellData[i].textIndexMonstr.text = DigitConverter.digitConverter((ulong)cellData[i].indexMonstr);
            }
        }
        #endregion
        #endregion endStart

        #region Transform

        #endregion

        private void Update()
        {
            if (BustManager.Instance.isHummer || BustManager.Instance.isExchange)
                return;
#if UNITY_EDITOR
            MoveWindow();
#else
            MoveAndriod();
#endif
        }

        #region MoveCell
        #region Window
        private void MoveWindow()
        {
            if (RaycastIsActive)
            {
                if (Input.GetMouseButton(0))
                {
                    Ray ray = GetPointToRay(Input.mousePosition);
                    PhysicsRaycast(ray);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    CanvasManager.Instance.canvasGameScr.InVisibleCell();
                    LineManager.Instance.ClearLine();
                    if (saveLastCell == null)
                        return;

                    ActionTutorital();

                    AudioClip clip = indexCell.Count > 4 ? AudioManager2248.Instance.audioContainer.updateCell : AudioManager2248.Instance.audioContainer.comboOrNewMaxMonstr;
                    AudioManager2248.Instance.StartEffectClip(clip);

                    Action<int> action = indexCell.Count > 4 ? MoneyManager.Instance.SetMoney : null;
                    action?.Invoke(1);
                    Action<string> callAnim = indexCell.Count > 4 ? anim.SetTrigger : null;
                    callAnim?.Invoke(EFFECT_TEXT);
                    Action<AudioClip> callSound = indexCell.Count > 4 ? AudioManager2248.Instance.StartEffectClip : null;
                    callSound?.Invoke(AudioManager2248.Instance.audioContainer.addDNKCombo);

                    if (indexCell.Count == 0 || indexCell.Count == 1)
                    {
                        DataReset();
                        return;
                    }

                    saveLastCell.isActive = false;

                    var conter = CheckUpdateCell();
                    UpdateCell(conter);

                    isFirstCell = false;
                    for (int i = 0; i < indexCell.Count; i++)
                    {
                        columnCell.Add(cellData[indexCell[i]].column);
                    }
                    for (int i = 0; i < indexCell.Count; i++)
                    {
                        cellData[indexCell[i]].DestroyObject(indexCell[i], cellData[indexCell[i]].column, cellData[indexCell[i]].indexColumn);
                    }
                    CanvasManager.Instance.canvasGameScr.MoveSpeed(conter, saveLastCell.transform);
                    saveLastCell = null;
                    RemoveCellList();
                    MoveAllCell();
                    SetListIndex();
                    SortCell();
                    DataReset();
                    SaveCell();
                    CheckLose();         
                }
            }
        }
        #endregion

        private void ActionTutorital()
        {
            if (!CanvasTrainingLevel.isActiveTutorital)
                return;

            if (CanvasTrainingLevel.StateStage1)
            {
                if (!CanvasTrainingLevel.StateStage2 && indexCell.Count == 2)
                {
                    CanvasTrainingLevel.StateStage2 = true;
                    CanvasManager.Instance.tutoritalLevel.CellManagerActiveStage3();
                }
                else if (!CanvasTrainingLevel.StateStage3)
                {
                    if (indexCell.Count == 5)
                    {
                        CanvasTrainingLevel.StateStage3 = true;
                        CanvasManager.Instance.tutoritalLevel.CellManagerActiveStage4();
                    }
                    else
                    {
                        DataReset();
                    }

                }
                else if (!CanvasTrainingLevel.StateStage4 && indexCell.Count == 5)
                {
                    if (indexCell.Count == 5)
                    {
                        CanvasTrainingLevel.StateStage4 = true;
                        CanvasManager.Instance.tutoritalLevel.ActiveStage5();
                        for (int i = 0; i < cellData.Count; i++)
                        {
                            cellData[i].isReady = true;
                        }
                    }
                    else
                    {
                        DataReset();
                    }
                }
                else if (CanvasTrainingLevel.StateStage4 && !CanvasTrainingLevel.StateStage5 && !isActiveState1)
                {
                    if (counter <= 3)
                        counter++;
                    else
                    {
                        isActiveState1 = true;
                        var minIndex = -1;
                        var minValues = (ulong)2;
                        var check = false;
                        for (int i = 0; i < cellData.Count; i++)
                        {
                            cellData[i].isReady = false;
                            if (cellData[i].indexMonstr == minValues && !check)
                            {
                                minIndex = i;
                                check = true;
                            }
                        }
                        if (check == false)
                        {
                            minValues = 4;
                            for (int i = 0; i < cellData.Count; i++)
                            {
                                if (cellData[i].indexMonstr == minValues && !check)
                                {
                                    minIndex = i;
                                    check = true;
                                }
                            }
                        }
                        if (check == false)
                        {
                            minValues = 8;
                            for (int i = 0; i < cellData.Count; i++)
                            {
                                if (cellData[i].indexMonstr == minValues && !check)
                                {
                                    minIndex = i;
                                    check = true;
                                }
                            }
                        }
                        if (check == false)
                        {
                            minValues = 16;
                            for (int i = 0; i < cellData.Count; i++)
                            {
                                if (cellData[i].indexMonstr == minValues && !check)
                                {
                                    minIndex = i;
                                    check = true;
                                }
                            }
                        }
                        if (check == false)
                        {
                            minValues = 32;
                            for (int i = 0; i < cellData.Count; i++)
                            {
                                if (cellData[i].indexMonstr == minValues && !check)
                                {
                                    minIndex = i;
                                    check = true;
                                }
                            }
                        }
                        if (check == false)
                        {
                            minValues = 65;
                            for (int i = 0; i < cellData.Count; i++)
                            {
                                if (cellData[i].indexMonstr == minValues && !check)
                                {
                                    minIndex = i;
                                    check = true;
                                }
                            }
                        }
                        Debug.Log("MININDEX = " + minIndex + " MinValue = " + minValues);
                        cellData[minIndex].isReady = true;
                        CanvasTrainingLevel.isActiveFIrstBust = true;
                        counter = 0;
                    }
                }
                else if (CanvasTrainingLevel.StateStage5 && !CanvasTrainingLevel.StateStage6 && !isActiveState2)
                {
                    if (counter <= 3)
                        counter++;
                    else
                    {
                        isActiveState2 = true;
                        var check = false;
                        var minIndexSecond = 0;
                        var minRandomX = UnityEngine.Random.Range(1, 4);
                        var minRandomY = UnityEngine.Random.Range(1, 8);
                        var minIndex = minRandomX * minRandomY - 1;
                        if (cellData[minIndex].indexMonstr != cellData[minIndex - 1].indexMonstr)
                        {
                            check = true;
                            minIndexSecond = minIndex - 1;
                        }
                        else if (cellData[minIndex].indexMonstr != cellData[minIndex + 1].indexMonstr)
                        {
                            check = true;
                            minIndexSecond = minIndex + 1;
                        }
                        else if (cellData[minIndex].indexMonstr != cellData[minIndex + 5].indexMonstr)
                        {
                            check = true;
                            minIndexSecond = minIndex + 5;
                        }
                        else if (cellData[minIndex].indexMonstr != cellData[minIndex - 5].indexMonstr)
                        {
                            check = true;
                            minIndexSecond = minIndex - 5;
                        }

                        while (!check)
                        {
                            if (!check)
                            {
                                minIndexSecond = 0;
                                minRandomX = UnityEngine.Random.Range(1, 4);
                                minRandomY = UnityEngine.Random.Range(1, 8);
                                minIndex = minRandomX * minRandomY - 1;
                                if (cellData[minIndex].indexMonstr != cellData[minIndex - 1].indexMonstr)
                                {
                                    check = true;
                                    minIndexSecond = minIndex - 1;
                                }
                                else if (cellData[minIndex].indexMonstr != cellData[minIndex + 1].indexMonstr)
                                {
                                    check = true;
                                    minIndexSecond = minIndex + 1;
                                }
                                else if (cellData[minIndex].indexMonstr != cellData[minIndex + 5].indexMonstr)
                                {
                                    check = true;
                                    minIndexSecond = minIndex + 5;
                                }
                                else if (cellData[minIndex].indexMonstr != cellData[minIndex - 5].indexMonstr)
                                {
                                    check = true;
                                    minIndexSecond = minIndex - 5;
                                }
                            }
                        }

                        Debug.Log("Index1 = " + minIndex + " Index2 = " + minIndexSecond);
                        CanvasTrainingLevel.isActiveSecondBust = true;
                        for (int i = 0; i < cellData.Count; i++)
                        {
                            cellData[i].isReady = false;
                        }
                        cellData[minIndex].isReady = true;
                        cellData[minIndexSecond].isReady = true;
                    }
                }
            }
        }

        #region Android
        private void MoveAndriod()
        {
            if (RaycastIsActive)
            {
                if (Input.touchCount > 0)
                {
                    touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                    {
                        Ray ray = GetPointToRay(touch.position);
                        PhysicsRaycast(ray);
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        CanvasManager.Instance.canvasGameScr.InVisibleCell();
                        LineManager.Instance.ClearLine();
                        if (saveLastCell == null)
                            return;

                        AudioClip clip = indexCell.Count > 4 ? AudioManager2248.Instance.audioContainer.updateCell : AudioManager2248.Instance.audioContainer.comboOrNewMaxMonstr;
                        AudioManager2248.Instance.StartEffectClip(clip);

                        Action<int> action = indexCell.Count > 4 ? MoneyManager.Instance.SetMoney : null;
                        action?.Invoke(1);
                        Action<string> callAnim = indexCell.Count > 4 ? anim.SetTrigger : null;
                        callAnim?.Invoke(EFFECT_TEXT);
                        Action<AudioClip> callSound = indexCell.Count > 4 ? AudioManager2248.Instance.StartEffectClip : null;
                        callSound?.Invoke(AudioManager2248.Instance.audioContainer.addDNKCombo);

                        if (indexCell.Count == 0 || indexCell.Count == 1)
                        {
                            DataReset();
                            return;
                        }

                        saveLastCell.isActive = false;

                        var conter = CheckUpdateCell();
                        UpdateCell(conter);

                        isFirstCell = false;
                        for (int i = 0; i < indexCell.Count; i++)
                        {
                            columnCell.Add(cellData[indexCell[i]].column);
                        }
                        for (int i = 0; i < indexCell.Count; i++)
                        {
                            cellData[indexCell[i]].DestroyObject(indexCell[i], cellData[indexCell[i]].column, cellData[indexCell[i]].indexColumn);
                        }
                        CanvasManager.Instance.canvasGameScr.MoveSpeed(conter, saveLastCell.transform);
                        saveLastCell = null;
                        RemoveCellList();
                        MoveAllCell();
                        SetListIndex();
                        SortCell();
                        DataReset();
                        SaveCell();
                        CheckLose();
                    }
                }
            }
        }

        #endregion
        #region UpdateCell
        private ulong CheckUpdateCell()
        {
            ulong counter = 0;
            List<ulong> ints = new List<ulong>();
            for (int i = 0; i < indexCell.Count; i++)
            {
                ints.Add(cellData[indexCell[i]].indexMonstr);
            }

            for (int i = 0; i < ints.Count; i++)
            {
                counter += ints[i];
            }

            for (int i = 0; i < 101; i++)
            {
                if (counter == Convert.ToUInt32(Mathf.Pow(2, i)))
                {
                    return counter;
                }
                else if (counter < Convert.ToUInt32(Mathf.Pow(2, i)))
                {
                    counter = Convert.ToUInt32(Mathf.Pow(2, i));
                    return counter;
                }
            }
            return 0;

            /*var counter = 0;
            var counterIndex = 0;
            var indexSave = cellData[indexCell[0]].indexMonstr;

            for (int i = 1; i < ints.Count; i++)
            {
                if (ints[i] == indexSave)
                {
                    counterIndex++;

                    if (counterIndex == 1)
                    {
                        counter++;
                    }    
                    else if (counterIndex == 2)
                    {
                        counter++;
                    }
                    else if (counterIndex == 3)
                    {
                    }
                    else if (counterIndex == 4)
                    {
                        counter++;
                    }
                    else if (counterIndex == 5)
                    {

                    }
                    else if (counterIndex == 6)
                    {
                        counter++;
                        counterIndex = 0;
                    }
                }
                else
                {
                    indexSave = ints[i];
                    counterIndex = 0;
                }
            }
            return counter;*/
        }

        public void RemoveCellList()
        {
            for (int i = cellData.Count - 1; i >= 0; i--)
            {
                if (cellData[i] == null)
                {
                    cellData.RemoveAt(i);
                }
            }
        }

        public void SaveCell()
        {
            for (int i = 0; i < cellData.Count; i++)
            {
                Save(true, Path.Cell + i + ".txt", cellData[i].indexMonstr.ToString());
            }
        }

        public void SortCell()
        {
            cellData.Sort(delegate (CellGame x, CellGame y)
            {
                return x.index.CompareTo(y.index);
            });
        }

        public void MoveAllCellStart()
        {
            for (int i = 0; i < cellManagerData.Count; i++)
            {
                SavingEmptyCells(ref cellManagerData[i].column, i, ref cellManagerData[i].counterList, ref cellManagerData[i].counter, ref cellManagerData[i].positionList);
            }

            for (int i = 0; i < cellManagerData.Count; i++)
            {
                NeedToMoovCell(ref cellManagerData[i].column, cellManagerData[i].positionList);
            }

            Spawn();

        }

        public void MoveAllCellEnd()
        {
            for (int i = 0; i < cellManagerData.Count; i++)
            {
                MoveCell(ref cellManagerData[i].column, ref cellManagerData[i].positionList, ref cellManagerData[i].isStartFor, ref cellManagerData[i].stepColumn, cellManagerData[i].cellDataTransform);
            }
        }

        public void MoveAllCell()
        {
            MoveAllCellStart();
            MoveAllCellEnd();
        }

        private void UpdateCell(ulong counter)
        {
            var saveIndex = indexCell[indexCell.Count - 1];
            if (indexCell.Count > 0)
                indexCell.RemoveAt(indexCell.Count - 1);
            if (middlePos.Count > 0)
                middlePos.RemoveAt(middlePos.Count - 1);

            StartCoroutine(IE());

            IEnumerator IE()
            {
                cellData[saveIndex].StartAnimationFade();
                var delay = DELAY_DESTROY / 2f;
                saveIndex = CheckSaveInddex(saveIndex);
                yield return new WaitForSeconds(delay);
                UpdateUICellData(saveIndex, counter);
                if (!CanvasTrainingLevel.isActiveTutorital)
                    CheckMaxNumberMonstr(saveIndex);
            }
        }

        private int CheckSaveInddex(int saveIndex)
        {
            var column = cellData[saveIndex].column;
            var indexColumn = cellData[saveIndex].indexColumn;

            var counter = 0;
            for (int i = 0; i < indexCell.Count; i++)
            {
                if (cellData[indexCell[i]].column == column && cellData[indexCell[i]].indexColumn < indexColumn && cellData[indexCell[i]].isActive)
                {
                    counter++;
                }
            }

            saveIndex -= counter * 5;
            return saveIndex;
        }

        private void UpdateUICellData(int saveIndex, ulong counter)
        {
            cellData[saveIndex].indexMonstr = counter;
            ulong number = (ulong)counter;
            cellData[saveIndex].textIndexMonstr.text = DigitConverter.digitConverter(number);
            //cellData[saveIndex].textIndexMonstr.text = cellData[saveIndex].indexMonstr.ToString();
            for (int i = 0; i < ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count; i++)
            {
                if (cellData[saveIndex].indexMonstr == ManagerMonstrPack.Instance.monsterCollection.monstrPack[i].indexMonstr)
                {
                    cellData[saveIndex].image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[i].iconMonstr;
                    cellData[saveIndex].numberMonstr = ManagerMonstrPack.Instance.monsterCollection.monstrPack[i].numberMonstr;
                }
            }
        }

        private void CheckMaxNumberMonstr(int saveIndex)
        {
            if (MonstrIndexManager.Instance.maxNumberMonstOpen < cellData[saveIndex].numberMonstr)
            {
                GameManager.Instance.GamePause(true, CanvasManager.Instance.canvasNewMonstrUnlock);
                MonstrIndexManager.Instance.maxNumberMonstOpen = cellData[saveIndex].numberMonstr;
                CanvasManager.Instance.seeMonstrPrize.Move();
                CanvasManager.Instance.canvasNewMonstrUnlockScr.InitializeSwitchRoulette();
                MonstrIndexManager.Instance.UpdateUIIcon(MonstrIndexManager.Instance.maxNumberMonstOpen);
            }
        }

        private (Sprite, string) UpdateUICell(ulong counter)
        {
            var saveIndex = indexCell[indexCell.Count - 1];
            Sprite sprite = null;
            string text = "";
            ulong index = 0;

            index = counter;
            for (int i = 0; i < ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count; i++)
            {
                if (index == ManagerMonstrPack.Instance.monsterCollection.monstrPack[i].indexMonstr)
                {
                    sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[i].iconMonstr;
                    text = index.ToString();
                }

            }
            return (sprite, text);
        }

        #endregion
        #region General Action Move 
        #region Press Action
        public Ray GetPointToRay(Vector3 pos)
        {
            Ray ray = _camera.ScreenPointToRay(pos);
            return ray;
        }

        public void PhysicsRaycast(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                endCellLine.transform.position = hit.point;
                HItCollider(hit);
                endPos = endCellLine.transform.position;
                if (isFirstCell)
                    LineManager.Instance.StartLine(startPos, endCellLine.transform.position, middlePos);
            }
        }

        private void HItCollider(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent(out CellGame data))
            {
                var checkItemData = data;
                CheckItemDataActive(checkItemData);
            }
            else
            {
                LineManager.Instance.isRaycastCell = false;
                if (saveCellLat != null)
                {
                    saveCellLat.isActive = false;
                    saveCellLat = null;
                }
            }

        }

        private void CheckItemDataActive(CellGame checkItemData)
        {
            if (!checkItemData.isActive && checkItemData.isReady)
            {
                if (!isFirstCell)
                {
                    isFirstCell = true;
                    startPos = checkItemData.transform.position;
                    indexFirstCell = checkItemData.indexMonstr;
                    ActionChoseCell(checkItemData);
                    UpdateUICanvasGameCell();

                }
                else
                {
                    indexFirstCell = cellData[indexCell[indexCell.Count - 1]].indexMonstr;
                    if (CheckOnlyIndex(checkItemData.indexMonstr))
                    {
                        SecondActionCell(checkItemData);
                    }
                    UpdateUICanvasGameCell();
                }
            }
            else if (checkItemData.isActive)
            {
                if (!LineManager.Instance.isRaycastCell)
                {
                    if (indexCell.Count == 1)
                        return;

                    if (middlePos[middlePos.Count - 1] == checkItemData.transform.position)
                    {
                        if (cellData[indexCell[indexCell.Count - 1]].indexMonstr != cellData[indexCell[indexCell.Count - 2]].indexMonstr)
                            indexFirstCell = cellData[indexCell[indexCell.Count - 2]].indexMonstr;
                        checkItemData.StartEffect(false);
                        middlePos.RemoveAt(middlePos.Count - 1);
                        indexCell.RemoveAt(indexCell.Count - 1);
                        saveCellLat = checkItemData;
                        if (AudioManager2248.Instance.isVibration)
                            VibrationManager.Instance.StartVibration();
                        AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.comboOrNewMaxMonstr);
                    }
                    UpdateUICanvasGameCell();
                }
            }
            LineManager.Instance.isRaycastCell = true;
        }

        private void UpdateUICanvasGameCell()
        {
            Sprite sprite = null;
            var text = "";
            var conter = CheckUpdateCell();
            (sprite, text) = UpdateUICell(conter);
            CanvasManager.Instance.canvasGameScr.VisibleCell(sprite, Convert.ToInt32(text));
        }

        private bool CheckOnlyIndex(ulong index)
        {
            if (indexCell.Count == 1)
            {
                if (index == indexFirstCell)
                {
                    return true;
                }
                else return false;
            }
            {
                if (index == indexFirstCell)
                {
                    return true;
                }
                else if (index == indexFirstCell * 2)
                {
                    indexFirstCell *= 2;
                    return true;
                }
                else return false;
            }
        }

        private void SecondActionCell(CellGame checkItemData)
        {
            if (checkItemData.column != 0 && checkItemData.column != 4)
            {
                var result = Mathf.Abs(indexCell[indexCell.Count - 1] - checkItemData.index);
                if (result == 4 || result == 5 || result == 6 || result == 1)
                {
                    ActionChoseCell(checkItemData);
                }
            }
            else
            {
                var result = indexCell[indexCell.Count - 1] - checkItemData.index;
                if (checkItemData.column == 0)
                {
                    if (Mathf.Abs(result) == 5 || result == 1 || result == -4 || result == 6)
                    {
                        ActionChoseCell(checkItemData);
                    }
                }
                else if (checkItemData.column == 4)
                {
                    if (Mathf.Abs(result) == 5 || result == -1 || result == -6 || result == 4)
                    {
                        ActionChoseCell(checkItemData);
                    }
                }
            }
        }

        private void ActionChoseCell(CellGame checkItemData)
        {
            checkItemData.isActive = true;
            checkItemData.StartEffect(true);
            indexCell.Add(checkItemData.index);
            middlePos.Add(checkItemData.transform.position);
            saveLastCell = checkItemData;
            if (AudioManager2248.Instance.isVibration)
                VibrationManager.Instance.StartVibration();
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.comboOrNewMaxMonstr);
        }
        #endregion
        #region LetGo Action
        private void SavingEmptyCells(ref List<CellGame> column, int step, ref List<int> counterList, ref int counter, ref List<int> positionList)
        {
            for (int i = column.Count - 1; i >= 0; i--)
            {
                if (column[i] == null)
                {
                    counterList.Add(step + (i * 5));
                    column.RemoveAt(i);
                    counter++;
                    positionList.Add(i);
                }
            }
        }

        private void NeedToMoovCell(ref List<CellGame> column, List<int> positionList)
        {
            if (positionList.Count == 0)
                return;

            for (int i = 0; i < column.Count; i++)
            {
                if (column[i].indexColumn < positionList[positionList.Count - 1])
                {
                    column[i].isNeedToMove = true;
                }
            }
        }

        private void Spawn()
        {
            var stepFirst = startCountList;
            var stepSecond = startCountList;
            var stepThird = startCountList;
            var stepFourth = startCountList;
            var stepFifth = startCountList;
            for (int i = 0; i < indexCell.Count; i++)
            {
                switch (columnCell[i])
                {
                    case 0:

                        SpawnCell(columnCell[i], indexCell[i], stepFirst);
                        stepFirst += 1;
                        break;
                    case 1:

                        SpawnCell(columnCell[i], indexCell[i], stepSecond);
                        stepSecond += 1;
                        break;
                    case 2:

                        SpawnCell(columnCell[i], indexCell[i], stepThird);
                        stepThird += 1;
                        break;
                    case 3:

                        SpawnCell(columnCell[i], indexCell[i], stepFourth);
                        stepFourth += 1;
                        break;
                    case 4:

                        SpawnCell(columnCell[i], indexCell[i], stepFifth);
                        stepFifth += 1;
                        break;
                    default: Debug.LogError("Error: column is not value"); break;
                }
            }
        }

        private void MoveCell(ref List<CellGame> column, ref List<int> postionList, ref bool isStartFor, ref int stepColumn, List<CellDataTransform> cellDataTransform)
        {
            var index = 0;
            valueStep = 1;

            for (int i = 0; i < column.Count; i++)
            {
                if (column[i] != null && index < postionList.Count && !column[i].isNeedToMove)
                {
                    var count = postionList.Count - 1;
                    if (isStartFor)
                    {
                        count -= valueStep;
                        valueStep++;
                    }

                    List<int> counts = new();
                    for (int j = 0; j < count + 1; j++)
                    {
                        if (count - j + stepColumn <= column[i].indexColumn && !column[i].isNewSpawn)
                            counts.Add(count - j + stepColumn);
                        else if (column[i].isNewSpawn)
                            counts.Add(count - j + stepColumn);
                    }
                    column[i].cellMove.StartMoveManyStep(counts, cellDataTransform);
                    index++;
                    postionList.Add(postionList[postionList.Count - 1] + 1);
                    stepColumn++;
                    isStartFor = true;
                }
                else
                {
                    stepColumn++;
                }
            }
        }

        public void DataReset()
        {
            indexCell.Clear();
            columnCell.Clear();
            middlePos.Clear();
            indexFirstCell = 0;
            isFirstCell = false;
            if (saveLastCell != null)
            {
                saveLastCell.isActive = false;
                saveLastCell = null;
            }

            for (int i = 0; i < cellManagerData.Count; i++)
            {
                cellManagerData[i].positionList.Clear();
                cellManagerData[i].isStartFor = false;
                cellManagerData[i].stepColumn = 0;
            }

            for (int i = 0; i < cellData.Count; i++)
            {
                cellData[i].isNewSpawn = false;
                cellData[i].isNeedToMove = false;
                cellData[i].isActive = false;
            }
        }
        #endregion
        #endregion
        #endregion
        #region SpawnCell
        private void SpawnCell(int column, int index, int step)
        {
            var posX = 0f;
            Vector3 vector3 = new Vector3();
            (posX, vector3) = GetSpawnPosition(column, step);
            var cell = Instantiate(cellPrefabe, new Vector3(vector3.x, vector3.y, 0f), new Quaternion(0f, 0f, 0f, 0f));
            cell.transform.SetParent(parentTransform);
            cell.transform.localScale = Vector3.one;
            var cellData = cell.transform.GetChild(0).GetComponent<CellGame>();
            cellData.InitializationData(false);
            AddListColumn(column, cellData);
            cellData.column = column;
            cellData.isReady = true;
            cellData.isNewSpawn = true;
            this.cellData.Add(cellData);
        }

        private (float, Vector3) GetSpawnPosition(int column, int step)
        {
            switch (column)
            {
                case 0:
                    return (posXFirst, cellManagerData[0].cellTransform[step].position);
                case 1:
                    return (posXSecond, cellManagerData[1].cellTransform[step].position);
                case 2:
                    return (posXThird, cellManagerData[2].cellTransform[step].position);
                case 3:
                    return (posXFourth, cellManagerData[3].cellTransform[step].position);
                case 4:
                    return (posXFifth, cellManagerData[4].cellTransform[step].position);
                default: Debug.LogError("Error: column is not value"); return (-1f, Vector3.zero);
            }
        }

        private void AddListColumn(int column, CellGame cellData)
        {
            switch (column)
            {
                case 0:
                    cellManagerData[0].column.Add(cellData);
                    break;
                case 1:
                    cellManagerData[1].column.Add(cellData);
                    break;
                case 2:
                    cellManagerData[2].column.Add(cellData);
                    break;
                case 3:
                    cellManagerData[3].column.Add(cellData);
                    break;
                case 4:
                    cellManagerData[4].column.Add(cellData);
                    break;
                default: Debug.LogError("Error: column is not value"); break;
            }
        }
        #endregion
        #region SetListData
        public void SetListIndex()
        {
            for (int i = 0; i < cellManagerData.Count; i++)
            {
                SetListIndex(ref cellManagerData[i].column, cellManagerData[i].indexColumn);
            }
        }

        private void SetListIndex(ref List<CellGame> column, List<int> indexColumn)
        {
            for (int i = 0; i < column.Count; i++)
            {
                column[i].indexColumn = i;
                column[i].index = indexColumn[i];
                column[i].isNewSpawn = false;
            }
        }


        #endregion
        #region CheckLose
        public void CheckLose ()
        {
            var countCheck = 0;
            for (int i = 0; i < cellData.Count; i++)
            {
                countCheck += CheckIndex(cellData[i], i);
            }
          //  Debug.Log("CountCheck = " + countCheck);
            if (countCheck == 0)
            {
               // DeleteData();
                CanvasManager.Instance.LoseGame();
                //CanvasManager.Instance.canvasGameScr.scoreValue = 0;
            }
        }

        private int CheckIndex(CellGame cellGame, int i)
        {
            if (cellGame.column == 0)
            {
                return CheckIndexColumnNull(cellGame, i);
            }
            else if (cellGame.column == 4)
            {
                return CheckIndexColumnFourth(cellGame, i);
            }
            else
            {
                return CheckIndexColumnOther(cellGame, i);
            }
        }

        private int CheckIndexColumnNull(CellGame cellGame, int i)
        {
            if (cellGame.indexColumn == 0)
            {
                if (CheckIndex(cellGame, i, 5, 6, 1))
                {
                    return 1;
                }
                else return 0;
            }
            else if (cellGame.indexColumn == 7)
            {
                if (CheckIndex(cellGame, i, 1, -4, -5))
                {
                    return 1;
                }
                else return 0;
            }
            else
            {
                if (CheckIndex(cellGame, i, 5, 6, 1, -4, -5))
                {
                    return 1;
                }
                else return 0;
            }
        }

        private int CheckIndexColumnFourth(CellGame cellGame, int i)
        {
            if (cellGame.indexColumn == 0)
            {
                if (CheckIndex(cellGame, i, 1, 4, 5))
                {
                    return 1;
                }
                else return 0;
            }
            else if (cellGame.indexColumn == 7)
            {
                if (CheckIndex(cellGame, i, -1, -5, -6))
                {
                    return 1;
                }
                else return 0;
            }
            else
            {
                if (CheckIndex(cellGame, i, 4, 5, -1, -5, -6))
                {
                    return 1;
                }
                else return 0;
            }
        }

        private int CheckIndexColumnOther(CellGame cellGame, int i)
        {
            if (cellGame.indexColumn == 0)
            {
                if (CheckIndex(cellGame, i, -1, 1, 4, 5, 6))
                {
                    return 1;
                }
                else return 0;
            }
            else if (cellGame.indexColumn == 7)
            {
                if (CheckIndex(cellGame, i, 1, -1, -4, -5, -6))

                {
                    return 1;
                }
                else return 0;
            }
            else
            {
                if (CheckIndex(cellGame, i, 1, 4, 5, 6, -1, -4, -5, -6))
                {
                    return 1;
                }
                else return 0;
            }
        }

        private bool CheckIndex(CellGame cellGame, int i, int i1, int i2, int i3)
        {
            if (cellGame.indexMonstr == cellData[i + i1].indexMonstr || cellGame.indexMonstr == cellData[i + i2].indexMonstr
|| cellGame.indexMonstr == cellData[i + i3].indexMonstr)
            {
                return true;
            }
            else return false;
        }

        private bool CheckIndex(CellGame cellGame, int i, int i1, int i2, int i3, int i4, int i5)
        {
            if (cellGame.indexMonstr == cellData[i + i1].indexMonstr || cellGame.indexMonstr == cellData[i + i2].indexMonstr
                      || cellGame.indexMonstr == cellData[i + i3].indexMonstr || cellGame.indexMonstr == cellData[i + i4].indexMonstr || cellGame.indexMonstr == cellData[i + i5].indexMonstr)
            {
                return true;
            }
            else return false;
        }

        private bool CheckIndex(CellGame cellGame, int i, int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8)
        {
            if (cellGame.indexMonstr == cellData[i + i1].indexMonstr || cellGame.indexMonstr == cellData[i + i2].indexMonstr
                       || cellGame.indexMonstr == cellData[i + i3].indexMonstr || cellGame.indexMonstr == cellData[i + i4].indexMonstr || cellGame.indexMonstr == cellData[i + i5].indexMonstr
                       || cellGame.indexMonstr == cellData[i + i6].indexMonstr || cellGame.indexMonstr == cellData[i + i7].indexMonstr || cellGame.indexMonstr == cellData[i + i8].indexMonstr)
            {
                return true;
            }
            else return false;
        }
        #endregion endCheckLose
        #region DeleteData
        public void DeleteData()
        {
            for (int i = 0; i < cellData.Count; i++)
            {
                Save(true, Path.Cell + i + ".txt", null);
            }
            Save(true, Path.Score, null);
            CanvasManager.Instance.canvasGameScr.DeleteData();
            MonstrIndexManager.Instance.ResetData();
            InitializingCellNewGame();
        }

        #endregion

        #region RestartGame
        public void RestartGame()
        {
            BustManager.Instance.isActiveHammer = false;
            BustManager.Instance.isActiveExchange = false;
            for (int i = 0; i < cellData.Count; i++)
            {
                cellData[i].StartAnimationRestart();
            }
            BustManager.Instance.DeAcriveAllBuster();
        }
        #endregion end RestartGame

        #region DestroyCellOld
        public void DestroyCellOld(ulong indexMonsr)
        {
            RaycastIsActive = false;
            for (int i = 0; i < cellData.Count; i++)
            {
                if (cellData[i].indexMonstr == indexMonsr)
                {
                    indexCell.Add(cellData[i].index);
                    columnCell.Add(cellData[i].column);
                }

            }
            if (indexCell.Count == 0)
                return;
        }

        public void DestroyCell()
        {
            for (int i = 0; i < indexCell.Count; i++)
            {
                cellData[indexCell[i]].DestroyObject(indexCell[i], cellData[indexCell[i]].column, cellData[indexCell[i]].indexColumn);
            }

            saveLastCell = null;
            RemoveCellList();
            MoveAllCell();
            StartCoroutine(IE());
            SetListIndex();
            SortCell();
            DataReset();
            SaveCell();
            CheckLose();

            IEnumerator IE()
            {
                yield return new WaitForSeconds(DELAY_DESTROY + 2f);
                for (int i = 0; i < cellData.Count; i++)
                {
                    cellData[i].transform.parent.position = allCellTransform[cellData[i].index].transform.position;
                }
                RaycastIsActive = true;
            }
        }
        #endregion

        #region ResumeGame
        public void ResumeGame()
        {
            List<CellDataRe> cellDataRe = new();

            for (int i = 0; i < cellData.Count; i++)
            {
                cellDataRe.Add(new CellDataRe(cellData[i].indexMonstr, cellData[i].image.sprite));
            }
            var shuffledList = ShuffleList(cellDataRe);

            for (int i = 0; i < cellData.Count; i++)
            {
                cellData[i].indexMonstr = shuffledList[i].indexMonstr;
                cellData[i].image.sprite = shuffledList[i].sprite;
                cellData[i].UpdateUICellData();
            }

            for (int i = 0; i < cellData.Count; i++)
            {
                Save(true, Path.Cell + i + ".txt", cellData[i].indexMonstr.ToString());
            }
        }

        public List<T> ShuffleList<T>(List<T> list)
        {
            var shuffledList = new List<T>();
            var rng = new System.Random();

            while (list.Count > 0)
            {
                var index = rng.Next(list.Count);
                shuffledList.Add(list[index]);
                list.RemoveAt(index);
            }

            return shuffledList;
        }
        #endregion endResume

        #region Save
        public void Save(string path)
        {
            throw new System.NotImplementedException();
        }

        public void Save(bool isNewData, string fileName, string fileData)
        {
            SaveManager.Instance.multiThread.SaveData(SaveManager.Instance.saveSystem.WritingDataToADocument, SaveTaskType.CellData, isNewData, fileName, fileData);
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
        #endregion Save

        #region Test
        public void Test()
        {
            for (int i = 0; i < cellData.Count; i++)
            {
                cellData[i].indexMonstr = (ulong)i;
            }
        }

        public void TestShuffle()
        {
            List<ulong> indexMonstr = new();
            for (int i = 0; i < cellData.Count; i++)
            {
                indexMonstr.Add(cellData[i].indexMonstr);
            }
            Shuffle(indexMonstr);
            for (int i = 0; i < cellData.Count; i++)
            {
                cellData[i].indexMonstr = indexMonstr[i];
                cellData[i].UpdateUICellData();
            }
        }
        #endregion

        public static void Shuffle<T>(List<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
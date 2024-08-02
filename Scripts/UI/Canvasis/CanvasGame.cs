using Kosilek.Manager;
using Kosilek.UI;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DredPack.Help;
using System;
using Kosilek.SaveAndLoad;
using Unity.VisualScripting;

public class CanvasGame : MonoBehaviour, ISaveble
{
    #region CellTop
    [Foldout("CellTop")]
    [SerializeField]
    private Image iconCell;
    [Foldout("CellTop")]
    [SerializeField]
    private TextMeshProUGUI textCell;
    [Foldout("CellTop")]
    [SerializeField]
    private GameObject cell;
    #endregion

    #region Text
    [Foldout("Text")]
    public TextMeshProUGUI bestScore;
    [Foldout("Text")]
    public TextMeshProUGUI score;
    #endregion

    #region Animation
    [Foldout("Animation")]
    [SerializeField]
    private float moveSpeed;
    [Foldout("Animation")]
    [SerializeField]
    private AnimationCurve animCurve;
    [Foldout("Animation")]
    [SerializeField]
    private GameObject scoreObject;
    [Foldout("Animation")]
    [SerializeField]
    private TextMeshProUGUI textSctoreObject;
    [Foldout("Animation")]
    [SerializeField]
    private Transform endScoreTransform;
    #endregion

    #region Value
    [HideInInspector] public ulong scoreValue = 0;
    #endregion

    #region Start
    private void Start()
    {
        InVisibleCell();
    }
    #endregion ensStart

    #region Initialize
    public void InitializeLoadData()
    {
        string[] score = Load(Path.DefaultPath + Path.Score);
        InitializeScore(score);
    }

    private void InitializeScore(string[] score)
    {
        try
        {
            var hasNameData = score.Length > 0;
            Action<string[]> action = hasNameData ? InitializeNameLoad : InitializeNameFirst;
            action.Invoke(score);
        }
        catch
        {
            Debug.LogError("Error: Data could not be watered, InitializeScore");
        }
    }

    private void InitializeNameFirst(string[] score)
    {
        scoreValue = 0;
        Save(true, Path.Score, scoreValue.ToString());
        this.score.text = scoreValue.ToString();
    }

    private void InitializeNameLoad(string[] score)
    {
        scoreValue = Convert.ToUInt32(score[0]);
        this.score.text = scoreValue.ToString();
    }
    #endregion endInitialize
    #region Insctance
    public void MoveSpeed(ulong score, Transform startPos)
    {
        textSctoreObject.text = score.ToString();
        scoreObject.transform.position = startPos.position;
        scoreObject.SetActive(true);
        StartCoroutine(IE());

        IEnumerator IE()
        {
            yield return StartCoroutine(Lerper.LerpVector3IE(scoreObject.transform.position, endScoreTransform.position, moveSpeed, animCurve, _ => scoreObject.transform.position = _));
            UpdateUITextScore(score);
            scoreObject.SetActive(false);
            Save(true, Path.Score, scoreValue.ToString());
        }
    }
    #endregion endInsctANCE
    #region UIUpdate
    private void UpdateUITextScore(ulong score)
    {
        scoreValue += score;
        CanvasManager.Instance.bestScore.SetBestScore(CanvasManager.Instance.canvasGameScr.scoreValue);
        this.score.text = scoreValue.ToString();
    }
    #endregion end UIUpdate
    #region Instanse Update UI
    public void VisibleCell(Sprite sprite, int text)
    {
        cell.SetActive(true);
        score.enabled = false;
        iconCell.sprite = sprite;
        textCell.text = DigitConverter.digitConverter((ulong)text);
        //textCell.text = text;
    }

    public void InVisibleCell()
    {
        iconCell.sprite = null;
        textCell.text = null;
        cell.SetActive(false);
        score.enabled = true;

    }
    #endregion
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
    #region DeleteData
    public void DeleteData()
    {
        InitializeNameFirst(null);
    }
    #endregion
}

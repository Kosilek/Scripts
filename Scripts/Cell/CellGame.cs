using Kosilek.Data;
using Kosilek.SaveAndLoad;
using NaughtyAttributes;
using UnityEngine;
using Kosilek.Manager;
using UnityEngine.UI;
using TMPro;
using System;
using Kosilek.MonstrPack;
using System.Collections;
using Unity.VisualScripting;

public class CellGame : MonoBehaviour, ISaveble
{
    public CellData cellData;

    public bool isReady = false;

    public int index;
    public int column;
    public bool isActive = false;

    public int indexColumn;

    [Foldout("MonstrData")]
    public Image image;
    [Foldout("MonstrData")]
    public ulong indexMonstr;
    [Foldout("MonstrData")]
    public TextMeshProUGUI textIndexMonstr;
    [Foldout("MonstrData")]
    public int numberMonstr;

    public CellMove cellMove;

    public bool isNeedToMove = false;

    public int maxIndexColumn = 0;

    public bool isNewSpawn = false;

    #region Animation
    [Foldout("Animation")]
    [SerializeField] private float moveSpeed = 1f;
    [Foldout("Animation")]
    public float moveSpeedAlpha = 1f;
    [Foldout("Animation")]
    public AnimationCurve animCurve;
    [Foldout("Animation")]
    public CanvasGroup canvasGroup;
    private bool isAcive = false;
    [Foldout("Animation")]
    [SerializeField] private Animator anim;
    #endregion

    #region Const
    private const float MAX_SCALE_CELL = 1.1f;

    public const string EFFECT = "Effect";
    public const string FADE = "Fade";
    public const string RESTART = "Restart";
    #endregion

    private void Start()
    {
        cellMove = transform.parent.GetComponent<CellMove>();
    }

    #region Initialization

    public void InitializationData(bool isStartGame)
    {
        try
        {
            Action action = isStartGame ? InitializationDataStartGame : InitializationDataSpawnCell;
            action.Invoke();
        }
        catch
        {
            Debug.LogError("Error: Failed cell initialization");
        }
    }

    private void InitializationDataStartGame()
    {

    }

    private void InitializationDataSpawnCell()
    {
        try
        {
            var maxRandIcon = ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count > CellManager.MAX_RAND_ICON ? CellManager.MAX_RAND_ICON : ManagerMonstrPack.Instance.monsterCollection.monstrPack.Count;
            var random = UnityEngine.Random.Range(0 + MonstrIndexManager.Instance.stepIndexMonstrFirst, maxRandIcon + MonstrIndexManager.Instance.stepIndexMonstrFirst);
            image.sprite = ManagerMonstrPack.Instance.monsterCollection.monstrPack[random].iconMonstr;
            numberMonstr = ManagerMonstrPack.Instance.monsterCollection.monstrPack[random].numberMonstr;
            InitializationText(ManagerMonstrPack.Instance.monsterCollection.monstrPack[random].indexMonstr);
        }
        catch
        {
            Debug.LogError("Error: Text initialization failed");
        }
    }

    public void InitializationText(ulong index)
    {
        indexMonstr = index;
        textIndexMonstr.text = DigitConverter.digitConverter((ulong)indexMonstr);
        //textIndexMonstr.text = indexMonstr.ToString();
    }

    public void UpdateUICellData()
    {
        textIndexMonstr.text = DigitConverter.digitConverter((ulong)indexMonstr);
    }

    #endregion endInitialization

    public void UpdateUICellImage()
    {
        
    }

    public void DestroyObject(int index, int column, int indexColumn)
    {
        CellManager.Instance.cellData[index] = null;

        switch (column)
        {
            case 0:
                CellManager.Instance.cellManagerData[0].column[indexColumn] = null;
                break;
            case 1:
                CellManager.Instance.cellManagerData[1].column[indexColumn] = null;
                break;
            case 2:
                CellManager.Instance.cellManagerData[2].column[indexColumn] = null;
                break;
            case 3:
                CellManager.Instance.cellManagerData[3].column[indexColumn] = null;
                break;
            case 4:
                CellManager.Instance.cellManagerData[4].column[indexColumn] = null;
                break;
            default: Debug.LogError("Error: column is not value"); break;
        }

        StartAnimation();

        StartCoroutine(IE());

        IEnumerator IE()
        {
            yield return new WaitForSeconds(CellManager.DELAY_DESTROY);
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    #region Animation
    public void StartEffect(bool isAdd)
    {
        if (isAcive)
            return;

        StartCoroutine(IE());

        IEnumerator IE()
        {
            isAcive = true;
            yield return StartCoroutine(IEStartEffectUpScale());
            yield return StartCoroutine(IEStartEffectLoyScale());
            isAcive = false;
        }
        // Start Music

    }

    public void StartEffestRotation(ulong index, Sprite sprite, string text, IEnumerator IELoy)
    {
        StartCoroutine(IE());

        IEnumerator IE()
        {
            yield return StartCoroutine(CustomIenumerator.IEImageAlphaCor(canvasGroup.alpha, 0f, moveSpeedAlpha, animCurve, _ => canvasGroup.alpha = _));
            indexMonstr = index;
            image.sprite = sprite;
            var number = Convert.ToInt32(text);
            textIndexMonstr.text = DigitConverter.digitConverter((ulong)number);
            //textIndexMonstr.text = text;
            StartCoroutine(CustomIenumerator.IEImageAlphaCor(canvasGroup.alpha, 1f, moveSpeedAlpha, animCurve, _ => canvasGroup.alpha = _));
            StartCoroutine(IELoy);
        }
    }

    public IEnumerator IEStartEffectUpScale()
    {
        yield return StartCoroutine(DredPack.Help.Lerper.LerpVector3IE(transform.localScale, new Vector3(MAX_SCALE_CELL, MAX_SCALE_CELL), moveSpeed, animCurve, _ => transform.localScale = _));

    }

    public IEnumerator IEStartEffectLoyScale()
    {
        yield return StartCoroutine(DredPack.Help.Lerper.LerpVector3IE(transform.localScale, Vector3.one, moveSpeed, animCurve, _ => transform.localScale = _));
    }
    #endregion

    #region Animator
    private void StartAnimation()
    {
        anim.SetTrigger(EFFECT);
    }

    public void StartAnimationFade()
    {
        anim.SetTrigger(FADE);
    }

    public void StartAnimationRestart()
    {
        anim.SetTrigger(RESTART);
    }
    #endregion end Animator

    #region Save
    public string[] Load(string path)
    {
        throw new System.NotImplementedException();
    }

    public void Save(string path)
    {
        throw new System.NotImplementedException();
    }

    public void Save(bool isNewdata, string path, string fileData)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}

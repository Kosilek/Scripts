using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SeeCell : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private GameObject scoreText;

    public void ViewingCell(Sprite sprite, string text)
    {
        scoreText.SetActive(false);
        gameObject.SetActive(true);
        image.sprite = sprite;
        this.text.text = text;
    }

    public void DeActiveCell()
    {
        gameObject.SetActive(false);
        scoreText.SetActive(true);
        image.sprite = null;
        this.text.text = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchBackGround : MonoBehaviour
{
    private BoxCollider boxCollider;
    private RectTransform rectTransform;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        rectTransform = GetComponent<RectTransform>();

        boxCollider.size = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
    }
}

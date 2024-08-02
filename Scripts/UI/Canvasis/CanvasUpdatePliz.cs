using DredPack.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUpdatePliz : MonoBehaviour
{
    public Window windwo;
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(ActionButton);
    }

    private void ActionButton()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.kosilek");
    }
}

using Kosilek.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButtonExchange : MonoBehaviour
{
    private Button button;
    private int cost = 100;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ActionButton);
    }

    private void ActionButton()
    {
        var check = MoneyManager.Instance.SpendMoney(cost);
        System.Action<int> action = check ? BustManager.Instance.UpdateUITextEchange : null;
        action?.Invoke(1);
    }
}
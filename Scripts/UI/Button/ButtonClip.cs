using Kosilek.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosilek.UI
{
    public class ButtonClip : MonoBehaviour
    {
        private Button button;
        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(ActionButton);
        }

        private void ActionButton()
        {
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.clickButon);
        }
    }
}
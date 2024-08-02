using DredPack.UI;
using Kosilek.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosilek.UI
{
    public class CanvasRateUs : MonoBehaviour
    {
        public Button buttonBack;
        public Button buttonRateUs;
        public Button buttonLater;
        public List<Image> imageButtonsStar;

        Window window;

        private void Start()
        {
            SetValues();
            AddListenerButton();
            InitializeStart();
        }

        private void SetValues()
        {
            window = GetComponent<Window>();

            for (int i = 0; i < imageButtonsStar.Count; i++)
            {
                imageButtonsStar[i].gameObject.SetActive(false);
            }
        }

        private void AddListenerButton()
        {
            buttonBack.onClick.AddListener(ActionButtonExit);
            buttonRateUs.onClick.AddListener(ActionButtonLater);
            buttonLater.onClick.AddListener(ActionButtonRateUs);
        }

        private void ActionButtonExit()
        {
            window.Close();
        }

        private void ActionButtonLater()
        {
            window.Close();
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.kosilek");
        }

        private void ActionButtonRateUs()
        {
            window.Close();
            InitializeLater();
        }

        public void ClickStar(int counter)
        {
            for (int i = 0; i < imageButtonsStar.Count; i++)
            {
                if (i < counter)
                    imageButtonsStar[i].gameObject.SetActive(true);
                else
                    imageButtonsStar[i].gameObject.SetActive(false);
            }
        }

        private void InitializeStart()
        {
            StartCoroutine(IE(240f));
        }

        private void InitializeLater()
        {
            window.Close();
            StartCoroutine(IE(240f));
        }

        private IEnumerator IE(float delay)
        {
            for (int i = 0; i < delay; i++)
            {
                yield return new WaitForSeconds(1f);
            }

            while (GameManager.Instance.isGame)
            {
                yield return new WaitForSeconds(1f);
            }

            window.Open();
        }
    }
}

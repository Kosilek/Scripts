using DredPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosilek.Manager
{
    public class VibrationManager : SimpleSingleton<VibrationManager>
    {
        private void Start()
        {
            Vibration.Init();
        }

        public void StartVibration()
        {
            Vibration.VibrateAndroid(13);
        }
    }
}
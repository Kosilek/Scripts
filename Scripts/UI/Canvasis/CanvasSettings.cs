using NaughtyAttributes;
using System.Collections;
using DredPack.Help;
using UnityEngine;
using UnityEngine.UI;
using System;
using Kosilek.SaveAndLoad;
using Kosilek.Manager;

namespace Kosilek.UI
{
    public class CanvasSettings : MonoBehaviour, ISaveble
    {
        #region Button
        [Foldout("Button")]
        [SerializeField]
        private Button buttonBack;
        [Foldout("Button")]
        [SerializeField]
        private Button buttonMusic;
        [Foldout("Button")]
        [SerializeField]
        private Button buttonSound;
        [Foldout("Button")]
        [SerializeField]
        private Button buttonVibration;
        [Foldout("Button")]
        [SerializeField]
        private Button buttonPrivacy;
        #endregion

        #region AnimationSwitcher
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private Image backGorundColorMusic;
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private Image backGorundColorSound;
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private Image backGorundColorVibration;
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private Color colorActive;
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private Color colorInactive;
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private Transform transformActive;
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private Transform transformInactive;
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private Image handlerMusic;
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private Image handlerSound;
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private Image handlerVibration;
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private Sprite handlerActive;
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private Sprite handlerInactive;
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private AnimationCurve switcherAnimCurve;
        [Foldout("AnimationSwitcher")]
        [SerializeField]
        private float moveSpeedSwitcher;
        private Coroutine coroutineSwitcherMusic;
        private Coroutine coroutineSwitcherSound;
        private Coroutine coroutineSwitcherVibration;

        private bool isFirst = false;
        #endregion

        #region Start
        private void Start()
        {
            InitializeButtons();
        }

        public void SettingInitialValues()
        {
            if (!AudioManager2248.Instance.isMusic)
            {
                AudioManager2248.Instance.isMusic = true;
                MoveSwitcher(ref coroutineSwitcherMusic, true, AudioManager2248.Instance.isMusic);
            }
            if (!AudioManager2248.Instance.isSound)
            {
                AudioManager2248.Instance.isSound = true;
                MoveSwitcher(ref coroutineSwitcherSound, false, AudioManager2248.Instance.isSound);
            }
            if (!AudioManager2248.Instance.isVibration)
            {
                AudioManager2248.Instance.isVibration = true;
                MoveSwitcher(ref coroutineSwitcherVibration, AudioManager2248.Instance.isVibration);
            }
        }

        public void InitializeLoadData()
        {
            string[] musicData = Load(Path.DefaultPath + Path.Music);
            string[] soundData = Load(Path.DefaultPath + Path.Sound);
            string[] vibrationData = Load(Path.DefaultPath + Path.Vibration);

            InitializeVolume(musicData, false, true);
            InitializeVolume(soundData, false, false);
            InitializeVibrtion(vibrationData, false);
        }

        #region InitializeVolume
        private void InitializeVolume(string[] volumeData, bool isActive, bool isMusic)
        {
            try
            {
                var hasVolumeData = volumeData.Length > 0;
                Action<string[], bool, bool> actionCheck = hasVolumeData ? HandleNonEmptyVolumeData : HandleEmptyVolumeData;
                actionCheck.Invoke(volumeData, isActive, isMusic);
            }
            catch
            {
                Debug.LogError("Error: attempt to call the null Action. Name Action = InitializeVolume");
            }
        }
        #region Vibration
        private void InitializeVibrtion(string[] vibrationData, bool isActive)
        {
            try
            {
                var hasVolumeData = vibrationData.Length > 0;
                Action<string[], bool> actionCheck = hasVolumeData ? HandleNonEmptyVibrationData : HandleEmptyVibrationData;
                actionCheck.Invoke(vibrationData, isActive);
            }
            catch
            {
                Debug.LogError("Error: attempt to call the null Action. Name Action = InitializeVolume");
            }
        }

        private void HandleEmptyVibrationData(string[] volumeData, bool isActive)
        {
            isActive = true;
            RecordingVibrationData(isActive);
            var fileData = isActive.ToString();
            var fileName = Path.Vibration;
            Save(fileData, true, fileName);
        }

        private void HandleNonEmptyVibrationData(string[] volumeData, bool isActive)
        {
            isActive = bool.Parse(volumeData[0]);
            //Debug.Log($"{volumeData} + isAcitve");
            RecordingVibrationData(isActive);
        }

        private void RecordingVibrationData(bool isActive)
        {
            try
            {
                AudioManager2248.Instance.isVibration = isActive;
            }
            catch
            {
                Debug.LogError("Error: attempt to call the null Action. Name Action = RecordingVolumeData");
            }
        }
        #endregion end Vibration
        private void HandleEmptyVolumeData(string[] volumeData, bool isActive, bool isMusic)
        {
            isActive = true;
            RecordingVolumeData(isMusic, isActive);
            var fileData = isActive.ToString();
            var fileName = isMusic ? Path.Sound : Path.Music;
            Save(fileData, true, fileName);
        }

        private void HandleNonEmptyVolumeData(string[] volumeData, bool isActive, bool isMusic)
        {
            isActive = bool.Parse(volumeData[0]);
            //Debug.Log($"{volumeData} + isAcitve");
            RecordingVolumeData(isMusic, isActive);
        }

        private void RecordingVolumeData(bool isMusic, bool isActive)
        {
            try
            {
                Action<bool> actionRecording = isMusic ? RecordingMusicData : RecordingSoundData;
                actionRecording.Invoke(isActive);
            }
            catch
            {
                Debug.LogError("Error: attempt to call the null Action. Name Action = RecordingVolumeData");
            }
        }

        private void RecordingMusicData(bool isActive)
        {
            AudioManager2248.Instance.isMusic = isActive;
            AudioManager2248.Instance.ActiveMusic();
        }

        private void RecordingSoundData(bool isActive)
        {
            AudioManager2248.Instance.isSound = isActive;
            AudioManager2248.Instance.ActiveSound();
        }
        #endregion

        private void InitializeButtons()
        {
            buttonBack.onClick.AddListener(ActionBack);
            buttonMusic.onClick.AddListener(ActionMusic);
            buttonSound.onClick.AddListener(ActionSound);
            buttonVibration.onClick.AddListener(ActionVibration);
            buttonPrivacy.onClick.AddListener(ActionPrivacy);
        }

        #region InitializeButtons
        private void ActionBack()
        {
            CanvasManager.Instance.actionWindow.OpenWindow(false, CanvasManager.Instance.canvasSettings, "CanvasMenu");
        }

        private void ActionMusic()
        {
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.tublerButton);
            MoveSwitcher(ref coroutineSwitcherMusic, true, AudioManager2248.Instance.isMusic);
        }

        private void ActionSound()
        {
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.tublerButton, AudioManager2248.Instance.isSound);
            MoveSwitcher(ref coroutineSwitcherSound, false, AudioManager2248.Instance.isSound);
        }

        private void ActionVibration()
        {
            AudioManager2248.Instance.StartEffectClip(AudioManager2248.Instance.audioContainer.tublerButton);
            MoveSwitcher(ref coroutineSwitcherVibration, AudioManager2248.Instance.isVibration);
        }

        private void ActionPrivacy()
        {

        }
        #endregion
        #endregion

        #region MoveSwitcher
        private void MoveSwitcher(ref Coroutine coroutine, bool isFirst, bool isAction)
        {
            if (coroutine != null)
                return;

            this.isFirst = isFirst;
            Action actionMove = MoveSwitcher(isAction);
            coroutine = StartCoroutine(IEMoveSwitcher());

            IEnumerator IEMoveSwitcher()
            {
                actionMove();
                RecordingNewDataVolume(this.isFirst);
                yield return new WaitForSeconds(1f / moveSpeedSwitcher);
                Action emptyCor = isFirst ? EmptyCoroutineMusic : EmptyCoroutineSound;
                emptyCor.Invoke();
            }
        }

        private void MoveSwitcher(ref Coroutine coroutine, bool isAction)
        {
            if (coroutine != null)
                return;

            Action actionMove = MoveSwitcherVibration(isAction);
            coroutine = StartCoroutine(IEMoveSwitcher());

            IEnumerator IEMoveSwitcher()
            {
                actionMove();
                RecordingNewDataVolume();
                yield return new WaitForSeconds(1f / moveSpeedSwitcher);
                EmptyCoroutineVibration();

            }
        }
        #region MoveSwitcher
        private void EmptyCoroutineMusic()
        {
            coroutineSwitcherMusic = null;
        }

        private void EmptyCoroutineSound()
        {
            coroutineSwitcherSound = null;
        }

        private void EmptyCoroutineVibration()
        {
            coroutineSwitcherVibration = null;
        }

        private Action MoveSwitcher(bool isAction)
        {
            Action action = isAction ?  MoveInAction : MoveAction;
            return action;
        }

        private void MoveAction()
        {
            var image = isFirst ? handlerMusic : handlerSound;
            image.sprite = handlerActive;

            StartCoroutine(Lerper.LerpVector3IE(image.transform.position, new Vector3(transformActive.position.x, image.transform.position.y, image.transform.position.z), moveSpeedSwitcher, switcherAnimCurve, _ => image.transform.position = _));
        }

        private void MoveInAction()
        {
            var image = isFirst ? handlerMusic : handlerSound;
            image.sprite = handlerInactive;

            StartCoroutine(Lerper.LerpVector3IE(image.transform.position, new Vector3(transformInactive.position.x, image.transform.position.y, image.transform.position.z), moveSpeedSwitcher, switcherAnimCurve, _ => image.transform.position = _));
        }

        private Action MoveSwitcherVibration(bool isAction)
        {
            Action action = isAction ? MoveInActionVibration : MoveActionVibration;
            return action;
        }

        private void MoveActionVibration()
        {
            var image = handlerVibration;
            image.sprite = handlerActive;

            StartCoroutine(Lerper.LerpVector3IE(image.transform.position, new Vector3(transformActive.position.x, image.transform.position.y, image.transform.position.z), moveSpeedSwitcher, switcherAnimCurve, _ => image.transform.position = _));
        }

        private void MoveInActionVibration()
        {
            var image = handlerVibration;
            image.sprite = handlerInactive;

            StartCoroutine(Lerper.LerpVector3IE(image.transform.position, new Vector3(transformInactive.position.x, image.transform.position.y, image.transform.position.z), moveSpeedSwitcher, switcherAnimCurve, _ => image.transform.position = _));
        }
        #endregion end MoveSwitcher

        #region  RecordingNewDataVolume
        private void RecordingNewDataVolume(bool isFirst)
        {
            try
            {
                Action action = isFirst ? RecordingNewDataMusic : RecordingNewDataSound;
                action.Invoke();
            }
            catch
            {
                Debug.LogError("Error: attempt to call the null Action. Name Action = RecordingNewDataVolume");
            }
        }

        private void RecordingNewDataVolume()
        {
            try
            {
                RecordingNewDataVibration();
            }
            catch
            {
                Debug.LogError("Error: attempt to call the null Action. Name Action = RecordingNewDataVolume");
            }
        }

        private void RecordingNewDataVibration()
        {
            AudioManager2248.Instance.isVibration = !AudioManager2248.Instance.isVibration;
            Save(AudioManager2248.Instance.isVibration.ToString(), true, Path.Vibration);
        }

        private void RecordingNewDataMusic()
        {
                AudioManager2248.Instance.isMusic = !AudioManager2248.Instance.isMusic;
                Save(AudioManager2248.Instance.isMusic.ToString(), true, Path.Music);
            AudioManager2248.Instance.ActiveMusic();
        }

        private void RecordingNewDataSound()
        {
            AudioManager2248.Instance.isSound = !AudioManager2248.Instance.isSound;
                Save(AudioManager2248.Instance.isSound.ToString(), true, Path.Sound);
            AudioManager2248.Instance.ActiveSound();
        }
        #endregion end RecordingNewDataVolume
        #endregion
        #region SaveAndLoad
        private void Save(string fileData, bool isNewData, string fileName)
        {
            SaveManager.Instance.multiThread.SaveData(SaveManager.Instance.saveSystem.WritingDataToADocument, SaveTaskType.SettingsVolume, isNewData, fileName, fileData);
        }

        public void Save(string path)
        {
            throw new NotImplementedException();
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

        public void Save(bool isNewdata, string path, string fileData)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

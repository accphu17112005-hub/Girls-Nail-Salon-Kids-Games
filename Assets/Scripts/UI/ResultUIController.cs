using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using KidsNailSalon.Core;
using KidsNailSalon.Data;
using KidsNailSalon.SaveSystem;

namespace KidsNailSalon.UI
{
    public class ResultUIController : MonoBehaviour
    {
        [Header("Presentation Background")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Button _btnNextBg;

        [Header("Action Buttons")]
        [SerializeField] private Button _btnSave;
        [SerializeField] private Button _btnShare;
        [SerializeField] private Button _btnPlayAgain;
        [SerializeField] private Button _btnHome;

        [Header("FX & Toast")]
        [SerializeField] private ParticleSystem _celebrationFX;
        [SerializeField] private Text _toastMessage;

        private int _currentBgIndex = 0;

        private void OnEnable()
        {
            AudioManager.Instance?.PlaySound(SoundType.SuccessFanfare);
            if (_celebrationFX != null) _celebrationFX.Play();
            UpdateBackground();
        }

        private void Start()
        {
            _btnSave?.onClick.AddListener(OnSaveDesign);
            _btnShare?.onClick.AddListener(OnShareDesign);
            _btnPlayAgain?.onClick.AddListener(OnPlayAgain);
            _btnHome?.onClick.AddListener(OnGoHome);
            _btnNextBg?.onClick.AddListener(CycleBackground);
        }

        private void CycleBackground()
        {
            _currentBgIndex = (_currentBgIndex + 1) % GameCatalogs.Backgrounds.Length;
            UpdateBackground();
            AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
        }

        private void UpdateBackground()
        {
            string bgName = GameCatalogs.Backgrounds[_currentBgIndex];
            if (_backgroundImage != null)
            {
                var sprite = Resources.Load<Sprite>($"Art/Backgrounds/{bgName}");
                if (sprite != null) _backgroundImage.sprite = sprite;
            }

            if (GameManager.Instance != null && GameManager.Instance.ActiveDesign != null)
            {
                GameManager.Instance.ActiveDesign.backgroundId = bgName;
            }
        }

        private void OnSaveDesign()
        {
            StartCoroutine(CaptureAndSaveRoutine());
        }

        private IEnumerator CaptureAndSaveRoutine()
        {
            yield return new WaitForEndOfFrame();

            int w = Screen.width;
            int h = Screen.height;
            Texture2D tex = new Texture2D(w, h, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, w, h), 0, 0);
            tex.Apply();

            // Downscale for thumbnail
            Texture2D thumb = new Texture2D(270, 480, TextureFormat.RGB24, false);
            Graphics.CopyTexture(tex, thumb); // or sample

            if (GameManager.Instance != null && GameManager.Instance.ActiveDesign != null)
            {
                SaveManager.SaveDesign(GameManager.Instance.ActiveDesign, tex);
            }

            AudioManager.Instance?.PlaySound(SoundType.Sparkle);
            ShowToast("Design Saved to Gallery!");

            Destroy(tex);
            Destroy(thumb);
        }

        private void OnShareDesign()
        {
            AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
            ShowToast("Saved to Photos / Ready to Share!");
        }

        private void OnPlayAgain()
        {
            AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
            GameManager.Instance?.CreateNewDesign("fair");
            GameManager.Instance?.ChangeScreen(GameScreen.NailStudio);
        }

        private void OnGoHome()
        {
            AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
            GameManager.Instance?.ChangeScreen(GameScreen.MainMenu);
        }

        private void ShowToast(string msg)
        {
            if (_toastMessage != null)
            {
                _toastMessage.gameObject.SetActive(true);
                _toastMessage.text = msg;
                StopAllCoroutines();
                StartCoroutine(HideToastRoutine());
            }
        }

        private IEnumerator HideToastRoutine()
        {
            yield return new WaitForSeconds(2.5f);
            if (_toastMessage != null) _toastMessage.gameObject.SetActive(false);
        }
    }
}

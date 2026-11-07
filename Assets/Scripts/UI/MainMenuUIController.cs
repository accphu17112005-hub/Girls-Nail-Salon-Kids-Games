using UnityEngine;
using UnityEngine.UI;
using KidsNailSalon.Core;
using KidsNailSalon.SaveSystem;

namespace KidsNailSalon.UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _btnPlay;
        [SerializeField] private Button _btnGallery;
        [SerializeField] private Button _btnSettings;
        [SerializeField] private Button _btnMusicToggle;
        [SerializeField] private Button _btnSfxToggle;

        [Header("Icons")]
        [SerializeField] private Image _musicIcon;
        [SerializeField] private Image _sfxIcon;

        private void Start()
        {
            _btnPlay?.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
                GameManager.Instance?.ChangeScreen(GameScreen.CharacterSelection);
            });

            _btnGallery?.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
                GameManager.Instance?.ChangeScreen(GameScreen.Gallery);
            });

            _btnSettings?.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
                GameManager.Instance?.ChangeScreen(GameScreen.Settings);
            });

            _btnMusicToggle?.onClick.AddListener(OnToggleMusic);
            _btnSfxToggle?.onClick.AddListener(OnToggleSfx);

            UpdateAudioVisuals();
        }

        private void OnToggleMusic()
        {
            bool next = !SaveManager.Settings.musicEnabled;
            AudioManager.Instance?.ToggleMusic(next);
            UpdateAudioVisuals();
        }

        private void OnToggleSfx()
        {
            bool next = !SaveManager.Settings.sfxEnabled;
            AudioManager.Instance?.ToggleSfx(next);
            UpdateAudioVisuals();
        }

        private void UpdateAudioVisuals()
        {
            if (_musicIcon != null)
            {
                _musicIcon.color = SaveManager.Settings.musicEnabled ? Color.white : new Color(1, 1, 1, 0.4f);
            }
            if (_sfxIcon != null)
            {
                _sfxIcon.color = SaveManager.Settings.sfxEnabled ? Color.white : new Color(1, 1, 1, 0.4f);
            }
        }
    }
}

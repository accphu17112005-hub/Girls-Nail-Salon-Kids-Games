using System;
using UnityEngine;
using UnityEngine.UI;
using KidsNailSalon.Core;
using KidsNailSalon.SaveSystem;

namespace KidsNailSalon.UI
{
    public class ParentalGate : MonoBehaviour
    {
        [Header("Modal UI")]
        [SerializeField] private GameObject _gatePanel;
        [SerializeField] private Text _challengeQuestionText;
        [SerializeField] private InputField _answerInputField;
        [SerializeField] private Button _btnConfirm;
        [SerializeField] private Button _btnCancel;
        [SerializeField] private Text _errorText;

        private int _expectedAnswer;
        private Action _onSuccessCallback;

        private void Start()
        {
            _btnConfirm?.onClick.AddListener(CheckAnswer);
            _btnCancel?.onClick.AddListener(CloseGate);
            if (_gatePanel != null) _gatePanel.SetActive(false);
        }

        public void RequestVerification(Action onSuccess)
        {
            _onSuccessCallback = onSuccess;
            GenerateChallenge();
            if (_errorText != null) _errorText.text = "";
            if (_answerInputField != null) _answerInputField.text = "";
            if (_gatePanel != null) _gatePanel.SetActive(true);
        }

        private void GenerateChallenge()
        {
            int a = UnityEngine.Random.Range(3, 9);
            int b = UnityEngine.Random.Range(3, 9);
            _expectedAnswer = a * b;

            if (_challengeQuestionText != null)
            {
                _challengeQuestionText.text = $"Parental Verification\n\nPlease ask a grown-up to solve:\n{a} x {b} = ?";
            }
        }

        private void CheckAnswer()
        {
            if (_answerInputField != null && int.TryParse(_answerInputField.text.Trim(), out int val))
            {
                if (val == _expectedAnswer)
                {
                    AudioManager.Instance?.PlaySound(SoundType.Sparkle);
                    if (_gatePanel != null) _gatePanel.SetActive(false);
                    _onSuccessCallback?.Invoke();
                    return;
                }
            }

            if (_errorText != null)
            {
                _errorText.text = "Incorrect answer. Please try again.";
            }
            GenerateChallenge();
            if (_answerInputField != null) _answerInputField.text = "";
        }

        private void CloseGate()
        {
            if (_gatePanel != null) _gatePanel.SetActive(false);
            AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
        }
    }

    public class SettingsUIController : MonoBehaviour
    {
        [Header("Toggles")]
        [SerializeField] private Toggle _toggleMusic;
        [SerializeField] private Toggle _toggleSfx;
        [SerializeField] private Toggle _toggleHaptics;

        [Header("Buttons")]
        [SerializeField] private Button _btnBack;
        [SerializeField] private Button _btnPrivacyPolicy;
        [SerializeField] private Button _btnTermsOfService;

        [Header("Parental Gate")]
        [SerializeField] private ParentalGate _parentalGate;

        private void Start()
        {
            _btnBack?.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
                GameManager.Instance?.ChangeScreen(GameScreen.MainMenu);
            });

            if (_toggleMusic != null)
            {
                _toggleMusic.isOn = SaveManager.Settings.musicEnabled;
                _toggleMusic.onValueChanged.AddListener(val => {
                    AudioManager.Instance?.ToggleMusic(val);
                });
            }

            if (_toggleSfx != null)
            {
                _toggleSfx.isOn = SaveManager.Settings.sfxEnabled;
                _toggleSfx.onValueChanged.AddListener(val => {
                    AudioManager.Instance?.ToggleSfx(val);
                });
            }

            if (_toggleHaptics != null)
            {
                _toggleHaptics.isOn = SaveManager.Settings.hapticsEnabled;
                _toggleHaptics.onValueChanged.AddListener(val => {
                    SaveManager.Settings.hapticsEnabled = val;
                    SaveManager.SaveSettings();
                });
            }

            _btnPrivacyPolicy?.onClick.AddListener(() => {
                _parentalGate?.RequestVerification(() => {
                    Application.OpenURL("https://kidsplaystudios.com/privacy");
                });
            });

            _btnTermsOfService?.onClick.AddListener(() => {
                _parentalGate?.RequestVerification(() => {
                    Application.OpenURL("https://kidsplaystudios.com/terms");
                });
            });
        }
    }
}

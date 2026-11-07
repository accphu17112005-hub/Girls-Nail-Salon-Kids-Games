using UnityEngine;
using UnityEngine.UI;
using KidsNailSalon.Core;
using KidsNailSalon.Data;

namespace KidsNailSalon.UI
{
    public class CharacterSelectionUIController : MonoBehaviour
    {
        [Header("Hand Preview")]
        [SerializeField] private Image _handPreviewImage;

        [Header("Skin Tone Buttons")]
        [SerializeField] private Button _btnToneFair;
        [SerializeField] private Button _btnToneLight;
        [SerializeField] private Button _btnToneTan;
        [SerializeField] private Button _btnToneCaramel;
        [SerializeField] private Button _btnToneDeep;

        [Header("Navigation Buttons")]
        [SerializeField] private Button _btnBack;
        [SerializeField] private Button _btnStartHandCare;
        [SerializeField] private Button _btnStartStudio;

        private string _selectedTone = "fair";

        private void Start()
        {
            _btnBack?.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
                GameManager.Instance?.ChangeScreen(GameScreen.MainMenu);
            });

            _btnToneFair?.onClick.AddListener(() => SelectSkinTone("fair"));
            _btnToneLight?.onClick.AddListener(() => SelectSkinTone("light"));
            _btnToneTan?.onClick.AddListener(() => SelectSkinTone("tan"));
            _btnToneCaramel?.onClick.AddListener(() => SelectSkinTone("caramel"));
            _btnToneDeep?.onClick.AddListener(() => SelectSkinTone("deep"));

            _btnStartHandCare?.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
                GameManager.Instance?.CreateNewDesign(_selectedTone);
                GameManager.Instance?.ChangeScreen(GameScreen.HandCare);
            });

            _btnStartStudio?.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
                GameManager.Instance?.CreateNewDesign(_selectedTone);
                GameManager.Instance?.ChangeScreen(GameScreen.NailStudio);
            });

            SelectSkinTone("fair");
        }

        private void SelectSkinTone(string toneId)
        {
            _selectedTone = toneId;
            AudioManager.Instance?.PlaySound(SoundType.ButtonClick, 1.2f);
            
            if (_handPreviewImage != null)
            {
                var sprite = Resources.Load<Sprite>($"Art/Hands/hand_{toneId}");
                if (sprite != null) _handPreviewImage.sprite = sprite;
            }
        }
    }
}

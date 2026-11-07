using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KidsNailSalon.Core;
using KidsNailSalon.Data;
using KidsNailSalon.SaveSystem;

namespace KidsNailSalon.UI
{
    public class GalleryUIController : MonoBehaviour
    {
        [Header("Containers & Navigation")]
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private Button _btnBack;
        [SerializeField] private Text _emptyStateText;

        private void OnEnable()
        {
            RefreshGalleryGrid();
        }

        private void Start()
        {
            _btnBack?.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
                GameManager.Instance?.ChangeScreen(GameScreen.MainMenu);
            });
        }

        public void RefreshGalleryGrid()
        {
            if (_gridContainer == null) return;

            foreach (Transform child in _gridContainer)
            {
                Destroy(child.gameObject);
            }

            var designs = SaveManager.LoadAllDesigns();

            if (designs == null || designs.Count == 0)
            {
                if (_emptyStateText != null) _emptyStateText.gameObject.SetActive(true);
                return;
            }

            if (_emptyStateText != null) _emptyStateText.gameObject.SetActive(false);

            foreach (var item in designs)
            {
                CreateGalleryCard(item);
            }
        }

        private void CreateGalleryCard(NailDesignData data)
        {
            var cardGO = new GameObject($"Card_{data.designId}");
            cardGO.transform.SetParent(_gridContainer, false);

            var cardImg = cardGO.AddComponent<Image>();
            cardImg.color = new Color(1f, 0.95f, 0.98f);

            var btn = cardGO.AddComponent<Button>();
            btn.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
                GameManager.Instance?.LoadExistingDesign(data);
            });

            // Card thumbnail child
            byte[] thumbBytes = SaveManager.GetThumbnailBytes(data.designId);
            if (thumbBytes != null && thumbBytes.Length > 0)
            {
                var thumbGO = new GameObject("Thumbnail");
                thumbGO.transform.SetParent(cardGO.transform, false);
                var raw = thumbGO.AddComponent<RawImage>();

                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(thumbBytes);
                raw.texture = tex;
            }
        }
    }
}

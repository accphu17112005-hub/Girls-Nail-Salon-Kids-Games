using System.Collections.Generic;
using UnityEngine;
using KidsNailSalon.Data;
using KidsNailSalon.Core;

namespace KidsNailSalon.Nail
{
    [RequireComponent(typeof(Collider2D))]
    public class NailSlot : MonoBehaviour
    {
        [Header("Slot Identity")]
        public string nailId; // thumb, index, middle, ring, pinky

        [Header("Renderers & Mask")]
        [SerializeField] private SpriteRenderer _nailBaseRenderer;
        [SerializeField] private SpriteMask _nailMask;
        [SerializeField] private SpriteRenderer _polishRenderer;
        [SerializeField] private SpriteRenderer _gradientRenderer;
        [SerializeField] private SpriteRenderer _patternRenderer;
        [SerializeField] private SpriteRenderer _effectOverlayRenderer;
        [SerializeField] private GameObject _selectionIndicator;

        [Header("Containers")]
        [SerializeField] private Transform _stickersContainer;
        [SerializeField] private Transform _gemsContainer;

        private SingleNailData _data;

        public SingleNailData Data => _data;

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnNailSelected += OnNailSelectionChanged;
                GameManager.Instance.OnDesignModified += RefreshView;
            }
            RefreshView();
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnNailSelected -= OnNailSelectionChanged;
                GameManager.Instance.OnDesignModified -= RefreshView;
            }
        }

        public void BindData(SingleNailData data)
        {
            _data = data;
            RefreshView();
        }

        public void RefreshView()
        {
            if (GameManager.Instance != null && GameManager.Instance.ActiveDesign != null)
            {
                _data = GameManager.Instance.ActiveDesign.GetNail(nailId);
            }

            if (_data == null) return;

            // 1. Nail Shape & Mask
            string shapeName = _data.shape.ToString().ToLower();
            Sprite nailSprite = Resources.Load<Sprite>($"Art/Nails/nail_{shapeName}");
            Sprite maskSprite = Resources.Load<Sprite>($"Art/Nails/Masks/mask_{shapeName}");

            if (_nailBaseRenderer != null && nailSprite != null)
            {
                _nailBaseRenderer.sprite = nailSprite;
            }

            if (_nailMask != null && maskSprite != null)
            {
                _nailMask.sprite = maskSprite;
            }

            // 2. Polish Color
            if (_polishRenderer != null)
            {
                if (ColorUtility.TryParseHtmlString(_data.primaryColorHex, out Color col))
                {
                    _polishRenderer.color = col;
                }
                _polishRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }

            // 3. Gradient
            if (_gradientRenderer != null)
            {
                _gradientRenderer.gameObject.SetActive(_data.hasGradient);
                if (_data.hasGradient && ColorUtility.TryParseHtmlString(_data.secondaryColorHex, out Color gradCol))
                {
                    _gradientRenderer.color = gradCol;
                }
                _gradientRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }

            // 4. Pattern
            if (_patternRenderer != null)
            {
                _patternRenderer.gameObject.SetActive(_data.pattern != PatternType.None);
                _patternRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }

            // 5. Texture Effects
            if (_effectOverlayRenderer != null)
            {
                bool hasEffect = _data.effect != TextureEffectType.None && _data.effect != TextureEffectType.Glossy;
                _effectOverlayRenderer.gameObject.SetActive(hasEffect);
                _effectOverlayRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }

            // 6. Selection State
            bool isSelected = GameManager.Instance != null && GameManager.Instance.SelectedNailId == nailId;
            if (_selectionIndicator != null)
            {
                _selectionIndicator.SetActive(isSelected);
            }

            // 7. Stickers & Gems sync
            SyncStickers();
            SyncGems();
        }

        private void OnNailSelectionChanged(string selectedId)
        {
            if (_selectionIndicator != null)
            {
                _selectionIndicator.SetActive(selectedId == nailId);
            }
        }

        private void SyncStickers()
        {
            if (_stickersContainer == null || _data == null) return;
            // Clear existing views
            foreach (Transform child in _stickersContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var st in _data.stickers)
            {
                var go = new GameObject($"Sticker_{st.stickerId}");
                go.transform.SetParent(_stickersContainer, false);
                go.transform.localPosition = new Vector3(st.localX, st.localY, 0);
                go.transform.localScale = Vector3.one * st.scale;
                go.transform.localRotation = Quaternion.Euler(0, 0, st.rotationAngle);

                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = Resources.Load<Sprite>($"Art/Stickers/{st.category}/{st.stickerId}");
                sr.sortingLayerName = "Stickers";
                sr.sortingOrder = st.zOrder;
                sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
        }

        private void SyncGems()
        {
            if (_gemsContainer == null || _data == null) return;
            foreach (Transform child in _gemsContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var gm in _data.gems)
            {
                var go = new GameObject($"Gem_{gm.gemId}");
                go.transform.SetParent(_gemsContainer, false);
                go.transform.localPosition = new Vector3(gm.localX, gm.localY, 0);
                go.transform.localScale = Vector3.one * gm.scale;
                go.transform.localRotation = Quaternion.Euler(0, 0, gm.rotationAngle);

                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = Resources.Load<Sprite>($"Art/Gems/{gm.gemId}");
                sr.sortingLayerName = "Gems";
                sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
        }

        private void OnMouseDown()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SelectNail(nailId);
                AudioManager.Instance?.PlaySound(SoundType.ButtonClick, 1.2f);
            }
        }
    }
}

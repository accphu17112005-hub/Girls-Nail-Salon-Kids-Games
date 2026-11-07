using System.Collections.Generic;
using UnityEngine;
using KidsNailSalon.Data;
using KidsNailSalon.Core;
using KidsNailSalon.Nail;

namespace KidsNailSalon.Hand
{
    public class HandModelController : MonoBehaviour
    {
        [Header("Hand Visuals")]
        [SerializeField] private SpriteRenderer _handRenderer;

        [Header("Nail Anchors")]
        [SerializeField] private NailSlot _thumbSlot;
        [SerializeField] private NailSlot _indexSlot;
        [SerializeField] private NailSlot _middleSlot;
        [SerializeField] private NailSlot _ringSlot;
        [SerializeField] private NailSlot _pinkySlot;

        [Header("Accessories Mounts")]
        [SerializeField] private SpriteRenderer _ringMount;
        [SerializeField] private SpriteRenderer _braceletMount;

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnDesignModified += RefreshHandView;
            }
            RefreshHandView();
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnDesignModified -= RefreshHandView;
            }
        }

        public void RefreshHandView()
        {
            if (GameManager.Instance == null || GameManager.Instance.ActiveDesign == null) return;

            var design = GameManager.Instance.ActiveDesign;

            // 1. Hand Skin Tone
            string tone = string.IsNullOrEmpty(design.skinToneId) ? "fair" : design.skinToneId;
            Sprite handSprite = Resources.Load<Sprite>($"Art/Hands/hand_{tone}");
            if (_handRenderer != null && handSprite != null)
            {
                _handRenderer.sprite = handSprite;
            }

            // 2. Refresh each NailSlot
            _thumbSlot?.RefreshView();
            _indexSlot?.RefreshView();
            _middleSlot?.RefreshView();
            _ringSlot?.RefreshView();
            _pinkySlot?.RefreshView();

            // 3. Ring Accessory
            if (_ringMount != null)
            {
                if (design.rings != null && design.rings.Count > 0 && !string.IsNullOrEmpty(design.rings[0]))
                {
                    _ringMount.gameObject.SetActive(true);
                    _ringMount.sprite = Resources.Load<Sprite>($"Art/Accessories/Rings/{design.rings[0]}");
                }
                else
                {
                    _ringMount.gameObject.SetActive(false);
                }
            }

            // 4. Bracelet Accessory
            if (_braceletMount != null)
            {
                if (!string.IsNullOrEmpty(design.bracelet))
                {
                    _braceletMount.gameObject.SetActive(true);
                    _braceletMount.sprite = Resources.Load<Sprite>($"Art/Accessories/Bracelets/{design.bracelet}");
                }
                else
                {
                    _braceletMount.gameObject.SetActive(false);
                }
            }
        }
    }
}

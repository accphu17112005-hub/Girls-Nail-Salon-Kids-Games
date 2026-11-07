using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KidsNailSalon.Data;
using KidsNailSalon.Core;
using KidsNailSalon.UndoRedo;
using KidsNailSalon.SaveSystem;

namespace KidsNailSalon.UI
{
    public enum StudioTab
    {
        Polish,
        Shapes,
        Gradients,
        Textures,
        Patterns,
        Stickers,
        Gems,
        Accessories
    }

    public class StudioUIController : MonoBehaviour
    {
        [Header("Top Navigation Buttons")]
        [SerializeField] private Button _btnBack;
        [SerializeField] private Button _btnUndo;
        [SerializeField] private Button _btnRedo;
        [SerializeField] private Button _btnReset;
        [SerializeField] private Button _btnApplyAll;
        [SerializeField] private Button _btnSaveCamera;

        [Header("Category Tabs")]
        [SerializeField] private Button _tabPolish;
        [SerializeField] private Button _tabShapes;
        [SerializeField] private Button _tabGradients;
        [SerializeField] private Button _tabTextures;
        [SerializeField] private Button _tabPatterns;
        [SerializeField] private Button _tabStickers;
        [SerializeField] private Button _tabGems;
        [SerializeField] private Button _tabAccessories;

        [Header("Palette Trays")]
        [SerializeField] private Transform _paletteContent;
        [SerializeField] private GameObject _paletteItemPrefab;

        private StudioTab _currentTab = StudioTab.Polish;

        private void Start()
        {
            RegisterButtonCallbacks();
            if (GameManager.Instance != null)
            {
                GameManager.Instance.History.OnHistoryChanged += UpdateUndoRedoButtons;
                GameManager.Instance.OnNailSelected += OnNailSelected;
            }
            UpdateUndoRedoButtons();
            SwitchTab(StudioTab.Polish);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.History.OnHistoryChanged -= UpdateUndoRedoButtons;
                GameManager.Instance.OnNailSelected -= OnNailSelected;
            }
        }

        private void RegisterButtonCallbacks()
        {
            _btnBack?.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
                GameManager.Instance?.ChangeScreen(GameScreen.CharacterSelection);
            });

            _btnUndo?.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
                GameManager.Instance?.History.Undo();
            });

            _btnRedo?.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
                GameManager.Instance?.History.Redo();
            });

            _btnReset?.onClick.AddListener(ResetSelectedNail);
            _btnApplyAll?.onClick.AddListener(ApplySelectedNailToAll);
            
            _btnSaveCamera?.onClick.AddListener(() => {
                AudioManager.Instance?.PlaySound(SoundType.Sparkle);
                GameManager.Instance?.ChangeScreen(GameScreen.Result);
            });

            _tabPolish?.onClick.AddListener(() => SwitchTab(StudioTab.Polish));
            _tabShapes?.onClick.AddListener(() => SwitchTab(StudioTab.Shapes));
            _tabGradients?.onClick.AddListener(() => SwitchTab(StudioTab.Gradients));
            _tabTextures?.onClick.AddListener(() => SwitchTab(StudioTab.Textures));
            _tabPatterns?.onClick.AddListener(() => SwitchTab(StudioTab.Patterns));
            _tabStickers?.onClick.AddListener(() => SwitchTab(StudioTab.Stickers));
            _tabGems?.onClick.AddListener(() => SwitchTab(StudioTab.Gems));
            _tabAccessories?.onClick.AddListener(() => SwitchTab(StudioTab.Accessories));
        }

        public void SwitchTab(StudioTab tab)
        {
            _currentTab = tab;
            AudioManager.Instance?.PlaySound(SoundType.ButtonClick, 1.1f);
            PopulatePaletteTray();
        }

        private void PopulatePaletteTray()
        {
            if (_paletteContent == null) return;

            // Clear previous items
            foreach (Transform child in _paletteContent)
            {
                Destroy(child.gameObject);
            }

            switch (_currentTab)
            {
                case StudioTab.Polish:
                    PopulatePolishColors();
                    break;
                case StudioTab.Shapes:
                    PopulateNailShapes();
                    break;
                case StudioTab.Gradients:
                    PopulateGradients();
                    break;
                case StudioTab.Textures:
                    PopulateTextures();
                    break;
                case StudioTab.Patterns:
                    PopulatePatterns();
                    break;
                case StudioTab.Stickers:
                    PopulateStickers();
                    break;
                case StudioTab.Gems:
                    PopulateGems();
                    break;
                case StudioTab.Accessories:
                    PopulateAccessories();
                    break;
            }
        }

        private void PopulatePolishColors()
        {
            foreach (var preset in GameCatalogs.PolishColors)
            {
                var btn = CreatePaletteButton(preset.name, preset.GetColor());
                btn.onClick.AddListener(() => {
                    ApplyColorToSelectedNail(preset.hexColor, preset.defaultEffect);
                });
            }
        }

        private void PopulateNailShapes()
        {
            foreach (NailShapeType shape in System.Enum.GetValues(typeof(NailShapeType)))
            {
                var btn = CreatePaletteButton(shape.ToString(), new Color(1f, 0.9f, 0.95f));
                btn.onClick.AddListener(() => {
                    ApplyShapeToSelectedNail(shape);
                });
            }
        }

        private void PopulateGradients()
        {
            var gradientPairs = new (string Name, string SecHex)[]
            {
                ("Sunrise Orange", "#FF9E80"),
                ("Sky Lilac", "#B388FF"),
                ("Mint Lemon", "#CCFF90"),
                ("Candy Pink", "#FF80AB"),
                ("Ocean Cyan", "#80D8FF"),
                ("Gold Shimmer", "#FFE57F")
            };

            foreach (var grad in gradientPairs)
            {
                ColorUtility.TryParseHtmlString(grad.SecHex, out Color c);
                var btn = CreatePaletteButton(grad.Name, c);
                btn.onClick.AddListener(() => {
                    ApplyGradientToSelectedNail(grad.SecHex);
                });
            }
        }

        private void PopulateTextures()
        {
            foreach (TextureEffectType eff in System.Enum.GetValues(typeof(TextureEffectType)))
            {
                var btn = CreatePaletteButton(eff.ToString(), Color.white);
                btn.onClick.AddListener(() => {
                    ApplyTextureToSelectedNail(eff);
                });
            }
        }

        private void PopulatePatterns()
        {
            foreach (PatternType pat in System.Enum.GetValues(typeof(PatternType)))
            {
                var btn = CreatePaletteButton(pat.ToString(), new Color(0.95f, 0.95f, 1f));
                btn.onClick.AddListener(() => {
                    ApplyPatternToSelectedNail(pat);
                });
            }
        }

        private void PopulateStickers()
        {
            string[] popular = {
                "heart_pink", "heart_sparkle", "star_gold", "star_smiling",
                "flower_sakura", "animal_kitty", "animal_bunny", "animal_panda",
                "bow_pink_satin", "butterfly_pink", "fruit_strawberry", "food_cupcake",
                "fantasy_tiara", "fantasy_unicorn", "seasonal_snowflake"
            };

            foreach (var stId in popular)
            {
                string cat = stId.Split('_')[0];
                cat = char.ToUpper(cat[0]) + cat.Substring(1) + "s";
                var btn = CreatePaletteButton(stId, Color.white);
                btn.onClick.AddListener(() => {
                    AddStickerToSelectedNail(cat, stId);
                });
            }
        }

        private void PopulateGems()
        {
            foreach (var (name, col) in new (string, Color)[] {
                ("gem_diamond_round", Color.white),
                ("gem_heart_ruby", Color.red),
                ("gem_emerald_cut", Color.green),
                ("gem_pearl_round", new Color(1f, 0.98f, 0.95f)),
                ("gem_star_crystal", Color.yellow)
            })
            {
                var btn = CreatePaletteButton(name, col);
                btn.onClick.AddListener(() => {
                    AddGemToSelectedNail(name);
                });
            }
        }

        private void PopulateAccessories()
        {
            foreach (var r in GameCatalogs.Rings)
            {
                var btn = CreatePaletteButton(r, new Color(1f, 0.85f, 0.4f));
                btn.onClick.AddListener(() => {
                    EquipRing(r);
                });
            }

            foreach (var b in GameCatalogs.Bracelets)
            {
                var btn = CreatePaletteButton(b, new Color(0.9f, 0.9f, 1f));
                btn.onClick.AddListener(() => {
                    EquipBracelet(b);
                });
            }
        }

        private Button CreatePaletteButton(string label, Color bgCol)
        {
            var go = new GameObject($"Btn_{label}");
            go.transform.SetParent(_paletteContent, false);

            var img = go.AddComponent<Image>();
            img.color = bgCol;

            var btn = go.AddComponent<Button>();
            return btn;
        }

        private void ApplyColorToSelectedNail(string hex, TextureEffectType effect)
        {
            var nail = GameManager.Instance?.GetSelectedNail();
            if (nail == null) return;

            var cmd = new PaintNailCommand(
                nail, hex, false, nail.secondaryColorHex, effect, nail.pattern,
                () => GameManager.Instance?.NotifyDesignModified()
            );

            GameManager.Instance?.History.ExecuteCommand(cmd);
            AudioManager.Instance?.PlaySound(SoundType.BrushStroke);
        }

        private void ApplyShapeToSelectedNail(NailShapeType shape)
        {
            var nail = GameManager.Instance?.GetSelectedNail();
            if (nail == null) return;

            var cmd = new ChangeShapeCommand(nail, shape, () => GameManager.Instance?.NotifyDesignModified());
            GameManager.Instance?.History.ExecuteCommand(cmd);
            AudioManager.Instance?.PlaySound(SoundType.NailFile);
        }

        private void ApplyGradientToSelectedNail(string secHex)
        {
            var nail = GameManager.Instance?.GetSelectedNail();
            if (nail == null) return;

            var cmd = new PaintNailCommand(
                nail, nail.primaryColorHex, true, secHex, nail.effect, nail.pattern,
                () => GameManager.Instance?.NotifyDesignModified()
            );
            GameManager.Instance?.History.ExecuteCommand(cmd);
            AudioManager.Instance?.PlaySound(SoundType.BrushStroke);
        }

        private void ApplyTextureToSelectedNail(TextureEffectType effect)
        {
            var nail = GameManager.Instance?.GetSelectedNail();
            if (nail == null) return;

            var cmd = new PaintNailCommand(
                nail, nail.primaryColorHex, nail.hasGradient, nail.secondaryColorHex, effect, nail.pattern,
                () => GameManager.Instance?.NotifyDesignModified()
            );
            GameManager.Instance?.History.ExecuteCommand(cmd);
            AudioManager.Instance?.PlaySound(SoundType.Sparkle);
        }

        private void ApplyPatternToSelectedNail(PatternType pattern)
        {
            var nail = GameManager.Instance?.GetSelectedNail();
            if (nail == null) return;

            var cmd = new PaintNailCommand(
                nail, nail.primaryColorHex, nail.hasGradient, nail.secondaryColorHex, nail.effect, pattern,
                () => GameManager.Instance?.NotifyDesignModified()
            );
            GameManager.Instance?.History.ExecuteCommand(cmd);
            AudioManager.Instance?.PlaySound(SoundType.BrushStroke);
        }

        private void AddStickerToSelectedNail(string category, string stickerId)
        {
            var nail = GameManager.Instance?.GetSelectedNail();
            if (nail == null) return;

            var sticker = new StickerInstance
            {
                instanceId = System.Guid.NewGuid().ToString(),
                stickerId = stickerId,
                category = category,
                localX = 0,
                localY = 0,
                scale = 0.5f,
                rotationAngle = 0,
                zOrder = nail.stickers.Count + 1
            };

            var cmd = new AddStickerCommand(nail, sticker, () => GameManager.Instance?.NotifyDesignModified());
            GameManager.Instance?.History.ExecuteCommand(cmd);
            AudioManager.Instance?.PlaySound(SoundType.StickerPlace);
        }

        private void AddGemToSelectedNail(string gemId)
        {
            var nail = GameManager.Instance?.GetSelectedNail();
            if (nail == null) return;

            var gem = new GemInstance
            {
                instanceId = System.Guid.NewGuid().ToString(),
                gemId = gemId,
                localX = 0,
                localY = -0.2f,
                scale = 0.4f
            };

            nail.gems.Add(gem);
            GameManager.Instance?.NotifyDesignModified();
            AudioManager.Instance?.PlaySound(SoundType.Sparkle);
        }

        private void EquipRing(string ringId)
        {
            var design = GameManager.Instance?.ActiveDesign;
            if (design == null) return;

            if (design.rings == null) design.rings = new List<string>();
            if (design.rings.Contains(ringId))
            {
                design.rings.Remove(ringId);
            }
            else
            {
                design.rings.Clear();
                design.rings.Add(ringId);
            }
            GameManager.Instance?.NotifyDesignModified();
            AudioManager.Instance?.PlaySound(SoundType.Sparkle);
        }

        private void EquipBracelet(string braceletId)
        {
            var design = GameManager.Instance?.ActiveDesign;
            if (design == null) return;

            if (design.bracelet == braceletId)
            {
                design.bracelet = "";
            }
            else
            {
                design.bracelet = braceletId;
            }
            GameManager.Instance?.NotifyDesignModified();
            AudioManager.Instance?.PlaySound(SoundType.Sparkle);
        }

        private void ResetSelectedNail()
        {
            var nail = GameManager.Instance?.GetSelectedNail();
            if (nail == null) return;

            nail.primaryColorHex = "#FFB6C1";
            nail.hasGradient = false;
            nail.effect = TextureEffectType.Glossy;
            nail.pattern = PatternType.None;
            nail.stickers.Clear();
            nail.gems.Clear();

            GameManager.Instance?.NotifyDesignModified();
            AudioManager.Instance?.PlaySound(SoundType.ButtonClick);
        }

        private void ApplySelectedNailToAll()
        {
            var sourceNail = GameManager.Instance?.GetSelectedNail();
            var design = GameManager.Instance?.ActiveDesign;
            if (sourceNail == null || design == null) return;

            foreach (var n in design.nails)
            {
                n.shape = sourceNail.shape;
                n.primaryColorHex = sourceNail.primaryColorHex;
                n.hasGradient = sourceNail.hasGradient;
                n.secondaryColorHex = sourceNail.secondaryColorHex;
                n.effect = sourceNail.effect;
                n.pattern = sourceNail.pattern;
            }

            GameManager.Instance?.NotifyDesignModified();
            AudioManager.Instance?.PlaySound(SoundType.Sparkle);
        }

        private void OnNailSelected(string nailId)
        {
            // Update UI indicators
        }

        private void UpdateUndoRedoButtons()
        {
            if (_btnUndo != null) _btnUndo.interactable = GameManager.Instance != null && GameManager.Instance.History.CanUndo;
            if (_btnRedo != null) _btnRedo.interactable = GameManager.Instance != null && GameManager.Instance.History.CanRedo;
        }
    }
}

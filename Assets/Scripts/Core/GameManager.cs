using System;
using UnityEngine;
using KidsNailSalon.Data;
using KidsNailSalon.SaveSystem;
using KidsNailSalon.UndoRedo;

namespace KidsNailSalon.Core
{
    public enum GameScreen
    {
        MainMenu,
        CharacterSelection,
        HandCare,
        NailStudio,
        Result,
        Gallery,
        Settings
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action<GameScreen> OnScreenChanged;
        public event Action<string> OnNailSelected;
        public event Action OnDesignModified;

        public GameScreen CurrentScreen { get; private set; } = GameScreen.MainMenu;
        public NailDesignData ActiveDesign { get; private set; }
        public string SelectedNailId { get; private set; } = "middle";
        public CommandHistory History { get; private set; } = new CommandHistory();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SaveManager.Initialize();
                CreateNewDesign("fair");
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ChangeScreen(GameScreen newScreen)
        {
            CurrentScreen = newScreen;
            OnScreenChanged?.Invoke(newScreen);
            Debug.Log($"[GameManager] Navigated to screen: {newScreen}");
        }

        public void CreateNewDesign(string skinToneId = "fair")
        {
            ActiveDesign = new NailDesignData
            {
                designId = "design_" + DateTime.UtcNow.Ticks,
                skinToneId = skinToneId,
                backgroundId = "bg_salon_pink"
            };

            // Initialize 5 fingernails with cute defaults
            foreach (var id in GameCatalogs.NailIds)
            {
                ActiveDesign.nails.Add(new SingleNailData
                {
                    nailId = id,
                    shape = NailShapeType.Almond,
                    primaryColorHex = "#FFB6C1",
                    hasGradient = false,
                    effect = TextureEffectType.Glossy,
                    pattern = PatternType.None
                });
            }

            History.Clear();
            SelectedNailId = "middle";
            OnDesignModified?.Invoke();
        }

        public void LoadExistingDesign(NailDesignData design)
        {
            if (design == null) return;
            ActiveDesign = design.Clone();
            History.Clear();
            SelectedNailId = "middle";
            OnDesignModified?.Invoke();
            ChangeScreen(GameScreen.NailStudio);
        }

        public void SelectNail(string nailId)
        {
            SelectedNailId = nailId;
            OnNailSelected?.Invoke(nailId);
        }

        public SingleNailData GetSelectedNail()
        {
            return ActiveDesign?.GetNail(SelectedNailId);
        }

        public void NotifyDesignModified()
        {
            OnDesignModified?.Invoke();
        }

        public void SetActiveSkinTone(string skinToneId)
        {
            if (ActiveDesign != null)
            {
                ActiveDesign.skinToneId = skinToneId;
                OnDesignModified?.Invoke();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using KidsNailSalon.Data;

namespace KidsNailSalon.SaveSystem
{
    [Serializable]
    public class UserSettings
    {
        public bool musicEnabled = true;
        public bool sfxEnabled = true;
        public bool hapticsEnabled = true;
        public string preferredLanguage = "en";
        public bool parentalGatePassed = false;
    }

    [Serializable]
    public class GalleryIndex
    {
        public List<string> savedDesignIds = new List<string>();
    }

    public static class SaveManager
    {
        private static string SaveDirectory => Path.Combine(Application.persistentDataPath, "SavedDesigns");
        private static string SettingsFilePath => Path.Combine(Application.persistentDataPath, "settings.json");
        private static string GalleryIndexFilePath => Path.Combine(SaveDirectory, "index.json");

        private static UserSettings _settings;

        public static UserSettings Settings
        {
            get
            {
                if (_settings == null) LoadSettings();
                return _settings;
            }
        }

        public static void Initialize()
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
            LoadSettings();
        }

        public static void LoadSettings()
        {
            if (File.Exists(SettingsFilePath))
            {
                try
                {
                    string json = File.ReadAllText(SettingsFilePath);
                    _settings = JsonUtility.FromJson<UserSettings>(json);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[SaveManager] Failed to load settings: {e.Message}");
                    _settings = new UserSettings();
                }
            }
            else
            {
                _settings = new UserSettings();
                SaveSettings();
            }
        }

        public static void SaveSettings()
        {
            try
            {
                string json = JsonUtility.ToJson(_settings, true);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to save settings: {e.Message}");
            }
        }

        public static string SaveDesign(NailDesignData design, Texture2D thumbnail = null)
        {
            Initialize();

            if (string.IsNullOrEmpty(design.designId))
            {
                design.designId = "design_" + DateTime.UtcNow.Ticks;
            }
            design.timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            string designPath = Path.Combine(SaveDirectory, $"{design.designId}.json");
            string json = JsonUtility.ToJson(design, true);
            File.WriteAllText(designPath, json);

            if (thumbnail != null)
            {
                string thumbPath = Path.Combine(SaveDirectory, $"{design.designId}_thumb.png");
                byte[] bytes = thumbnail.EncodeToPNG();
                File.WriteAllBytes(thumbPath, bytes);
            }

            // Update Gallery Index
            var index = LoadGalleryIndex();
            if (!index.savedDesignIds.Contains(design.designId))
            {
                index.savedDesignIds.Insert(0, design.designId);
                SaveGalleryIndex(index);
            }

            Debug.Log($"[SaveManager] Saved design {design.designId} successfully.");
            return design.designId;
        }

        public static NailDesignData LoadDesign(string designId)
        {
            Initialize();
            string path = Path.Combine(SaveDirectory, $"{designId}.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<NailDesignData>(json);
            }
            return null;
        }

        public static List<NailDesignData> LoadAllDesigns()
        {
            Initialize();
            var list = new List<NailDesignData>();
            var index = LoadGalleryIndex();

            foreach (var id in index.savedDesignIds)
            {
                var data = LoadDesign(id);
                if (data != null)
                {
                    list.Add(data);
                }
            }
            return list;
        }

        public static bool DeleteDesign(string designId)
        {
            Initialize();
            string jsonPath = Path.Combine(SaveDirectory, $"{designId}.json");
            string thumbPath = Path.Combine(SaveDirectory, $"{designId}_thumb.png");

            bool deleted = false;
            if (File.Exists(jsonPath))
            {
                File.Delete(jsonPath);
                deleted = true;
            }
            if (File.Exists(thumbPath))
            {
                File.Delete(thumbPath);
            }

            var index = LoadGalleryIndex();
            if (index.savedDesignIds.Remove(designId))
            {
                SaveGalleryIndex(index);
            }

            return deleted;
        }

        public static byte[] GetThumbnailBytes(string designId)
        {
            string thumbPath = Path.Combine(SaveDirectory, $"{designId}_thumb.png");
            if (File.Exists(thumbPath))
            {
                return File.ReadAllBytes(thumbPath);
            }
            return null;
        }

        private static GalleryIndex LoadGalleryIndex()
        {
            if (File.Exists(GalleryIndexFilePath))
            {
                try
                {
                    string json = File.ReadAllText(GalleryIndexFilePath);
                    return JsonUtility.FromJson<GalleryIndex>(json);
                }
                catch
                {
                    return new GalleryIndex();
                }
            }
            return new GalleryIndex();
        }

        private static void SaveGalleryIndex(GalleryIndex index)
        {
            try
            {
                string json = JsonUtility.ToJson(index, true);
                File.WriteAllText(GalleryIndexFilePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to save gallery index: {e.Message}");
            }
        }
    }
}

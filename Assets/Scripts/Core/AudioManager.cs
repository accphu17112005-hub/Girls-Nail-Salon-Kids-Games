using System.Collections.Generic;
using UnityEngine;
using KidsNailSalon.SaveSystem;

namespace KidsNailSalon.Core
{
    public enum SoundType
    {
        ButtonClick,
        NailFile,
        BrushStroke,
        StickerPlace,
        Sparkle,
        SuccessFanfare
    }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _musicSource;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip _sfxClick;
        [SerializeField] private AudioClip _sfxFile;
        [SerializeField] private AudioClip _sfxBrush;
        [SerializeField] private AudioClip _sfxSticker;
        [SerializeField] private AudioClip _sfxSparkle;
        [SerializeField] private AudioClip _sfxSuccess;
        [SerializeField] private AudioClip _bgmMusic;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeSources();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            UpdateMusicPlayback();
        }

        private void InitializeSources()
        {
            if (_sfxSource == null) _sfxSource = gameObject.AddComponent<AudioSource>();
            if (_musicSource == null) _musicSource = gameObject.AddComponent<AudioSource>();

            _musicSource.loop = true;
            _musicSource.playOnAwake = false;

            // Attempt to load from Resources if not assigned in Inspector
            if (_sfxClick == null) _sfxClick = Resources.Load<AudioClip>("Audio/sfx_button_click");
            if (_sfxFile == null) _sfxFile = Resources.Load<AudioClip>("Audio/sfx_nail_file");
            if (_sfxBrush == null) _sfxBrush = Resources.Load<AudioClip>("Audio/sfx_brush_stroke");
            if (_sfxSticker == null) _sfxSticker = Resources.Load<AudioClip>("Audio/sfx_sticker_place");
            if (_sfxSparkle == null) _sfxSparkle = Resources.Load<AudioClip>("Audio/sfx_sparkle");
            if (_sfxSuccess == null) _sfxSuccess = Resources.Load<AudioClip>("Audio/sfx_success");
            if (_bgmMusic == null) _bgmMusic = Resources.Load<AudioClip>("Audio/bgm_calm_melody");

            if (_bgmMusic != null)
            {
                _musicSource.clip = _bgmMusic;
            }
        }

        public void PlaySound(SoundType type, float pitch = 1.0f)
        {
            if (!SaveManager.Settings.sfxEnabled || _sfxSource == null) return;

            AudioClip clip = type switch
            {
                SoundType.ButtonClick => _sfxClick,
                SoundType.NailFile => _sfxFile,
                SoundType.BrushStroke => _sfxBrush,
                SoundType.StickerPlace => _sfxSticker,
                SoundType.Sparkle => _sfxSparkle,
                SoundType.SuccessFanfare => _sfxSuccess,
                _ => null
            };

            if (clip != null)
            {
                _sfxSource.pitch = pitch;
                _sfxSource.PlayOneShot(clip);
            }
        }

        public void ToggleMusic(bool enabled)
        {
            SaveManager.Settings.musicEnabled = enabled;
            SaveManager.SaveSettings();
            UpdateMusicPlayback();
        }

        public void ToggleSfx(bool enabled)
        {
            SaveManager.Settings.sfxEnabled = enabled;
            SaveManager.SaveSettings();
        }

        private void UpdateMusicPlayback()
        {
            if (_musicSource == null) return;

            if (SaveManager.Settings.musicEnabled && _musicSource.clip != null)
            {
                if (!_musicSource.isPlaying)
                {
                    _musicSource.Play();
                }
            }
            else
            {
                _musicSource.Pause();
            }
        }
    }
}

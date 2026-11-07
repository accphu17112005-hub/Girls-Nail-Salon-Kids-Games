using System;
using UnityEngine;
using UnityEngine.UI;
using KidsNailSalon.Core;

namespace KidsNailSalon.HandCare
{
    public class HandCareController : MonoBehaviour
    {
        public enum SpaStep
        {
            FileNails,
            ApplyCream,
            TowelDry,
            Completed
        }

        [Header("Spa Tools")]
        [SerializeField] private Image _toolNailFile;
        [SerializeField] private Image _toolHandCream;
        [SerializeField] private Image _toolTowel;

        [Header("Feedback Elements")]
        [SerializeField] private Text _stepInstructionText;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private ParticleSystem _sparkleParticles;
        [SerializeField] private Button _btnNextStudio;

        private SpaStep _currentStep = SpaStep.FileNails;
        private float _stepProgress = 0.0f;

        private void Start()
        {
            _btnNextStudio?.onClick.AddListener(OnProceedToStudio);
            SetStep(SpaStep.FileNails);
        }

        public void SetStep(SpaStep step)
        {
            _currentStep = step;
            _stepProgress = 0f;
            if (_progressSlider != null) _progressSlider.value = 0f;

            if (_toolNailFile != null) _toolNailFile.gameObject.SetActive(step == SpaStep.FileNails);
            if (_toolHandCream != null) _toolHandCream.gameObject.SetActive(step == SpaStep.ApplyCream);
            if (_toolTowel != null) _toolTowel.gameObject.SetActive(step == SpaStep.TowelDry);

            if (_stepInstructionText != null)
            {
                _stepInstructionText.text = step switch
                {
                    SpaStep.FileNails => "Step 1: Gently file the nails to make them neat!",
                    SpaStep.ApplyCream => "Step 2: Rub soothing berry cream onto the hand!",
                    SpaStep.TowelDry => "Step 3: Pat dry with a fluffy towel for shiny skin!",
                    SpaStep.Completed => "All Clean & Glowing! Ready for Nail Art!",
                    _ => ""
                };
            }

            if (_btnNextStudio != null)
            {
                _btnNextStudio.gameObject.SetActive(step == SpaStep.Completed);
            }
        }

        public void AddCareInteraction(float amount)
        {
            if (_currentStep == SpaStep.Completed) return;

            _stepProgress = Mathf.Clamp01(_stepProgress + amount);
            if (_progressSlider != null) _progressSlider.value = _stepProgress;

            if (_currentStep == SpaStep.FileNails)
            {
                AudioManager.Instance?.PlaySound(SoundType.NailFile);
            }
            else
            {
                AudioManager.Instance?.PlaySound(SoundType.BrushStroke);
            }

            if (_stepProgress >= 1.0f)
            {
                AudioManager.Instance?.PlaySound(SoundType.Sparkle);
                if (_sparkleParticles != null) _sparkleParticles.Play();

                if (_currentStep == SpaStep.FileNails) SetStep(SpaStep.ApplyCream);
                else if (_currentStep == SpaStep.ApplyCream) SetStep(SpaStep.TowelDry);
                else if (_currentStep == SpaStep.TowelDry) SetStep(SpaStep.Completed);
            }
        }

        private void OnProceedToStudio()
        {
            AudioManager.Instance?.PlaySound(SoundType.SuccessFanfare);
            GameManager.Instance?.ChangeScreen(GameScreen.NailStudio);
        }
    }
}

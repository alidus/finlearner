using System;
using System.Globalization;
using UnityEngine.UI;

namespace UnityEngine
{
    public class GameSettings : MonoBehaviour
    {
        // UI elements
        GameObject uiCanvas;
        GameObject settingMenuPanel;

        Button backButton;
        
        // Sliders
        Slider masterVolumeSlider;
        
        // Text values
        Text masterVolumeValueText;

        // Volume fields
        float globalVolume;

        void Awake()
        {
            UpdateReferences();
            DontDestroyOnLoad(gameObject);
        }
        
        public void UpdateReferences()
        {
            uiCanvas = GameObject.Find("UICanvas");
            if (uiCanvas != null)
            {
                Transform volumeSlidersPanelTransform = transform.Find("VolumeSlidersPanel");
                Transform masterVolumeTransform = volumeSlidersPanelTransform?.transform.Find("MasterVolume");
                Transform masterVolumeSliderTransform = masterVolumeTransform?.transform.Find("Slider");
                masterVolumeSlider = masterVolumeSliderTransform.GetComponent<Slider>();
                Transform masterVolumeValueTextTransform = masterVolumeTransform?.transform.Find("ValueText");
                masterVolumeValueText = masterVolumeValueTextTransform.GetComponent<Text>();
                backButton = transform.Find("BackButton")?.GetComponent<Button>();
            }
        }
        
        private void Start()
        {
            Init();
        }

        void Init()
        {
            backButton.onClick.AddListener(Hide);

            globalVolume = 0.5f;
            UpdateGlobalVolumeSliderValue();
            UpdateGlobalVolumeText();
        }

        public void SetGlobalVolume(float globalVolume)
        {
            this.globalVolume = globalVolume;
            UpdateGlobalVolumeSliderValue();
            UpdateGlobalVolumeText();
            AudioListener.volume = globalVolume;
            Debug.Log(globalVolume);
        }

        public float GetGlobalVolume()
        {
            return globalVolume;
        }

        private void UpdateGlobalVolumeText()
        {
            masterVolumeValueText.text = Convert.ToInt32(globalVolume * 100)
                                                    .ToString(CultureInfo.InvariantCulture) + "%";
        }
        
        private void UpdateGlobalVolumeSliderValue()
        {
            masterVolumeSlider.value = globalVolume;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
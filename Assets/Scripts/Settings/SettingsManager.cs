using System;
using System.Globalization;
using UnityEngine.UI;

namespace UnityEngine
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager instance;

        // Managers, Controllers
        SettingsManager settingsManager;

        // UI elements
        private GameObject uiCanvas;
        private GameObject settingMenuPanel;
        
        // Sliders
        private Slider masterVolumeSlider;
        
        // Counters
        private Text masterVolumeValueText;

        // Volume fields
        private float globalVolume;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            UpdateReferences();
            DontDestroyOnLoad(gameObject);
        }
        
        public void UpdateReferences()
        {
            uiCanvas = GameObject.Find("UICanvas");
            if (uiCanvas != null)
            {
                Transform settingMenuPanelTransform = uiCanvas.transform.Find("SettingsMenuPanel");
                if (settingMenuPanelTransform != null)
                {
                    Transform volumeSlidersPanelTransform = settingMenuPanelTransform.transform.Find("VolumeSlidersPanel");
                    if (volumeSlidersPanelTransform)
                    {
                        Transform masterVolumeTransform = volumeSlidersPanelTransform.transform.Find("MasterVolume");
                        if (masterVolumeTransform != null)
                        {
                            Transform masterVolumeSliderTransform = masterVolumeTransform.transform.Find("Slider");
                            if (masterVolumeSliderTransform)
                            {
                                masterVolumeSlider = masterVolumeSliderTransform.GetComponent<Slider>();
                            }
                            Transform masterVolumeValueTextTransform = masterVolumeTransform.transform.Find("ValueText");
                            if (masterVolumeValueTextTransform != null)
                            {
                                masterVolumeValueText = masterVolumeValueTextTransform.GetComponent<Text>();
                            }
                        }
                    }
                }
            }
        }
        
        private void Start()
        {
            Init();
        }

        void Init()
        {
            settingsManager = SettingsManager.instance;
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
            instance.masterVolumeValueText.text = Convert.ToInt32(globalVolume * 100)
                                                    .ToString(CultureInfo.InvariantCulture) + "%";
        }
        
        private void UpdateGlobalVolumeSliderValue()
        {
            instance.masterVolumeSlider.value = globalVolume;
        }
    }
}
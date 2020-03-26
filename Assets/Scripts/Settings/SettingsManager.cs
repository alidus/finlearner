using System;
using System.Globalization;
using UnityEngine.UI;

namespace UnityEngine
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager instance;
        
        // UI elements
        private GameObject uiCanvas;
        private GameObject settingMenuPanel;
        
        // Sliders
        private Slider globalVolumeSlider;
        
        // Counters
        private Text globalVolumeCounter;

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
            settingMenuPanel = uiCanvas.transform.Find("SettingsMenuPanel").gameObject;
            globalVolumeSlider = settingMenuPanel.transform.Find("GlobalVolumeSlider").GetComponent<Slider>();
            globalVolumeCounter = settingMenuPanel.transform.Find("GlobalVolumeCounter").GetComponent<Text>();
        }
        
        private void Start()
        {
            Init();
        }

        void Init()
        {
            SettingsManager settingsManager = SettingsManager.instance;
            globalVolume = 0.5f;
            SetGlobalVolumeSliderPosition();
            SetGlobalVolumeCounterText();
        }

        public void SetGlobalVolume(float globalVolume)
        {
            instance.globalVolume = globalVolume;
            SetGlobalVolumeSliderPosition();
            SetGlobalVolumeCounterText();
            AudioListener.volume = globalVolume;
            Debug.Log(globalVolume);
        }

        public float GetGlobalVolume()
        {
            return instance.globalVolume;
        }

        public void SetGlobalVolumeCounterText()
        {
            instance.globalVolumeCounter.text = Convert.ToInt32(globalVolume * 100)
                                                    .ToString(CultureInfo.InvariantCulture) + "%";
        }
        
        public void SetGlobalVolumeSliderPosition()
        {
            instance.globalVolumeSlider.value = globalVolume;
        }
    }
}
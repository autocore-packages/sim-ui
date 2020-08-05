
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class PanelSettings : PanelBase<PanelSettings>, ISimuPanel
    {
        public Button btn_close;
        public Toggle toggle_isFullScreen;

        public Toggle toggle_isSimuControl;
        public Dropdown dropdown_resolution;
        public Dropdown dropdown_quality;
        public Toggle toggle_SimuMessage;
        public Toggle toggle_simu_ndt;
        public Toggle toggle_FollowCarPos;
        public Toggle toggle_FollowCarRot;
        public Text text_maxCameraRange;
        public Slider slider_maxCameraRange;
        public Text text_maxSteerAngle;
        public Slider slider_maxSteerAngle;
        public Text text_friction;
        public Slider slider_friction;
        public Text text_maxTorque;
        public Slider slider_maxTorque;
        public Text text_maxSpeed;
        public Slider slider_maxSpeed;
        void Start()
        {
            InitResolutionItem();
            btn_close?.onClick.AddListener(() => { SetPanelActive(false); });
            toggle_isFullScreen?.onValueChanged.AddListener((bool value) => { isFullScreen = value; SetScreenResolution(); });
            toggle_SimuMessage?.onValueChanged.AddListener(ToogleSimuMessagePanel);
            toggle_isSimuControl?.onValueChanged.AddListener(SetSimuDriveMode);
            slider_maxSteerAngle?.onValueChanged.AddListener(SetMaxSteerAngle);
            slider_friction?.onValueChanged.AddListener(SetFriction);
            slider_maxTorque?.onValueChanged.AddListener(SetMaxTorque);
            slider_maxCameraRange?.onValueChanged.AddListener(SetMaxCameraRange);
            slider_maxSpeed?.onValueChanged.AddListener(SetMaxSpeed);
            dropdown_quality.onValueChanged.AddListener(SetQuality);
            SetPanelActive(false); 
            PanelInit();
        }

        private void PanelInit()
        {
            SetSimuDriveMode(toggle_isSimuControl.isOn);
            ToogleSimuMessagePanel(toggle_SimuMessage.isOn);
            SetQuality(2);
        }
        private void InitResolutionItem()
        {
            dropdown_resolution.ClearOptions();
            Dropdown.OptionData temoData;
            for (int i = 0; i < DicResolution.Count; i++)
            {
                if (DicResolution.TryGetValue(i, out Resolution resolution))
                {
                    temoData = new Dropdown.OptionData();
                    temoData.text = resolution.ToString();
                    dropdown_resolution.options.Add(temoData);
                }
                else
                {
                    Debug.Log("GetResolution fail");
                }
            }
            dropdown_resolution.onValueChanged.AddListener(SetResolution);
            dropdown_resolution.value = 2;
            SetResolution(2);
        }
        public GameObject GOMaxSpd;

        public void SetSimuDriveMode(bool value)
        {
            slider_maxSpeed.interactable = value;
        }
        private void SetMaxSteerAngle(float arg0)
        {
            if (text_maxSteerAngle != null)
            {
                text_maxSteerAngle.text = string.Format(arg0.ToString());
            }
        }
        private void ToogleSimuMessagePanel(bool arg0)
        {
            PanelSimuMessage.Instance.SetPanelActive(arg0);
        }
        private void SetFriction(float arg0)
        {
            if (text_friction != null)
            {
                text_friction.text = string.Format(arg0.ToString());
            }
        }
        private void SetMaxTorque(float arg0)
        {
            if (text_maxTorque != null)
            {
                text_maxTorque.text = string.Format(arg0.ToString());
            }
        }
        private void SetMaxCameraRange(float arg0)
        {
            if (text_maxCameraRange != null)
            {
                text_maxCameraRange.text = string.Format(arg0.ToString());
            }
        }

        private void SetMaxSpeed(float value)
        {
            if (text_maxSpeed != null)
            {
                text_maxSpeed.text = value.ToString();
            }
        }
        private Dictionary<int, Resolution> DicResolution = new Dictionary<int, Resolution>
        {
            {0,new Resolution(2560,1440)},
            {1,new Resolution(1920,1080)},
            {2,new Resolution(1600,900)},
            {3,new Resolution(1366,768)},
            {4,new Resolution(1280,720)},
            {5,new Resolution(1024,576)}
        };
        private struct Resolution
        {
            public int width;
            public int height;

            public Resolution(int v1, int v2)
            {
                width = v1;
                height = v2;
            }
            public override string ToString()
            {
                return width.ToString() + "*" + height.ToString();
            }
        }
        private bool isFullScreen;
        private Resolution resolution;
        private void SetResolution(int value)
        {
            if (DicResolution.TryGetValue(value, out resolution))
            {
                SetScreenResolution();
            }
            else
            {
                Debug.LogError("No such Resolution");
            }
        }
        private void SetScreenResolution()
        {
            Screen.SetResolution(resolution.width, resolution.height, isFullScreen);
        }
        private void SetQuality(int lv)
        {
            QualitySettings.SetQualityLevel(lv);
        }
    }

}

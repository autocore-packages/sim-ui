
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class PanelSimuMessage : PanelBase<PanelSimuMessage>, ISimuPanel
    {

        public Button button_exit;
        public Text text_version;
        public Text text_IP;
        public Text text_FPS;
        public Text text_mode;
        public Text text_Control;
        void Start()
        {
            text_version.text = Application.version;
            button_exit?.onClick.AddListener(() =>
            {
                SetPanelActive(false);
            });
        }
        void Update()
        {
            if (isActive) ShowFPS();
        }

        private float timeFPSDelta = 1;
        private float timePass;
        private int countFrame;
        private float FPS;
        private void ShowFPS()
        {
            if (text_FPS == null) return;
            countFrame++;
            timePass += Time.deltaTime;
            if (timePass >= timeFPSDelta)
            {
                FPS = countFrame / timePass;
                text_FPS.text = FPS.ToString("0.0");
                timePass = 0;
                countFrame = 0;
            }
        }
        public void SetControlModeText(string text)
        {
            text_Control.text = text;
        }

        public void SetModeText(string text)
        {
            text_mode.text = text;
        }
        public void SetIPText(string text)
        {

            text_IP.text = text.Replace("http://", string.Empty);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.SimuUI
{
    public class PanelCarMessage : PanelBase<PanelCarMessage>, ISimuPanel
    {
        public Button btn_MessagePanelHide;
        private Animation anim_PanelMessage;
        public Text text_throttle;
        public Text text_brake;
        public Text text_steer;
        public Text text_speed;
        public Text text_exceptSpeed;
        public Text text_Odom;
        public Image image_wheel;
        private MessageShow ms;
        private Animation Anim_PanelMessage
        {
            get
            {
                if(anim_PanelMessage==null)
                    anim_PanelMessage = GetComponent<Animation>();
                return anim_PanelMessage;
            }
        }
        protected override void Start()
        {
            base.Start();
            ms = GetComponent<MessageShow>();
            btn_MessagePanelHide?.onClick.AddListener(() =>
            {
                isActive = !isActive;
                SetPanelActive(isActive);
            });
        }
        public void UpdateCarmessage(float steer,string odom,float brake,float throttle,float speed,float expSpeed)
        {
            image_wheel.rectTransform.rotation = Quaternion.Euler(0, 0, -steer * 540);
            text_Odom.text = odom;
            text_brake.text = brake.ToString("0.00");
            text_throttle.text = throttle.ToString("0.00");
            text_steer.text = steer.ToString("0.00");
            text_speed.text = speed.ToString("0.00") + "km/h";
            text_exceptSpeed.text = expSpeed.ToString();
            ms.UpdateImage(throttle, brake, steer, speed, expSpeed);
        }
        public override void SetPanelActive(bool value)
        {
            if (value)
            {
                Anim_PanelMessage["PanelMessageHide"].normalizedTime = 0;
                Anim_PanelMessage["PanelMessageHide"].speed = 1;
                Anim_PanelMessage.Play("PanelMessageHide");
            }
            else
            {
                Anim_PanelMessage["PanelMessageHide"].normalizedTime = 1.0f;
                Anim_PanelMessage["PanelMessageHide"].speed = -1;
                Anim_PanelMessage.Play("PanelMessageHide");
            }
        }
    }
}

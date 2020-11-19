
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class PanelOther : PanelBase<PanelOther>, ISimuPanel
    {
        public Text text_Tips;
        public Text text_mousePos;
        public Text text_name;
        // Start is called before the first frame update

        private float timeTemp;
        private float time_tipTemp;
        // Update is called once per frame
        void Update()
        {
            timeTemp += Time.deltaTime;
            if (timeTemp > 1)
            {
                timeTemp = 0;
            }
            if (time_tipTemp < time_tip)
            {
                time_tipTemp += Time.deltaTime;
            }
            else
            {
                text_Tips.text = string.Empty;
            }
        }
        public void ShowMouseDis2Car(bool isCarcamera, float front,float right)
        {
            text_mousePos.gameObject.SetActive(!isCarcamera && !MainUI.Instance.isMouseOnUI);
            text_mousePos.rectTransform.position = new Vector3(Input.mousePosition.x + 20, Input.mousePosition.y, 0);
            text_mousePos.text = front.ToString("0.00") + "\n" + right.ToString("0.00");
        }
        private float time_tip = 5;
        public void SetTipText(string str)
        {
            text_Tips.text = str;
            time_tipTemp = 0;
        }
    }
}

#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion
using SyntaxTree.VisualStudio.Unity.Bridge;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class PanelInspector : PanelBase<PanelInspector>, ISimuPanel
    {
        public ElementAttbutes elementAttbutes;
        /// <summary>
        /// 0 name
        /// 1 pos
        /// 2 rot
        /// 3 scale
        /// 4 human
        /// 5 traffic
        /// 6 carAI
        /// 7 delete
        /// </summary>
        public GameObject[] attGameObjects;
        public GameObject attName;
        public GameObject attPos;
        public GameObject attRot;
        public GameObject attScale;
        public GameObject attHuman;
        public GameObject attCarAI;
        public GameObject attTrafficLight;
        public GameObject attDelete;


        public UnityAction[] SetInspectorActions;
        public InputField inputField_carAIspeed;
        public Button button_changeLeft;
        public Button button_changeRight;
        public Button button_SetAim;

        public Button btn_DeleteObj;
        public ContentSizeFitter cs;

        public void InspectorInit(ElementAttbutes attbutes)
        {
            SetPanelActive(true);
            elementAttbutes = attbutes;
            attName.SetActive(elementAttbutes.isShowName);
            attScale.SetActive(elementAttbutes.isShowSca);
            attRot.SetActive(elementAttbutes.isShowRot);
            attPos.SetActive(elementAttbutes.isShowPos);
            attTrafficLight.SetActive(elementAttbutes.isShowTraffic);
            attHuman.SetActive(elementAttbutes.isShowHuman);
            attCarAI.SetActive(elementAttbutes.isShowCarAI);
            attDelete.SetActive(elementAttbutes.isShowDelete);
            if (elementAttbutes.isShowHuman) UpdateHumanData();
            if (elementAttbutes.isShowCarAI) UpdateCarAIData();
            if (elementAttbutes.isShowTraffic) UpdateTrafficLightData();
        }
        public void InspectorUpdate(ElementAttbutes attbutes)
        {
            elementAttbutes = attbutes;
            if (elementAttbutes.isShowName) UpdateName();
            if (elementAttbutes.isShowPos || elementAttbutes.isShowRot || elementAttbutes.isShowSca) UpdateTransformDate();
        }
        private void UpdateName()
        {
            inputField_name.text = elementAttbutes.Name;
        }
        private void UpdateTransformDate()
        {
            inputField_posX.text = elementAttbutes.TransformData.V3Pos.X.ToString("0.00");
            inputField_posY.text = elementAttbutes.TransformData.V3Pos.Y.ToString("0.00");
            inputField_posZ.text = elementAttbutes.TransformData.V3Pos.Z.ToString("0.00");
            inputField_scale.text = elementAttbutes.TransformData.V3Sca.Y.ToString("0.00");
            inputField_rot.text = elementAttbutes.TransformData.V3Rot.Y.ToString("0.00");
        }
        public List<AimPos> ListAimPos;
        private void UpdateHumanData()
        {
            if (ListAimPos.Count != 10) Debug.Log("aimlist count error");
            if (elementAttbutes.PosArray.Count == 0) return;
            for (int i = 0; i < ListAimPos.Count; i++)
            {
                ListAimPos[i].gameObject.SetActive(i < elementAttbutes.PosArray.Count);
                if (i < elementAttbutes.PosArray.Count) ListAimPos[i].Init(elementAttbutes.PosArray[i]);
            }
            Toggle_isHumanWait.isOn = elementAttbutes.IsWait;
            Toggle_isHumanRepeat.isOn = elementAttbutes.IsRepeat;
            inputField_humanSpeed.text = elementAttbutes.Speed.ToString();
        }
        private void UpdateCarAIData()
        {
            inputField_carAIspeed.text = elementAttbutes.Speed.ToString();
        }
        private void UpdateTrafficLightData()
        {
            inputField_switchtime.text = elementAttbutes.SwitchTime.ToString("0.00");
            inputField_waittime.text = elementAttbutes.WaitTime.ToString("0.00");
        }
        public UnityAction<ElementAttbutes> ElementUpdate=new UnityAction<ElementAttbutes>((value)=> { });
        public UnityAction OnSwitchLight;
        public GameObject AimPos;
        public Transform HumanOther;
        public Toggle Toggle_isHumanRepeat;
        public Toggle Toggle_isHumanWait;
        public InputField inputField_humanSpeed;
        public Button button_AddPos;
        public InputField inputField_name;

        public InputField inputField_posX;
        public InputField inputField_posY;
        public InputField inputField_posZ;
        public InputField inputField_rot;
        public InputField inputField_scale;
        public Button button_SwitchLight;
        public InputField inputField_switchtime;
        public InputField inputField_waittime;
        private void Start()
        {
            cs.enabled = true;
            cs.verticalFit = ContentSizeFitter.FitMode.MinSize;
            inputField_name?.onEndEdit.AddListener((string value) =>
            {
                elementAttbutes.Name = value; 
                ElementUpdate.Invoke(elementAttbutes);
            });
            inputField_posX?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.TransformData.V3Pos.X = num;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            inputField_posY?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.TransformData.V3Pos.Y = num;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            inputField_posZ?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.TransformData.V3Pos.Z = num;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            inputField_rot?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.TransformData.V3Rot.Y = num;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            inputField_scale?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.TransformData.V3Sca = new Vec3(num, num, num);
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            Toggle_isHumanRepeat.onValueChanged.AddListener((bool value) =>
            {
                elementAttbutes.IsRepeat = value;
                ElementUpdate.Invoke(elementAttbutes);
            });
            inputField_humanSpeed.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float speed))
                {
                    elementAttbutes.Speed = speed;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });

            button_SwitchLight?.onClick.AddListener(() =>
            {
                OnSwitchLight.Invoke();
            });
            inputField_switchtime?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.SwitchTime = num;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            inputField_waittime?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.WaitTime = num;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            inputField_carAIspeed?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.Speed= num;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            SetPanelActive(false);
        }
    }
}

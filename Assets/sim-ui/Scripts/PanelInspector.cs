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
using Assets.Scripts.simai;

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
        private ElementObject elementObject;
        public void OnChangeElement(ElementObject obj)
        {
            elementObject = obj;
            InspectorInit();
        }

        public void InspectorInit()
        {
            SetPanelActive(true);
            elementAttbutes = elementObject.GetObjAttbutes();
            attName.SetActive(elementAttbutes.IsShowName);
            attScale.SetActive(elementAttbutes.IsShowSca);
            attRot.SetActive(elementAttbutes.IsShowRot);
            attPos.SetActive(elementAttbutes.IsShowPos);
            attTrafficLight.SetActive(elementAttbutes.IsShowTraffic);
            attHuman.SetActive(elementAttbutes.IsShowHuman);
            attCarAI.SetActive(elementAttbutes.IsShowCarAI);
            attDelete.SetActive(elementAttbutes.IsShowDelete);
            if (elementAttbutes.IsShowName) UpdateName();
            if (elementAttbutes.IsShowHuman) UpdateHumanData();
            if (elementAttbutes.IsShowCarAI) UpdateCarAIData();
            if (elementAttbutes.IsShowTraffic) UpdateTrafficLightData();
        }
        public void InspectorUpdate()
        {
            elementAttbutes = elementObject.GetObjAttbutes();
            if (elementAttbutes.IsShowPos || elementAttbutes.IsShowRot || elementAttbutes.IsShowSca) UpdateTransformDate();
        }
        private void UpdateName()
        {
            if (!inputField_name.isFocused) inputField_name.text = elementAttbutes.Name;
        }
        private void UpdateTransformDate()
        {
            if (!inputField_posX.isFocused) inputField_posX.text = elementAttbutes.TransformData.V3Pos.X.ToString("0.00");
            if (!inputField_posY.isFocused) inputField_posY.text = elementAttbutes.TransformData.V3Pos.Y.ToString("0.00");
            if (!inputField_posZ.isFocused) inputField_posZ.text = elementAttbutes.TransformData.V3Pos.Z.ToString("0.00");
            if (!inputField_scale.isFocused) inputField_scale.text = elementAttbutes.TransformData.V3Sca.Y.ToString("0.00");
            if (!inputField_rot.isFocused) inputField_rot.text = elementAttbutes.TransformData.V3Rot.Y.ToString("0.00");
        }
        public List<AimPos> ListAimPos;
        private void UpdateHumanData()
        {
            if (ListAimPos.Count != 10) Debug.LogError("aimlist count error");
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
                UpdateSelectedElementAttbutes();
            });
            inputField_posX?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.TransformData.V3Pos.X = num;
                    UpdateSelectedElementAttbutes();
                }
            });
            inputField_posY?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.TransformData.V3Pos.Y = num;
                    UpdateSelectedElementAttbutes();
                }
            });
            inputField_posZ?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.TransformData.V3Pos.Z = num;
                    UpdateSelectedElementAttbutes();
                }
            });
            inputField_rot?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.TransformData.V3Rot.Y = num;
                    UpdateSelectedElementAttbutes();
                }
            });
            inputField_scale?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.TransformData.V3Sca = new Vec3(num, num, num);
                    UpdateSelectedElementAttbutes();
                }
            });
            Toggle_isHumanRepeat.onValueChanged.AddListener((bool value) =>
            {
                elementAttbutes.IsRepeat = value;
                UpdateSelectedElementAttbutes();
            });
            Toggle_isHumanWait.onValueChanged.AddListener((bool value) =>
            {
                elementAttbutes.IsWait = value;
                UpdateSelectedElementAttbutes();
            });
            inputField_humanSpeed.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float speed))
                {
                    elementAttbutes.Speed = speed;
                    UpdateSelectedElementAttbutes();
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
                    UpdateSelectedElementAttbutes();
                }
            });
            inputField_waittime?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.WaitTime = num;
                    UpdateSelectedElementAttbutes();
                }
            });
            inputField_carAIspeed?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.Speed= num;
                    UpdateSelectedElementAttbutes();
                }
            });
            SetPanelActive(false);
        }
        private void Update()
        {
            if (elementObject != ElementsManager.Instance.SelectedElement)
            {
                elementObject = ElementsManager.Instance.SelectedElement;
                if (elementObject == null) SetPanelActive(false);
                else
                {
                    InspectorInit();
                }
            }
        }
        public void UpdateSelectedElementAttbutes()
        {
            ElementsManager.Instance.SelectedElement.SetObjAttbutes(elementAttbutes);
        }
    }
}

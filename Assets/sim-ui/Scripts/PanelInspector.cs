#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class ElementAttbutes
    {
        public bool[] attributes=new bool[8] {false,false,false,false,false,false,false,false };
        public string name;//0
        public Vector3 pos;//1
        public float sca;//2
        public float rot;//3
        public CarAIAtt carAIAtt;//4
        public HumanAtt humanAtt;//5
        public TrafficLigghtAtt trafficLigghtAtt;//6
        public bool canDelete;//7
    }
    public class CarAIAtt
    {
        public float spdCarAI;
    }
    public class HumanAtt
    {
        public float speed;
        public bool isRepeat;
        public bool isWait;
        public List<Vector3> aimList;
    }
    public class TrafficLigghtAtt
    {
        public float timeSwitch;
        public float timeWait;
        public int mode;
    }
    public class PanelInspector : PanelBase<PanelInspector>, ISimuPanel
    {
        private ElementAttbutes elementAttbutes;
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
            for (int i = 0; i < elementAttbutes.attributes.Length; i++)
            {
                attGameObjects[i].SetActive(elementAttbutes.attributes[i]);
                if (elementAttbutes.attributes[i]) SetInspectorActions[i].Invoke();
            }
        }
        public void InspectorUpdate(ElementAttbutes attbutes)
        {
            elementAttbutes = attbutes;
            for (int i = 0; i < elementAttbutes.attributes.Length; i++)
            {
                if (elementAttbutes.attributes[i]) SetInspectorActions[i].Invoke();
            }
        }
        private Vector3 objPos;
        private Vector3 ObjPos
        {
            get
            {
                return objPos;
            }
            set
            {
                objPos = value;
                inputField_posX.text = ObjPos.x.ToString();
                inputField_posY.text = ObjPos.y.ToString();
                inputField_posZ.text = ObjPos.z.ToString();
            }
        }
        public UnityAction<ElementAttbutes> ElementUpdate=new UnityAction<ElementAttbutes>((value)=> { });

        public GameObject AimPos;
        public Transform HumanOther;
        public Toggle Toggle_isHumanRepeat;
        public Toggle Toggle_isHumanWait;
        public InputField inputField_humanSpeed;
        public Button button_AddPos;
        private List<AimPos> ListAimPos = new List<AimPos>();
        private void SetHumanAtt()
        {
            if (elementAttbutes.humanAtt.aimList != null)
            {
                for (int i = 0; i < elementAttbutes.humanAtt.aimList.Count; i++)
                {
                    Vector3 pos = elementAttbutes.humanAtt.aimList[i];
                    AimPos aimPos = Instantiate(AimPos, attGameObjects[4].transform).GetComponent<AimPos>();
                    aimPos.Init(pos);
                    ListAimPos.Add(aimPos);
                }
            }
            HumanOther.SetAsLastSibling();
            Toggle_isHumanWait.isOn = elementAttbutes.humanAtt.isWait;
            Toggle_isHumanRepeat.isOn = elementAttbutes.humanAtt.isRepeat;
            inputField_humanSpeed.text = elementAttbutes.humanAtt.speed.ToString();
        }

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
            SetInspectorActions = new UnityAction[]
            {
                ()=>{ inputField_name.text=elementAttbutes.name; },
                ()=>{ ObjPos = elementAttbutes.pos;},
                ()=>{ inputField_rot.text = elementAttbutes.rot.ToString();},
                ()=>{ inputField_scale.text = elementAttbutes.sca.ToString(); },
                SetHumanAtt,
                ()=>{ inputField_switchtime.text = elementAttbutes.trafficLigghtAtt.timeSwitch.ToString();
                    inputField_waittime.text = elementAttbutes.trafficLigghtAtt.timeWait.ToString();},
                ()=>{ inputField_carAIspeed.text = elementAttbutes.carAIAtt.spdCarAI.ToString(); },
                ()=>{ btn_DeleteObj.gameObject.SetActive(elementAttbutes.canDelete);}
            };

            cs.enabled = true;
            cs.verticalFit = ContentSizeFitter.FitMode.MinSize;
            inputField_name?.onEndEdit.AddListener((string value) =>
            {
                elementAttbutes.name = value; 
                ElementUpdate.Invoke(elementAttbutes);
            });
            inputField_posX?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    objPos.x = num;
                    elementAttbutes.pos = objPos;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            inputField_posY?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    objPos.y = num;
                    elementAttbutes.pos = objPos;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            inputField_posZ?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    objPos.z = num;
                    elementAttbutes.pos = objPos;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            inputField_rot?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.rot = num;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            inputField_scale?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.sca = num;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            Toggle_isHumanRepeat.onValueChanged.AddListener((bool value) =>
            {
                elementAttbutes.humanAtt.isRepeat = value;
                ElementUpdate.Invoke(elementAttbutes);
            });
            inputField_humanSpeed.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float speed))
                {
                    elementAttbutes.humanAtt.speed = speed;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });

            button_SwitchLight?.onClick.AddListener(() =>
            {
            });
            inputField_switchtime?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.trafficLigghtAtt.timeSwitch = num;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            inputField_waittime?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.trafficLigghtAtt.timeWait = num;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            inputField_carAIspeed?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    elementAttbutes.carAIAtt.spdCarAI= num;
                    ElementUpdate.Invoke(elementAttbutes);
                }
            });
            SetPanelActive(false);
        }
    }
}

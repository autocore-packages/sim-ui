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
        private string objName;
        private string ObjName
        {
            get
            {
                return objName;
            }
            set
            {
                if (value != objName)
                {
                    objName = value;
                    inputField_name.text = objName;
                }
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
                if (value != objPos)
                {
                    objPos = value;
                    inputField_posX.text = ObjPos.x.ToString("0.00");
                    inputField_posY.text = ObjPos.y.ToString("0.00");
                    inputField_posZ.text = ObjPos.z.ToString("0.00");
                }
            }
        }
        private float scale;
        private float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                if (value != scale)
                {
                    scale = value;
                    inputField_scale.text = scale.ToString("0.00");
                }
            }
        }
        private float rotate;
        private float Rotate
        {
            get
            {
                return rotate;
            }
            set
            {
                if (value != rotate)
                {
                    rotate = value;
                    inputField_rot.text = rotate.ToString("0.00");
                }
            }
        }
        public List<AimPos> ListAimPos = new List<AimPos>();
        private List<Vector3> humanPoses;
        private List<Vector3> HumanPoses
        {
            get
            {
                return humanPoses; 
            }
            set
            {
                if (value.TrueForAll(humanPoses.Contains))
                {
                    humanPoses = value;
                    while (ListAimPos.Count > humanPoses.Count)
                    {
                        Destroy(ListAimPos[ListAimPos.Count - 1].gameObject);
                        ListAimPos.RemoveAt(ListAimPos.Count - 1);
                    }
                    while (attGameObjects[4].transform.childCount -2 < humanPoses.Count)
                        Instantiate(AimPos, attGameObjects[4].transform);
                    HumanOther.SetAsLastSibling();
                    for (int i = 0; i < humanPoses.Count; i++)
                    {
                        AimPos aim = attGameObjects[4].transform.GetChild(i + 1).GetComponent<AimPos>();
                        if (aim == null) 
                        {
                            Debug.Log("Human error");
                        }
                        Vector3 pos = humanPoses[i];
                        aim.Init(pos);
                    }
                    Toggle_isHumanWait.isOn = elementAttbutes.humanAtt.isWait;
                    Toggle_isHumanRepeat.isOn = elementAttbutes.humanAtt.isRepeat;
                    inputField_humanSpeed.text = elementAttbutes.humanAtt.speed.ToString();
                }
            }
        }


        private float switchTime;
        private float SwitchTime
        {
            get
            {
                return switchTime;
            }
            set
            {
                if (value != switchTime)
                {
                    switchTime = value;
                    inputField_switchtime.text = switchTime.ToString("0.00");
                }
            }
        }
        private float waitTime;
        private float WaitTime
        {
            get
            {
                return waitTime;
            }
            set
            {
                if (value != waitTime)
                {
                    waitTime = value;
                    inputField_waittime.text = waitTime.ToString("0.00");
                }
            }
        }
        private float speedCarAI;
        private float SpeedCarAI
        {
            get
            {
                return speedCarAI;
            }
            set
            {
                if (value != speedCarAI)
                {
                    speedCarAI = value; 
                    inputField_carAIspeed.text = speedCarAI.ToString();
                }
            }
        }

        public UnityAction<ElementAttbutes> ElementUpdate=new UnityAction<ElementAttbutes>((value)=> { });

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
            SetInspectorActions = new UnityAction[]
            {
                ()=>{ objName=elementAttbutes.name; },
                ()=>{ ObjPos = elementAttbutes.pos;},
                ()=>{ Rotate=elementAttbutes.rot; },
                ()=>{ Scale =elementAttbutes.sca; },
                ()=>{ HumanPoses=elementAttbutes.humanAtt.aimList; },
                ()=>{ SwitchTime= elementAttbutes.trafficLigghtAtt.timeSwitch;
                    waitTime=elementAttbutes.trafficLigghtAtt.timeWait;},
                ()=>{ SpeedCarAI=elementAttbutes.carAIAtt.spdCarAI; },
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

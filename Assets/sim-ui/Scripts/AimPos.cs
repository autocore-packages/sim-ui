#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class AimPos : MonoBehaviour
    {
        public int index
        {
            get
            {
                return transform.GetSiblingIndex() - 1;
            }
        }
        public Vector3 Value;
        public InputField inputField_X;
        public InputField inputField_Y;
        public Button btn_Delete;
        void Start()
        {
            if (!PanelInspector.Instance.ListAimPos.Contains(this))
            {
                PanelInspector.Instance.ListAimPos.Add(this);
            }
            inputField_X.onEndEdit.AddListener(
                (string value) =>
                {
                    if (float.TryParse(value, out float num))
                    {
                        Value.x = num;
                        SetInspector();
                    }
                });
            inputField_Y.onEndEdit.AddListener(
                (string value) =>
                {
                    if (float.TryParse(value, out float num))
                    {
                        Value.z = num; 
                        SetInspector();
                    }
                });
        }
        public void Init(Vector3 pos)
        {
            Value = pos;
            inputField_X.text = Value.x.ToString();
            inputField_Y.text = Value.z.ToString();
            btn_Delete.onClick.RemoveAllListeners();
            btn_Delete.onClick.AddListener(() =>
            {
                PanelInspector.Instance.elementAttbutes.humanAtt.aimList.RemoveAt(index);
                PanelInspector.Instance.ElementUpdate.Invoke(PanelInspector.Instance.elementAttbutes);
                Destroy(gameObject);
            });
        }
        public void SetInspector()
        {
            PanelInspector.Instance.elementAttbutes.humanAtt.aimList[index] = Value;
            PanelInspector.Instance.ElementUpdate.Invoke(PanelInspector.Instance.elementAttbutes);
        }

        private void OnDestroy()
        {
            Destroy(gameObject);
        }
    }

}

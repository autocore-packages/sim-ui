#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion

using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.simai;

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
        public Vec3 Value;
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
                        Value.X = num;
                        SetInspector();
                    }
                });
            inputField_Y.onEndEdit.AddListener(
                (string value) =>
                {
                    if (float.TryParse(value, out float num))
                    {
                        Value.Z = num; 
                        SetInspector();
                    }
                });
        }
        public void Init(Vec3 pos)
        {
            Value = pos;
            inputField_X.text = Value.X.ToString();
            inputField_Y.text = Value.Z.ToString();
            btn_Delete.onClick.RemoveAllListeners();
            btn_Delete.onClick.AddListener(() =>
            {
                PanelInspector.Instance.elementAttbutes.PosArray.RemoveAt(index);
                PanelInspector.Instance.UpdateSelectedElementAttbutes();
            });
        }
        public void SetInspector()
        {
            PanelInspector.Instance.elementAttbutes.PosArray[index] = Value;
            PanelInspector.Instance.UpdateSelectedElementAttbutes();
        }

        private void OnDestroy()
        {
            Destroy(gameObject);
        }
    }

}

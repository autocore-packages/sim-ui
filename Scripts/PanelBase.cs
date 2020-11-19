using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.SimuUI
{
    public class PanelBase<T> : MonoBehaviour where T : PanelBase<T>, ISimuPanel 
    {
        public static T Instance
        {
            get;
            private set;
        }
        protected virtual void Awake()
        {
            Instance = (T)this;
        }
        protected virtual void Start()
        {
            SetPanelActive(isActive);
        }
        public bool isActive;
        public virtual void SwitchPanelActive()
        {
            isActive = !isActive;
            SetPanelActive(isActive);
        }
        public bool IsPanelActive()
        {
            return isActive;
        }
        public virtual void SetPanelActive(bool value)
        {
            isActive = value;
            if (isActive)
            {
                MainUI.Instance.AddPanel((ISimuPanel)this);
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                MainUI.Instance.RemovePanel((ISimuPanel)this);
                transform.localScale = Vector3.zero;
            }
        }
    }

}

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    [RequireComponent(typeof(Button))]
    public class ToolsMenuButton : MonoBehaviour
    {
        public Button m_button;
        public GameObject targetGO;
        public ISimuPanel panel;
        public bool IsOpen
        {
            get
            {
                if (panel == null)
                {
                    return panel.IsPanelActive();
                }
                else
                {
                    return targetGO.activeSelf;
                }
            }
        }
        void Start()
        {
            m_button = GetComponent<Button>();
            m_button.onClick.AddListener(() =>
            {
                PanelTools.Instance.CloseAllMenu();
                SwitchMenuActive();
            });
            if (targetGO == null) targetGO = transform.GetChild(1).gameObject;
            var p= targetGO.GetComponent<ISimuPanel>();
            if (p != null) panel = p;
        }
        public void SetMenuActive(bool isActive)
        {
            if (panel != null)
            {
                panel.SetPanelActive(isActive);
            }
            else
            {
                targetGO.SetActive(isActive);
            }
        }
        public void SwitchMenuActive()
        {
            if (panel != null)
            {
                panel.SetPanelActive(!IsOpen);
            }
            else
            {
                targetGO.SetActive(!IsOpen);
            }
        }

    }
}

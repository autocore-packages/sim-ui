
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class PanelTools : PanelBase<PanelTools>, ISimuPanel
    {
        #region Buttons
        public Button button_resetAll;
        public Button button_resetEgo;
        public Button button_addNPC;
        public Button button_addPed;
        public Button button_addObs;
        public Button button_RemoveAll;
        #endregion
        public Transform transform_menu;
        private ToolsMenuButton[] menuButtons;
        public ToolsMenuButton[] MenuButtons
        {
            get
            {
                if(menuButtons==null) menuButtons= GetComponentsInChildren<ToolsMenuButton>();
                return menuButtons;
            }
        }
        private ToolsMenuButton menuButtonSelected = null;
        public ToolsMenuButton MenuButtonSelected
        {
            get
            {
                return menuButtonSelected;
            }
            set
            {
                menuButtonSelected = value;
                if (menuButtonSelected != null)
                {
                    OpenSeletedMenu();
                }
            }
        }
        void Start()
        {
            button_resetAll = MenuButtons[0].transform.GetChild(1).GetChild(0).GetComponent<Button>();
            button_resetEgo = MenuButtons[0].transform.GetChild(1).GetChild(1).GetComponent<Button>();
            button_addNPC = MenuButtons[1].transform.GetChild(1).GetChild(0).GetComponent<Button>();
            button_addPed = MenuButtons[1].transform.GetChild(1).GetChild(1).GetComponent<Button>();
            button_addObs = MenuButtons[1].transform.GetChild(1).GetChild(2).GetComponent<Button>();
            CloseAllMenu();
        }
        public void OpenSeletedMenu()
        {
            foreach (ToolsMenuButton button in MenuButtons)
            {
                button.SetMenuActive(button == MenuButtonSelected && !menuButtonSelected.isOpen);
            }
        }
        public void CloseAllMenu()
        {
            foreach (ToolsMenuButton button in MenuButtons)
            {
                button.SetMenuActive(false);
            }
        }
        public void OpenGitURL()
        {
            Application.OpenURL("https://github.com/autocore-ai/autocore_pcu_doc/blob/master/docs/Simulation_autoware.md");
        }
        
        public override void SetPanelActive(bool value)
        {
            return;
        }
    }
}

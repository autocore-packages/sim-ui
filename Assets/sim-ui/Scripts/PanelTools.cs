using UnityEngine;

namespace Assets.Scripts.SimuUI
{
    public class PanelTools : PanelBase<PanelTools>, ISimuPanel
    {
        #region Buttons
        public ToolsMenuButton button_resetAll;
        public ToolsMenuButton button_resetEgo;
        public ToolsMenuButton button_addNPC;
        public ToolsMenuButton button_addPed;
        public ToolsMenuButton button_addObs;
        public ToolsMenuButton button_RemoveAll;
        #endregion
        public Transform transform_menu;
        public ToolsMenuButton[] menuButtons;
        void Start()
        {
            menuButtons = GetComponentsInChildren<ToolsMenuButton>();
            button_resetAll = menuButtons[0].transform.GetChild(1).GetChild(0).GetComponent<ToolsMenuButton>();
            button_resetEgo = menuButtons[0].transform.GetChild(1).GetChild(1).GetComponent<ToolsMenuButton>();
            button_addNPC = menuButtons[1].transform.GetChild(1).GetChild(0).GetComponent<ToolsMenuButton>();
            button_addPed = menuButtons[1].transform.GetChild(1).GetChild(1).GetComponent<ToolsMenuButton>();
            button_addObs = menuButtons[1].transform.GetChild(1).GetChild(2).GetComponent<ToolsMenuButton>();
            CloseAllMenu();
        }
        public void CloseAllMenu()
        {
            foreach (ToolsMenuButton button in menuButtons)
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

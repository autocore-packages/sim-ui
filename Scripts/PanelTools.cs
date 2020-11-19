using UnityEngine;
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
        public ToolsMenuButton[] menuButtons;
        protected override void Start()
        {
            base.Start();
            menuButtons = GetComponentsInChildren<ToolsMenuButton>();
            button_resetAll = menuButtons[0].transform.GetChild(1).GetChild(0).GetComponent<Button>();
            button_resetEgo = menuButtons[0].transform.GetChild(1).GetChild(1).GetComponent<Button>();
            button_addNPC = menuButtons[1].transform.GetChild(1).GetChild(0).GetComponent<Button>();
            button_addPed = menuButtons[1].transform.GetChild(1).GetChild(1).GetComponent<Button>();
            button_addObs = menuButtons[1].transform.GetChild(1).GetChild(2).GetComponent<Button>();
            CloseAllMenu();
        }
        public void CloseAllMenu(ToolsMenuButton menubutton=null)
        {
            foreach (ToolsMenuButton button in menuButtons)
            {
                if (menubutton == button) button.SwitchMenuActive();
                else button.SetMenuActive(false);
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

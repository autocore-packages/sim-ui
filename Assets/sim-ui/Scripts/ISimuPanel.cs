using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SimuUI
{
    public interface ISimuPanel
    {
        bool IsPanelActive();
        void SetPanelActive(bool active);
    }
}

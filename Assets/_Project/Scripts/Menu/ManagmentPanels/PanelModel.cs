using UniRx;
using UnityEngine;

namespace Assets._Project.Scripts.Menu.ManagmentPanels
{
    public class PanelModel
    {
        public ReactiveProperty<GameObject> PreviousPanel { get; set; }
        public ReactiveProperty<GameObject> CurrentPanel { get; set; }

        public PanelModel()
        {
            PreviousPanel = new ReactiveProperty<GameObject>();
            CurrentPanel = new ReactiveProperty<GameObject>();
        }
    }
}
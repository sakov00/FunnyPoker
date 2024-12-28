using UniRx;
using UnityEngine;

namespace _Project.Scripts.Menu.ManagmentPanels
{
    public class PanelModel
    {
        public PanelModel()
        {
            PreviousPanel = new ReactiveProperty<GameObject>();
            CurrentPanel = new ReactiveProperty<GameObject>();
        }

        public ReactiveProperty<GameObject> PreviousPanel { get; set; }
        public ReactiveProperty<GameObject> CurrentPanel { get; set; }
    }
}
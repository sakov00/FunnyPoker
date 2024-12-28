using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Menu.ManagmentPanels
{
    public class PanelView : MonoBehaviour
    {
        [SerializeField] private Button networkGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button previousPanelButton;

        public IObservable<Unit> OnShowNetworkGamePanel => networkGameButton.OnClickAsObservable();
        public IObservable<Unit> OnShowSettingsPanel => settingsButton.OnClickAsObservable();
        public IObservable<Unit> OnShowPreviousPanel => previousPanelButton.OnClickAsObservable();
    }
}
using Assets._Project.Scripts.Menu.Enums;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets._Project.Scripts.Menu.ManagmentPanels
{
    public class PanelPresenter : MonoBehaviour
    {
        [Inject] private PanelModel model;
        [Inject] private PanelView view;

        [field: SerializeField] public GameObject MainMenuPanel { get; private set; }
        [field: SerializeField] public GameObject NetworkPanel { get; private set; }
        [field: SerializeField] public GameObject SettingPanel { get; private set; }
        [field: SerializeField] public GameObject CurrentRoomPanel { get; private set; }
        [field: SerializeField] public GameObject ConstantPanel { get; private set; }

        private void Start()
        {
            model.CurrentPanel.Subscribe(panel => SetCurrentPanel(panel)).AddTo(this);

            view.OnShowPreviousPanel
                .Subscribe(_ => ChangePanel(model.PreviousPanel.Value))
                .AddTo(this);

            view.OnShowNetworkGamePanel
                .Subscribe(_ => ChangePanel(NetworkPanel))
                .AddTo(this);

            view.OnShowSettingsPanel
                .Subscribe(_ => ChangePanel(SettingPanel))
                .AddTo(this);

            ChangePanel(MainMenuPanel);
        }

        public void ChangePanel(TypePanel typePanel)
        {
            switch (typePanel)
            {
                case TypePanel.MainMenu:
                    ChangePanel(MainMenuPanel);
                    break;
                case TypePanel.Network:
                    ChangePanel(NetworkPanel);
                    break;
                case TypePanel.Setting:
                    ChangePanel(SettingPanel);
                    break;
                case TypePanel.CurrentRoom:
                    ChangePanel(CurrentRoomPanel);
                    break;
                default:
                    break;
            }
        }

        private void ChangePanel(GameObject panel)
        {
            model.PreviousPanel.Value = model.CurrentPanel.Value;
            model.CurrentPanel.Value = panel;
        }

        private void SetCurrentPanel(GameObject panel)
        {
            if (panel == null)
                return;

            HideAllPanels();
            panel.SetActive(true);

            if (MainMenuPanel == panel || CurrentRoomPanel == panel)
            {
                ConstantPanel.SetActive(false);
            }
            else
            {
                ConstantPanel.SetActive(true);
            }
        }

        private void HideAllPanels()
        {
            MainMenuPanel.SetActive(false);
            NetworkPanel.SetActive(false);
            SettingPanel.SetActive(false);
            CurrentRoomPanel.SetActive(false);
        }
    }
}
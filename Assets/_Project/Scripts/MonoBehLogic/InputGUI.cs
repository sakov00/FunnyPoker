using Assets._Project.Scripts.MonoBehLogic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InputGUI : MonoBehaviour
{
    [Inject] private PlayersTurnService playersTurnService;
    public Button buttonNextTurn;

    public void Start()
    {
        buttonNextTurn.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        playersTurnService.NextPlayer();
    }
}

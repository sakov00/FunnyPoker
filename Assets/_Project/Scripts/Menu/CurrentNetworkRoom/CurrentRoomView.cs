using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Scripts.Menu.CurrentNetworkRoom
{
    public class CurrentRoomView : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button leaveRoomButton;
        [SerializeField] private TMP_InputField inputPlayerName;

        [field: SerializeField] public Transform ContentPlayers { get; private set; }

        public IObservable<Unit> OnStartGame => startGameButton.OnClickAsObservable();
        public IObservable<Unit> OnLeaveRoom => leaveRoomButton.OnClickAsObservable();
    }
}

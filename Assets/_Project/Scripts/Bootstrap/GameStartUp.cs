using _Project.Scripts.Services.Game;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class GameStartUp : MonoBehaviour
    {
        [Inject] private ServicePlaces _servicePlaces;

        public void StartGame()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            _servicePlaces.ActivateRandomPlace();
        }
    }
}
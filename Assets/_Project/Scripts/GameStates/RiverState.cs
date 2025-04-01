using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameStates
{
    public class RiverState : IGameState
    {
        [Inject] private GameData gameData;
        
        public void EnterState()
        {
            Debug.Log("Игроки делают ставки RiverState");
            
            if(!PhotonNetwork.IsMasterClient)
                return;

            var bigBlindPlace = gameData.AllPlayerPlaces.First(place => place.IsBigBlind);
            bigBlindPlace.Next.IsEnabled = true;
        }

        public void ExitState()
        {
            gameData.AllPlayerPlaces.ForEach(place => place.IsEnabled = false);
            
            Debug.Log("Ставки приняты RiverState");
        }
    }
}
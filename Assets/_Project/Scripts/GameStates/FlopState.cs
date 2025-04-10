using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using _Project.Scripts.MVP.Table;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameStates
{
    public class FlopState : IGameState
    {
        [Inject] private GameData gameData;
        
        public void EnterState()
        {
            Debug.Log("Игроки делают ставки FlopState");
            
            if(!PhotonNetwork.IsMasterClient)
                return;

            var bigBlindPlace = gameData.AllPlayerPlaces.First(place => place.IsBigBlind);
            bigBlindPlace.Next.IsEnabled = true;
            var table = gameData.TablePresenter;
            table.ShowCard(0);
            table.ShowCard(1);
            table.ShowCard(2);
        }

        public void ExitState()
        {
            gameData.AllPlayerPlaces.ForEach(place => place.IsEnabled = false);
            
            Debug.Log("Ставки приняты FlopState");
        }
    }
}
using _Project.Scripts.Interfaces;
using _Project.Scripts.Managers;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameStates
{
    public class DealingCardsState : IGameState
    {
        [Inject] private GameStateManager gameStateManager;
        [Inject] private CardsManager cardsManager;
        
        public void EnterState()
        {
            Debug.Log("Раздача карт...");
            
            if(!PhotonNetwork.IsMasterClient)
                return;
            
            cardsManager.DealTwoCardsToPlayers();
            gameStateManager.Next();
        }


        public void ExitState()
        {
            Debug.Log("Карты разданы.");
        }
    }
}
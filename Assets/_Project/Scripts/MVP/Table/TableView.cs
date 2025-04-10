using _Project.Scripts.GameLogic.Data;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MVP.Table
{
    public class TableView : MonoBehaviour
    {
        [Inject] private GameData gameData;
        
        public void ShowCard(int cardId)
        {
            var card = gameData.AllPlayingCards.Find(card => card.Id == cardId);
            card.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }
    }
}
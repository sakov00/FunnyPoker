using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Data;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.Services
{
    public class DeskService : MonoBehaviourPun
    {
        [Inject] private PlacesManager _placesManager;
        
        [SerializeField] private List<PlayingCard> playingCards = new ();
        [SerializeField] private Transform playingCardsParent;
        
        public void DealCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var place in _placesManager.AllPlayerPlaces)
                {
                    var card = GetRandomPlayingCard();
                    place.PlayingCards.Add(card);
                    
                    photonView.RPC(nameof(UpdateCardTransformRPC), RpcTarget.AllBuffered,
                        card.PhotonView.ViewID, place.CardsParent.ViewID, place.CardPoints[i].localPosition, place.CardPoints[i].localRotation);
                }
            }
        }

        [PunRPC]
        private void UpdateCardTransformRPC(int cardViewID, int parentViewID, Vector3 position, Quaternion rotation)
        {
            PhotonView cardPhotonView = PhotonView.Find(cardViewID);
            PhotonView parentPhotonView = PhotonView.Find(parentViewID);

            if (cardPhotonView != null && parentPhotonView != null)
            {
                var card = cardPhotonView.transform;
                card.SetParent(parentPhotonView.transform);
                card.localPosition = position;
                card.localRotation = rotation;
            }
        }

        private PlayingCard GetRandomPlayingCard()
        {
            var card = playingCards.FirstOrDefault();
            if (card != null)
            {
                playingCards.Remove(card);
            }
            return card;
        }
    }
}
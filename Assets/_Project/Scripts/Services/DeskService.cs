using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.MVP.PlayingCard;
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
        
        public void DealCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var place in _placesManager.AllPlayerPlaces)
                {
                    var card = GetRandomPlayingCard();
                    card.gameObject.SetActive(true);
                    place.Data.PlayingCards.Add(card);
                    
                    photonView.RPC(nameof(UpdateCardTransformRPC), RpcTarget.AllBuffered,
                        card.PhotonView.ViewID, place.Data.CardsParent.ViewID,
                        place.Data.CardPoints[i].localPosition, place.Data.CardPoints[i].localRotation);
                }
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
    }
}
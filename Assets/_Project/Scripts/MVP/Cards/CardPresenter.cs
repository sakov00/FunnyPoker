using System;
using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MVP.Cards
{
    public class CardPresenter : MonoBehaviourPunCallbacks
    {
        private readonly CompositeDisposable _disposable = new ();

        [SerializeField] private CardData data;
        [SerializeField] private CardSync sync;
        [SerializeField] private CardView view;
        
        public int Id => data.id;
        public PlayingCardRank Rank => data.rank;
        public PlayingCardSuit Suit => data.suit;

        private void OnValidate()
        {
            data ??= GetComponent<CardData>();
            sync ??= GetComponent<CardSync>();
            view ??= GetComponent<CardView>();
        }

        private void Start()
        {

        }

        public void UpdateCardPosition(Transform parent, Transform point)
            => view.UpdateCardPosition(parent, point);
        
        private void SyncProperty(string propertyName, object value)
        {
            Hashtable property = new() { { propertyName + data.id, value } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(property);
        }
        
        public override void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            LoadFromPhoton();
        }

        public void LoadFromPhoton()
        {
            var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

            // if (roomProps.TryGetValue(nameof(sync.ownerPlaceIdReactive) + data.id, out var owner))
            //     sync.ownerPlaceIdReactive.Value = (int)owner;
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}
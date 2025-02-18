using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.MVP.Cards;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.MVP.Place
{
    [Serializable]
    public class PlaceSync : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlaceData data;
        
        [SerializeField] private BoolReactiveProperty isFreeReactive = new ();
        [SerializeField] private BoolReactiveProperty isEnabledReactive = new ();
        [SerializeField] private IntReactiveProperty playerActorNumberReactive = new ();
        [SerializeField] private BoolReactiveProperty isSmallBlindReactive = new ();
        [SerializeField] private BoolReactiveProperty isBigBlindReactive = new ();
        [SerializeField] private ReactiveCollection<int> playingCardIdsInHand = new();
        
        private readonly CompositeDisposable _disposableProperties = new ();
        private readonly CompositeDisposable _disposablePlayingCardsInHand = new ();
        
        public IObservable<bool> IsFreeReactive  => isFreeReactive;
        public IObservable<bool> IsEnabledReactive => isEnabledReactive;
        public IObservable<int> PlayerActorNumberReactive   => playerActorNumberReactive;
        public IObservable<bool> IsSmallBlindReactive => isSmallBlindReactive;
        public IObservable<bool> IsBigBlindReactive => isBigBlindReactive;
        public IReactiveCollection<int> PlayingCardIdsInHand => playingCardIdsInHand;
        
        public bool IsFree
        {
            get => isFreeReactive.Value;
            set => isFreeReactive.Value = value;
        }
        
        public bool IsEnabled
        {
            get => isEnabledReactive.Value;
            set => isEnabledReactive.Value = value;
        }
        
        public int PlayerActorNumber
        {
            get => playerActorNumberReactive.Value;
            set => playerActorNumberReactive.Value = value;
        }
        
        public bool IsSmallBlind
        {
            get => isSmallBlindReactive.Value;
            set => isSmallBlindReactive.Value = value;
        }
        
        public bool IsBigBlind
        {
            get => isBigBlindReactive.Value;
            set => isBigBlindReactive.Value = value;
        }
        
        private void OnValidate()
        {
            if (data == null)
                data = GetComponent<PlaceData>();
        }
        
        public void Start()
        {
            isFreeReactive.Skip(1).Subscribe(value => SyncProperty(nameof(isFreeReactive), value)).AddTo(_disposableProperties);
            isEnabledReactive.Skip(1).Subscribe(value => SyncProperty(nameof(isEnabledReactive), value)).AddTo(_disposableProperties);
            playerActorNumberReactive.Skip(1).Subscribe(value => SyncProperty(nameof(playerActorNumberReactive), value)).AddTo(_disposableProperties);
            isSmallBlindReactive.Skip(1).Subscribe(value => SyncProperty(nameof(isSmallBlindReactive), value)).AddTo(_disposableProperties);
            isBigBlindReactive.Skip(1).Subscribe(value => SyncProperty(nameof(isBigBlindReactive), value)).AddTo(_disposableProperties);

            SubscribeToPlayingCardsInHand();
        }

        private void SyncProperty(string propertyName, object value)
        {
            Hashtable property = new() { { propertyName + data.Id, value } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(property);
        }
        
        public override void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            LoadFromPhoton();
        }

        public void LoadFromPhoton()
        {
            var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

            if (roomProps.TryGetValue(nameof(isFreeReactive) + data.Id, out var isFree))
                isFreeReactive.Value = (bool)isFree;

            if (roomProps.TryGetValue(nameof(isEnabledReactive) + data.Id, out var isEnabled))
                isEnabledReactive.Value = (bool)isEnabled;

            if (roomProps.TryGetValue(nameof(playerActorNumberReactive) + data.Id, out var actorNumber))
                playerActorNumberReactive.Value = (int)actorNumber;

            if (roomProps.TryGetValue(nameof(isSmallBlindReactive) + data.Id, out var isSmallBlind))
                isSmallBlindReactive.Value = (bool)isSmallBlind;

            if (roomProps.TryGetValue(nameof(isBigBlindReactive) + data.Id, out var isBigBlind))
                isBigBlindReactive.Value = (bool)isBigBlind;
        }
        
        public void UpdatePlayingCardsInHand()
        {
            photonView?.RPC("SyncPlayingCardsInHandRPC", RpcTarget.Others, playingCardIdsInHand.ToArray());
        }
        
        [PunRPC]
        private void SyncPlayingCardsInHandRPC(int[] receivedCards)
        {
            UnsubscribeFromPlayingCardsInHand();
            playingCardIdsInHand.Clear();
            playingCardIdsInHand.AddRange(receivedCards);
            SubscribeToPlayingCardsInHand();
        }
        
        private void SubscribeToPlayingCardsInHand()
        {
            playingCardIdsInHand.ObserveAdd().Subscribe(addEvent => UpdatePlayingCardsInHand()).AddTo(_disposablePlayingCardsInHand);
            playingCardIdsInHand.ObserveRemove().Subscribe(removeEvent => UpdatePlayingCardsInHand()).AddTo(_disposablePlayingCardsInHand);
        }
        
        private void UnsubscribeFromPlayingCardsInHand()
        {
            _disposablePlayingCardsInHand.Dispose();
        }
        
 
        
        public void OnDestroy()
        {
            _disposableProperties.Dispose();
            _disposablePlayingCardsInHand.Dispose();
        }
    }
}
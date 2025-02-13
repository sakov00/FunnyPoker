using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.MVP.Place
{
    [Serializable]
    public class PlaceSync : IDisposable
    {
        [SerializeField] private BoolReactiveProperty isFreeReactive = new ();
        [SerializeField] private BoolReactiveProperty isEnabledReactive = new ();
        [SerializeField] private IntReactiveProperty playerActorNumberReactive = new ();
        [SerializeField] private BoolReactiveProperty isSmallBlindReactive = new ();
        [SerializeField] private BoolReactiveProperty isBigBlindReactive = new ();
        
        private int _number;
        private readonly CompositeDisposable _disposables = new ();
        
        public IObservable<bool> IsFreeReactive  => isFreeReactive;
        public IObservable<bool> IsEnabledReactive => isEnabledReactive;
        public IObservable<int> PlayerActorNumberReactive   => playerActorNumberReactive;
        public IObservable<bool> IsSmallBlindReactive => isSmallBlindReactive;
        public IObservable<bool> IsBigBlindReactive => isBigBlindReactive;
        
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
        
        public void Init(int number)
        {
            _number = number;
                        
            isFreeReactive.Skip(1).Subscribe(value => SyncProperty(nameof(isFreeReactive), value)).AddTo(_disposables);
            isEnabledReactive.Skip(1).Subscribe(value => SyncProperty(nameof(isEnabledReactive), value)).AddTo(_disposables);
            playerActorNumberReactive.Skip(1).Subscribe(value => SyncProperty(nameof(playerActorNumberReactive), value)).AddTo(_disposables);
            isSmallBlindReactive.Skip(1).Subscribe(value => SyncProperty(nameof(isSmallBlindReactive), value)).AddTo(_disposables);
            isBigBlindReactive.Skip(1).Subscribe(value => SyncProperty(nameof(isBigBlindReactive), value)).AddTo(_disposables);
        }

        private void SyncProperty(string propertyName, object value)
        {
            Hashtable property = new() { { propertyName + _number, value } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(property);
        }

        public void LoadFromPhoton()
        {
            var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

            if (roomProps.TryGetValue(nameof(isFreeReactive) + _number, out var isFree))
                isFreeReactive.Value = (bool)isFree;

            if (roomProps.TryGetValue(nameof(isEnabledReactive) + _number, out var isEnabled))
                isEnabledReactive.Value = (bool)isEnabled;

            if (roomProps.TryGetValue(nameof(playerActorNumberReactive) + _number, out var actorNumber))
                playerActorNumberReactive.Value = (int)actorNumber;

            if (roomProps.TryGetValue(nameof(isSmallBlindReactive) + _number, out var isSmallBlind))
                isSmallBlindReactive.Value = (bool)isSmallBlind;

            if (roomProps.TryGetValue(nameof(isBigBlindReactive) + _number, out var isBigBlind))
                isBigBlindReactive.Value = (bool)isBigBlind;
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.MVP.Models.DataSync
{
    [Serializable]
    public class PlaceSync : IDisposable
    {
        [field:SerializeField] public BoolReactiveProperty IsFree { get; private set; }
        [field:SerializeField] public BoolReactiveProperty IsEnabled { get; private set; }
        [field:SerializeField] public IntReactiveProperty PlayerActorNumber { get; private set; }
        [field:SerializeField] public BoolReactiveProperty IsSmallBlind { get; private set; }
        [field:SerializeField] public BoolReactiveProperty IsBigBlind { get; private set; }
        
        private int _number;
        
        private readonly CompositeDisposable _disposables;
        
        public PlaceSync()
        {
            _disposables = new CompositeDisposable();

            IsFree = new BoolReactiveProperty();
            IsEnabled = new BoolReactiveProperty();
            PlayerActorNumber = new IntReactiveProperty();
            IsSmallBlind = new BoolReactiveProperty();
            IsBigBlind = new BoolReactiveProperty();
        }
        
        public void Init(int number)
        {
            _number = number;
                        
            IsFree.Skip(1).Subscribe(value => SyncProperty(nameof(IsFree), value)).AddTo(_disposables);
            IsEnabled.Skip(1).Subscribe(value => SyncProperty(nameof(IsEnabled), value)).AddTo(_disposables);
            PlayerActorNumber.Skip(1).Subscribe(value => SyncProperty(nameof(PlayerActorNumber), value)).AddTo(_disposables);
            IsSmallBlind.Skip(1).Subscribe(value => SyncProperty(nameof(IsSmallBlind), value)).AddTo(_disposables);
            IsBigBlind.Skip(1).Subscribe(value => SyncProperty(nameof(IsBigBlind), value)).AddTo(_disposables);
        }

        private void SyncProperty(string propertyName, object value)
        {
            Hashtable property = new() { { propertyName + _number, value } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(property);
        }

        public void LoadFromPhoton()
        {
            var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

            if (roomProps.TryGetValue(nameof(IsFree) + _number, out var isFree))
                IsFree.Value = (bool)isFree;

            if (roomProps.TryGetValue(nameof(IsEnabled) + _number, out var isEnabled))
                IsEnabled.Value = (bool)isEnabled;

            if (roomProps.TryGetValue(nameof(PlayerActorNumber) + _number, out var actorNumber))
                PlayerActorNumber.Value = (int)actorNumber;

            if (roomProps.TryGetValue(nameof(IsSmallBlind) + _number, out var isSmallBlind))
                IsSmallBlind.Value = (bool)isSmallBlind;

            if (roomProps.TryGetValue(nameof(IsBigBlind) + _number, out var isBigBlind))
                IsBigBlind.Value = (bool)isBigBlind;
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
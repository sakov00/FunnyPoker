using System;
using System.Collections.Generic;
using _Project.Scripts.MVP.Views;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.MVP.Place
{
    public class PlacePresenter : MonoBehaviourPunCallbacks
    {
        private readonly CompositeDisposable _disposables = new ();
        
        [field: SerializeField] public PlaceData Data { get; private set; }
        [field: SerializeField] public PlaceSync Sync { get; private set; }
        [field: SerializeField] private PlaceView View { get; set; }

        private void OnValidate()
        {
            if (Data == null)
                Data = GetComponent<PlaceData>();
            if (Sync == null)
                Sync = GetComponent<PlaceSync>();
            if (View == null)
                View = GetComponent<PlaceView>();
        }

        private void Start()
        {
            Sync.IsEnabledReactive.Subscribe(value => View.UpdateButton(value)).AddTo(_disposables);
        }
        
        public Transform GetLastOccupiedCardPlace()
        {
            return Data.CardPoints[Sync.PlayingCardIdsInHand.Count - 1];
        }
        
        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
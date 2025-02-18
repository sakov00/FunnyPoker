using System;
using _Project.Scripts.Enums;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.MVP.Cards
{
    public class CardPresenter : MonoBehaviourPun
    {
        private readonly CompositeDisposable _disposables = new ();
        
        [field: SerializeField] public CardData Data { get; private set; }
        [field: SerializeField] public CardSync Sync { get; private set; }
        [field: SerializeField] private CardView View { get; set; }

        private void OnValidate()
        {
            if (Data == null)
                Data = GetComponent<CardData>();
            if (Sync == null)
                Sync = GetComponent<CardSync>();
            if (View == null)
                View = GetComponent<CardView>();
        }

        private void Start()
        {
            Sync.OwnerPlaceIdReactive.Subscribe(value => View.UpdateCardOwner(value)).AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
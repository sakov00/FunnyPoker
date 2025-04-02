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
    public class CardPresenter : MonoBehaviourPun
    {
        [Inject] private SyncData syncData;
        
        private readonly CompositeDisposable disposable = new ();

        [SerializeField] private CardData data;
        [SerializeField] private CardSync sync;
        [SerializeField] private CardView view;

        public int Id => photonView.ViewID;
        public string ObjectName => nameof(CardPresenter) + Id;
        public PlayingCardRank Rank => data.rank;
        public PlayingCardSuit Suit => data.suit;
        
        public bool IsFree
        {
            get => sync.isFreeReactive.Value;
            set => sync.isFreeReactive.Value = value;
        }

        private void OnValidate()
        {
            data ??= GetComponent<CardData>();
            sync ??= GetComponent<CardSync>();
            view ??= GetComponent<CardView>();
        }
        
        private void Start()
        {
            sync.isFreeReactive
                .Skip(1)
                .Subscribe(value => syncData.SyncProperty(ObjectName, nameof(IsFree), value))
                .AddTo(disposable);
        }

        public void UpdateCardPosition(Transform parent, Transform point)
            => view.UpdateCardPosition(parent, point);

        private void OnDestroy()
        {
            disposable.Dispose();
        }
    }
}
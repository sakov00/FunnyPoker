using System;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.MVP.Place
{
    [Serializable]
    public class PlaceSync
    {
        [SerializeField] public bool isSyncData = true;
        
        [SerializeField] public BoolReactiveProperty isFreeReactive = new ();
        [SerializeField] public BoolReactiveProperty isEnabledReactive = new ();
        [SerializeField] public BoolReactiveProperty isFoldedReactive = new ();
        
        [SerializeField] public IntReactiveProperty moneyReactive = new ();
        [SerializeField] public IntReactiveProperty bettingMoneyReactive = new ();
        [SerializeField] public BoolReactiveProperty isSmallBlindReactive = new ();
        [SerializeField] public BoolReactiveProperty isBigBlindReactive = new ();
        
        [SerializeField] public IntReactiveProperty playerActorNumberReactive = new ();
        [SerializeField] public ReactiveCollection<int> handPlayingCards = new();
    }
}
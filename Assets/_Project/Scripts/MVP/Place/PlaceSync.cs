using System;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.MVP.Place
{
    [Serializable]
    public class PlaceSync : MonoBehaviourPunCallbacks
    {
        [SerializeField] public BoolReactiveProperty isFreeReactive = new ();
        [SerializeField] public BoolReactiveProperty isEnabledReactive = new ();
        [SerializeField] public IntReactiveProperty playerActorNumberReactive = new ();
        [SerializeField] public BoolReactiveProperty isSmallBlindReactive = new ();
        [SerializeField] public BoolReactiveProperty isBigBlindReactive = new ();
        [SerializeField] public ReactiveCollection<int> handPlayingCards = new();
    }
}
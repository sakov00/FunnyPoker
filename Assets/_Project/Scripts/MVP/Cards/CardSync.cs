using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.MVP.Cards
{
    [Serializable]
    public class CardSync
    {
        [SerializeField] public bool isSyncData = true;
        
        [SerializeField] public BoolReactiveProperty isFreeReactive = new(true);
    }
}
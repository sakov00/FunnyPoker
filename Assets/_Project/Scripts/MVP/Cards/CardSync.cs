using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.MVP.Cards
{
    public class CardSync : MonoBehaviourPunCallbacks
    {
        [SerializeField] public IntReactiveProperty ownerPlaceIdReactive = new ();
    }
}
using System;
using _Project.Scripts.GameLogic.PlayerInput;
using _Project.Scripts.Managers;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class GameStartUp : MonoBehaviour
    {
        [Inject] private PlayerInputHandler playerInputHandler;
        [Inject] private PlacesManager placesManager;
    }
}
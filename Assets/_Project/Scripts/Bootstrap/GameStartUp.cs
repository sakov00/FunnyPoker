using System;
using _Project.Scripts.GameLogic.InputHandlers;
using _Project.Scripts.Managers;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class GameStartUp : MonoBehaviour
    {
        [Inject] private InputHandler inputHandler;
        [Inject] private PlacesManager placesManager;
    }
}
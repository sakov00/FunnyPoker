using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using _Project.Scripts.Factories;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.MVP.Cards;
using _Project.Scripts.MVP.Place;
using _Project.Scripts.MVP.Table;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Zenject;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Services
{
    public class SyncData : IInitializable, IInRoomCallbacks
    {
        [Inject] private PlayerFactory playerFactory;
        [Inject] private GameData gameData;

        private bool isSyncData = true;

        public void Initialize()
        {
            DestroyEmptyPlaces();
            LoadFromPhoton();

            var playerPlaceInfo = gameData.AllPlayerPlaces.First(place => place.IsFree);
            playerPlaceInfo.PlayerActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            playerPlaceInfo.IsFree = false;
            playerFactory.CreatePlayer(playerPlaceInfo.PlayerPoint.position, playerPlaceInfo.PlayerPoint.rotation);
        }

        private void DestroyEmptyPlaces()
        {
            for (int i = gameData.AllPlayerPlaces.Count - 1; i >= 0; i--)
            {
                if (i >= PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    Object.Destroy(gameData.AllPlayerPlaces[i].gameObject);
                    gameData.AllPlayerPlaces.RemoveAt(i);
                }
            }
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            var playerPlaceInfo = gameData.AllPlayerPlaces.First(place => place.IsFree);
            playerPlaceInfo.PlayerActorNumber = newPlayer.ActorNumber;
            playerPlaceInfo.IsFree = false;
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            var placeInfo = gameData.AllPlayerPlaces.First(place => place.PlayerActorNumber == otherPlayer.ActorNumber);
            placeInfo.PlayerActorNumber = 0;
            placeInfo.IsFree = true;
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
        }

        public void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            LoadFromPhoton(changedProps);
        }

        public void SyncProperty<T>(string objectName, string propertyName, T value)
        {
            if (!isSyncData)
                return;

            Hashtable property = new()
            {
                { objectName + propertyName, value }
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(property);
        }

        private void LoadFromPhoton(Hashtable properties = null)
        {
            isSyncData = false;

            properties ??= PhotonNetwork.CurrentRoom.CustomProperties;

            var hashtable = properties.First();
            var objAndProp = hashtable.Key.ToString();
            Match match = Regex.Match(objAndProp, @"^(\D*\d+)(\D+)$");

            string objectName = match.Groups[1].Value;
            string propertyName = match.Groups[2].Value;
            var value = hashtable.Value;

            if (objectName.Contains(nameof(PlacePresenter)))
            {
                var targetObject = gameData.AllPlayerPlaces.FirstOrDefault(x => x.ObjectName == objectName);
                ApplyPlaceData(targetObject, propertyName, value);
            }
            else if (objectName.Contains(nameof(CardPresenter)))
            {
                var targetObject = gameData.AllPlayingCards.FirstOrDefault(x => x.ObjectName == objectName);
                ApplyCardData(targetObject, propertyName, value);
            }
            else if (objectName.Contains(nameof(TablePresenter)))
            {
                var targetObject = gameData.TablePresenter;
                ApplyTableData(targetObject, propertyName, value);
            }

            isSyncData = true;
        }

        private void ApplyPlaceData(PlacePresenter target, string propertyName, object value)
        {
            var propertyActions = new Dictionary<string, Action>
            {
                { nameof(target.IsFree), () => target.IsFree = Convert.ToBoolean(value) },
                { nameof(target.IsEnabled), () => target.IsEnabled = Convert.ToBoolean(value) },
                { nameof(target.IsFolded), () => target.IsFolded = Convert.ToBoolean(value) },
                { nameof(target.PlayerActorNumber), () => target.PlayerActorNumber = Convert.ToInt32(value) },
                { nameof(target.Money), () => target.Money = Convert.ToInt32(value) },
                { nameof(target.BettingMoney), () => target.BettingMoney = Convert.ToInt32(value) },
                { nameof(target.IsSmallBlind), () => target.IsSmallBlind = Convert.ToBoolean(value) },
                { nameof(target.IsBigBlind), () => target.IsBigBlind = Convert.ToBoolean(value) }
            };

            propertyActions[propertyName].Invoke();
        }

        private void ApplyCardData(CardPresenter target, string propertyName, object value)
        {
            var propertyActions = new Dictionary<string, Action>
            {
                { nameof(target.IsFree), () => target.IsFree = Convert.ToBoolean(value) }
            };

            propertyActions[propertyName].Invoke();
        }

        private void ApplyTableData(TablePresenter target, string propertyName, object value)
        {
            var propertyActions = new Dictionary<string, Action>
            {
                { nameof(target.Bank), () => target.Bank = Convert.ToInt32(value) }
            };

            propertyActions[propertyName].Invoke();
        }
    }
}
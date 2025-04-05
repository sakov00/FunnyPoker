using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using _Project.Scripts.Bootstrap;
using _Project.Scripts.GameLogic.Data;
using _Project.Scripts.MVP.Cards;
using _Project.Scripts.MVP.Place;
using _Project.Scripts.MVP.Table;
using ExitGames.Client.Photon;
using Photon.Pun;
using Zenject;

namespace _Project.Scripts.Services
{
    public class DataSync : IInitializable, IDisposable
    {
        [Inject] private NetworkCallBacks networkCallBacks;
        [Inject] private GameData gameData;

        private bool isSyncData = true;
        private bool isGetInfo = true;

        public void Initialize()
        {
            networkCallBacks.PropertiesUpdated += PropertiesUpdate;
        }
        
        public void SyncProperty<T>(string objectName, string propertyName, T value)
        {
            if (!isSyncData)
                return;
            
            isGetInfo = false;
            
            Hashtable property = new()
            {
                { objectName + propertyName, value }
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(property);
        }

        private void PropertiesUpdate(Hashtable changedProps)
        {
            LoadFromPhoton(changedProps);
        }

        public void LoadFromPhoton(Hashtable properties = null)
        {
            if (!isGetInfo)
            {
                isGetInfo = true;
                return;
            }
            
            isSyncData = false;
            properties ??= PhotonNetwork.CurrentRoom.CustomProperties;

            foreach (var property in properties)
            {
                
            }

            properties.TryGetValue()

            var hashtable = properties.First();
            var objAndProp = hashtable.Key.ToString();
            Match match = Regex.Match(objAndProp, @"^(\D*\d+)(\D+)$");

            string objectName = match.Groups[1].Value;
            string propertyName = match.Groups[2].Value;
            var value = hashtable.Value;

            if (properties.ContainsKey(nameof(PlacePresenter)))
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
        
        public void Dispose()
        {
            networkCallBacks.PropertiesUpdated -= PropertiesUpdate;
        }
    }
}
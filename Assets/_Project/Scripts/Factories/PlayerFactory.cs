using Photon.Pun;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets._Project.Scripts.Factories
{
    public class PlayerFactory
    {
        private DiContainer _container;
        private AssetReference playerPrefabAssetRef;

        public PlayerFactory(DiContainer container, AssetReference playerPrefabAssetRef)
        {
            _container = container;
            this.playerPrefabAssetRef = playerPrefabAssetRef;
        }

        public void CreatePlayer(Vector3 position, Quaternion rotation)
        {
            PhotonNetwork.Instantiate(playerPrefabAssetRef.AssetGUID, position, rotation);
        }
    }
}

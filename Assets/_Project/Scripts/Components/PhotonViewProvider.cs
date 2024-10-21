using Photon.Pun;
using UnityEngine;
using Voody.UniLeo.Lite;

namespace Assets._Project.Scripts.Components
{
    [RequireComponent(typeof(PhotonView))]
    public sealed class PhotonViewProvider : MonoProvider<PhotonViewComponent>
    {
        private void Awake()
        {
            value.PhotonView = GetComponent<PhotonView>();
        }
    }

    public struct PhotonViewComponent
    {
        public PhotonView PhotonView;
    }
}

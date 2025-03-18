using _Project.Scripts.Enums;
using _Project.Scripts.GameLogic.PlayerCanvases;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Zenject;

namespace _Project.Scripts.Managers
{
    public class CanvasesManager : IInRoomCallbacks
    {
        [Inject] private StartGameCanvas startGameCanvas;
        [Inject] private MainGameCanvas mainGameCanvas;
        [Inject] private EndGameCanvas endGameCanvas;
        
        private const string PlayerCanvasKey = "PlayerCanvas";
        
        [Inject]
        public void Initialize()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void ShowCanvas(PlayerCanvas playerCanvas, bool isNetwork = true)
        {
            switch (playerCanvas)
            {
                case PlayerCanvas.None: 
                    HideCurrentCanvas();
                    break;
                case PlayerCanvas.StartGame: 
                    startGameCanvas.gameObject.SetActive(true);
                    break;
                case PlayerCanvas.MainGame: 
                    mainGameCanvas.gameObject.SetActive(true);
                    break;
                case PlayerCanvas.EndGame: 
                    endGameCanvas.gameObject.SetActive(true);
                    break;
            }

            if (isNetwork)
            {
                Hashtable property = new() { { PlayerCanvasKey, (int)playerCanvas } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(property);
            }
        }
        
        private void HideCurrentCanvas()
        {
            startGameCanvas.gameObject.SetActive(false);
            mainGameCanvas.gameObject.SetActive(false);
            endGameCanvas.gameObject.SetActive(false);
        }
        
        public void OnPlayerEnteredRoom(Player newPlayer) { }
        public void OnPlayerLeftRoom(Player otherPlayer) { }
        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) { }

        public void OnRoomPropertiesUpdate(Hashtable changedProps)
        {
            if (PhotonNetwork.IsMasterClient)
                return;
            
            if (changedProps.TryGetValue(PlayerCanvasKey, out var playerCanvasKey))
            {
                PlayerCanvas canvas = (PlayerCanvas)(int)playerCanvasKey;
                ShowCanvas(canvas, false);
            }
        }
        public void OnMasterClientSwitched(Player newMasterClient) { }
        
    }
}
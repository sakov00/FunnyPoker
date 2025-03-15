using _Project.Scripts.Enums;
using _Project.Scripts.GameLogic.PlayerCanvases;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Zenject;

namespace _Project.Scripts.Services
{
    public class CanvasesService : IInRoomCallbacks
    {
        [Inject] private StartGameCanvas startGameCanvas;
        [Inject] private MainGameCanvas mainGameCanvas;
        [Inject] private EndGameCanvas endGameCanvas;

        public void ShowCanvas(PlayerCanvas playerCanvas)
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
            
            var props = new Hashtable();
            props["PlayerCanvas"] = (int)playerCanvas;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
        
        private void HideCurrentCanvas()
        {
            startGameCanvas.gameObject.SetActive(false);
            mainGameCanvas.gameObject.SetActive(false);
            endGameCanvas.gameObject.SetActive(false);
        }

        public void OnRoomPropertiesUpdate(Hashtable properties)
        {
            if (properties.ContainsKey("PlayerCanvas"))
            {
                PlayerCanvas canvas = (PlayerCanvas)(int)properties["PlayerCanvas"];
                ShowCanvas(canvas);
            }
        }
        
        public void OnPlayerEnteredRoom(Player newPlayer) { }
        public void OnPlayerLeftRoom(Player otherPlayer) { }
        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) { }
        public void OnMasterClientSwitched(Player newMasterClient) { }
    }
}
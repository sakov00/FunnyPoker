using _Project.Scripts.Services.Network;

namespace _Project.Scripts.Services.Game
{
    public class QueuePlayerController
    {
        private readonly PlayersInfoInRoomService _playersInfoInRoomService;
        
        private int _currentPlayerIndex;

        public QueuePlayerController(PlayersInfoInRoomService playersInfoInRoomService)
        {
            _playersInfoInRoomService = playersInfoInRoomService;
        }
        
        public void NextPlayer()
        {
            var playersActivity = _playersInfoInRoomService.PlayersActivity;
            
            if(_currentPlayerIndex + 1 > playersActivity.Count || _currentPlayerIndex < 0)
                return;

            var currentPlayer = playersActivity[_currentPlayerIndex];
            var nextPlayer = playersActivity[_currentPlayerIndex + 1];
            
            currentPlayer.DisableActivity();
            nextPlayer.EnableActivity();
        }
    }
}
using System.Collections.Generic;
using Voody.UniLeo.Lite;

namespace Assets._Project.Scripts.Components.Turn
{
    public sealed class TurnListProvider : MonoProvider<TurnListComponent> { }

    public struct TurnListComponent 
    {
        public List<int> PlayerTurnList;
        public int CurrentPlayerTurn;
    }
}

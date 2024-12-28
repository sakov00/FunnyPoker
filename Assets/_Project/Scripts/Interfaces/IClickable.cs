using System;

namespace _Project.Scripts.Interfaces
{
    public interface IClickable
    {
        void OnClick();
        
        event Action OnClicked;
    }
}
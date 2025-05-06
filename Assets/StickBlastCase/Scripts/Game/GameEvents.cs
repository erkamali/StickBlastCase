using System;

namespace StickBlastCase.Game
{
    public static class GameEvents
    {
        public static event Action OnGameStarted;
        
        public static void StartGame()  { OnGameStarted?.Invoke(); }
    }
}
using System;
using GameManagementModule;

namespace Models
{
    public class GameData
    {
        public Guid Id { get; } = Guid.NewGuid();

        public int CurrentScore = 0;
        
        public GameState GameState = GameState.Ready;
    }
}
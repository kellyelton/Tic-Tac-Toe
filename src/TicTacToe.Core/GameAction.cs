﻿namespace TicTacToe.Core
{
    public abstract class GameAction
    {
        internal Game Game { get; set; }
        internal IPlayer Player { get; set; }

        protected GameAction(Game state, IPlayer player)
        {
            this.Game = state;
            this.Player = player;
        }

        internal void Log(string message, params object[] args)
        {
            this.Game.GameLog.Add(string.Format(message,args));
        }

        public abstract void Do();
    }
}
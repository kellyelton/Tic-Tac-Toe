﻿namespace TicTacToe.Core.Actions
{
    using System;
    using System.Linq;

    public class OccupyGameAction : GameAction
    {
        internal int X { get; set; }
        internal int Y { get; set; }

        public OccupyGameAction(IPlayer player, Game state, int x, int y):base(state, player)
        {
            if (state == null) throw new ArgumentException("state cannot be null.", "state");
            if (player == null) throw new ArgumentException("player cannot be null.", "player");
            if (x >= this.Game.Board.BoardPositions.First().Length)
                throw new ArgumentException("x must be between 0 and " + this.Game.Board.BoardPositions.First().Length, "x");
            if (y >= this.Game.Board.BoardPositions.First().Length)
                throw new ArgumentException("y must be between 0 and " + this.Game.Board.BoardPositions.Length, "y");
            this.Player = player;
            this.X = x;
            this.Y = y;
        }

        public override void Do()
        {
            if (this.Game.Board.IsPositionOccupied(this.X, this.Y)) 
                throw new InvalidOperationException("Position " + this.X + ":" + this.Y + " is already occupied.");

            this.Game.Board.Occupy(this.Player, this.X, this.Y);
            this.Log("{0} Occupy's {1}:{2}",this.Player,this.X,this.Y);
        }
    }
}
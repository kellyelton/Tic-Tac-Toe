﻿using TicTacToe.Core.Players;

namespace TicTacToe.Core
{
    using System;
    using System.Linq;

    public class GameBoard
    {
        /// <summary>
        /// Positions and state of the board
        /// </summary>
        public IPlayer[][] BoardPositions { get; internal set; }

        public delegate void OccupyDelegate(IPlayer player, int x, int y);
		public event OccupyDelegate OnOccupy;

        internal static int[][] WinConditions;

        public const int Width = 3;
        public const int Height = 3;

        public GameBoard()
        {
            //Create three rows of three board positions
            BoardPositions = new IPlayer[Height][];
            for (var i = 0; i < Height; i++)
            {
                BoardPositions[i] = new IPlayer[Width];
            }

            if (WinConditions == null)
            {
                WinConditions = new int[8][];
                WinConditions[0] = new[] { 0, 1, 2 };
                WinConditions[1] = new[] { 3, 4, 5 };
                WinConditions[2] = new[] { 6, 7, 8 };
                WinConditions[3] = new[] { 0, 3, 6 };
                WinConditions[4] = new[] { 1, 4, 7 };
                WinConditions[5] = new[] { 2, 5, 8 };
                WinConditions[6] = new[] { 0, 4, 8 };
                WinConditions[7] = new[] { 2, 4, 6 };
            }
        }

        public bool IsPositionOccupied(int x, int y)
        {
            return BoardPositions[y][x] != null;
        }

		/// <summary>
		/// Is the flat array position occupied?
		/// </summary>
		/// <param name="idx">index</param>
		/// <returns>True if Occupied, otherwise False</returns>
        public bool IsPositionOccupied(int idx)
		{
		    int x, y;
			IndexToCoords(idx,out x, out y);
		    return IsPositionOccupied(x, y);
		}

        public IPlayer GetPosition(int idx)
        {
            int x, y;
            IndexToCoords(idx,out x, out y);
            return BoardPositions[y][x];
        }

        public void Occupy(IPlayer player, int x, int y)
        {
            if (IsPositionOccupied(x, y))
                throw new InvalidOperationException("Position " + x + ":" + y + " is already occupied.");
            BoardPositions[y][x] = player;
		    FireOnOccupy(player, x, y);
        }

		/// <summary>
		/// Occupy a position on a flat array version of the board
		/// </summary>
		/// <param name="player">Player occupying</param>
		/// <param name="idx">Index</param>
        public void Occupy(IPlayer player, int idx)
        {
		    int x, y;
			IndexToCoords(idx,out x, out y);
            Occupy(player,x, y);
        }

        /// <summary>
        /// Determines if the game board positions are all taken
        /// </summary>
        /// <returns>True if the board is full, otherwise false</returns>
        public bool IsFull()
        {
            return BoardPositions.Any(x => x.Any(y => y == null)) == false;
        }

		/// <summary>
		/// Determines if the game board is empty
		/// </summary>
		/// <returns>True if it's empty, otherwise False</returns>
        public bool IsEmpty()
		{
		    return BoardPositions.All(x => x.All(y => y == null));
		}

        /// <summary>
        /// Checks to see who the winner is for the current board state
        /// </summary>
        /// <returns><cref see="IPlayer"/> if there is a winner, or null if there is a tie or no winner yet.</returns>
        public IPlayer Winner()
        {
            foreach (var condition in WinConditions)
            {
                int x,y;
                IndexToCoords(condition[0],out x, out y);
                var p1 = BoardPositions[y][x];
                IndexToCoords(condition[1],out x, out y);
                var p2 = BoardPositions[y][x];
                IndexToCoords(condition[2],out x, out y);
                var p3 = BoardPositions[y][x];

                if (p1 == null || p2 == null || p3 == null) continue;

                if (p1 == p2 && p2 == p3)
                {
                    return p1;
                }
            }

            return null;
        }

        public int? WinLocation(IPlayer player)
        {
            var moveItems = new MoveItem[3];
            foreach (var condition in WinConditions)
            {
                int x, y;
                IndexToCoords(condition[0], out x, out y);
                moveItems[0] = new MoveItem(condition[0],BoardPositions[y][x]);
                IndexToCoords(condition[1], out x, out y);
                moveItems[1] = new MoveItem(condition[1],BoardPositions[y][x]);
                IndexToCoords(condition[2], out x, out y);
                moveItems[2] = new MoveItem(condition[2],BoardPositions[y][x]);

                if (moveItems.Count(m => m.Player == player) == 2 && moveItems.Count(m=>m.Player == null) == 1)
                {
                    var move = moveItems.First(m => m.Player == null);
                    return move.Move;
                }
            }

            return null;
        }

        internal void IndexToCoords(int idx, out int x, out int y)
        {
            x = idx % 3;
            y = idx / 3;
        }

        protected virtual void FireOnOccupy(IPlayer player, int x, int y)
        {
            var handler = OnOccupy;
            if (handler != null)
            {
                handler(player, x, y);
            }
        }
    }
}
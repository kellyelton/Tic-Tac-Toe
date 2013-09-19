﻿namespace TicTacToe.Core
{
    using System.Collections.Generic;
    using System.Linq;

    using TicTacToe.Core.Actions;
    using TicTacToe.Core.Utils;

    public class AiPlayer : IPlayer
    {
        public string Name { get; internal set; }

        /// <summary>
        /// Create a <see cref="AiPlayer"/>
        /// </summary>
        /// <param name="name">Name of the <see cref="AiPlayer"/></param>
        public AiPlayer(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets invoked when it's the <cref see="AiPlayer"/>'s turn.
        /// </summary>
        /// <param name="state">State of the game</param>
        public void OnTurn(Game state)
        {
            // If board is empty, then we're going first, so pick the middle one always
            if (state.Board.IsEmpty())
            {
				// For testing sakes, we're going to pick a random location
				//   for now
                var xx = RngRandom.Instance.Next(0, 3);
                var yy = RngRandom.Instance.Next(0, 3);
                var a = new OccupyGameAction(state, this, xx, yy);
				state.PerformAction(a);
                return;
            }

			// Otherwise, things get a bit sticky here.
			// I suppose we can brute force this sucker and see
			//    how long it takes first.

            var result = new PlayItOutResults(this, state.Board, state);
            var nextMove = result.NextMove();
            int x, y;
			state.Board.IndexToCoords(nextMove,out x,out y);
            var act = new OccupyGameAction(state, this, x, y);
            state.PerformAction(act);
        }

        public override string ToString()
        {
            return Name;
        }

        internal class PlayItOutResults
        {
			public int StartIndex { get; internal set; }
			public int Index { get; internal set; }
            public List<PlayItOutResults> Moves { get; internal set; }
			public GameWinStatus Status { get; internal set; }
			public IPlayer WinPlayer { get; internal set; }
			public IPlayer Me { get; internal set; }
			public Game Game { get; set; }

            public PlayItOutResults(IPlayer movePlayer, GameBoard board, Game game)
            {
                Me = movePlayer;
                Game = game;
                Index = -1;
                Moves = new List<PlayItOutResults>();
                for (var i = 0; i < 9; i++)
                {
                    if (!board.IsPositionOccupied(i))
                    {
                        var res = new PlayItOutResults(i,i, Me, movePlayer, board, game);
						Moves.Add(res);
                    }
                }
            }

            public PlayItOutResults(int startIdx, int idx, IPlayer me, IPlayer movePlayer, GameBoard board, Game game)
            {
                StartIndex = startIdx;
                Me = me;
                Game = game;
                Moves = new List<PlayItOutResults>();
                Index = idx;
                var bclone = CloneBoard(board);
                bclone.Occupy(movePlayer, idx);

				//Check if it's a win
                WinPlayer = bclone.Winner();
                if (WinPlayer != null)
                {
                    if (WinPlayer == Me) Status = GameWinStatus.Win;
                    return;
                }

				// Check if it's a tie
                if (bclone.IsFull())
                {
                    Status = GameWinStatus.Tie;
                    return;
                }

                // invert move player
                var newMovePlayer = movePlayer == Game.Player1 ? Game.Player2 : Game.Player1;

                for (var i = 0; i < 9; i++)
                {
                    if (!bclone.IsPositionOccupied(i))
                    {
                        var res = new PlayItOutResults(StartIndex,i, Me, newMovePlayer, bclone, Game);
                        Moves.Add(res);
                    }
                }
            }

            internal GameBoard CloneBoard(GameBoard board)
            {
                var bclone = new GameBoard();
                for (var row = 0; row < board.BoardPositions.Length; row++)
                {
                    for (var p = 0; p < board.BoardPositions[row].Length; p++)
                    {
                        bclone.BoardPositions[row][p] = board.BoardPositions[row][p];
                    }
                }
                return bclone;
            }

            internal int NextMove()
            {
                var results = GetEndResult();
                var winResults = results
					.OrderBy(x=>x.Status)
                    .GroupBy(x => x.StartIndex)
					.Select(x=>new
					           {
					               WinCount=x.Count(y=>y.Status == GameWinStatus.Win),
                                   TieCount = x.Count(y => y.Status == GameWinStatus.Tie),
                                   Results=x
					           })
                    .OrderByDescending(x=>x.TieCount)
					.ThenBy(x=>x.WinCount)
					.Where(x=>x.TieCount > 0)
                    .ToArray();
                return winResults.First().Results.First().StartIndex;
            }

            internal List<PlayItOutResults> GetEndResult()
            {
                return GetEndResult(this.Moves);
            }

            internal List<PlayItOutResults> GetEndResult(List<PlayItOutResults> moves)
            {
                var ret = new List<PlayItOutResults>();
                foreach (var m in moves)
                {
                    if (m.Status != GameWinStatus.None) ret.Add(m);
                    else
                    {
                        ret.AddRange(GetEndResult(m.Moves));
                    }
                }
                return ret;
            }
        }
    }
}
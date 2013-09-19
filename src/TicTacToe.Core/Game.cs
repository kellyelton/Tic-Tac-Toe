﻿namespace TicTacToe.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Common.Logging;

    using TicTacToe.Core.Actions;
    using TicTacToe.Core.Annotations;
    using TicTacToe.Core.Utils;

    public class Game : INotifyPropertyChanged
    {
        internal static ILog GameLogger = LogManager.GetLogger("GameLog");

        private IPlayer playerTurn;
        private GameStatus status;
        private GameWinStatus winStatus;
        private IPlayer winner;

        public IPlayer Player1 { get; internal set; }
        public IPlayer Player2 { get; internal set; }
        public GameBoard Board { get; internal set; }

        /// <summary>
        /// List of actions taken during the game.
        /// </summary>
        public List<GameAction> GameActions { get; internal set; }

        /// <summary>
        /// Log of events that happen during the game.
        /// </summary>
        public List<string> GameLog { get; internal set; }

        /// <summary>
        /// Gets the <see cref="IPlayer"/> who's turn it is.
        /// </summary>
        public IPlayer PlayerTurn
        {
            get
            {
                return this.playerTurn;
            }
            internal set
            {
                if (Equals(value, this.playerTurn))
                {
                    return;
                }
                this.playerTurn = value;
                this.OnPropertyChanged("PlayerTurn");
                this.playerTurn.OnTurn(this);
            }
        }

        public GameStatus Status
        {
            get
            {
                return this.status;
            }
            internal set
            {
                if (value == this.status)
                {
                    return;
                }
                this.status = value;
                this.OnPropertyChanged("Status");
            }
        }

        public GameWinStatus WinStatus
        {
            get
            {
                return this.winStatus;
            }
            internal set
            {
                if (value == this.winStatus)
                {
                    return;
                }
                this.winStatus = value;
                this.OnPropertyChanged("WinStatus");
            }
        }

        public IPlayer Winner
        {
            get
            {
                return this.winner;
            }
            internal set
            {
                if (Equals(value, this.winner))
                {
                    return;
                }
                this.winner = value;
                this.OnPropertyChanged("Winner");
            }
        }

        /// <summary>
        /// Create a new <see cref="Game"/> with 2 players
        /// </summary>
        /// <param name="player1">Player 1</param>
        /// <param name="player2">Player 2</param>
        /// <param name="gameBoard">Game board to use</param>
        public Game(IPlayer player1, IPlayer player2)
        {
            if (player1 == null) 
                throw new ArgumentException("player1 can't be null", "player1");
            if (player2 == null) 
                throw new ArgumentException("player2 can't be null", "player2");
            GameLog = new List<string>();
            GameActions = new List<GameAction>();
            Player1 = player1;
            Player2 = player2;
            Reset();
        }

        public void PerformAction(GameAction action)
        {
            if (action == null)
                throw new ArgumentOutOfRangeException("action", "action can not be null");
            if (Status != GameStatus.Running && (action is ResetGameAction) == false)
                throw new InvalidOperationException("Cannot do that action because the game is finished.");
            if ((action is ResetGameAction) == false && action.Player != PlayerTurn) 
                throw new InvalidOperationException("It's not " + action.Player.Name + "'s turn");
            GameActions.Add(action);
            action.Do();
            OnPropertyChanged("GameActions");
            OnPropertyChanged("GameLog");
            CheckGameState();
            if (Status == GameStatus.Running)
            {
                PlayerTurn = PlayerTurn == Player1 ? Player2 : Player1;
            }
        }

        /// <summary>
        /// Checks and updates the game state based on previous actions
        /// </summary>
        internal void CheckGameState()
        {
            var boardWinner = Board.Winner();
            if (boardWinner != null)
            {
                Status = GameStatus.Finished;
                WinStatus = GameWinStatus.Win;
                Winner = boardWinner;
            }
            else
            {
                if (!this.Board.IsFull())
                    return;
                this.Status = GameStatus.Finished;
                this.WinStatus = GameWinStatus.Tie;
                this.Winner = null;
            }
        }

        internal void Reset()
        {
            Status = GameStatus.Running;
            WinStatus = GameWinStatus.None;
            Winner = null;
            Board = new GameBoard();
            if (PlayerTurn == null)
            {
                // Randomly picks who the starting player will be
                // I think normally I would let the constructor of this class determine this
                //    , but for the sake of this project I'll just decide here.
                PlayerTurn = RngRandom.Instance.Next(0, 1) == 0 ? Player1 : Player2;
            }
            else
            {
                PlayerTurn = PlayerTurn == Player1 ? Player2 : Player1;
            }
            OnPropertyChanged("Board");
            OnPropertyChanged("GameActions");
            OnPropertyChanged("GameLog");
        }

        internal void ActionLog(string message)
        {
			GameLogger.Info(message);
            GameLog.Add(message);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public enum GameStatus
    {
        Running,
        Finished
    }

    public enum GameWinStatus
    {
        None,
        Win,
        Tie
    }
}

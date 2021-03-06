﻿using TicTacToe.Core.Players;

namespace TicTacToe.Core.Tests
{
    using System;

    using NUnit.Framework;

    public class GameBoardTests
    {
        [Test]
        public void Constructor_FillsClass()
        {
            var board = new GameBoard();
            Assert.AreEqual(3, board.BoardPositions.Length);
            foreach (var row in board.BoardPositions)
            {
				Assert.AreEqual(3, row.Length);
            }

            Assert.AreEqual(8, GameBoard.WinConditions.Length);
            foreach (var row in GameBoard.WinConditions)
            {
				Assert.AreEqual(3, row.Length);
            }
        }

        [Test]
        public void IsPositionOccupied_IsCorrect()
        {
            var board = new GameBoard();
            var p1 = new HumanPlayer("jim");

            Assert.False(board.IsPositionOccupied(1, 1));
            board.Occupy(p1, 1, 1);
            Assert.True(board.IsPositionOccupied(1, 1));
        }

        [Test]
        public void Occupy_SetsBoardPosition()
        {
            var board = new GameBoard();
            var p1 = new HumanPlayer("jim");

            Assert.False(board.BoardPositions[1][1] != null);
            board.Occupy(p1, 1, 1);
            Assert.True(board.BoardPositions[1][1] != null);
            Assert.AreEqual(p1, board.BoardPositions[1][1]);
        }

        [Test]
        public void Occupy_ThowsIfOccupied()
        {
            var board = new GameBoard();
            var p1 = new HumanPlayer("jim");

            board.Occupy(p1, 1, 1);
            Assert.Throws<InvalidOperationException>(() => board.Occupy(p1, 1, 1));
        }

        [Test]
        public void IsFull_IsCorrect()
        {
            var board = new GameBoard();
            var p1 = new HumanPlayer("jim");

            Assert.False(board.IsFull());

            board.BoardPositions[0][0] = p1;
            board.BoardPositions[1][0] = p1;
            Assert.False(board.IsFull());
            board.BoardPositions[2][0] = p1;
            board.BoardPositions[0][1] = p1;
            Assert.False(board.IsFull());
            board.BoardPositions[1][1] = p1;
            board.BoardPositions[2][1] = p1;
            Assert.False(board.IsFull());
            board.BoardPositions[0][2] = p1;
            board.BoardPositions[1][2] = p1;
            Assert.False(board.IsFull());
            board.BoardPositions[2][2] = p1;

            Assert.True(board.IsFull());
        }

        [Test]
        public void IsEmpty_Correct()
        {
            var p1 = new HumanPlayer("jim");

            for (var i = 0; i < 9; i++)
            {
                var board = new GameBoard();
                Assert.True(board.IsEmpty());

                int x, y;
				board.IndexToCoords(i,out x, out y);
				board.Occupy(p1,x,y);
                Assert.False(board.IsEmpty());
            }


        }

        [Test]
        public void IndexToCoords_IsCorrect()
        {
            var board = new GameBoard();
            int x;
            int y;
			board.IndexToCoords(0,out x,out y);
            Assert.AreEqual(0, x);
            Assert.AreEqual(0, y);
			board.IndexToCoords(1,out x,out y);
            Assert.AreEqual(1, x);
            Assert.AreEqual(0, y);
			board.IndexToCoords(2,out x,out y);
            Assert.AreEqual(2, x);
            Assert.AreEqual(0, y);
			board.IndexToCoords(3,out x,out y);
            Assert.AreEqual(0, x);
            Assert.AreEqual(1, y);
			board.IndexToCoords(4,out x,out y);
            Assert.AreEqual(1, x);
            Assert.AreEqual(1, y);
			board.IndexToCoords(5,out x,out y);
            Assert.AreEqual(2, x);
            Assert.AreEqual(1, y);
			board.IndexToCoords(6,out x,out y);
            Assert.AreEqual(0, x);
            Assert.AreEqual(2, y);
			board.IndexToCoords(7,out x,out y);
            Assert.AreEqual(1, x);
            Assert.AreEqual(2, y);
			board.IndexToCoords(8,out x,out y);
            Assert.AreEqual(2, x);
            Assert.AreEqual(2, y);
        }

        [Test]
        public void Winner_IsCorrect()
        {
// ReSharper disable once JoinDeclarationAndInitializer
            GameBoard board;
            var p1 = new HumanPlayer("jim");
            
            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[0][0] = p1;
            board.BoardPositions[0][1] = p1;
            board.BoardPositions[0][2] = p1;
            Assert.AreEqual(p1, board.Winner());

            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[1][0] = p1;
            board.BoardPositions[1][1] = p1;
            board.BoardPositions[1][2] = p1;
            Assert.AreEqual(p1, board.Winner());

            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[2][0] = p1;
            board.BoardPositions[2][1] = p1;
            board.BoardPositions[2][2] = p1;
            Assert.AreEqual(p1, board.Winner());

            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[0][0] = p1;
            board.BoardPositions[1][0] = p1;
            board.BoardPositions[2][0] = p1;
            Assert.AreEqual(p1, board.Winner());

            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[0][1] = p1;
            board.BoardPositions[1][1] = p1;
            board.BoardPositions[2][1] = p1;
            Assert.AreEqual(p1, board.Winner());

            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[0][2] = p1;
            board.BoardPositions[1][2] = p1;
            board.BoardPositions[2][2] = p1;
            Assert.AreEqual(p1, board.Winner());

            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[0][0] = p1;
            board.BoardPositions[1][1] = p1;
            board.BoardPositions[2][2] = p1;
            Assert.AreEqual(p1, board.Winner());

            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[0][2] = p1;
            board.BoardPositions[1][1] = p1;
            board.BoardPositions[2][0] = p1;
            Assert.AreEqual(p1, board.Winner());

			// Make sure doesn't work if one of the spots is occupied by someone else
            var p2 = new HumanPlayer("tim");

            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[0][0] = p2;
            board.BoardPositions[0][1] = p1;
            board.BoardPositions[0][2] = p1;
            Assert.Null(board.Winner());

            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[0][0] = p1;
            board.BoardPositions[0][1] = p2;
            board.BoardPositions[0][2] = p1;
            Assert.Null(board.Winner());

            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[0][0] = p1;
            board.BoardPositions[0][1] = p1;
            board.BoardPositions[0][2] = p2;
            Assert.Null(board.Winner());

			// Make sure doesn't work if one of the spots is not occupied
            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[0][0] = null;
            board.BoardPositions[0][1] = p1;
            board.BoardPositions[0][2] = p1;
            Assert.Null(board.Winner());

            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[0][0] = p1;
            board.BoardPositions[0][1] = null;
            board.BoardPositions[0][2] = p1;
            Assert.Null(board.Winner());

            board = new GameBoard();
			Assert.Null(board.Winner());
            board.BoardPositions[0][0] = p1;
            board.BoardPositions[0][1] = p1;
            board.BoardPositions[0][2] = null;
            Assert.Null(board.Winner());
        }
    }
}
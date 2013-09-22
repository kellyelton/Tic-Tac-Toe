﻿namespace TicTacToe.Core.Tests
{
    using System;

    using FakeItEasy;

    using NUnit.Framework;

    using TicTacToe.Core.Actions;

    public class GameActionTests
    {
        [Test]
        public void Constructor_SetsClass()
        {
            var p1 = new HumanPlayer("jim");
            var p2 = new HumanPlayer("tim");
            using (var game = new Game(p1, p2))
            {

                var action = A.Fake<GameAction>(x => x.WithArgumentsForConstructor(new object[] { game, p1 }));

                Assert.AreEqual(p1, action.Player);
                Assert.AreEqual(game, action.Game);
            }
        }

        [Test]
        public void Constructor_Constraints()
        {
            var p1 = new HumanPlayer("jim");
            var p2 = new HumanPlayer("tim");
            using (var game = new Game(p1, p2))
            {
                Assert.Throws<ArgumentException>(() => new ResetGameAction(null, p1));
                Assert.Throws<ArgumentException>(() => new ResetGameAction(game, null));
            }
        }

        [Test]
        public void Log_AddsToGameLog()
        {
            var p1 = new HumanPlayer("jim");
            var p2 = new HumanPlayer("tim");
            using (var game = new Game(p1, p2))
            {
                var action = A.Fake<GameAction>(x => x.WithArgumentsForConstructor(new object[] { game, p1 }));

                Assert.AreEqual(0, game.GameLog.Count);
                action.Log("message");
                Assert.AreEqual(1, game.GameLog.Count);
            }
        }

        [Test]
        public void Log_FormatsMessage()
        {
            var p1 = new HumanPlayer("jim");
            var p2 = new HumanPlayer("tim");
            using (var game = new Game(p1, p2))
            {
                var action = A.Fake<GameAction>(x => x.WithArgumentsForConstructor(new object[] { game, p1 }));

                action.Log("message {0}", "hi");
                Assert.AreEqual("message hi", game.GameLog[0]);
            }
        }
    }
}
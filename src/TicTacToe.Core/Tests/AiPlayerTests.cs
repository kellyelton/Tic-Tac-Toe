﻿namespace TicTacToe.Core.Tests
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    using NUnit.Framework;

    public class AiPlayerTests
    {
        [Test]
        public void Constructor_FillsClass()
        {
            var p = new AiPlayer("jim",false);
            Assert.AreEqual("jim", p.Name);
        }

        [Test]
        public void ToString_ReturnsName()
        {
            var p = new AiPlayer("jim", false);
            Assert.AreEqual("jim", p.ToString());
        }

        [Test]
        public void PlayAGame()
        {
            //var p1 = new AiPlayer("tim");
            //var p2 = new AiPlayer("jim");
            ////return;
            //for (var i = 0; i < 10; i++)
            //{
            //    var time = new Stopwatch();
            //    time.Start();
            //    var game = new Game(p1, p2);
            //    game.Start();
            //    time.Stop();
            //    Console.WriteLine("{0} - {1}",game.WinStatus,game.Winner);
            //    Console.WriteLine("Time: {0}", time.ElapsedMilliseconds);
            //}
        }
    }
}
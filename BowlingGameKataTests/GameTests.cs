using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BowlingGameKata;
using BowlingGameKata.Exceptions;

namespace BowlingGameUnitTest
{
    [TestClass]
    public class GameTests
    {
        private Game game;

        [TestInitialize]
        public void SetUp()
        {
            game = new Game();
        } 

        [TestMethod]
        [ExpectedException(typeof(RollInputIsOutOfBoundsException))]
        public void ExceptionIsThrownWhenPinNumberIsLessThanZero()
        {
            game.roll(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(RollInputIsOutOfBoundsException))]
        public void ExceptionIsThrownWhenPinNumberIsGreaterThanZero()
        {
            game.roll(11);
        }

        [TestMethod]
        [ExpectedException(typeof(RollInputIsOutOfBoundsException))]
        public void ExceptionIsThrownWhenSecondRollIsGreaterThanPinsLeft()
        {
            RollFrames(1, 4, 8);
        }

        [TestMethod]
        [ExpectedException(typeof(CurrentFrameIsGreaterThanMaxFramesException))]
        public void ExceptionIsThrownWhenCurrentFrameExceedsMaxFrames()
        {
            RollFrames(12, 4, 6);
        }

        [TestMethod]
        [ExpectedException(typeof(CurrentFrameIsGreaterThanMaxFramesException))]
        public void ExceptionIsThrownAfterNoSpareOrStrikeInFinalFrameForThirdRoll()
        {
            rollStrikes(9);
            RollFrames(1, 3, 3);
            game.roll(4);
        }

        [TestMethod]
        public void NoExceptionIsThrownWhenPinNumberIsWithinBounds()
        {
            for (var i = 0; i < 10; i++)
                RollFrames(1, i, 0);
        }

        [TestMethod]
        public void ScoreIsZeroWhenNoFramesHaveBeenRolled()
        {
            Assert.AreEqual(0, game.score());
        }

        [TestMethod]
        public void ScoreIsZeroWhenZeroGameIsRolled()
        {
            RollFrames(10, 0, 0);
            Assert.AreEqual(0, game.score());
        }

        [TestMethod]
        public void ScoreIsZeroWhenRollIsZero()
        {
            game.roll(0);
            Assert.AreEqual(0, game.score());
        }

        [TestMethod]
        public void ScoreIsFiveWhenRollIsFive()
        {
            game.roll(5);
            Assert.AreEqual(5, game.score());
        }

        [TestMethod]
        public void ScoreIsIncrementedProperlyAfterSpare()
        {
            RollFrames(1, 3, 7);
            game.roll(4);
            Assert.AreEqual(18, game.score());
        }

        [TestMethod]
        public void ScoreIsIncrementedProperlyAfterSpareThenStrike()
        {
            RollFrames(1, 3, 7);
            rollStrikes(1);
            Assert.AreEqual(30, game.score());
        }

        [TestMethod]
        public void ScoreIsIncrementedProperlyAfterStrikeThenSpare()
        {
            rollStrikes(1);
            RollFrames(1, 3, 7);
            Assert.AreEqual(30, game.score());
        }

        [TestMethod]
        public void ScoreIsIncrementedProperlyAfterStrike()
        {
            rollStrikes(1);
            RollFrames(1, 3, 4);
            Assert.AreEqual(24, game.score());
        }

        [TestMethod]
        public void ScoreIsIncrementedProperlyAfterTwoStrikes()
        {
            rollStrikes(2);
            RollFrames(1, 4, 6);
            Assert.AreEqual(54, game.score());
        }

        [TestMethod]
        public void ScoreIsIncrementedProperlyAfterSpareInFinalFrame()
        {
            rollStrikes(9);
            RollFrames(1, 4, 6);
            game.roll(4);
            Assert.AreEqual(268, game.score());
        }

        [TestMethod]
        public void SparesAndStrikes()
        {
            RollFrames(5, 3, 7);
            rollStrikes(7);
            Assert.AreEqual(222, game.score());
        }

        [TestMethod]
        public void AllSpares()
        {
            RollFrames(10, 3, 7);
            game.roll(3);
            Assert.AreEqual(130, game.score());
        }

        [TestMethod]
        public void BowlATimGame()
        {
            rollStrikes(12);
            Assert.AreEqual(300, game.score());
        }

        private void RollFrames(Int32 frames, Int32 firstRoll, Int32 secondRoll)
        {
            for (Int32 i = 0; i < frames; i++)
            {
                game.roll(firstRoll);
                game.roll(secondRoll);
            }
        }

        private void rollStrikes(Int32 strikes)
        {
            for (Int32 i = 0; i < strikes; i++)
                game.roll(10);
        }
    }
}

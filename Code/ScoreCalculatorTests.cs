using System;
using NUnit.Framework;

namespace BowlingGame
{
    [TestFixture]
    public class ScoreCalculatorTests
    {
        private ScoreCalculator _scoreCalculator;

        [SetUp]
        public void TestInitialize()
        {
            _scoreCalculator = new ScoreCalculator();
        }

        [Test]
        public void Can_create_game()
        {
            Assert.IsNotNull(_scoreCalculator);
        }

        [Test]
        public void GetScore_returns_0_without_rolling()
        {
            var score = _scoreCalculator.GetScore();

            Assert.AreEqual(0, score);
        }

        [TestCase(1, 1)]
        [TestCase(10, 1)]
        [TestCase(1, 9)]
        [TestCase(2, 20)]
        public void GetScore_returns_sum_of_knocked_down_pins_when_rolling_many_times(int knockedDownPins, int numberOfRolls)
        {
            RollMany(numberOfRolls, knockedDownPins);

            var score = _scoreCalculator.GetScore();

            Assert.AreEqual((knockedDownPins * numberOfRolls), score);
        }

        [Test]
        public void GetScore_returns_game_score_having_one_spare_frame()
        {
            _scoreCalculator.Roll(5);
            _scoreCalculator.Roll(5);
            RollMany(18, 1);

            var score = _scoreCalculator.GetScore();

            Assert.AreEqual(29, score);
        }

        [Test]
        public void GetScore_returns_150_rolling_spare_in_all_frames()
        {
            RollMany(21, 5);

            var score = _scoreCalculator.GetScore();

            Assert.AreEqual(150, score);
        }

        [Test]
        public void GetScore_returns_game_score_having_one_strike_frame()
        {
            _scoreCalculator.Roll(10);
            RollMany(18, 1);

            var score = _scoreCalculator.GetScore();

            Assert.AreEqual(30, score);
        }

        [Test]
        public void GetScore_returns_300_rolling_strike_in_all_frames()
        {
            RollMany(12, 10);

            var score = _scoreCalculator.GetScore();

            Assert.AreEqual(300, score);
        }

        [TestCase(-1)]
        [TestCase(-99)]
        [TestCase(11)]
        [TestCase(102)]
        public void Roll_throws_when_pins_are_out_of_range(int pins)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _scoreCalculator.Roll(pins));
        }

        [Test]
        public void GetScore_returns_correct_incomplete_game_score_of_14_when_rolled_2_pins_after_a_spare()
        {
            _scoreCalculator.Roll(0);
            _scoreCalculator.Roll(10);
            _scoreCalculator.Roll(2);

            var score = _scoreCalculator.GetScore();

            Assert.AreEqual(14, score);
        }
        [Test]
        public void GetScore_returns_correct_incomplete_game_score_of_14_when_rolled_0_and_2_pins_after_a_strike()
        {
            _scoreCalculator.Roll(10); 
            _scoreCalculator.Roll(0);
            _scoreCalculator.Roll(2);

            var score = _scoreCalculator.GetScore();

            Assert.AreEqual(14, score);
        }

        private void RollMany(int numberOfRolls, int numberOfKnockedDownPins)
        {
            for (var i = 0; i < numberOfRolls; i++)
                _scoreCalculator.Roll(numberOfKnockedDownPins);
        }
    }
}

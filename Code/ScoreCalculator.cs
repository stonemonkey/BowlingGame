using System;

namespace BowlingGame
{
    public class ScoreCalculator
    {
        private const int RolsPerFrame = 2;
        private const int MaxNumberOfRols = 21;
        private const int MaxNumberOfFrames = 10;
        private const int MaxNumberOfPinsKnockedDownPerFrame = 10;
        private const int FirstRollOfLastFrame = 18;

        private int _currentRoll;
        private int _firstRollOfCurrentFrame;
        private readonly int[] _pins = new int[MaxNumberOfRols];

        public int GetScore()
        {
            var score = 0;

            for (var frame = 0; frame < MaxNumberOfFrames; frame++)
                score += GetFrameScore(frame);

            return score;
        }

        private int GetFrameScore(int frame)
        {
            _firstRollOfCurrentFrame = (frame * RolsPerFrame);

            var score = GetFramePins();

            if (IsStrike()) 
                score += GetStrikeBonus();
            else if (IsSpare()) 
                score += GetSpareBonus();

            return score;
        }

        private int GetSpareBonus()
        {
            return _pins[_firstRollOfCurrentFrame + 2];
        }

        private bool IsSpare()
        {
            return AreAllKnockedDown(GetFramePins());
        }

        private int GetFramePins()
        {
            return (_pins[_firstRollOfCurrentFrame] + _pins[_firstRollOfCurrentFrame + 1]);
        }

        private int GetStrikeBonus()
        {
            var bonus = _pins[_firstRollOfCurrentFrame + 2];

            if (IsNotInLastFrame(_firstRollOfCurrentFrame))
                bonus += GetNormalFrameStrikeBonus();

            return bonus;
        }

        private int GetNormalFrameStrikeBonus()
        {
            var bonus = _pins[_firstRollOfCurrentFrame + 3];

            if (bonus == 0)
                bonus += _pins[_firstRollOfCurrentFrame + 4];
            
            return bonus;
        }

        private bool IsStrike()
        {
            return AreAllKnockedDown(GetFirstRollPins());
        }

        private int GetFirstRollPins()
        {
            return _pins[_firstRollOfCurrentFrame];
        }

        public void Roll(int pins)
        {
            if ((pins < 0) ||
                (pins > MaxNumberOfPinsKnockedDownPerFrame))
                throw new ArgumentOutOfRangeException();

            // TODO: I do not like next code here. I presume it can be refactored ... using a list instead of an array
            _pins[_currentRoll++] = pins;

            if (AreAllKnockedDown(pins) && IsNotInLastFrame(_currentRoll))
                _currentRoll++;
        }

        private static bool IsNotInLastFrame(int roll)
        {
            return (roll < FirstRollOfLastFrame);
        }

        private static bool AreAllKnockedDown(int pins)
        {
            return (pins == MaxNumberOfPinsKnockedDownPerFrame);
        }
    }
}
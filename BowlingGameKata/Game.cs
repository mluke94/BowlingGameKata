using System;
using BowlingGameKata.Exceptions;

namespace BowlingGameKata
{
    public class Game
    {
        private const Int32 MaxNormalFrames = 9;
        private const Int32 RollsPerFrame = 2;
        private const Int32 PinsPerFrame = 10;
        private Int32[,] frames = new Int32[MaxNormalFrames, RollsPerFrame];
        private Int32[] finalFrame = new Int32[3];
        private Int32 currentFrame = 0;
        private Int32 currentRoll = 0;
        private Int32 finalFrameRoll = 0;

        public void roll(Int32 pins)
        {
            if (currentFrame >= MaxNormalFrames + 1)
                throw new CurrentFrameIsGreaterThanMaxFramesException();

            if (currentFrame < MaxNormalFrames)
                rollNormalFrame(pins);
            else
                rollFinalFrame(pins);
        }

        public Int32 score()
        {
            var scoreTotal = 0;

            for (var scoreFrame = 0; scoreFrame < MaxNormalFrames; scoreFrame++)
                scoreTotal += frames[scoreFrame, 0] + frames[scoreFrame, 1]
                    + calculateSpareBonus(scoreFrame)
                    + calculateStrikeBonus(scoreFrame);

            foreach (var i in finalFrame)
                scoreTotal += i;

            return scoreTotal;
        }

        private void rollNormalFrame(Int32 pins)
        {
            if (rollIsWithinBounds(pins))
                throw new RollInputIsOutOfBoundsException();

            frames[currentFrame, currentRoll] = pins;

            if (isLastRollOfFrame() || isStrike(currentFrame))
            {
                currentRoll = 0;
                currentFrame++;
            }
            else
            {
                currentRoll++;
            }
        }

        private void rollFinalFrame(Int32 pins)
        {
            finalFrame[finalFrameRoll] = pins;
            finalFrameRoll++;

            if (finalFrameRoll == 3 || checkForThirdRoll())
                currentFrame++;
        }

        private Boolean checkForThirdRoll()
        {
            return finalFrameRoll == 2 && finalFrame[0] + finalFrame[1] < 10;
        }

        private Boolean rollIsWithinBounds(Int32 pins)
        {
            return pins < 0 || currentFrame < MaxNormalFrames && pins > PinsPerFrame - frames[currentFrame, 0];
        }

        private Boolean isLastRollOfFrame()
        {
            return currentRoll == RollsPerFrame - 1;
        }

        private Int32 calculateSpareBonus(Int32 frame)
        {
            if (!isStrike(frame) && isSpare(frame))
                return isSecondToLastFrame(frame) ? finalFrame[0] : frames[frame + 1, 0];

            return 0;
        }
        private Int32 calculateStrikeBonus(Int32 frame)
        {
            if (isStrike(frame))
                if (isSecondToLastFrame(frame))
                    return finalFrame[0] + finalFrame[1];
                else if (isThirdToLastFrame(frame))
                    return isStrike(frame + 1) ? 
                        frames[frame + 1, 0] + finalFrame[0] : 
                        frames[frame + 1, 0] + frames[frame + 1, 1];
                else
                    return isStrike(frame + 1) ?
                        frames[frame + 1, 0] + frames[frame + 2, 0] :
                        frames[frame + 1, 0] + frames[frame + 1, 1];

            return 0;
        }

        private Boolean isSpare(Int32 frame)
        {
            return frames[frame, 0] + frames[frame, 1] == PinsPerFrame;
        }


        private Boolean isSecondToLastFrame(Int32 frame)
        {
            return frame == MaxNormalFrames - 1;
        }

        private Boolean isThirdToLastFrame(Int32 frame)
        {
            return frame == MaxNormalFrames - 2;
        }

        private Boolean isStrike(Int32 frame)
        {
            return frames[frame, 0] == PinsPerFrame;
        }
    }
}

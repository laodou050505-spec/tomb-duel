using System;

namespace Guandan.Game
{
    public enum TreasureRoute
    {
        Steady,
        Risky,
    }

    public enum RiskOutcome
    {
        Double,
        PlusOne,
        Normal,
        MinusOne,
        Retreat,
    }

    public readonly struct TreasureMoveResult
    {
        public readonly int Team;
        public readonly TreasureRoute Route;
        public readonly RiskOutcome Outcome;
        public readonly int BaseSteps;
        public readonly int Delta;
        public readonly int Position;

        public TreasureMoveResult(int team, TreasureRoute route, RiskOutcome outcome, int baseSteps, int delta, int position)
        {
            Team = team;
            Route = route;
            Outcome = outcome;
            BaseSteps = baseSteps;
            Delta = delta;
            Position = position;
        }

        public string Label => Outcome switch
        {
            RiskOutcome.Double => "Flash of insight — advance doubled!",
            RiskOutcome.PlusOne => "Quick step — advance +1!",
            RiskOutcome.Normal => "Steady advance",
            RiskOutcome.MinusOne => "A stumble — advance −1",
            RiskOutcome.Retreat => "Trap triggered — retreat 1 tile",
            _ => "Advance",
        };
    }

    [Serializable]
    public sealed class TreasureRace
    {
        // The authored treasure room has one starting tile plus twelve destinations.
        public const int TrackLength = 12;

        private readonly int[] positions = new int[2];

        public int GetPosition(int team) => positions[Math.Clamp(team, 0, 1)];
        public bool IsComplete => positions[0] >= TrackLength || positions[1] >= TrackLength;
        public int WinningTeam => positions[0] >= TrackLength ? 0 : positions[1] >= TrackLength ? 1 : -1;

        public void Reset()
        {
            positions[0] = 0;
            positions[1] = 0;
        }

        public TreasureMoveResult Move(int team, int baseSteps, TreasureRoute route, double randomValue)
        {
            team = Math.Clamp(team, 0, 1);
            baseSteps = Math.Clamp(baseSteps, 0, 3);
            var outcome = RiskOutcome.Normal;
            var delta = baseSteps;

            if (route == TreasureRoute.Risky)
            {
                if (randomValue < 0.10)
                {
                    outcome = RiskOutcome.Double;
                    delta = baseSteps * 2;
                }
                else if (randomValue < 0.25)
                {
                    outcome = RiskOutcome.PlusOne;
                    delta = baseSteps + 1;
                }
                else if (randomValue < 0.80)
                {
                    outcome = RiskOutcome.Normal;
                    delta = baseSteps;
                }
                else if (randomValue < 0.92)
                {
                    outcome = RiskOutcome.MinusOne;
                    delta = Math.Max(0, baseSteps - 1);
                }
                else
                {
                    outcome = RiskOutcome.Retreat;
                    delta = -1;
                }
            }

            positions[team] = Math.Clamp(positions[team] + delta, 0, TrackLength);
            return new TreasureMoveResult(team, route, outcome, baseSteps, delta, positions[team]);
        }
    }
}

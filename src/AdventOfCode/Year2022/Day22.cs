namespace AdventOfCode.Year2022;

[Description("Monkey Map")]
public class Day22 : IPuzzle
{
    public object Part1(string input)
    {
        var data = ParseInput(input);

        var current = (Position: LocateStart(data), Direction: ((int dx, int dy))(1, 0));

        var state = StateAt(data, (11, 0));

        var instructions = Instructions(data).ToArray();

        current = instructions.Aggregate(current, (next, instruction) => instruction switch
        {
            Walk walk => WalkTo(data, next, walk.Steps),
            Rotate rotate => (next.Position, rotate.Apply(next.Direction)),
            _ => next
        });

        var facing = current.Direction switch
        {
            (1, 0) => 0,
            (0, 1) => 1,
            (-1, 0) => 2,
            (0, -1) => 3,
            _ => throw new ArgumentOutOfRangeException()
        };

        return (1000 * (current.Position.Y + 1)) + (4 * (current.Position.X + 1)) + facing;
    }

    public object Part2(string input)
    {
        return string.Empty;
    }

    private static ((int X, int Y) Position, (int dx, int dy) Direction) WalkTo((LineData[] map, string moveList) data, ((int X, int Y) Position, (int dx, int dy) Direction) current, int walkSteps)
    {
        (int dX, int dY) inverseDirection = (-current.Direction.dx, -current.Direction.dy);
        for (int i = 0; i < walkSteps; i++)
        {
            (int X, int Y) nextStep = (current.Position.X + current.Direction.dx, current.Position.Y + current.Direction.dy);
            State state = StateAt(data, nextStep);
            if (state == State.Void)
            {
                (int X, int Y) prevStep;
                do
                {
                    prevStep = nextStep;
                    nextStep = (nextStep.X + inverseDirection.dX, nextStep.Y + inverseDirection.dY);
                } while (StateAt(data, nextStep) != State.Void);

                if (StateAt(data, prevStep) == State.Rock)
                {
                    continue;
                }

                nextStep = prevStep;
            }

            if (state == State.Rock)
            {
                break;
            }

            current = (nextStep, current.Direction);
        }

        return current;
    }

    private static (LineData[] map, string moveList) ParseInput(string input)
    {
        IEnumerable<LineData> ParseMap(IEnumerator<string> enumerator)
        {
            while (enumerator.MoveNext() && !string.IsNullOrEmpty(enumerator.Current))
            {
                string current = enumerator.Current;
                var index = current.IndexOfAny(new[] { '.', '#' });
                var data = current[index..];
                yield return new LineData(data, index, data.Length + index);
            }
        }

        string GetMoveMap(IEnumerator<string> enumerator)
        {
            while (string.IsNullOrEmpty(enumerator.Current) && enumerator.MoveNext())
                ;

            return enumerator.Current;
        }

        using IEnumerator<string> enumerator = input.ToLines().GetEnumerator();
        var map = ParseMap(enumerator).ToArray();
        var moveList = GetMoveMap(enumerator);
        return (map, moveList);
    }

    private static (int X, int Y) LocateStart((LineData[] Map, string MoveList) data)
    {
        return (data.Map[0].LeftEdge, 0);
    }

    private static State StateAt((LineData[] Map, string MoveList) data, (int x, int y) position)
    {
        if (position.y < 0 || position.y >= data.Map.Length)
        {
            return State.Void;
        }

        LineData lineData = data.Map[position.y];
        if (position.x < lineData.LeftEdge || position.x >= lineData.RightEdge)
        {
            return State.Void;
        }

        return (lineData.Data[position.x - lineData.LeftEdge] == '#')
            ? State.Rock
            : State.Empty;
    }

    private static IEnumerable<Instruction> Instructions((LineData[] Map, string MoveList) data)
    {
        bool isNumber = false;
        int number = 0;

        foreach (var ch in data.MoveList)
        {
            if (char.IsLetter(ch))
            {
                if (isNumber)
                {
                    yield return new Walk(number);
                }
                yield return new Rotate(ch);
                number = 0;
                isNumber = false;
            }

            if (!char.IsAsciiDigit(ch))
            {
                continue;
            }

            isNumber = true;
            number = number * 10 + (ch - '0');
        }

        if (isNumber)
        {
            yield return new Walk(number);
        }
    }

    private enum State
    {
        Empty,
        Rock,
        Void
    }

    private record LineData(string Data, int LeftEdge, int RightEdge);

    private record Instruction;

    private record Rotate(char Turn) : Instruction
    {
        public (int dX, int dY) Apply((int dX, int dY) direction)
        {
            return Turn switch
            {
                'L' => (direction.dY, -direction.dX),
                'R' => (-direction.dY, direction.dX),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    private record Walk(int Steps) : Instruction;
}
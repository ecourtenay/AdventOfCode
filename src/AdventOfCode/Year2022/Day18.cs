namespace AdventOfCode.Year2022;

[Description("Boiling Boulders")]
public class Day18 : IPuzzle
{
    private static readonly (int x, int y, int z)[] Neighbours = { (1, 0, 0), (-1, 0, 0), (0, 1, 0), (0, -1, 0), (0, 0, 1), (0, 0, -1) };


    public object Part1(string input)
    {
        var lines = ParseInput(input).ToArray();

        var exposed = new List<(int x, int y, int z)>();
        foreach ((int x, int y, int z) point in lines)
        {
            exposed.AddRange(Neighbours.Select(direction => (point.x + direction.x, point.y + direction.y, point.z + direction.z)));
        }

        return exposed.Except(lines).Count();
    }

    public object Part2(string input) => string.Empty;

    private static IEnumerable<(int, int, int)> ParseInput(string input)
    {
        return input.ToLines(s =>
        {
            var segments = s.Split(',', StringSplitOptions.TrimEntries);
            return (int.Parse(segments[0]), int.Parse(segments[1]), int.Parse(segments[2]));
        });
    }
}
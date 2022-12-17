using AdventOfCode.Year2022;

namespace AdventOfCode.Tests.Year2022;

public class Day17Tests : IClassFixture<Day17>
{
    private readonly Day17 _sut;

    private const string TestData = """
    >>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>
    """;

    public Day17Tests(Day17 sut)
    {
        _sut = sut;
    }

    [Fact(DisplayName = "Part1 should return expected results from example data")]
    public void Part1Example()
    {
        _sut.Part1(TestData).As<int>().Should().Be(3068);
    }

    [Fact(DisplayName = "Part2 should return expected results from example data")]
    public void Part2Example()
    {
        _sut.Part2(TestData).As<int>().Should().Be(-1);
    }
}
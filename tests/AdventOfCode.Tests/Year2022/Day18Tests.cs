using AdventOfCode.Year2022;

namespace AdventOfCode.Tests.Year2022;

public class Day18Tests : IClassFixture<Day18>
{
    private readonly Day18 _sut;

    private const string TestData = """
    Insert Test Data Here
    """;

    public Day18Tests(Day18 sut)
    {
        _sut = sut;
    }

    [Fact(DisplayName = "Part1 should return expected results from example data")]
    public void Part1Example()
    {
        _sut.Part1(TestData).As<int>().Should().Be(-1);
    }

    [Fact(DisplayName = "Part2 should return expected results from example data")]
    public void Part2Example()
    {
        _sut.Part2(TestData).As<int>().Should().Be(-1);
    }
}
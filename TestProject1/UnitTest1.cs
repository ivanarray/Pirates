using FluentAssertions;
using NUnit.Framework;
using Pirates;

namespace TestProject1;

public class Tests
{
    [Test]
    public void Test1()
    {
        var pirates = new[]
        {
            new Pirate(144, 22, 100, 1),
            new Pirate(216, 22, 100, 2)
        };

        var laser = new LaserGun(0, 0.05, pirates);

        var result = laser.Calculate();

        result.Should().BeEquivalentTo("12.000\n2\n1");
    }

    [Test]
    public void Test2()
    {
        var pirates = new[]
        {
            new Pirate(144, 20, 100, 1),
            new Pirate(216, 20, 100, 2)
        };

        var laser = new LaserGun(0, 0.05, pirates);

        var result = laser.Calculate();

        result.Should().BeEquivalentTo("Impossible");
    }

    [Test]
    public void Gun_should_choose_far()
    {
        var pirates = new[]
        {
            new Pirate(15, 20, 20, 1),
            new Pirate(200, 0.5, 60, 2)
        };

        var laser = new LaserGun(0, 0.9, pirates);

        var result = laser.Calculate();

        result.Should().BeEquivalentTo("1.034\n2\n1");
    }

    [Test]
    public void Gun_should_choose_near()
    {
        var pirates = new[]
        {
            new Pirate(15, 20, 20, 1),
            new Pirate(200, 0.5, 60, 2)
        };

        var laser = new LaserGun(0, 1.1, pirates);

        var result = laser.Calculate();

        result.Should().BeEquivalentTo("0.480\n1\n2");
    }
}
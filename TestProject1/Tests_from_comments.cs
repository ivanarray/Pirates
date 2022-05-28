using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Pirates;

namespace TestProject1;

[TestFixture]
public class TestsFromComments
{
    [Test]
    public void Test1()
    {
        var pirates = new[]
        {
            new Ship(30, 2, 60, 1),
            new Ship(330, 4, 60, 2),
            new Ship(90, 8, 60, 3),
            new Ship(60, 10, 60, 4)
        };

        var gun = new LaserGun(0, 0.083334, pirates.ToList());

        var ships = gun.Process().Select(x => x.Id).ToArray();
        var time = gun.CurrentTimeInMinutes;

        ships.Should().ContainEquivalentOf(new[]
        {
            1, 2, 4, 3
        });
        time.Should().BeApproximately(7d, 0.001);
    }

    [Test]
    public void Test2()
    {
        var pirates = new[]
        {
            new Ship(30, 2, 60, 1),
            new Ship(90, 8, 60, 2),
            new Ship(60, 10, 60, 3),
        };

        var gun = new LaserGun(0, 0.083334, pirates.ToList());

        var ships = gun.Process().Select(x => x.Id);
        var time = gun.CurrentTimeInMinutes;

        ships.Should().ContainInOrder(1, 2, 3);
        time.Should().BeApproximately(3d, 0.001);
    }

    [Test]
    public void Test3()
    {
        var pirates = new[]
        {
            new Ship(210, 6, 60, 1),
            new Ship(150, 20, 60, 2),
        };

        var gun = new LaserGun(0, 0.083334, pirates.ToList());

        var ships = gun.Process().Select(x => x.Id);
        var time = gun.CurrentTimeInMinutes;

        ships.Should().ContainInOrder(1, 2);
        time.Should().BeApproximately(7d, 0.001);
    }

    [Test]
    public void Test4()
    {
        var pirates = new[]
        {
            new Ship(330, 2, 60, 1),
            new Ship(150, 8, 60, 2),
            new Ship(30, 4, 60, 3),
            new Ship(210, 20, 60, 3)
        };

        var gun = new LaserGun(0, 0.083334, pirates.ToList());

        var ships = gun.Process().Select(x => x.Id);
        var time = gun.CurrentTimeInMinutes;

        ships.Should().ContainInOrder(1, 3, 2, 4);
        time.Should().BeApproximately(9d, 0.001);
    }

    [Test]
    public void Test5()
    {
        var pirates = new[]
        {
            new Ship(90, 10, 60, 1),
            new Ship(330, 10, 60, 2),
        };

        var gun = new LaserGun(0, 0.083334, pirates.ToList());

        var ships = gun.Process().Select(x => x.Id);
        var time = gun.CurrentTimeInMinutes;

        ships.Should().ContainInOrder(2, 1);
        time.Should().BeApproximately(5d, 0.001);
    }
}
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


        var gun = new LaserGun(0, new(0.083334), pirates.ToList());

        var ships = gun.Process().Select(x => x.Id).ToArray();
        var time = gun.CurrentTimeInMinutes;

        ships.Should().ContainEquivalentOf(new[]
        {
            1, 2, 4, 3
        });
        time.Should().BeApproximately(7, new(0.001));
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

        var gun = new LaserGun(0, new(0.083334), pirates.ToList());

        var ships = gun.Process().Select(x => x.Id);
        var time = gun.CurrentTimeInMinutes;

        ships.Should().ContainInOrder(1, 2, 3);
        time.Should().BeApproximately(3, new(0.001));
    }

    [Test]
    public void Test3()
    {
        var pirates = new[]
        {
            new Ship(210, 6, 60, 1),
            new Ship(150, 20, 60, 2),
        };

        var gun = new LaserGun(0, new(0.083334), pirates.ToList());

        var ships = gun.Process().Select(x => x.Id);
        var time = gun.CurrentTimeInMinutes;

        ships.Should().ContainInOrder(1, 2);
        time.Should().BeApproximately(7, new(0.001));
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

        var gun = new LaserGun(0, new(0.083334), pirates.ToList());

        var ships = gun.Process().Select(x => x.Id);
        var time = gun.CurrentTimeInMinutes;

        ships.Should().ContainInOrder(1, 3, 2, 4);
        time.Should().BeApproximately(9, new(0.001));
    }

    [Test]
    public void Test5()
    {
        var pirates = new[]
        {
            new Ship(90, 10, 60, 1),
            new Ship(330, 10, 60, 2),
        };

        var gun = new LaserGun(0, new(0.083334), pirates.ToList());

        var ships = gun.Process().Select(x => x.Id);
        var time = gun.CurrentTimeInMinutes;

        ships.Should().ContainInOrder(2, 1);
        time.Should().BeApproximately(5, new(0.001));
    }

    [Test]
    public void Test6()
    {
        var pirates = new[]
        {
            new Ship(0, 1000, 1, 1),
            new Ship(45, 1000, 1, 2),
            new Ship(90, 1000, 1, 3),
            new Ship(135, 1000, 1, 4),
            new Ship(180, 1000, 1, 5),
            new Ship(225, 1000, 1, 6),
            new Ship(270, new(1.25), 60, 7)
        };

        var gun = new LaserGun(0, 1, pirates.ToList());

        var res = gun.Process();
        res.Should().NotBeNull();

        var ids = res!.Select(x => x.Id).ToArray();

        ids.Should().ContainInOrder(1, 7, 6, 5, 4, 3, 2);
    }

    [Test]
    public void Test7()
    {
        var pirates = new[]
        {
            new Ship(new(359.999), new(167.667), 100, 1),
            new Ship(180, new(84.334), 100, 2),
            new Ship(new(0.001), new(1.001), 100, 3)
        };

        var gun = new LaserGun(0, new(0.010), pirates.ToList());

        var res = gun.Process();

        res.Should().NotBeNull();
        res!.Select(x => x.Id).Should().ContainInOrder(3, 2, 1);
        gun.CurrentTimeInMinutes.Should().BeApproximately(100, new(0.001));
    }

    [Test]
    public void Test8()
    {
        var pirates = new[]
        {
            new Ship(new(359.999), new(167.666), 100, 1),
            new Ship(180, new(84.334), 100, 2),
            new Ship(new(0.001), new(1.001), 100, 3)
        };

        var gun = new LaserGun(0, new(0.010), pirates.ToList());

        gun.Process().Should().BeNull();
    }
}
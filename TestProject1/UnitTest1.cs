using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Pirates;

namespace TestProject1
{

    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var pirates = new[]
            {
                new Ship(144, 22, 100, 1),
                new Ship(216, 22, 100, 2)
            };

            var laser = new LaserGun(0, new decimal(0.05), pirates.ToList());

            var result = laser.GetResultInString();

            result.Should().BeEquivalentTo("12.000\n1\n2\n");
        }

        [Test]
        public void Test2()
        {
            var pirates = new[]
            {
                new Ship(144, 20, 100, 1),
                new Ship(216, 20, 100, 2)
            };

            var laser = new LaserGun(0, new decimal(0.05), pirates.ToList());

            var result = laser.GetResultInString();

            result.Should().BeEquivalentTo("Impossible");
        }

        [Test]
        public void Gun_should_choose_far()
        {
            var pirates = new[]
            {
                new Ship(15, 21, 20, 1),
                new Ship(200, new decimal(1.5), 60, 2)
            };

            var laser = new LaserGun(0, new decimal(0.9), pirates.ToList());

            var result = laser.GetResultInString();

            result.Should().BeEquivalentTo("1.034\n2\n1\n");
        }

        [Test]
        public void Gun_should_choose_near()
        {
            var pirates = new[]
            {
                new Ship(15, 21, 20, 1),
                new Ship(200, new decimal(1.5), 60, 2)
            };

            var laser = new LaserGun(0, new decimal(1.1), pirates.ToList());

            var result = laser.GetResultInString();

            result.Should().BeEquivalentTo("0.480\n1\n2\n");
        }
        [Test]
        public void Test_for_one_ship()
        {
            var pirates = new[]
            {
                new Ship(15, 21, 20, 1),
            };

            var laser = new LaserGun(0, new decimal(1.1), pirates.ToList());

            var result = laser.GetResultInString();

            result.Should().BeEquivalentTo("0.038\n1\n");
        }
    }
}
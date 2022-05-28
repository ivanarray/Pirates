using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Pirates;

namespace TestProject1
{

    [TestFixture]
    public class CyclicLinkListTests
    {
        private readonly int[] source = {1, 2, 3, 4, 5};
        private CyclicLinkList<int> list;

        [SetUp]
        public void SetUp()
        {
            list = new CyclicLinkList<int>(source);
        }

        [Test]
        public void List_should_init_correct()
        {
            list.Select(x => x.Value).Should().Contain(source);
        }

        [Test]
        public void List_should_save_ordering()
        {
            list.Select(x => x.Value).Should().ContainInOrder(source);
        }

        [Test]
        public void List_should_add_element()
        {
            list.Add(6);

            list.Select(x => x.Value).Should().ContainInOrder(1, 2, 3, 4, 5, 6);
        }

        [Test]
        public void List_should_remove_right()
        {
            list.RemoveRight();

            list.Select(x => x.Value).Should().ContainInOrder(1, 3, 4, 5);
        }

        [Test]
        public void List_should_remove_left()
        {
            list.RemoveLeft();

            list.Select(x => x.Value).Should().ContainInOrder(1, 2, 3, 4);
        }

        [Test]
        public void List_should_removeRight_single_value()
        {
            var ls = new CyclicLinkList<int>(new[] {1});

            ls.RemoveRight();

            ls.Should().BeEmpty();
        }

        [Test]
        public void List_should_removeLeft_single_value()
        {
            var ls = new CyclicLinkList<int>(new[] {1});

            ls.RemoveRight();

            ls.Should().BeEmpty();
        }

        [Test]
        public void List_should_contain_link_on_self_when_after_removingRight()
        {
            var ls = new CyclicLinkList<int>(new[] {1, 2});

            ls.RemoveRight();

            Assert.IsTrue(ls.Left == ls.Right && ls == ls.Left && ls.IsSingleValue);
        }

        [Test]
        public void List_should_contain_link_on_self_when_after_removingLeft()
        {
            var ls = new CyclicLinkList<int>(new[] {1, 2});

            ls.RemoveLeft();

            Assert.IsTrue(ls.Left == ls.Right && ls == ls.Left && ls.IsSingleValue);
        }
    }
}
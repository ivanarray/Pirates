using FluentAssertions;
using NUnit.Framework;
using Pirates.DataStructures;

namespace TestProject1;

[TestFixture]
public class CyclicLinkListTests
{
    private readonly int[] source = { 1, 2, 3, 4, 5 };
    private CyclicLinkList<int> list;

    [SetUp]
    public void SetUp()
    {
        list = new CyclicLinkList<int>(source);
    }

    [Test]
    public void List_should_init_correct()
    {
        list.Should().Contain(source);
    }

    [Test]
    public void List_should_save_ordering()
    {
        list.Should().ContainInOrder(source);
    }

    [Test]
    public void List_should_add_element()
    {
        list.Add(6);

        list.Should().ContainInOrder(1, 2, 3, 4, 5, 6);
    }

    [Test]
    public void List_should_remove_right()
    {
        list.RemoveRight();

        list.Should().ContainInOrder(1, 3, 4, 5);
    }

    [Test]
    public void List_should_remove_left()
    {
        list.RemoveLeft();

        list.Should().ContainInOrder(1, 2, 3, 4);
    }
}
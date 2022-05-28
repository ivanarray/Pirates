using System.Collections;

namespace Pirates.DataStructures;

public class CyclicLinkList<T> : IEnumerable<CyclicLinkList<T>>
{
    public CyclicLinkList(IEnumerable<T> list)
    {
        foreach (var t in list)
        {
            Add(t);
        }
    }

    private CyclicLinkList()
    {
    }

    public T? Value { get; set; }
    public CyclicLinkList<T>? Left { get; private set; }
    public CyclicLinkList<T>? Right { get; private set; }

    public bool IsEmpty => Left is null;

    public bool IsSingleValue => this == Left;

    public void RemoveRight()
    {
        if (IsEmpty) throw new InvalidOperationException("List is Empty");
        if (IsSingleValue)
        {
            Value = default;
            Left = Right = null;
        }
        else
        {
            Right!.Right!.Left = this;
            Right = Right.Right;
        }
    }

    public void RemoveLeft()
    {
        if (IsEmpty) throw new InvalidOperationException("List is Empty");
        if (IsSingleValue)
        {
            Value = default;
            Left = Right = null;
        }
        else
        {
            Left!.Left!.Right = this;
            Left = Left.Left;
        }
    }

    public void Add(T val)
    {
        if (Left is null || Right is null)
        {
            Value = val;
            Left = this;
            Right = this;
        }
        else
        {
            var res = new CyclicLinkList<T>
            {
                Value = val,
                Left = Left,
                Right = this
            };
            Left.Right = res;
            Left = res;
        }
    }

    public IEnumerator<CyclicLinkList<T>> GetEnumerator()
    {
        if (IsEmpty)
            yield break;
        yield return this;

        var cur = Right;
        while (!cur!.Equals(this))
        {
            yield return cur;
            cur = cur.Right;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        return $"Value: {Value}\n" +
               $"Left: {Left.Value}\n" +
               $"Right: {Right.Value}";
    }
}
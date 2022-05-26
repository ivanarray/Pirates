using System.Collections;

namespace Pirates.DataStructures;

public class CyclicLinkList<T> : IEnumerable<T>
{
    public CyclicLinkList(IEnumerable<T> list)
    {
        foreach (var t in list)
        {
            Add(t);
        }
    }
    
    private CyclicLinkList(){}

    public T? Value { get; set; }
    public CyclicLinkList<T>? Left { get; private set; }
    public CyclicLinkList<T>? Right { get; private set; }

    public bool IsEmpty => Left is null || Right is null;

    public void RemoveRight()
    {
        if (IsEmpty) throw new InvalidOperationException("List is Empty");
        Right!.Right!.Left = this;
        Right = Right.Right;
    }
    public void RemoveLeft()
    {
        if (IsEmpty) throw new InvalidOperationException("List is Empty");
        Left!.Left!.Right = this;
        Left = Left.Left;
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

    public IEnumerator<T> GetEnumerator()
    {
        
        if (Value is null)
            yield break;
        
        yield return Value;

        var cur = Right;
        while (!cur!.Equals(this))
        {
            yield return cur.Value!;
            cur = cur.Right;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
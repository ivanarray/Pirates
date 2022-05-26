using System.Collections;

namespace Pirates.DataStructures;

public class LinkList<T> : IEnumerable<T>
{
    private class Node<TNode>
    {
        public Node<TNode>? Previous { get; set; }
        public TNode Value { get; set; }

        public Node(TNode val, Node<TNode>? prev = null)
        {
            Value = val;
            Previous = prev;
        }
    }

    private Node<T>? current;

    public LinkList(T val)
    {
        current = new Node<T>(val);
    }

    public LinkList()
    {
        current = null;
    }

    public void Add(T value)
    {
        var cur = new Node<T>(value, current);
        current = cur;
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (current is null) yield break;
        var cur = current;
        
        while (cur.Previous is not null)
        {
            yield return cur.Value;
            cur = cur.Previous;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
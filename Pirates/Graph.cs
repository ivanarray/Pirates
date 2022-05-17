namespace Pirates;

public class Graph<TNode, TWeigh>
    where TWeigh : IComparable
{
    public IEnumerable<Node<TNode, TWeigh>> Nodes => nodes;
    private readonly ISet<Node<TNode, TWeigh>> nodes;

    public Graph(ISet<Node<TNode, TWeigh>> nodes)
    {
        this.nodes = nodes;
    }

    public void AddNode(Node<TNode, TWeigh> node) => nodes.Add(node);
}

public class Edge<TWeigh, TNode>
    where TWeigh : IComparable
{
    public readonly Node<TNode, TWeigh> From;
    public readonly Node<TNode, TWeigh> To;
    public TWeigh Weigh { get; set; }

    public Edge(Node<TNode, TWeigh> from, Node<TNode, TWeigh> to, TWeigh weigh = default!)
    {
        From = from;
        To = to;
        Weigh = weigh;
    }
}

public class Node<TNode, TWeigh>
    where TWeigh : IComparable
{
    public readonly TNode Value;

    public IReadOnlyList<Edge<TWeigh, TNode>> Edges => edges;

    private readonly List<Edge<TWeigh, TNode>> edges = new();

    public Node(TNode value) => Value = value;

    public void AddEdge(TWeigh weigh, Node<TNode, TWeigh> to) =>
        edges.Add(new Edge<TWeigh, TNode>(this, to, weigh));
}
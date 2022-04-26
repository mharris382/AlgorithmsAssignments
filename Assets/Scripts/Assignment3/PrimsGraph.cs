using Assignment1;
using Assignment2;

public class PrimsGraph
{
    private Graph<PrimVertex> _graph;
    private PriorityQueue<PrimVertex> _queue;

    public PrimsGraph(string filePath)
    {
        var parser = new GraphParser(filePath);
        var graphType = parser.GetGraphType();
        _graph = parser.ParseGraph<PrimVertex>(ParseStringToVertex);
        _queue = new PriorityQueue<PrimVertex>();
        _queue.InitCapacity(100);
    }

    private PrimVertex ParseStringToVertex(string str)
    {
        return new PrimVertex()
        {
            Value = str,
            Cost = 0,
            Parent = null
        };
    }
}
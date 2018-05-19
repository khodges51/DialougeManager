using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialouge : ScriptableObject
{

    private List<Node> _nodes;
    private List<Connection> _connections;

    public void Initialize(List<Node> nodes, List<Connection> connections)
    {
        _nodes = nodes;
        _connections = connections;
    }

    public List<Node> GetNodes()
    {
        return _nodes;
    }

    public List<Connection> GetConnections()
    {
        return _connections;
    }
}

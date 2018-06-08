using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNodes : ScriptableObject
{
    public List<Node> nodes;

    public List<Connection> connections;
}

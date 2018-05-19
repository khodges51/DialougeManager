using System;
using UnityEditor;
using UnityEngine;


[Serializable]
public class Node : ScriptableObject
{
    [SerializeField]
    public Rect rect;
    [SerializeField]
    public string title;
    [SerializeField]
    public bool isDragged;
    [SerializeField]
    public bool isSelected;

    [SerializeField]
    public ConnectionPoint inPoint;
    [SerializeField]
    public ConnectionPoint outPoint;

    [SerializeField]
    public GUIStyle style;
    [SerializeField]
    public GUIStyle defaultNodeStyle;
    [SerializeField]
    public GUIStyle selectedNodeStyle;

    [SerializeField]
    public Action<Node> OnRemoveNode;

    public void OnEnable()
    {
        hideFlags = HideFlags.HideAndDontSave;
    }

    public void Initialize(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;
        inPoint = ScriptableObject.CreateInstance<ConnectionPoint>();
        outPoint = ScriptableObject.CreateInstance<ConnectionPoint>();
        inPoint.Initialize(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint.Initialize(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public void Draw()
    {
        inPoint.Draw();
        outPoint.Draw();
        GUI.Box(rect, title, style);
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }

                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                {
                    ProcessContextMenu();
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }
}

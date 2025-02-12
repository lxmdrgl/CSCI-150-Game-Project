using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class NodeGraphEditor : EditorWindow
{
    // private NodeGraphView graphView;

    [MenuItem("Window/Custom Node Graph")]
    public static void OpenWindow()
    {
        var window = GetWindow<NodeGraphEditor>();
        window.titleContent = new GUIContent("Node Graph");
        window.Show();
    }

    // private void OnEnable()
    // {
    //     graphView = new NodeGraphView
    //     {
    //         style = { flexGrow = 1 }
    //     };
    //     rootVisualElement.Add(graphView);
    // }

    // private void OnDisable()
    // {
    //     rootVisualElement.Remove(graphView);
    // }
}

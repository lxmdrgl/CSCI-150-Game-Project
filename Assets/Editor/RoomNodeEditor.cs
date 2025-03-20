using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(RoomNode))]
public class RoomNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoomNode roomNode = (RoomNode)target;

        // Update the list of room types
        RoomNode.UpdateRoomTypes();
        List<string> roomTypes = RoomNode.RoomTypes;

        // Display the dropdown
        int selectedIndex = Mathf.Max(0, roomTypes.IndexOf(roomNode.RoomType));
        selectedIndex = EditorGUILayout.Popup("Room Type", selectedIndex, roomTypes.ToArray());

        // Apply the selected value
        roomNode.RoomType = roomTypes[selectedIndex];

        // Save changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(roomNode);
        }

        DrawDefaultInspector();
    }
}
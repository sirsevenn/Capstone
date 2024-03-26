using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryManager))]
public class InventoryManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        InventoryManager inventoryManager = (InventoryManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Reset Saved Values"))
        {
            inventoryManager.ResetSavedValues();
            Debug.Log("Saved values reset!");
        }
    }
}
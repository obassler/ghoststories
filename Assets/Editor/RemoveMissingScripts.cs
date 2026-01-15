using UnityEngine;
using UnityEditor;

public class RemoveMissingScripts : EditorWindow
{
    [MenuItem("Tools/Remove Missing Scripts from Scene")]
    public static void RemoveFromScene()
    {
        int removedCount = 0;
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject go in allObjects)
        {
            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
            if (count > 0)
            {
                Undo.RegisterCompleteObjectUndo(go, "Remove Missing Scripts");
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                removedCount += count;
            }
        }

        Debug.Log($"Removed {removedCount} missing script references from scene.");
    }

    [MenuItem("Tools/Remove Missing Scripts from Selected")]
    public static void RemoveFromSelected()
    {
        int removedCount = 0;
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject go in selectedObjects)
        {
            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
            if (count > 0)
            {
                Undo.RegisterCompleteObjectUndo(go, "Remove Missing Scripts");
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                removedCount += count;
            }

            foreach (Transform child in go.GetComponentsInChildren<Transform>(true))
            {
                int childCount = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(child.gameObject);
                if (childCount > 0)
                {
                    Undo.RegisterCompleteObjectUndo(child.gameObject, "Remove Missing Scripts");
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(child.gameObject);
                    removedCount += childCount;
                }
            }
        }

        Debug.Log($"Removed {removedCount} missing script references from selected objects.");
    }
}

using UnityEngine;
using UnityEditor;

namespace WE
{
    public class RemoveMissingComponentsTool : MonoBehaviour
    {
        [MenuItem("Willy/RemoveMissingComponents")]
        private static void RemoveMissingComponents()
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            int totalRemoved = 0;

            foreach (GameObject go in selectedObjects)
            {
                Undo.RegisterFullObjectHierarchyUndo(go, "Remove Missing Components");

                totalRemoved += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);

                foreach (Transform child in go.GetComponentsInChildren<Transform>(true))
                {
                    if (child == null) continue;
                    totalRemoved += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(child.gameObject);
                }
            }

            if (totalRemoved > 0)
            {
                Debug.Log($"Removed {totalRemoved} missing component(s) from selected GameObject(s) and their children.");
            }
            else
            {
                Debug.Log("No missing components found.");
            }
        }

        [MenuItem("Willy/RemoveMissingComponents", true)]
        private static bool ValidateRemoveMissingComponents()
        {
            return Selection.gameObjects.Length > 0;
        }
    }
}

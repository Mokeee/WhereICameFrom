using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneLocalizer : MonoBehaviour
{
    public LocalizationChannel channel;

#if UNITY_EDITOR
    [MenuItem("GameObject/Setup/SceneLocalizer")]
    static void CreateSceneLocalizer(MenuCommand menuCommand)
    {
        var go = SpawnLocalizer(menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    public static GameObject SpawnLocalizer(GameObject parent)
    {
        GameObject go = new GameObject("SceneLocalizer", typeof(SceneLocalizer));

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, parent);

        return go;
    }
#endif

    public void RegisterText(LocalizedText text)
    {
        channel.RaiseRegisterEvent(text);
    }
    public void TranslateText(LocalizedText text)
    {
        channel.RaiseRegisterEvent(text);
    }
}

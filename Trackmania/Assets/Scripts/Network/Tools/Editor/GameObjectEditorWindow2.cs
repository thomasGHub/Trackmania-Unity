using UnityEngine;
using UnityEditor;

[CustomPreview(typeof(GameObject))]
public class GameObjectEditorWindow2 : ObjectPreview
{
    public override bool HasPreviewGUI()
    {
        return true;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        GUI.Label(r, target.name + " is being previewed");
    }

    public override GUIContent GetPreviewTitle()
    {
        
        return new GUIContent($"INFORMATION {target.name}       ");
    }

    public override string GetInfoString() //null
    {
        return string.Empty;
    }
}
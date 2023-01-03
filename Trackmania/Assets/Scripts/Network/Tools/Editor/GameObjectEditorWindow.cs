using UnityEngine;
using UnityEditor;
using System;
using MirrorBasics;

[CustomPreview(typeof(GameObject))]
public class GameObjectEditorWindow : ObjectPreview
{
    //class Styles
    //{
    //    public GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
    //    public GUIStyle componentName = new GUIStyle(EditorStyles.boldLabel);
    //    public GUIStyle disabledName = new GUIStyle(EditorStyles.miniLabel);

    //    public Styles()
    //    {
    //        Color fontColor = new Color(0.7f, 0.7f, 0.7f);
    //        labelStyle.padding.right += 20;
    //        labelStyle.normal.textColor = fontColor;
    //        labelStyle.active.textColor = fontColor;
    //        labelStyle.focused.textColor = fontColor;
    //        labelStyle.hover.textColor = fontColor;
    //        labelStyle.onNormal.textColor = fontColor;
    //        labelStyle.onActive.textColor = fontColor;
    //        labelStyle.onFocused.textColor = fontColor;
    //        labelStyle.onHover.textColor = fontColor;

    //        componentName.normal.textColor = fontColor;
    //        componentName.active.textColor = fontColor;
    //        componentName.focused.textColor = fontColor;
    //        componentName.hover.textColor = fontColor;
    //        componentName.onNormal.textColor = fontColor;
    //        componentName.onActive.textColor = fontColor;
    //        componentName.onFocused.textColor = fontColor;
    //        componentName.onHover.textColor = fontColor;

    //        disabledName.normal.textColor = fontColor;
    //        disabledName.active.textColor = fontColor;
    //        disabledName.focused.textColor = fontColor;
    //        disabledName.hover.textColor = fontColor;
    //        disabledName.onNormal.textColor = fontColor;
    //        disabledName.onActive.textColor = fontColor;
    //        disabledName.onFocused.textColor = fontColor;
    //        disabledName.onHover.textColor = fontColor;
    //    }
    //}

    //GUIContent title;
    //Styles styles = new Styles();

    //public override GUIContent GetPreviewTitle()
    //{
    //    if (title == null)
    //    {
    //        title = new GUIContent("Network Information");
    //    }
    //    return title;
    //}

    //public override bool HasPreviewGUI()
    //{
    //    // need to check if target is null to stop MissingReferenceException
    //    return target != null && target is GameObject gameObject;
    //}


    //public override void OnPreviewGUI(Rect r, GUIStyle background)
    //{
    //    if (Event.current.type != EventType.Repaint)
    //        return;

    //    if (target == null)
    //        return;

    //    GameObject targetGameObject = target as GameObject;

    //    if (targetGameObject == null)
    //        return;

    //    MonoBehaviour script = targetGameObject.GetComponent<UILobby>();

    //    if (script == null)
    //    {
    //        Debug.Log("script null");
    //        return;
    //    }

    //    if (styles == null)
    //        styles = new Styles();


    //    // padding
    //    RectOffset previewPadding = new RectOffset(-5, -5, -5, -5);
    //    Rect paddedr = previewPadding.Add(r);

    //    //Centering
    //    float initialX = paddedr.x + 10;
    //    float Y = paddedr.y + 10;

    //    Debug.Log("tesdt");
    //    System.Reflection.FieldInfo[] properties = target.GetType().GetFields();

    //    for (int i = 0; i < properties.Length; i++)
    //    {
    //        Debug.Log(properties[i].Name);
    //        Debug.Log(properties[i].GetType());
    //    }
    //    //Y = DrawNetworkIdentityInfo(initialX, Y);

    //    //Y = DrawNetworkBehaviors(script, initialX, Y);

    //    //Y = DrawObservers(script, initialX, Y);


    //}

    //float DrawNetworkIdentityInfo(float initialX, float Y)
    //{
    //    IEnumerable<NetworkIdentityInfo> infos = GetNetworkIdentityInfo(identity);
    //    // Get required label size for the names of the information values we're going to show
    //    // There are two columns, one with label for the name of the info and the next for the value
    //    Vector2 maxNameLabelSize = new Vector2(140, 16);
    //    Vector2 maxValueLabelSize = GetMaxNameLabelSize(infos);

    //    Rect labelRect = new Rect(initialX, Y, maxNameLabelSize.x, maxNameLabelSize.y);
    //    Rect idLabelRect = new Rect(maxNameLabelSize.x, Y, maxValueLabelSize.x, maxValueLabelSize.y);

    //    foreach (NetworkIdentityInfo info in infos)
    //    {
    //        GUI.Label(labelRect, info.name, styles.labelStyle);
    //        GUI.Label(idLabelRect, info.value, styles.componentName);
    //        labelRect.y += labelRect.height;
    //        labelRect.x = initialX;
    //        idLabelRect.y += idLabelRect.height;
    //    }

    //    return labelRect.y;
    //}



    //// Get the maximum size used by the value of information items
    //Vector2 GetMaxNameLabelSize(IEnumerable<NetworkIdentityInfo> infos)
    //{
    //    Vector2 maxLabelSize = Vector2.zero;
    //    foreach (NetworkIdentityInfo info in infos)
    //    {
    //        Vector2 labelSize = styles.labelStyle.CalcSize(info.value);
    //        if (maxLabelSize.x < labelSize.x)
    //        {
    //            maxLabelSize.x = labelSize.x;
    //        }
    //        if (maxLabelSize.y < labelSize.y)
    //        {
    //            maxLabelSize.y = labelSize.y;
    //        }
    //    }
    //    return maxLabelSize;
    //}



}
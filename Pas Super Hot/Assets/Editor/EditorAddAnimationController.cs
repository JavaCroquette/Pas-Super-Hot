using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AddAnimationController))]
public class EditorAddAnimationController : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AddAnimationController myScript = (AddAnimationController)target;
        if (GUILayout.Button("Set Animators"))
        {
            myScript.SetAnimators();
        }
        if (GUILayout.Button("ReversClips"))
        {
            myScript.ReverseClip();
        }
    }
    private static Motion ReverseClip(AnimationClip clip)
    {
        EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
        return null;
    }
}

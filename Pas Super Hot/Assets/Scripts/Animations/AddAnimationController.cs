using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

public class AddAnimationController : MonoBehaviour
{
    public int ID = 0;
    public AnimationClip clip = default;
#if UNITY_EDITOR

    public void SetAnimators()
    {
        //animators = GetComponentInChildren<Animator>();
        foreach (Transform brique in transform)//buildingBase
        {
            Animator anim = brique.gameObject.GetComponent<Animator>();
            //Debug.Log(brique.name);

            /*Motion clipReverse = ReverseClip("Assets/Animation_" + ID + "/" + "explosion_" + ID + "_" +
                brique.name);*/
            string namePath = "Assets/AnimationController_" + ID + "/" + "explosion_" + ID + "_" +
                brique.name;
            string assetPath = namePath + ".controller";
            AssetDatabase.FindAssets(assetPath);
            AnimatorController myController = AssetDatabase.LoadAssetAtPath<AnimatorController>(assetPath);
            //myController.AddMotion(clipReverse);
            anim.runtimeAnimatorController = myController;
        }
        //    }
        //}
    }

    public Motion ReverseClip()
    {
        if (clip == null)
            return null;
        float clipLength = clip.length;
        EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
        EditorCurveBinding[] objectReferenceCurveBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
        clip.ClearCurves();
        //foreach (EditorCurveBinding curveBinding in curveBindings)
        //{
        //    curveBinding.
        //}
        return null;
    }/*
    private static Motion ReverseClip(string clipPath)
    {
        string copiedFilePath = clipPath + "_Reversed.anim";
        var clip = GetSelectedClip();

        AssetDatabase.CopyAsset(clipPath + ".anim", copiedFilePath);

        clip = (AnimationClip)AssetDatabase.LoadAssetAtPath(copiedFilePath, typeof(AnimationClip));

        if (clip == null)
            return null;
        float clipLength = clip.length;
        var curves = AnimationUtility.GetAllCurves(clip, true);
        clip.ClearCurves();
        foreach (AnimationClipCurveData curve in curves)
        {
            var keys = curve.curve.keys;
            int keyCount = keys.Length;
            var postWrapmode = curve.curve.postWrapMode;
            curve.curve.postWrapMode = curve.curve.preWrapMode;
            curve.curve.preWrapMode = postWrapmode;
            for (int i = 0; i < keyCount; i++)
            {
                Keyframe K = keys[i];
                K.time = clipLength - K.time;
                var tmp = -K.inTangent;
                K.inTangent = -K.outTangent;
                K.outTangent = tmp;
                keys[i] = K;
            }
            curve.curve.keys = keys;
            clip.SetCurve(curve.path, curve.type, curve.propertyName, curve.curve);
        }
        var events = AnimationUtility.GetAnimationEvents(clip);
        if (events.Length > 0)
        {
            for (int i = 0; i < events.Length; i++)
            {
                events[i].time = clipLength - events[i].time;
            }
            AnimationUtility.SetAnimationEvents(clip, events);
        }
        Debug.Log("Animation reversed!");
        return clip;
    }
    public static AnimationClip GetSelectedClip()
    {
        var clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets);
        if (clips.Length > 0)
        {
            return clips[0] as AnimationClip;
        }
        return null;
    }*/
#endif
}

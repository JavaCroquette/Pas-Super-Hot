using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif
public class RecordingExplosion : MonoBehaviour
{
    private AnimationClip clip, reversedClip;
    private GameObjectRecorder m_Recorder;

    public string ID = "0";
    [SerializeField]
    private float recordingFreq = 1f;
    private float timer = 1f;

    void Start()
    {
        // Instanciate animationClips
        clip = new AnimationClip();
        reversedClip = new AnimationClip();
        // Create animationClips by using parents' names 
        string familyName = name;
        while (transform.parent)
        {
            familyName = transform.parent.name + familyName;
            Debug.Log(familyName);
        }
        AssetDatabase.CreateAsset(clip, "Assets/Animation_" + ID + "/" + "explosion_" + ID + "_" +
            familyName + ".anim");
        AssetDatabase.CreateAsset(reversedClip, "Assets/Animation_" + ID + "/" + "reversexplosion_" + ID + "_" +
            familyName + ".anim");

        // Create recorder and record the script GameObject.
        m_Recorder = new GameObjectRecorder(gameObject);

        // Bind all the Transforms on the GameObject and all its children.
        m_Recorder.BindComponentsOfType<Transform>(gameObject, true);
        timer = recordingFreq;
        m_Recorder.TakeSnapshot(timer);
        timer = 0;
    }

    void LateUpdate()
    {
        if (clip == null)
            return;

        // Take a snapshot and record all the bindings values for this frame.
        timer += Time.deltaTime;

        //Debug.Log("timer=" + timer);
        if (timer >= recordingFreq)
        {
            //Debug.Log("register");
            m_Recorder.TakeSnapshot(timer);
            /*Time.deltaTime*/
            timer = 0f;
        }
    }

    void OnDisable()
    {
        if (clip == null)
            return;

        if (m_Recorder.isRecording)
        {
            // Save the recorded session to the clip.
            m_Recorder.SaveToClip(clip);
            //m_Recorder.SaveToClip(reverseClip);
            CreateController();
        }
    }

    void CreateController()
    {
        // Creates the controller
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(
            "Assets/AnimationController_" + ID + "/" + "explosion_" + ID + "_" +
             gameObject.name + ".controller");
        //controller.AddMotion(clip);
        //controller.AddMotion(reverseClip);
        //Animator anim = gameObject.GetComponent<Animator>();
        //anim.runtimeAnimatorController = controller;
        //anim.Rebind();
    }

    //private static Motion ReverseClip(string clipPath)
    //{
    //    string copiedFilePath = clipPath + "_Reversed.anim";
    //    var clip = GetSelectedClip();

    //    AssetDatabase.CopyAsset(clipPath + ".anim", copiedFilePath);

    //    clip = (AnimationClip)AssetDatabase.LoadAssetAtPath(copiedFilePath, typeof(AnimationClip));

    //    if (clip == null)
    //        return null;
    //    float clipLength = clip.length;
    //    var curves = AnimationUtility.GetAllCurves(clip, true);
    //    clip.ClearCurves();
    //    foreach (AnimationClipCurveData curve in curves)
    //    {
    //        var keys = curve.curve.keys;
    //        int keyCount = keys.Length;
    //        var postWrapmode = curve.curve.postWrapMode;
    //        curve.curve.postWrapMode = curve.curve.preWrapMode;
    //        curve.curve.preWrapMode = postWrapmode;
    //        for (int i = 0; i < keyCount; i++)
    //        {
    //            Keyframe K = keys[i];
    //            K.time = clipLength - K.time;
    //            var tmp = -K.inTangent;
    //            K.inTangent = -K.outTangent;
    //            K.outTangent = tmp;
    //            keys[i] = K;
    //        }
    //        curve.curve.keys = keys;
    //        clip.SetCurve(curve.path, curve.type, curve.propertyName, curve.curve);
    //    }
    //    var events = AnimationUtility.GetAnimationEvents(clip);
    //    if (events.Length > 0)
    //    {
    //        for (int i = 0; i < events.Length; i++)
    //        {
    //            events[i].time = clipLength - events[i].time;
    //        }
    //        AnimationUtility.SetAnimationEvents(clip, events);
    //    }
    //    Debug.Log("Animation reversed!");
    //    return clip;
    //}
}

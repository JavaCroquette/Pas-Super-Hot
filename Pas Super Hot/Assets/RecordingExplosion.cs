using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif
public class RecordingExplosion : MonoBehaviour
{
    [SerializeField]
    private string ID = "0";
    [SerializeField]
    private float recordingFreq = 1f;

    private float timer = 1f;
    private AnimationClip clip;
    private GameObjectRecorder m_Recorder;

    void Start()
    {
        // Instanciate animationClips
        clip = new AnimationClip();

        // Create animationClips by using parents' names 
        string familyName = name;
        Transform tmpTransform = transform;
        while (tmpTransform.parent)
        {
            tmpTransform = tmpTransform.parent;
            familyName = tmpTransform.name + familyName;
        }
        CreateDirectories();
        AssetDatabase.CreateAsset(clip, "Assets/Animation_" + ID + "/" + "explosion_" + ID + "_" +
            familyName + ".anim");

        // Create recorder and record the script GameObject.
        m_Recorder = new GameObjectRecorder(gameObject);

        // Bind all the Transforms on the GameObject and all its children.
        m_Recorder.BindComponentsOfType<Transform>(gameObject, true);
        timer = recordingFreq;
        m_Recorder.TakeSnapshot(timer);
        timer = 0;
    }

    private void CreateDirectories()
    {
        System.IO.Directory.CreateDirectory("Assets/Animation_" + ID + "/");
        System.IO.Directory.CreateDirectory("Assets/AnimationController_" + ID + "/");
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
        controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
        AnimatorStateMachine rootStateMachine = controller.layers[0].stateMachine;
        var explosionState = rootStateMachine.AddState("Explode");
        explosionState.speedParameterActive = true;
        explosionState.speedParameter = "Speed";
        explosionState.motion = clip;
    }
}

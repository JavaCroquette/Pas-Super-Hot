using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif
public class RecordingExplosion : MonoBehaviour
{
    #region Vars
    [SerializeField]
    private string ID = "0";
    [SerializeField]
    private float recordingFreq = 1f;

    private float timer = 1f;
    private AnimationClip clip;
    private GameObjectRecorder m_Recorder;
    #endregion

    #region BuildIn
    void Start()
    {
        CreateDirectories();
        CreateAnimationClip();
        InitialiseRecorder();
    }

    void LateUpdate()
    {
        RecordActualFrame();
    }

    void OnDisable()
    {
        if (clip == null)
            return;

        if (m_Recorder.isRecording)
        {
            // Save the recorded session to the clip.
            m_Recorder.SaveToClip(clip);
            // Create the controller associated to the clip.
            CreateController();
        }
    }
    #endregion

    #region Functions
    private void RecordActualFrame()
    {
        if (clip == null)
        {
            return;
        }

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

    private void InitialiseRecorder()
    {
        // Create recorder and record the script GameObject.
        m_Recorder = new GameObjectRecorder(gameObject);

        // Bind all the Transforms on the GameObject and all its children.
        m_Recorder.BindComponentsOfType<Transform>(gameObject, true);

        // Recorde first Transform
        timer = recordingFreq;
        m_Recorder.TakeSnapshot(timer);
        timer = 0;
    }

    private void CreateAnimationClip()
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
        AssetDatabase.CreateAsset(clip, "Assets/Animations/Animation_" + ID + "/" + "explosion_" + ID + "_" +
            familyName + ".anim");
    }

    private void CreateDirectories()
    {
        Directory.CreateDirectory("Assets/Animations/Animation_" + ID + "/");
        Directory.CreateDirectory("Assets/Animations/AnimationController_" + ID + "/");
    }

    void CreateController()
    {
        // Creates the controller
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(
            "Assets/Animations/AnimationController_" + ID + "/" + "explosion_" + ID + "_" +
             gameObject.name + ".controller");
        controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
        AnimatorStateMachine rootStateMachine = controller.layers[0].stateMachine;
        var explosionState = rootStateMachine.AddState("Explode");
        explosionState.speedParameterActive = true;
        explosionState.speedParameter = "Speed";
        explosionState.motion = clip;
    }
    #endregion
}

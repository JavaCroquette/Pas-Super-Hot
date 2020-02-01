using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class RecordingExplosion : MonoBehaviour
{
    private AnimationClip clip;
    //private AnimationClip reverseClip;

    private GameObjectRecorder m_Recorder;
    public int ID = 0;
    [SerializeField]
    private float recordingFreq = 1.0f;
    private float timer = 1f;

    void Start()
    {
        clip = new AnimationClip();
        //reverseClip = new AnimationClip();
        AssetDatabase.CreateAsset(clip, "Assets/Animation_" + ID + "/" + "explosion_" + ID + "_" +
            gameObject.name + ".anim");
        //AssetDatabase.CreateAsset(reverseClip, "Assets/Animation_" + ID + "/" + "reversexplosion_" + ID + "_" +
        //    gameObject.name + ".anim");

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


}

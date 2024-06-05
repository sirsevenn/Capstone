using UnityEditor.Animations;
using UnityEngine;

public class DiceRecorder : MonoBehaviour
{
    [SerializeField] private AnimationClip rollClip;
    [SerializeField] private bool isRecording;
    private GameObjectRecorder recorder;

    [Space(10)]
    [SerializeField] private GameObject diceColliderObj;
    private Collider diceCollider;
    private Rigidbody diceRB;

    [Space(10)]
    [SerializeField] private Vector3 forceVector;
    [SerializeField] private float randTorqueValue;


    void Start()
    {
        isRecording = false;

        diceCollider = diceColliderObj.GetComponent<Collider>();
        diceCollider.enabled = true;
        diceRB = diceColliderObj.GetComponent<Rigidbody>();
        diceRB.isKinematic = false;
    }

    private void CreateRecorder()
    {
        recorder = new GameObjectRecorder(this.gameObject);
        recorder.BindComponent(diceColliderObj.GetComponent<Transform>());
    }

    private void Update()
    {
        if (isRecording && diceRB.velocity.sqrMagnitude <= 0.0001 && diceRB.angularVelocity.sqrMagnitude <= 0.0001)
        {
            isRecording = false;
            recorder.SaveToClip(rollClip);
            recorder.ResetRecording();
        }

        if (Input.touchCount > 0)
        {
            diceRB.velocity = Vector3.zero;
            diceRB.angularVelocity = Vector3.zero;

            diceColliderObj.transform.localPosition = new Vector3(0, 1, 0);
            diceRB.AddForce(forceVector);
            diceRB.AddTorque(new Vector3(Random.Range(-randTorqueValue, randTorqueValue), Random.Range(-randTorqueValue, randTorqueValue), Random.Range(-randTorqueValue, randTorqueValue)));

            isRecording = true;
            CreateRecorder();
        }
    }

    private void LateUpdate()
    {
        if (rollClip == null)
            return;

        if (isRecording)
        {
            // Take a snapshot and record all the bindings values for this frame.
            recorder.TakeSnapshot(Time.deltaTime);
        }
    }

    private void OnDisable()
    {
        if (rollClip == null)
            return;

        if (recorder.isRecording)
        {
            // Save the recorded session to the clip.
            recorder.SaveToClip(rollClip);
            recorder.ResetRecording();
        }
    }
}

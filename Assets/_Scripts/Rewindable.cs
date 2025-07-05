using UnityEngine;

public abstract class Rewindable : MonoBehaviour
{
    [SerializeField] protected bool isRewinding = false;
    public bool isRewindable = true;

    public static float rewindTime = 15f;

    protected virtual void Awake() { }
    protected virtual void Start()
    {
        rewindTime = 15f;
    }

    private void FixedUpdate()
    {
        if (!isRewindable) return;

        if (isRewinding)
            Rewind();
        else
            Record();
    }

    public virtual void StartRewind()
    {
        isRewinding = true;
    }

    public virtual void StopRewind()
    {
        isRewinding = false;
    }

    // Implement these in child classes
    public abstract void Record();
    public abstract void Rewind();
}

public class PointInTime
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public PointInTime(Vector3 _position, Quaternion _rotation, Vector3 _velocity)
    {
        position = _position;
        rotation = _rotation;
        velocity = _velocity;
    }
}
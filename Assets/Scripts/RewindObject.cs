using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindObject : MonoBehaviour
{
    [SerializeField] bool isRewinding = false;
    public bool isRewindable = true;
    [SerializeField] ParticleSystem rewindEffect;
    List<PointInTime> pointsInTime;

    public static float rewindTime = 15f;

    Rigidbody rb;

    Vector3 pos0;
    Vector3 pos1;
    Vector3 velocity;

    Vector3 rot0;
    Vector3 rot1;
    [SerializeField] Vector3 rotVelocity;
    [SerializeField] Vector3 currentRot;
    public void Start()
    {
        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody>();
        rewindTime = 15f;
        rewindEffect.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    StartRewind();
            
        //}
        //if (Input.GetKeyUp(KeyCode.R))
        //{
        //    StopRewind();
            
        //}
    }

    private void FixedUpdate()
    {
        if (isRewindable)
        {
            if (isRewinding)
            {
                Rewind();
            }
            else
            {
                Record();
            }
        }
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    public void Record()
    {
        if (pointsInTime.Count > Mathf.Round(rewindTime / Time.deltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation,rb.linearVelocity));
    }

    public void Rewind()
    {
        
        if (pointsInTime.Count > 1)
        {
            //pos0 = transform.position;
            //rot0 = transform.rotation.eulerAngles;
            PointInTime pointInTime = pointsInTime[0];
            PointInTime pointInTime2 = pointsInTime[1];
            //transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            //pos1 = transform.position;
            //rot1 = transform.rotation.eulerAngles;
            pointsInTime.RemoveAt(0);
            //velocity = (pointInTime2.position- pointInTime.position) / Time.fixedDeltaTime;
            velocity = -pointInTime.velocity;
            rb.linearVelocity = velocity;
            //rotVelocity = (rot1 - rot0);
        }
        else
        {
            StopRewind();
        }

    }

    public void StartRewind()
    {
        isRewinding = true;
        //rb.isKinematic = true;
        rb.useGravity = false;
        rewindEffect.Play();
    }

    public void StopRewind()
    {
        rewindEffect.Stop();
        isRewinding = false;
        //rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = -velocity;
        //rb.angularVelocity = -rotVelocity;
    }
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

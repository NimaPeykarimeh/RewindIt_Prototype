using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindObject : Rewindable
{
    [SerializeField] ParticleSystem rewindEffect;
    [SerializeField] LineRenderer lineRenderer;

    protected override void Start()
    {
        base.Start();
        rewindEffect.Stop();
        lineRenderer.gameObject.SetActive(false);
    }

    public override void StartRewind()
    {
        base.StartRewind();
        rewindEffect.Play();
        lineRenderer.gameObject.SetActive(true);
    }

    public override void StopRewind()
    {
        base.StopRewind();
        rewindEffect.Stop();
        lineRenderer.gameObject.SetActive(false);
    }

    public override void Rewind()
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

            velocity = (pointInTime2.position - pointInTime.position) / Time.fixedDeltaTime;
            //velocity = -pointInTime.velocity;

            rb.linearVelocity = velocity;
            //rotVelocity = (rot1 - rot0);

            for (int i = 0; i < pointsInTime.Count; i++)
            {
                lineRenderer.positionCount = pointsInTime.Count;
                lineRenderer.SetPosition(i, pointsInTime[i].position);

            }

        }
        else
        {
            StopRewind();
        }
    }
}

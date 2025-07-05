using System.Collections.Generic;
using UnityEngine;

public class RewindObject : Rewindable
{
    [SerializeField] ParticleSystem rewindEffect;
    [SerializeField] LineRenderer lineRenderer;

    private Rigidbody rb;
    private List<PointInTime> pointsInTime = new();
    private Vector3 velocity;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();

        rewindEffect.Stop();
        lineRenderer.gameObject.SetActive(false);
    }

    public override void Record()
    {
        if (pointsInTime.Count > Mathf.Round(rewindTime / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, rb.linearVelocity));
    }

    public override void Rewind()
    {
        if (pointsInTime.Count > 1)
        {
            PointInTime pointInTime = pointsInTime[0];
            PointInTime pointInTime2 = pointsInTime[1];

            // Only set rotation, position handled by velocity
            transform.rotation = pointInTime.rotation;

            pointsInTime.RemoveAt(0);

            velocity = (pointInTime2.position - pointInTime.position) / Time.fixedDeltaTime;
            rb.linearVelocity = velocity;

            lineRenderer.positionCount = pointsInTime.Count;
            for (int i = 0; i < pointsInTime.Count; i++)
            {
                lineRenderer.SetPosition(i, pointsInTime[i].position);
            }
        }
        else
        {
            StopRewind();
        }
    }

    public override void StartRewind()
    {
        base.StartRewind();
        rb.useGravity = false;

        rewindEffect.Play();
        lineRenderer.gameObject.SetActive(true);
    }

    public override void StopRewind()
    {
        base.StopRewind();
        rb.useGravity = true;
        rb.linearVelocity = -velocity;

        rewindEffect.Stop();
        lineRenderer.gameObject.SetActive(false);

        // NO pointsInTime.Clear() here, as per your original code!
    }
}

using System.Collections.Generic;
using UnityEngine;

public class RewindableBreakableWall : Rewindable
{
    [SerializeField] WallBreakController wallBreakController;
    [SerializeField] List<Rigidbody> fragments;

    private Dictionary<Rigidbody, List<FragInTime>> fragmentPoints = new();
    private Dictionary<Rigidbody, Vector3> lastVelocities = new();

    protected override void Start()
    {
        base.Start();

        foreach (var frag in fragments)
        {
            fragmentPoints[frag] = new List<FragInTime>();
            lastVelocities[frag] = Vector3.zero;
        }
    }
    public void InitializeFragmentsAtBreak()
    {
        foreach (var frag in fragments)
        {
            if (!fragmentPoints.ContainsKey(frag))
                fragmentPoints[frag] = new List<FragInTime>();

            var list = fragmentPoints[frag];

            // Clear any old frames, just in case
            list.Clear();

            PointInTime startPoint = new PointInTime(frag.position, frag.rotation, frag.linearVelocity);
            list.Add(new FragInTime(startPoint, true)); // true = fixed at break start
        }
    }

    public override void Record()
    {
        bool currentlyFixed = wallBreakController.IsWallFixed();  // You need to add this in your controller

        foreach (var frag in fragments)
        {
            //if (!frag.gameObject.activeInHierarchy) continue;

            var list = fragmentPoints[frag];

            if (list.Count > Mathf.Round(rewindTime / Time.fixedDeltaTime))
                list.RemoveAt(list.Count - 1);

            PointInTime p = new PointInTime(frag.position, frag.rotation, frag.linearVelocity);
            list.Insert(0, new FragInTime(p, currentlyFixed));
        }
    }

    public override void Rewind()
    {
        bool hasAnyFrames = false;

        foreach (var frag in fragments)
        {
            if (!frag.gameObject.activeInHierarchy) continue;

            var list = fragmentPoints[frag];
            if (list.Count > 1)
            {
                FragInTime current = list[0];
                FragInTime next = list[1];

                //frag.position = current.point.position; // keep commented if physics driven
                frag.rotation = current.point.rotation;

                Vector3 calculatedVelocity = (next.point.position - current.point.position) / Time.fixedDeltaTime;
                frag.linearVelocity = calculatedVelocity;

                lastVelocities[frag] = calculatedVelocity;

                // You can read current.isFixed here if needed for some logic

                list.RemoveAt(0);
                hasAnyFrames = true;
            }
        }

        if (!hasAnyFrames)
            StopRewind();
    }

    public override void StartRewind()
    {
        base.StartRewind();

        foreach (var frag in fragments)
            frag.useGravity = false;
    }

    public override void StopRewind()
    {
        base.StopRewind();

        foreach (var frag in fragments)
        {
            frag.useGravity = true;

            if (lastVelocities.TryGetValue(frag, out var vel))
                frag.linearVelocity = -vel;
        }

        // Check last frame's fixed state and fix wall if needed
        if (IsLastFrameFixed())
        {
            wallBreakController.FixWall();
        }
    }

    private bool IsLastFrameFixed()
    {
        // Return true only if ALL fragments are fixed in the last recorded frame
        foreach (var frag in fragments)
        {
            var list = fragmentPoints[frag];
            if (list.Count == 0) return false;

            if (!list[0].isFixed) return false;
        }
        return true;
    }
}

public class FragInTime
{
    public PointInTime point;
    public bool isFixed;

    public FragInTime(PointInTime _point, bool _isFixed)
    {
        point = _point;
        isFixed = _isFixed;
    }
}

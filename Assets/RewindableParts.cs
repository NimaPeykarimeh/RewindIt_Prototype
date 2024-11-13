using UnityEngine;

public class RewindableParts : Rewindable
{
    MeshCollider m_Parts;
    [SerializeField] WallBreakController wallBreakController;

    protected override void Start()
    {
        base.Start();
        m_Parts = GetComponent<MeshCollider>();
    }
    public override void StartRewind()
    {
        base.StartRewind();
        m_Parts.isTrigger = true;
    }
    private void OnEnable()
    {
        rb.isKinematic = false;
    }
    public override void StopRewind()
    {
        base.StopRewind();
        wallBreakController.FixWall();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        m_Parts.isTrigger = false;
    }
}

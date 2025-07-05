using System.Linq;
using UnityEngine;

public class WallBreakController : MonoBehaviour
{
    [SerializeField] GameObject baseWall;
    [SerializeField] GameObject[] brokenWalls;
    [SerializeField] BoxCollider BoxCollider;
    [SerializeField] Transform[] originTransform;

    [SerializeField] Vector3[] originPosition;
    [SerializeField] Quaternion[] originRotation;
    [SerializeField] float breakSpeedLimit = 7f;
    [SerializeField] BoxCollider mainWallCollider;
    [SerializeField] private RewindableBreakableWall rewindableBreakableWall;


    private void Start()
    {
        originTransform = new Transform[brokenWalls.Length];
        originPosition= new Vector3[brokenWalls.Length];
        originRotation= new Quaternion[brokenWalls.Length];
        for (int i = 0; i < brokenWalls.Length; i++)
        {
            originTransform[i] = brokenWalls[i].transform;
            originPosition[i] = brokenWalls[i].transform.position;
            originRotation[i] = brokenWalls[i].transform.rotation;
            brokenWalls[i].gameObject.SetActive(false);
        }
        //foreach (GameObject wall in brokenWalls)
        //{
        //    wall.SetActive(false);
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.linearVelocity.magnitude > breakSpeedLimit)
        {
            BreakWall();
        }
        else
        {
            mainWallCollider.enabled = true;
        }
    }
    public bool IsWallFixed()
    {
        // Return true if the wall is currently fixed (e.g. baseWall active and brokenWalls inactive)
        return baseWall.activeSelf && !brokenWalls.Any(w => w.activeSelf);
    }

    private void OnTriggerExit(Collider other)//Check Later
    {
        mainWallCollider.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.contacts[0].otherCollider.attachedRigidbody.linearVelocity.magnitude);
        if (collision.contacts[0].otherCollider.attachedRigidbody.linearVelocity.magnitude > breakSpeedLimit)
        {
            BreakWall();
        }
        
    }
    void BreakWall()
    {
        baseWall.SetActive(false);
        for (int i = 0; i < brokenWalls.Length; i++)
        {

            brokenWalls[i].gameObject.SetActive(true);
            brokenWalls[i].transform.position = originPosition[i];
            brokenWalls[i].transform.rotation = originRotation[i];
        }

        //foreach (GameObject wall in brokenWalls)
        //{
        //    wall.SetActive(true);
        //}
        //BoxCollider.isTrigger = true;
        BoxCollider.enabled = false;
        rewindableBreakableWall.InitializeFragmentsAtBreak();
    }
    public void FixWall()
    {
        baseWall.SetActive(true);


        for (int i = 0; i < brokenWalls.Length; i++)
        {
            brokenWalls[i].transform.position = originPosition[i];
            brokenWalls[i].transform.rotation = originRotation[i];

            brokenWalls[i].gameObject.SetActive(false);
        }
        BoxCollider.enabled = true;

        //foreach (GameObject wall in brokenWalls)
        //{
        //    brokenWalls[i].transform.position = originTransform[i].position;
        //    brokenWalls[i].transform.rotation = originTransform[i].rotation;
        //    wall.SetActive(false);
        //}
        //BoxCollider.isTrigger = true;
    }
}

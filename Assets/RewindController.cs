using UnityEngine;

public class RewindController : MonoBehaviour
{
    [SerializeField] bool isRewinding;
    [SerializeField] float rewindDuration;
    [SerializeField] float rewindTimer;

    [SerializeField] GameObject light_Obj;
    [SerializeField] RewindObject[] allRewindObjects;

    private void Start()
    {
        allRewindObjects = FindObjectsByType<RewindObject>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            light_Obj.SetActive(!light_Obj.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (RewindObject obj in allRewindObjects)
            {
                obj.StartRewind();
            }

        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            foreach (RewindObject obj in allRewindObjects)
            {
                obj.StopRewind();
            }

        }
    }

}

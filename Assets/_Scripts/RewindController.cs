using UnityEngine;

public class RewindController : MonoBehaviour
{
    [SerializeField] bool isRewinding;
    [SerializeField] float rewindDuration;
    [SerializeField] float rewindTimer;

    [SerializeField] GameObject light_Obj;
    [SerializeField] Rewindable[] allRewindObjects;

    private void Awake()
    {
        allRewindObjects = FindObjectsByType<Rewindable>(FindObjectsSortMode.None);
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            light_Obj.SetActive(!light_Obj.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (Rewindable obj in allRewindObjects)
            {
                obj.StartRewind();
            }

        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            foreach (Rewindable obj in allRewindObjects)
            {
                obj.StopRewind();
            }

        }
    }

}

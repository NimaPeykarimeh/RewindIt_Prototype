using UnityEngine;

public class DoorActivation : MonoBehaviour,IActivator
{
    [SerializeField] Transform doorTransform;
    [SerializeField] float openingSpeed = 5f;
    [SerializeField] bool isActivated;
    [SerializeField] bool isOpening;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] Vector3 targetPosition;

    private void Start()
    {
        startingPosition = doorTransform.localPosition;
    }

    public void Activate(bool _isActive)
    {
        if (_isActive != isActivated)
        {
            isActivated = _isActive;
        }
    }

    private void Update()
    {
        if (isActivated) 
        {
            doorTransform.localPosition = Vector3.MoveTowards(doorTransform.localPosition, targetPosition,openingSpeed * Time.deltaTime);
        }
        else
        {
            doorTransform.localPosition = Vector3.MoveTowards(doorTransform.localPosition, startingPosition, openingSpeed * Time.deltaTime);
        }
    }
}

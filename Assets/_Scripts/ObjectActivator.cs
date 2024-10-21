using UnityEngine;

public class ObjectActivator : MonoBehaviour, IActivator
{
    [SerializeField] GameObject objectToActivate;
    bool isActivate;
    public void Activate(bool _isActivated)
    {
        if (isActivate != _isActivated)
        {
            isActivate = _isActivated;
            objectToActivate.SetActive(isActivate);
        }
    }
}

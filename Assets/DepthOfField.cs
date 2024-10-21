using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DepthOfField : MonoBehaviour
{
    [Header("dof Effect")]
    [SerializeField] Volume volume;
    [SerializeField] VolumeProfile volProf;
    public UnityEngine.Rendering.Universal.DepthOfField dofEffect;
    [SerializeField] LayerMask depthLayer;
    [SerializeField] float focusSpeed = 0.125f;

    private void Start()
    {
        volume = FindAnyObjectByType<Volume>();
        volProf = volume.profile;
        volProf.TryGet<UnityEngine.Rendering.Universal.DepthOfField>(out dofEffect);
    }

    private void LateUpdate()
    {
        Transform _cam= Camera.main.transform;
        if (Physics.Raycast(_cam.position, _cam.forward,out RaycastHit _hit,Mathf.Infinity,depthLayer))
        {
            dofEffect.focusDistance.value = Mathf.Lerp(dofEffect.focusDistance.value, _hit.distance, focusSpeed); // kötü kodlanmýþ -ibrahim
        }
    }
}

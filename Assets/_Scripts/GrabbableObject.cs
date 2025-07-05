using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class GrabbableObject : MonoBehaviour
{
    public Material selectedMaterial;
    
    private void Awake()
    {
        gameObject.tag = "Grabbable";
        selectedMaterial = GetComponent<MeshRenderer>().materials[1];
    }
    
}

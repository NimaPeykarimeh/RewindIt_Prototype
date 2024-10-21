using UnityEngine;
using static BoxManager;

public class PressurePlateController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Collider[] hit;

    [SerializeField] LayerMask pressLayer;
    [SerializeField] Vector3 checkBoxSize;
    [SerializeField] Vector3 checkBoxOffset;
    [SerializeField] ConstantForce force;
    [SerializeField] float pushForce = 2f;
    [SerializeField] bool isPressed;
    [SerializeField] GameObject activatedObject;
    IActivator activator;

    [Header("TYPE")]
    [SerializeField] BoxManager.BoxTypeNames boxTypeToActivate;
    [SerializeField] MeshRenderer m_MeshRenderer;
    BoxManager boxManager;
    BoxManager.BoxTypeNames prevBoxType;

    private void Awake()
    {
        //rb =transform.GetChild(0).GetComponent<Rigidbody>();
        activatedObject = transform.parent.Find("ActivatedObject").GetChild(0).gameObject;
        activator = activatedObject.GetComponent<IActivator>();
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + checkBoxOffset, checkBoxSize);
    }

    private void OnValidate()
    {
        m_MeshRenderer ??= transform.GetChild(0).GetComponent<MeshRenderer>();
        boxManager ??= FindFirstObjectByType<BoxManager>();

        if (boxTypeToActivate != prevBoxType)
        {
            prevBoxType = boxTypeToActivate;
            SetValues();
        }

    }

    void SetValues()
    {
        if (boxManager!=null)
        {
            m_MeshRenderer.material = boxManager.GetBoxMaterial(boxTypeToActivate);
            print("VALUE CHANGED");
        }
    }

    void Update()
    {
        hit = Physics.OverlapBox(transform.position + checkBoxOffset, checkBoxSize / 2f, transform.rotation, pressLayer);
        if (hit.Length > 0)
        {
            if (!isPressed )
            {
                foreach (Collider _collider in hit)
                {
                    BoxTypeNames _type = _collider.GetComponent<BoxController>().boxType;
                    if (_type == boxTypeToActivate || _type == BoxTypeNames.White)
                    {
                        print("PRESSED");
                        //rb.isKinematic = true;
                        isPressed = true;
                        activator.Activate(true);
                        force.force = Vector3.zero;
                    }
                }
                
            }
        }
        else
        {
            if (isPressed)
            {
                //rb.isKinematic = false;
                isPressed = false;
                activator.Activate(false);
                force.force = Vector3.up * pushForce;
            }
        }
    }
}

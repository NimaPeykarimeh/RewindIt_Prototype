using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField] float grabRange;
    [SerializeField] Vector3 followPosition;
    [SerializeField] float holdDistance;
    [SerializeField] LayerMask grabLayer;
    [SerializeField] GameObject grabbedObject;
    [SerializeField] Transform grabVisualizer;
    [SerializeField] bool isGrabbed;
    [Header("Grab Speed")]
    [SerializeField] float rotateSpeed;
    [SerializeField] float followSpeed;
    [SerializeField] float maxFollowSpeed = 5;
    [SerializeField] float minFollowSpeed = 1;
    Rigidbody currentObjectRb;
    [SerializeField] float throwPower = 10;
    [Header("Hover")]
    [SerializeField]bool isHovering;
    public GameObject currentHoveredObject;
    [SerializeField]GrabbableObject currentGrabbable;
    [SerializeField] float _vel;
    [SerializeField] GameObject selected;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundOffset = 1f;
    float rayToGroundRange;
    [SerializeField] bool onGround;
    [SerializeField] float angularVel;
    [SerializeField] float maxAngulerVel = 5f;
    [SerializeField] float rotationSpeed;
    [SerializeField] float rotSped;
    [SerializeField] Vector3 orAngle;
    [SerializeField] float rotDur = 0.5f;
    [SerializeField] float rotTimr;
    [SerializeField] Vector3 _lastKnownDir;
    public void GrabObject(bool _isGrabbed)
    {
        if (isGrabbed != _isGrabbed)
        {
            isGrabbed = _isGrabbed;
            if (isGrabbed)
            {
                currentObjectRb.useGravity = false;
                //currentGrabbable.selectedMaterial.SetInt("_IsSelected", 1);
            }
            else if (!isGrabbed && currentObjectRb != null)
            {
                currentObjectRb.useGravity = true;
                currentObjectRb = null;
                grabbedObject = null;
                //currentGrabbable.selectedMaterial.SetInt("_IsSelected", 0);
            }
        }
        
    }

    void CheckHover()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit _hit, grabRange, grabLayer) && !isGrabbed)
        {
            GameObject _hoverObject = _hit.collider.gameObject;
            //holdDistance = Vector3.Distance(Camera.main.transform.position, _hoverObject.transform.position);

            if (_hoverObject != currentHoveredObject)
            {
                if (_hoverObject.CompareTag("Grabbable"))
                {
                    if (currentGrabbable)
                    {
                        currentGrabbable.selectedMaterial.SetInt("_IsSelected", 0);
                    }

                    currentHoveredObject = _hoverObject;
                    currentGrabbable = currentHoveredObject.GetComponent<GrabbableObject>();
                }
            }
            else
            {
                isHovering = true;
                currentGrabbable.selectedMaterial.SetInt("_IsSelected", 1);
            }

        }
        else if (currentHoveredObject && !isGrabbed)
        {
            isHovering = false;
            currentGrabbable.selectedMaterial.SetInt("_IsSelected", 0);
            currentGrabbable = null;
            currentHoveredObject = null;
        }
    }
    void Update()
    {
        CheckHover();
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Physics.Raycast(ray, out RaycastHit _hit, grabRange, grabLayer);
        if (Input.GetMouseButtonDown(0))
        {
            if (currentGrabbable)
            {
                if (currentHoveredObject.CompareTag("Grabbable") && isHovering)
                {
                    
                    currentObjectRb = _hit.rigidbody;
                    grabbedObject = currentHoveredObject;
                    currentGrabbable = grabbedObject.GetComponent<GrabbableObject>();

                    rotTimr = rotDur;
                    orAngle = currentObjectRb.angularVelocity;
                    //currentGrabbable.selectedMaterial.SetFloat(1f);
                    //grabVisualizer.transform.position = grabbedObject.transform.position;
                    GrabObject(true);
                }

            }

        }
        else if(Input.GetMouseButtonUp(0) && isGrabbed)
        {
            currentObjectRb.linearVelocity = Vector3.zero;
            GrabObject(false);
        }
        if (isGrabbed && Input.GetMouseButtonDown(1))
        {
            currentObjectRb.linearVelocity = Camera.main.transform.forward * throwPower;
            GrabObject(false);
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 _origin = Vector3.zero;
        if (grabbedObject) 
        {
            _origin = currentGrabbable.transform.position; 
        }
        
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_origin, Vector3.down * 3f);
        
        Gizmos.DrawLine(_origin, _origin + (Vector3.down * 3f));
        Gizmos.DrawWireSphere(followPosition, 0.3f);

        if (Physics.Raycast(_origin,Vector3.down, out RaycastHit _hit, 3f, groundLayer))
        {
            Gizmos.DrawWireSphere(_hit.point, 0.3f);
        }
    }
    void CheckGround()
    {
        if (onGround)
        {
            Vector3 _origin = Vector3.zero;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit _hit, grabRange + 2f, groundLayer))
            {
                _origin = _hit.point;
                followPosition = _hit.point + groundOffset * Vector3.up;
            }
        }
        else
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.SphereCast(ray.origin,groundOffset,ray.direction,out RaycastHit _hit,grabRange, groundLayer))
            {

                followPosition = ray.GetPoint(_hit.distance);
                _lastKnownDir = ray.direction;
                holdDistance = _hit.distance;
            }
            else
            {
                holdDistance = grabRange;
            }

        }
    }

    void AlignCube()
    {
        Quaternion currentRotation = currentObjectRb.rotation;

        // Convert current rotation to Euler angles
        Vector3 currentEulerAngles = currentRotation.eulerAngles;
        // Calculate the target Euler angles as the nearest multiple of 90 degrees
        Vector3 targetEulerAngles = new Vector3(
            Mathf.Round(currentEulerAngles.x / 90) * 90,
            Mathf.Round(currentEulerAngles.y / 90) * 90,
            Mathf.Round(currentEulerAngles.z / 90) * 90
        );

        // Define the target rotation using the calculated Euler angles
        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

        // Interpolate between the current rotation and the target rotation
        Quaternion newRotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // Calculate the angular velocity needed to reach the new interpolated rotation
        Quaternion deltaRotation = newRotation * Quaternion.Inverse(currentRotation);
        deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);

        // Convert the angle to radians per second and apply to angular velocity
        float angularVelocityMagnitude = angle * Mathf.Deg2Rad / Time.fixedDeltaTime;
        Vector3 angularVelocity = axis.normalized * angularVelocityMagnitude;

        rotTimr -= Time.fixedDeltaTime;
        rotTimr = Mathf.Clamp(rotTimr, 0f, rotDur);
        // Apply angular velocity to the Rigidbody
        currentObjectRb.angularVelocity = Vector3.Lerp(orAngle, angularVelocity, 1f - (rotTimr / rotDur));
    }

    void Grabbing()
    {
        if (isGrabbed)
        {
            //Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            followPosition = ray.GetPoint(holdDistance);
            CheckGround();

            Vector3 _DirectionToPoiont = followPosition - grabbedObject.transform.position;
            float _distance = _DirectionToPoiont.magnitude;
            _vel = Mathf.Clamp(followSpeed * _distance, minFollowSpeed, maxFollowSpeed);
            currentObjectRb.linearVelocity = _DirectionToPoiont.normalized * Mathf.Clamp(followSpeed * _distance, minFollowSpeed, maxFollowSpeed);

            angularVel = currentObjectRb.angularVelocity.magnitude;
            if (angularVel < maxAngulerVel)
            {
                AlignCube();
            }
            else
            {
                rotTimr = rotDur;
                orAngle = currentObjectRb.angularVelocity;
            }
        }
    }

    private void FixedUpdate()
    {
        Grabbing();
    }
}

using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DecalFollow : MonoBehaviour
{
    [SerializeField] Transform decalTransform;
    [SerializeField] bool up;
    [SerializeField] bool right;
    [SerializeField] bool forward;

    [SerializeField] float upF;
    [SerializeField] float rightF;
    [SerializeField] float forwardF;
    [SerializeField] float _angle;
    [SerializeField] float _targetAngle;
    [SerializeField] Vector3 _rotation;
    Vector3 _cross;
    float currentAngle;
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] Axis currentAxis;
    [SerializeField] Vector3 angleFrom;
    [SerializeField] float angleOffset;
    [SerializeField] bool doOffset;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector3 boxSize;
    enum Axis
    {
        Up,
        Right,
        Forward
    }

    void SwitchAngle(Axis _axis)
    {
        if (currentAxis != _axis)
        {
            Vector3 t_right = transform.right;
            t_right.y = 0;
            float _up = Vector3.SignedAngle(t_right, Vector3.right, Vector3.up);

            Vector3 t_forward = transform.forward;
            t_forward.y = 0;
            float _right = Vector3.SignedAngle(t_forward, Vector3.right, Vector3.up);

            Vector3 t_up = transform.up;
            t_up.y = 0;
            float _forward = Vector3.SignedAngle(t_up, Vector3.right, Vector3.up);
            if (_axis == Axis.Up)
            {
                if (currentAxis == Axis.Right)
                {
                    angleOffset += _up - _right;
                    
                }
                else if (currentAxis == Axis.Forward) 
                {
                    angleOffset += _up - _forward;
                }
                currentAxis = _axis;
            }

            else if (_axis == Axis.Right)
            {
                if (currentAxis == Axis.Up)
                {
                    angleOffset += _right - _up;
                }
                else if (currentAxis == Axis.Forward)
                {
                    angleOffset += _right - _forward;
                }
                currentAxis = _axis;
            }

            else if (_axis == Axis.Forward)
            {
                if (currentAxis == Axis.Right)
                {
                    angleOffset += _forward - _right;
                }
                else if (currentAxis == Axis.Up)
                {
                    angleOffset += _forward - _up;
                }
                currentAxis = _axis;
            }
            angleOffset = Mathf.Round(angleOffset / 90f) * 90f;
            //_angle -= angleOffset;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position,boxSize * 2f);
    }
    private void LateUpdate()
    {
        _rotation = new Vector3(Mathf.RoundToInt(transform.localEulerAngles.x), Mathf.RoundToInt(transform.localEulerAngles.y), Mathf.RoundToInt(transform.localEulerAngles.z)) ;

        upF = Mathf.Abs(Vector3.Dot(Vector3.up, transform.up));
        rightF = Mathf.Abs(Vector3.Dot(Vector3.up, transform.right));
        forwardF = Mathf.Abs(Vector3.Dot(Vector3.up, transform.forward));

        up = upF > rightF && upF > forwardF;
        right = rightF > forwardF && rightF > upF;
        forward = forwardF > upF && forwardF > rightF;

        if (up)
        {
            SwitchAngle(Axis.Up);
            //_cross = Vector3.Cross(Vector3.up, transform.up);

            Vector3 t_right = transform.right;
            t_right.y = 0;

            _angle = Vector3.SignedAngle(t_right, Vector3.right, Vector3.up);

            //_angle = Mathf.Acos(Vector3.Dot(transform.right, Vector3.right) ) * Mathf.Rad2Deg;
        }
        else if (right)
        {
            SwitchAngle(Axis.Right);
            //_cross = Vector3.Cross(Vector3.up, transform.right);
            Vector3 t_forward = transform.forward;
            t_forward.y = 0;
            _angle = Vector3.SignedAngle(t_forward, Vector3.right, Vector3.up);
        }
        else if (forward)
        {
            SwitchAngle(Axis.Forward);
            Vector3 t_up = transform.up;
            t_up.y = 0;
            //_cross = Vector3.Cross(Vector3.up, transform.forward);
            _angle = Vector3.SignedAngle(t_up, Vector3.right, Vector3.up);
        }
        if (_angle<0)
        {
            _angle += 360f;
        }
        if (doOffset)
        {
         _targetAngle = _angle - angleOffset;
        currentAngle = Mathf.LerpAngle(currentAngle, _targetAngle, rotationSpeed * Time.deltaTime);

        }
        else
        {
            _targetAngle = _angle;
            currentAngle = _targetAngle;
        }
        

        //decalTransform.rotation = Quaternion.LookRotation(_cross);
       
        decalTransform.eulerAngles = new Vector3(0f, -currentAngle, 0f);

        //decalTransform.eulerAngles = new Vector3(0f, _rotation.y, 0f);

        Vector3 _pos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z) ;
        
        if (Physics.BoxCast(transform.position, boxSize, Vector3.down, out RaycastHit hit, Quaternion.identity, 50f, groundLayer))
        {
            _pos.y = transform.position.y - hit.distance;
        }

        decalTransform.position = Vector3.MoveTowards(decalTransform.position, _pos, 0.1f);
        

    }
    

    
}

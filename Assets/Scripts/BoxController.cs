using System;
using UnityEngine;


public class BoxController : MonoBehaviour
{
    BoxManager boxManager;

    BoxManager.BoxTypeNames prevBoxType;
    public BoxManager.BoxTypeNames boxType;

    private RewindObject rewindObject;
    private MeshRenderer m_MeshRenderer;
    private MeshRenderer r_MeshRenderer;
    private void OnValidate()
    {
        m_MeshRenderer ??= GetComponent<MeshRenderer>();
        rewindObject ??= GetComponent<RewindObject>();
        r_MeshRenderer ??= transform.Find("RingHolder").Find("Plane").gameObject.GetComponent<MeshRenderer>();
        boxManager ??= FindFirstObjectByType<BoxManager>();
        if (boxType != prevBoxType)
        {
            prevBoxType = boxType;
            SetValues();
        }
    }

    private void Start()
    {
        Color _color= m_MeshRenderer.materials[0].GetColor("_BaseColor");
        GameObject _plane = transform.Find("RingHolder").Find("Plane").gameObject;
        _plane.GetComponent<MeshRenderer>().material.SetColor("_MainColor", _color);
    }
    void SetValues()
    {
        if (boxManager != null)
        {
            m_MeshRenderer.material = boxManager.GetBoxMaterial(boxType);
            r_MeshRenderer.material = boxManager.GetRingMaterial(boxType);
            print("VALUE CHANGED");
        }
        else
        {
            print("NO BOX MANAGER FOUND");
        }
    }

}

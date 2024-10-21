using System;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager:MonoBehaviour
{
    [SerializeField] BoxType[] boxTypeList;

    public enum BoxTypeNames
    {
        None,
        Orange,
        Red,
        White,
        Blue,
        Green,
        Black,
    }

    
    [Serializable]
    public class BoxType
    {
        public BoxTypeNames typeName;
        public Material typeMaterial;
        public Material ringMaterial;
    }

    public Material GetBoxMaterial(BoxTypeNames _name)
    {
        foreach (BoxType _type in boxTypeList)
        {
            if (_type.typeName == _name)
            {
                return _type.typeMaterial;
            }
        }
        Debug.LogError("NO Box MATERIAL FOUND");
        return null;
    }

    public Material GetRingMaterial(BoxTypeNames _name)
    {
        foreach (BoxType _type in boxTypeList)
        {
            if (_type.typeName == _name)
            {
                return _type.ringMaterial;
            }
        }
        Debug.LogError("NO Ring MATERIAL FOUND");
        return null;
    }
}

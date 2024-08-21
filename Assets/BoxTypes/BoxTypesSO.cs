using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "BoxTypes", menuName = "Scriptable Objects/BoxTypes")]
public class BoxTypesSO : ScriptableObject
{
    public bool Rewindable = true;
    public boxColors boxColor;
    public Material boxMaterial;

    public enum boxColors
    {
        Red,
        Orange,
        Black,
        White
    } 
}

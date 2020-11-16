using UnityEngine;

[System.Serializable]
public class AttackZone
{
    public bool showZone;
    public Color color = Color.blue;

    //public LayerMask layerMask;
    public float distance;
    public float angleOffset;
    public Vector3 cubeSize;

}
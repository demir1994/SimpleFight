using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEditor;
using UnityEngine;

public class WalkableZone : MonoBehaviour
{
    public Transform min_posX;
    public Transform max_posX;
    public Transform min_posZ;
    public Transform max_posZ;

    //public Color drawColor;

    //private void OnDrawGizmosSelected()
    //{
    //    // Draw a color at visible moveable position
    //    Gizmos.color = new Color(drawColor.r, drawColor.g, drawColor.b, drawColor.a);

    //    Gizmos.DrawCube(walkableArea_Position, new Vector3(walkableArea_Scale.x, walkableArea_Scale.y, walkableArea_Scale.z));
    //}

}

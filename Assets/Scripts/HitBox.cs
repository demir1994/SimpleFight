using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    /// <summary>
    /// Hit box collider
    /// </summary>
    public BoxCollider _thisCollider;

    private void Start()
    {
        _thisCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC")
        {
            _thisCollider.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC")
        {
            _thisCollider.enabled = false;
        }
    }
}

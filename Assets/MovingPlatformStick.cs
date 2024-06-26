using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformStick : MonoBehaviour
{
    private GameObject target = null;
    private Vector3 offset;

    void Start()
    {
        target = null;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        target = coll.gameObject;
        offset = target.transform.position - transform.position;
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        target = null;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            target.transform.position = transform.position + offset;
        }

    }
}

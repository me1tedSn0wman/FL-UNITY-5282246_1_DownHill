using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityOne : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Physics.gravity = -1 *this.gameObject.transform.up * 20;
        }
    }
}

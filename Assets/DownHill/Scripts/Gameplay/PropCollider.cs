using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PropCollider : MonoBehaviour
{
    [SerializeField] private bool isHaveBeenActivated;
    [SerializeField] private int scorePoints = 50;

    public void Awake()
    {
        isHaveBeenActivated = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (isHaveBeenActivated) return;
        if (collision.gameObject.CompareTag("Player")) {
            isHaveBeenActivated = true;
            GameplayManager.Instance.AddScore(scorePoints);
        }
    }
}

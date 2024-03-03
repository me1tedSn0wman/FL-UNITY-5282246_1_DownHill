using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PropPickUp : MonoBehaviour
{
    [SerializeField] private int scorePoints = 50;
    [SerializeField] private float rotateSpeed = 2f;

    public void Awake()
    {
        transform.Rotate(0, Random.Range(0, 180), 0, Space.Self);
    }
    public void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            GameplayManager.Instance.AddScore(scorePoints);
            GameplayManager.Instance.PlayPickUpSound();
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.Self);
    }
}

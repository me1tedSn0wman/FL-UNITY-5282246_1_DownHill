using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DeathTrigger : MonoBehaviour
{
    [SerializeField] private bool isHaveBeenActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isHaveBeenActivated && GameManager.Instance.gameState == GameState.Gameplay)
        {
            isHaveBeenActivated = true;
            GameplayManager.Instance.FinishLevel(false);
        }
    }
}

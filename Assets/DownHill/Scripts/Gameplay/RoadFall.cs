using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RoadFall : MonoBehaviour
{
    [SerializeField] private List<RoadFallPiece> roadFallPieces;
    [SerializeField] private List<RoadFallPiece> roadLastFallPieces;
    [SerializeField] private float startFalling=-1f;
    [SerializeField] private float fallingDuraton = 1f;
    [SerializeField] private float fallingLastPiecesDuraton = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
//            Debug.Log("Player");
            StartFall();
        }
    }

    public void StartFall() {
        startFalling = Time.time;
//        Debug.Log(startFalling);
    }

    public void Update()
    {
        if (startFalling == -1f) return;
        if (Time.time > startFalling + fallingDuraton)
        {
            int count = roadFallPieces.Count;
            if (count > 0)
            {
                int newPieceInd = Random.Range(0, count);
                roadFallPieces[newPieceInd].StartFall();
                roadFallPieces.RemoveAt(newPieceInd);
                startFalling = Time.time;
                return;
            }
        }
        else return;
        if (Time.time > startFalling + fallingLastPiecesDuraton) { 
            int count = roadLastFallPieces.Count;
            if (count > 0) {
                int newPieceInd = Random.Range(0, count);
                roadLastFallPieces[newPieceInd].StartFall();
                roadLastFallPieces.RemoveAt(newPieceInd);
                startFalling = Time.time;
                return;
            }
            startFalling = -1;
        }
    }
}

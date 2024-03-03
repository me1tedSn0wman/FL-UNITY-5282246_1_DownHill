using System;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Some Car Reference", menuName = "Scriptable Objects/Car Management/Car Reference", order = 1)]
public class CarReference : ScriptableObject
{
    public int carId;
    public string carName;
    public bool isBlocked;
    public int carPrice;
    public GameObject carPrefab;
    public GameObject carMeshPrefab;
}

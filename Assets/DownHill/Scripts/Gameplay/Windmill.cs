using UnityEngine;

public class Windmill : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public Vector3 rotateAroundVector;

    public void Update()
    {
        this.transform.Rotate(rotationSpeed * Time.deltaTime * rotateAroundVector, Space.Self);
    }
}

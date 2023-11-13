using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed;

    // Rotates the gameobject at specified speed;
    void FixedUpdate()
    {
        transform.Rotate(0f, 0f, speed * Time.deltaTime);
    }
}

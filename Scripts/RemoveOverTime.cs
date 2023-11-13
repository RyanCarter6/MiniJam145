using UnityEngine;

public class RemoveOverTime : MonoBehaviour
{
    [SerializeField] private float time;

    // Removes gameobject after a designated amount of time
    void Start()
    {
        Destroy(gameObject, time);
    }

}

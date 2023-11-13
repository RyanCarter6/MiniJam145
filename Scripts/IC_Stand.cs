using System.Collections;
using UnityEngine;

public class IC_Stand : MonoBehaviour
{
    [SerializeField] private GameObject icProj;
    [SerializeField] private Transform spawnRotation;
    [SerializeField] private float projForce;

    // Calls spawn ice cream method to happen at random intervals
    void Start()
    {
        StartCoroutine(SpawnIceCream());
    }

    // Shoots out an ice cream for the player to grab 
        //(refills player health when grabbed)
    IEnumerator SpawnIceCream()
    {
        // Waits a random amount of time before spawning a projectile
        yield return new WaitForSeconds(Random.Range(3.5f, 6f));

        // Spawns ice cream projectile and adds force to it
        GameObject projRef = Instantiate(icProj, gameObject.transform.position, spawnRotation.rotation);
        projRef.GetComponent<Rigidbody2D>().AddForce(projRef.transform.up * projForce);

        // Recursive call
        StartCoroutine(SpawnIceCream());
    }
}

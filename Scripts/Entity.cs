using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Fields all entities use
    private int hp = 0;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;

    // Returns hp for the entity
    public int GetHP()
    {
        return hp;
    }

    // Returns speed for the entity
    public Rigidbody2D GetRB()
    {
        return rb;
    }

    // Returns animator for the entity
    public Animator GetAnimator()
    {
        return animator;
    }

    // Returns speed for the entity
    public float GetSpeed()
    {
        return speed;
    }

    // Returns speed for the entity
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Adds health to the entity, used to initialize hp and for regen
    public void AddHP(int amount)
    {
        hp += amount;
    }

    // Removes health
    public void TakeDamage(int damage)
    {
        hp -= damage;

        // If entity should perish
        if(hp <= 0)
        {
            Perish();
        }
        // If still alive, show damage was taken
        else
        {
            AudioManager.i.Play("Hit");
            StartCoroutine(HighlightEntity());
        }
    }

    // Makes the entity briefly become red to show damage was taken
    IEnumerator HighlightEntity()
    {
        sr.color = Color.red;

        yield return new WaitForSeconds(0.25f);

        sr.color = Color.white;
    }

    // Called if an entity should perish
    void Perish()
    {
        // Stops time since the game ends after either the player or boss is defeated
        Time.timeScale = 0f;
        animator.SetTrigger("Perish");
    }
}

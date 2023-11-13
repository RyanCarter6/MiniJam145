using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : Entity
{
    // Makes PlayerController accessible from other scripts
    private static PlayerController instance;
    public static PlayerController i { get { return instance; } }

    // Ice cream related vars
    [SerializeField] private Transform icHolder;        // Ref to gameobject that holds ice cream in the cone
    [SerializeField] private GameObject icPrefab;       // Ref to ice cream prefab     
    [SerializeField] List<GameObject> icList = new List<GameObject>();   // Ref to all ice creams on the cone

    // General movement vars
    [SerializeField] private float jumpForce;
    [SerializeField] private CollisionCheck groundCheck;
    [SerializeField] private bool canDoubleJump = true;

    // Player laser (projectile)
    [SerializeField] private GameObject laserProj;
    [SerializeField] private float projForce;

    // Provides brief damage immunity to the player after being hit
    float nextDamage = 0f;

    // Initializes variables, called at start of game
    void Awake()
    {
        // Sets static reference
        instance = gameObject.GetComponent<PlayerController>();

        // Player starts with 3 hp/ice creams
        for (int i = 0; i < 3; i++)
        {
            AddIceCream();
        }

        // Sets reference to function for CollisionCheck
        groundCheck.SetFunctionToCall("AddJump");
    }

    // Called when damaged by boss
    public void TakeBossDamage()
    {
        if(Time.time > nextDamage && GetHP() > 0)
        {
            RemoveIceCream();
            TakeDamage(1);
            nextDamage = Time.time + 0.25f;
        }
    }
    
    // Removes an ice cream from cone
    void RemoveIceCream()
    {
        // Checks ice cream list would be empty
        if(icList.Count > 0)
        {
            // Removes the bottom ice cream from the list and game
            icHolder.position = new Vector2(icHolder.position.x, icHolder.position.y - .4375f);
            Destroy(icList[icList.Count - 1]);
            icList.Remove(icList[icList.Count - 1]);
        }
    }

    // Called on player input
    void FireLaser()
    {
        TakeDamage(1);
        RemoveIceCream();
        
        // Spawn laser if player lived after taking damage
        if(GetHP() > 0)
        {
            SpawnLaser();
        }
    }

    // Instantiates a laser after the requirements are met
    void SpawnLaser()
    {
        // Play sound
        AudioManager.i.Play("Shoot");

        // Gets what rotation the laser should spawn at
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Spawns laser and makes it move
        GameObject projRef = Instantiate(laserProj, transform.position, Quaternion.Euler(new Vector3(0, 0, rotationZ)));
        projRef.GetComponent<Rigidbody2D>().AddForce(projRef.transform.right * projForce);
    }

    // Called when player picks up ice cream
    public void AddIceCream()
    {
        // Gives player one more hp
        AddHP(1);

        // If list isn't empty
        if(icList.Count > 0)
        {
            // Adds ice cream onto the cone
            GameObject ic = Instantiate(icPrefab, new Vector2(gameObject.transform.position.x, icList.First().transform.position.y + 0.4375f),
                            Quaternion.identity, icHolder);
            icList.Insert(0, ic);
        }
        // Special case for first item in list
        else
        {
            // Adds ice cream onto the cone
            GameObject ic = Instantiate(icPrefab, new Vector2(gameObject.transform.position.x, 0), Quaternion.identity, icHolder);
            icList.Insert(0, ic);
        }
    }

    // Called once per frame to get player input
    void Update()
    {
        // Jump on W
        if(Input.GetButtonDown("Vertical"))
        {
            Jump();
        }

        // Rotate player to face movement
        Rotate(Input.GetAxis("Horizontal"));

        // Sets animator
        GetAnimator().SetFloat("Speed", Input.GetAxis("Horizontal"));

        // Shoot on LMB
        if(Input.GetButtonDown("Fire1"))
        {
            FireLaser();
        }
    }
    
    // Called every fixed-frame to get player input
    void FixedUpdate()
    {
        // Move Left on A and Right on D
        GetRB().AddForce(new Vector2(Input.GetAxis("Horizontal") * GetSpeed(), GetRB().velocity.y));
    }
    
    // Makes the player face the right direction
    void Rotate(float dir)
    {
        // Right
        if(dir >= 0.1)
        {
            transform.localScale = new Vector2(1, 1);
        }
        // Left
        else if (dir <= -0.1)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }
    
    // Checks jump conditions + makes player jump
    void Jump()
    {
        // Single jump
        if(groundCheck.CheckCollision())
        {
            GetRB().AddForce(new Vector2(GetRB().velocity.x, jumpForce));
        }
        // Double jump
        else if(canDoubleJump)
        {
            GetRB().AddForce(new Vector2(GetRB().velocity.x, jumpForce));
            canDoubleJump = false;
        }
    }

    // Resets player's ability to double jump
    public void AddJump()
    {
        canDoubleJump = true;
    }

    // Opens death screen, called in death animation
    public void OpenGameOverUI()
    {
        AudioManager.i.Stop("Song");
        AudioManager.i.Play("GameOver");
        GameInterface.i.OpenGameUI(false);
    }
}


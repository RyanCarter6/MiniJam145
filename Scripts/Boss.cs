using System.Collections;
using UnityEngine;

public class Boss : Entity
{
    // Makes Boss accessible from other scripts
    private static Boss instance;
    public static Boss i { get { return instance; } }

    // For cherry attack
    [SerializeField] private GameObject cherry;
    [SerializeField] private Transform[] cherrySpawns;
    [SerializeField] private float cherryForce;

    // For spoon attack
    [SerializeField] private GameObject spoon;
    [SerializeField] private Transform[] spoonSpawns;
    [SerializeField] private float spoonForce;

    // For slam/shockwave attack
    [SerializeField] private GameObject shockwave;
    [SerializeField] private Transform[] shockwaveSpawns;
    [SerializeField] private float followSpeed;
    [SerializeField] private float slamSpeed;
    [SerializeField] private float shockwaveSpeed;
    bool isSlamAttacking;
    bool followingPlayer;

    // Reference to the y value the boss should be near when moving
    [SerializeField] private Transform bossMoveRef;
    private float dirChangeX, dirChangeY;

    // Called at the beginning of the game
    void Awake()
    {
        // Sets static reference
        instance = gameObject.GetComponent<Boss>();

        // Sets starting hp
        AddHP(15);

        // Sets up attack pattern
        StartCoroutine(Attack(-1));
        
        // Randomly changes the direction the boss is moving
        StartCoroutine(ChangeDirection());
    }

    // Randomly changes the direction the boss should move
    IEnumerator ChangeDirection()
    {
        dirChangeX = Random.Range(-4f, 4f);
        dirChangeY = Random.Range(-2f, 2f);

        yield return new WaitForSeconds(Random.Range(0.5f, 1f));

        // Recursive call
        StartCoroutine(ChangeDirection());
    }

    // Used for boss movement
    void FixedUpdate()
    {
        // Normal movement
        if(!isSlamAttacking)
        {
            // Moves above player at a slowish speed
            GetRB().position = Vector2.MoveTowards(GetRB().position, new Vector2(PlayerController.i.transform.position.x + dirChangeX, 
                                    bossMoveRef.position.y + dirChangeY), Time.deltaTime * GetSpeed());
        }
        // If boss is doing slam attack
        else
        {
            // If boss should be following player during slam attack
            if(followingPlayer)
            {
                // Moves boss quickly above player
                GetRB().position = Vector2.MoveTowards(GetRB().position, new Vector2(PlayerController.i.transform.position.x, 
                                    PlayerController.i.transform.position.y + 8f), Time.deltaTime * followSpeed);
            }
        }
    }

    // Calls attack methods
    IEnumerator Attack(int lastAttack)
    {
        // Attacks at random interval
        yield return new WaitForSeconds(Random.Range(3.5f, 5.5f));

        // Rolls random num
        int attackNum = Random.Range(0, 3);

        // If the attack is the same as the last, roll until a new attack is chosen
        while(attackNum == lastAttack)
        {
            attackNum = Random.Range(0, 3);
        }

        // Randomly selects attacks
        switch (attackNum)
        {
            // Case for cherry attack (burst)
            case 0:
                AttackCherry();
                break;
            // Case for spoon attack (homing)
            case 1:
                AttackSpoon();
                break;
            // Case for slam/shockwave attack (slam into shockwave)
            case 2:
                AttackSlam();
                break;
        }

        // Recursive call
        StartCoroutine(Attack(attackNum));
    }

    // Spawns cherries at all cherry spawn points
    void AttackCherry()
    {
        // Play sound
        AudioManager.i.Play("BossAttack");

        for (int i = 0; i < cherrySpawns.Length; i++)
        {
            // Adds cherry
            GameObject projRef = Instantiate(cherry, cherrySpawns[i].position, Quaternion.identity);

            // Adds small force to cherry
            projRef.GetComponent<Rigidbody2D>().AddForce(-cherrySpawns[i].up * cherryForce, ForceMode2D.Force);

            // Starts counter to add speed to cherry
            StartCoroutine(AddSpeed(projRef.GetComponent<Rigidbody2D>(), cherrySpawns[i]));
        }
    }

    // Adds speeds to cherries after a delay
    IEnumerator AddSpeed(Rigidbody2D rb, Transform spawn)
    {
        yield return new WaitForSeconds(1f);

        if(rb != null) 
        {
            rb.AddForce(-spawn.up * cherryForce * 11.5f, ForceMode2D.Force);
        }
    }

    // Spawns spoons at both spoon spawn points
    void AttackSpoon()
    {
        // Play sound
        AudioManager.i.Play("BossAttack");

        for (int i = 0; i < spoonSpawns.Length; i++)
        {
            // Adds spoon
            GameObject projRef = Instantiate(spoon, spoonSpawns[i].position, Quaternion.identity, gameObject.transform);

            // Fires the spoon after a delay
            StartCoroutine(ShootSpoon(i + 0.75f, projRef.GetComponent<Rigidbody2D>()));
        }
    }

    // Shoots the spoon towards the player
    IEnumerator ShootSpoon(float delay, Rigidbody2D rb)
    {
        yield return new WaitForSeconds(delay);

        // Get player position then shoot
        if(rb != null) 
        {
            rb.gameObject.transform.parent = null;
            rb.velocity = (PlayerController.i.transform.position - rb.transform.position).normalized * spoonForce;
        }
    }

    // Makes boss follow above player
    void AttackSlam()
    {
        isSlamAttacking = followingPlayer = true;

        StartCoroutine(DropBoss());
    }

    // Drops boss down then shoots shockwave out
    IEnumerator DropBoss()
    {
        yield return new WaitForSeconds(1.5f);

        // Stops following above player and moves down
        followingPlayer = false;
        GetRB().AddForce(Vector2.down * slamSpeed, ForceMode2D.Force);

        yield return new WaitForSeconds(0.9f);

        // Play sound
        AudioManager.i.Play("BossAttack");

        // Spawns shockwave
        for (int i = 0; i < shockwaveSpawns.Length; i++)
        {
            GameObject projRef = Instantiate(shockwave, shockwaveSpawns[i].position, shockwaveSpawns[i].rotation);
            projRef.GetComponent<Rigidbody2D>().AddForce(projRef.transform.right * shockwaveSpeed, ForceMode2D.Force);
        }

        // Returns to normal movement
        GetRB().velocity = Vector2.zero;
        isSlamAttacking = false;

        // Gives boss a speed boost to get back above player quickly
        StartCoroutine(RemoveSpeedBoost(GetSpeed()));
        SetSpeed(GetSpeed() * 3f);
    }

    // Removes boss speed boost after slam
    IEnumerator RemoveSpeedBoost(float oldSpeed)
    {
        yield return new WaitForSeconds(0.75f);
        SetSpeed(oldSpeed);
    }

    // Checks for collision on player
    void OnTriggerEnter2D(Collider2D entity)
    {
        // Damages the player on collision
        if(entity.gameObject.layer == 3)
        {
            PlayerController.i.TakeBossDamage();
        }
    }

    // Opens win screen, called in death animation
    public void OpenWinScreenUI()
    {
        AudioManager.i.Stop("Song");
        AudioManager.i.Play("GameWon");
        GameInterface.i.OpenGameUI(true);
    }
}

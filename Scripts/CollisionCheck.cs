using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    [SerializeField] private int layerSelection;
    private bool isColliding;
    [SerializeField] private string funcToCall;

    // Checks for collision
    void OnTriggerEnter2D(Collider2D entity)
    {
        if(entity.gameObject.layer == layerSelection)
        {
            isColliding = true;

            // If a function should be called on collision
            if(funcToCall != string.Empty)
            {
                CallFunctionOnCollision();
            }
        }
    }

    // Checks for leaving collider
    void OnTriggerExit2D(Collider2D entity)
    {
        if(entity.gameObject.layer == layerSelection)
        {
            isColliding = false;
        }
    }

    // Returns the value of the collision
    public bool CheckCollision()
    {
        return isColliding;
    }

    // Sets the function that should be called if collision happens
    public void SetFunctionToCall(string funcName)
    {
        funcToCall = funcName;
    }

    // Calls different functions when a collision happens
    void CallFunctionOnCollision()
    {
        switch (funcToCall)
        {
            // Case for ground check gameobject on player
            case "AddJump":
                PlayerController.i.AddJump();
                break;
            // Case for ice cream projectile shot from an ice cream stand
            case "AddIceCream":
                AudioManager.i.Play("Collect");
                PlayerController.i.AddIceCream();
                Destroy(gameObject);
                break;
            // Case for projectiles shot by boss
            case "DamagePlayer":
                PlayerController.i.TakeBossDamage();
                Destroy(gameObject);
                break;
            case "DamageBoss":
                Boss.i.TakeDamage(1);
                Destroy(gameObject);
                break;
        }
    }
}

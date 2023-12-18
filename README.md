# MiniJam145

Code for my first game jam game which was made in under 72 hours for Mini Jam 145: Frozen. This game can be best summarized as a short 2D boss fight where the player has to beat the boss to win the game.

This was my first time doing a game jam and I think it was a good learning experience that helped me grow as a game developer and understand what it means to meet a deadline. I'm pretty happy with the code and feel like I utilized the principles of OOP adequately and added sufficient comments which helped the code be efficient and readable. 

If I had to nitpick, looking back now, adding getters for components on GameObjects like the animator and rigidbody may seem a little unnecessary. I could've also changed how the projectiles function; instead of instantiating one and removing it on collision, there could be a pool of projectiles already in the game that don't get deleted and instead become inactive and reactivated + repositioned when necessary, saving resources by not having to instantiate a new gameobject every time a projectile should be fired.

More info on the game can be found through this itch.io link: https://goopoe.itch.io/rebornasanicecreamconetheherowithheatvisionmustnowdefeatthefinalboss

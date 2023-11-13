using UnityEngine;
using UnityEngine.SceneManagement;

// Used in two scenes, intro and main game
public class GameInterface : MonoBehaviour  
{
    // Makes GameInterface accessible from other scripts
    private static GameInterface instance;
    public static GameInterface i { get { return instance; } }

    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject winScreen;

    // Initializes variables, called at start of game
    void Awake()
    {
        // Sets static reference
        instance = gameObject.GetComponent<GameInterface>();
    }

    void Start()
    {
        // Play song when entering main game, won't play during main menu
        AudioManager.i.Play("Song");
    }

    // Opens either the death screen or win screen
    public void OpenGameUI(bool isWinScreen)
    {
        switch(isWinScreen)
        {
            case true:
                winScreen.SetActive(true);
                break;
            case false:
                deathScreen.SetActive(true);
                break;
        }
    }

    // Called on button press, loads desired scene
    public void LoadScene(int sceneChoice)
    {
        // Resets time scale since buttons will only be accessible when time scale is 0
        Time.timeScale = 1f;

        // Starts to load the new scene
        SceneManager.LoadSceneAsync(sceneChoice);
    }

    // Called on button press, closes the game
    public void QuitGame()
    {
        Application.Quit();
    }
}

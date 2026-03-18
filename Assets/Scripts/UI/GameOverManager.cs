using UnityEngine;
using UnityEngine.SceneManagement; //this makes it posible to reload the scene

//this script is for handling the GAME OVER logic
//it:
//listens for the player dying
//shpws the game over screen
//pauses the game
// allows restarting the game

public class GameOverManager : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject gameOverPanel;

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";

    private Health playerHealth;

    private void Start()
    {
        //starts runs once when the scene begins
        
        //finds the player Object using tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //if it found the player
        if (player != null)
        {
            //gets the health component from the player
            playerHealth = player.GetComponent<Health>();

            // if the health component exist          
            if (playerHealth != null)
            {
                playerHealth.OnDeath += ShowGameOver; //calls the OnDeath event when the player dies and then shows the game over screen
            }
        }

        //make sure the game over screen is hidden at the start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= ShowGameOver;
        }
    }

    private void ShowGameOver()
    {
        Debug.Log("Game Over Triggered!");

        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        
        Time.timeScale = 0f;
    }

    
    public void RestartGame()
    {
        
        Time.timeScale = 1f; //resumes the time before reloading the scene

        //reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // We must unpause time before loading the new scene!
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject shopPanel;

    [Header("Scene Name")]
    public string gameSceneName = "GameScene";

    private void Start()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void StartNewGame()
    { 
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(gameSceneName);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Game Exiting...");
        Application.Quit();
    }

    public void OpenShop()
    {
        mainPanel.SetActive(false);
        shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        mainPanel.SetActive(true);
        shopPanel.SetActive(false);
    }
}
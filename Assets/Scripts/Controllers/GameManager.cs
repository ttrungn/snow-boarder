using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject pauseMenuPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 0f;
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void StartGameButton()
    {
        mainMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}

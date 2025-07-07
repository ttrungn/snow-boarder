using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public FloatVariable CurrentTime;

    public float StartTimeOffset { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    public IntVariable StartMenuStatus;
    public Text MessageText;
    public Text ScoreText; 
    [SerializeField] private GameObject pauseMenuPanel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (this.StartMenuStatus.Value == 0)
        {
            this.MessageText.text = "Welcome to the game!";
            this.ScoreText.text = "Let's start the game!";
        } else if (this.StartMenuStatus.Value == 1) {
            this.MessageText.text = "Game Over";
            this.ScoreText.text = "Score: " + this.CurrentTime.Value.ToString("F1");
            this.StartMenuStatus.SetValue(0);
        } else if (this.StartMenuStatus.Value == 2) {
            this.MessageText.text = "Winner Winner Chicken Dinner!";
            this.ScoreText.text = "Score: " + this.CurrentTime.Value.ToString("F1");
            this.StartMenuStatus.SetValue(0);
        }
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
        this.StartTimeOffset = Time.time;
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

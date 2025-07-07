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
    public Text StartMenuBgMuteText;
    public Text StartMenuFxMuteText;
    [SerializeField] private GameObject pauseMenuPanel;
    public Text PauseMenuBgMuteText;
    public Text PauseMenuFxMuteText;

    [Header("Audio")]
    public AudioSource BgMusic;
    public AudioSource JumpSound;
    public AudioSource OughSound;
    public AudioSource UmphSound;
    public AudioSource VictorySound;

    private bool IsBgMuted = false;
    private bool IsFxMuted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.StartTimeOffset = Time.time;
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
        this.CurrentTime.SetValue(0f);
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

    public void ToggleBgMute()
    {
        this.IsBgMuted = !this.IsBgMuted;
        this.BgMusic.mute = this.IsBgMuted;
        if (this.IsBgMuted) {
            this.StartMenuBgMuteText.text = "Unmute Music";
            this.PauseMenuBgMuteText.text = "Unmute Music";
        } else {
            this.StartMenuBgMuteText.text = "Mute Music";
            this.PauseMenuBgMuteText.text = "Mute Music";
        }
    }

    public void ToggleFxMute()
    {
        if (this.IsFxMuted)
        {
            this.IsFxMuted = false;
            this.StartMenuFxMuteText.text = "Mute Fx Sounds";
            this.PauseMenuFxMuteText.text = "Mute Fx Sounds";
        } else {
            this.IsFxMuted = true;
            this.StartMenuFxMuteText.text = "Unmute Fx Sounds";
            this.PauseMenuFxMuteText.text = "Unmute Fx Sounds";
        }
    }

    public void PlayJumpSound()
    {
        if (!this.IsFxMuted)
        {
            this.JumpSound.Play();
        }
    }

    public void PlayOughSound()
    {
        if (!this.IsFxMuted)
        {
            this.OughSound.Play();
        }
    }

    public void PlayUmphSound()
    {
        if (!this.IsFxMuted)
        {
            this.UmphSound.Play();
        }
    }

    public void PlayVictorySound()
    {
        if (!this.IsFxMuted)
        {
            this.VictorySound.Play();
        }
    }
}

using Database;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    private SqLiteGameDb sqLiteGameDb;


    public GameObject Ground;

    public FloatVariable TorqueForce;

    public FloatVariable SpeedNormalizationRate;

    public FloatVariable MinSpeed;

    public FloatVariable MaxSpeed;

    public FloatVariable AccelerationRate;

    public FloatVariable DefaultSpeed;

    public FloatVariable CurrentTime;

    public BoolVariable IsAlive;

    private Rigidbody2D rigidBody;
    private GroundSpeed groundSpeed;

    [Header("Dashboard")]
    public GameObject Dashboard;
    public Text speedText;
    public Text timeText;


    [Header("Top Score")]
    public Text top1Text;
    public Text top2Text;
    public Text top3Text;


    public float score = 0;

    private float inputVertical;
    private float inputHorizontal;

    private void Awake()
    {
        this.rigidBody = this.GetComponent<Rigidbody2D>();
        this.groundSpeed = this.Ground.GetComponent<GroundSpeed>();
        this.IsAlive.SetValue(true);
        sqLiteGameDb = FindObjectOfType<SqLiteGameDb>();
        Instance = this;
    }

    private void Update()
    {
        if (this.IsAlive.Value)
        {
            this.inputVertical = Input.GetAxisRaw("Vertical");
            this.inputHorizontal = Input.GetAxisRaw("Horizontal");
        }
        UpdateUI();
        UpdateTopScores();
    }

    private void UpdateUI()
    {
        // Update speed text
        if (this.speedText != null)
        {
            float currentSpeed = this.groundSpeed.GetGroundSpeed();
            this.speedText.text = "Speed: " + currentSpeed.ToString("F1");
        }

        // Update time text
        if (this.timeText != null)
        {
            float newTime = Time.time - GameManager.Instance.StartTimeOffset;
            this.CurrentTime.SetValue(newTime);
            this.score = this.CurrentTime.Value;
            this.timeText.text = "Time: " + this.score.ToString("F1");
        }
    }

    private void UpdateTopScores()
    {
        // Get top scores from the database
        var topScores = sqLiteGameDb.GetTop3Scores();

        // Update UI with top scores
        top1Text.text = topScores.Count > 0 ? "Top 1: " + topScores[0] + "s" : "Top 1: -";
        top2Text.text = topScores.Count > 1 ? "Top 2: " + topScores[1] + "s" : "Top 2: -";
        top3Text.text = topScores.Count > 2 ? "Top 3: " + topScores[2] + "s" : "Top 3: -";
    }

    private void FixedUpdate()
    {
        float defaultSpeed = this.IsAlive.Value ? this.DefaultSpeed.Value : 0;

        // Player rotation
        this.rigidBody.AddTorque(this.inputHorizontal * this.TorqueForce.Value);

        // User-input induced speed change
        if (this.inputVertical != 0)
        {
            float oldSpeed = this.groundSpeed.GetGroundSpeed();
            this.groundSpeed.SetGroundSpeed(this.getUpdatedSpeed(this.inputVertical, this.AccelerationRate.Value, this.groundSpeed.GetGroundSpeed(), this.MinSpeed.Value, this.MaxSpeed.Value));

#if UNITY_EDITOR
            Debug.Log(string.Format("PlayerMovement.getUpdatedSpeed user input speed change [from: {0}] [to: {1}]", oldSpeed, this.groundSpeed.GetGroundSpeed()));
#endif
        }

        // Speed normalization
        else if (!this.isDefaultSpeed(this.groundSpeed.GetGroundSpeed(), defaultSpeed))
        {
            float oldSpeed = this.groundSpeed.GetGroundSpeed();
            this.groundSpeed.SetGroundSpeed(getNormalizedSpeed(this.groundSpeed.GetGroundSpeed(), defaultSpeed, this.SpeedNormalizationRate.Value));
#if UNITY_EDITOR
            Debug.Log(string.Format("PlayerMovement.getNormalizedSpeed [from: {0}] [to: {1}]", oldSpeed, this.groundSpeed.GetGroundSpeed()));
#endif
        }

        // Cleanup
        this.inputVertical = 0;
        this.inputHorizontal = 0;
    }

    /// <summary>
    /// Calculate updated speed based on user input not to exceed maxSpeed or to fall below minSpeed.
    /// </summary>
    /// <param name="inputVertical"></param>
    /// <param name="accelerationRate"></param>
    /// <param name="currentSpeed"></param>
    /// <param name="minSpeed"></param>
    /// <param name="maxSpeed"></param>
    /// <returns></returns>
    private float getUpdatedSpeed(float inputVertical, float accelerationRate, float currentSpeed, float minSpeed, float maxSpeed)
    {
        return Mathf.Min(
            Mathf.Max(
                minSpeed
                , currentSpeed + (inputVertical * Time.deltaTime * accelerationRate)
            )
            , maxSpeed
        );
    }

    /// <summary>
    /// Determines if currentSpeed is approximately equal to defaultSpeed.
    /// </summary>
    /// <param name="currentSpeed"></param>
    /// <param name="defaultSpeed"></param>
    /// <returns>True if the two speeds are approximately equal.</returns>
    private bool isDefaultSpeed(float currentSpeed, float defaultSpeed)
    {
        return Mathf.Approximately(currentSpeed, defaultSpeed);
    }

    /// <summary>
    /// Normalize player's speed back towards defaultSpeed
    /// </summary>
    /// /// <param name="currentSpeed"></param>
    /// <param name="defaultSpeed"></param>
    /// <param name="speedNormalizationRate"></param>
    private float getNormalizedSpeed(float currentSpeed, float defaultSpeed, float speedNormalizationRate)
    {
        float speedDifferential = (currentSpeed - defaultSpeed) / speedNormalizationRate;
        return Mathf.Approximately(defaultSpeed, currentSpeed - speedDifferential)
            ? defaultSpeed
            : currentSpeed - speedDifferential
        ;
    }
}

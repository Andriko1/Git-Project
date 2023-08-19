using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Fields
    private static UIManager _instance;

    private Player _player;

    private TMP_Text _scoreText;
    private TMP_Text _healthText;
    private TMP_Text _highScoreText;
    private Image _gunTimer;
    #endregion

    #region Properties
    public static UIManager Instance
    {
        get { return _instance; }
    }
    #endregion

    #region Unity Methods
    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _player = Player.Instance;
        GameManager.Instance.RestartedGame += ResetUI;
        _player.Health.OnHealthUpdate += UpdateHealth;
        _player.PlayerNuke += UpdateNukeCount;
        _gunTimer = _player.transform.GetChild(0).Find("GunTimer").GetComponent<Image>();


        _gunTimer.color = Color.magenta;
        _gunTimer.fillAmount = 0.0f;

        UpdateNukeCount();


        _scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        _scoreText.SetText($"Score: {GameManager.Instance.Score}");
        _healthText = transform.Find("HealthText").GetComponent<TextMeshProUGUI>();
        _healthText.SetText($"Health: {_player.CurHealth}");
        _highScoreText = transform.Find("HighScoreText").GetComponent<TextMeshProUGUI>();
        _highScoreText.SetText($"High Score: {GameManager.Instance.HighScore}");
    }

    private void Update()
    {
        if (_player == null || !_player.gameObject.activeSelf) return;

        if (_player.AutoShootPUTimer < _player.AutoShootLength)
        {
            ShowGunTime( 1 - (_player.AutoShootPUTimer / _player.AutoShootLength));
        }
        else
        {
            _gunTimer.fillAmount = 0.0f;
        }


    }

    private void OnDestroy()
    {
        _player.Health.OnHealthUpdate -= UpdateHealth;
    }

    #endregion

    #region Public Methods
    public void UpdateNukeCount()
    {
        Transform nukeCanvasTransform = transform.Find("Nukes");
        switch (_player.NukeCount)
        {
            case 0:
                nukeCanvasTransform.GetChild(0).gameObject.SetActive(false);
                nukeCanvasTransform.GetChild(1).gameObject.SetActive(false);
                nukeCanvasTransform.GetChild(2).gameObject.SetActive(false);
                break;
            case 1:
                nukeCanvasTransform.GetChild(0).gameObject.SetActive(true);
                nukeCanvasTransform.GetChild(1).gameObject.SetActive(false);
                nukeCanvasTransform.GetChild(2).gameObject.SetActive(false);
                break;
            case 2:
                nukeCanvasTransform.GetChild(0).gameObject.SetActive(true);
                nukeCanvasTransform.GetChild(1).gameObject.SetActive(true);
                nukeCanvasTransform.GetChild(2).gameObject.SetActive(false);
                break;
            case 3:
                nukeCanvasTransform.GetChild(0).gameObject.SetActive(true);
                nukeCanvasTransform.GetChild(1).gameObject.SetActive(true);
                nukeCanvasTransform.GetChild(2).gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void UpdateHealth(int currentHealth)
    {
        _healthText.SetText($"Health: {currentHealth}");
    }

    public void UpdateScore() 
    {
        _scoreText.SetText($"Score: {GameManager.Instance.Score}");
        _highScoreText.SetText($"High Score: { GameManager.Instance.HighScore}");
    }
    #endregion

    #region Private Methods
    private void ResetUI()
    {
        UpdateHealth((int)_player.Health.GetHealth());
        UpdateScore();
        UpdateNukeCount();
    }

    private void ShowGunTime(float timeLeft)
    {
        _gunTimer.fillAmount = timeLeft;
    }

    #endregion
}

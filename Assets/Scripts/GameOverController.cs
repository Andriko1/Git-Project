using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    #region Fields
    private TMP_Text ScoreResultText;
    private Button RetryButton;
    private Button QuitButton;
    private Button ClearHighScoreButton;
    private float _timer;
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    private void Awake()
    {
        RetryButton = transform.Find("RetryButton").GetComponent<Button>();
        RetryButton.onClick.AddListener(OnRetryButtonClicked);
        QuitButton = transform.Find("QuitButton").GetComponent<Button>();
        QuitButton.onClick.AddListener(OnQuitButtonClicked);
        ClearHighScoreButton = transform.Find("ClearHighScoreButton").GetComponent<Button>();
        ClearHighScoreButton.onClick.AddListener(OnClearHighScoreButtonClicked);
        ScoreResultText = transform.Find("ScoreResultText").GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (ScoreResultText.text == "NEW HIGHSCORE!!!") 
        {
            _timer += Time.deltaTime;
            if ( _timer >= 2.0f ) 
            {
                _timer = 0.0f;
            }
            if ( _timer <= 1.0f )
            {
                ScoreResultText.alpha = 1.0f;
            }
            else
            {
                ScoreResultText.alpha = 0.0f;
            }
        }
    }
    #endregion

    #region Public Methods
    public void CongratulatePlayer()            //Called from GameManager when the high score is beaten
    {
        if (ScoreResultText != null)            //For some reason when the TMP_Text component was referenced in Start() (in a previous version of this code) this would be null??? Which is weird because the gameObject was set to active before calling this function
        {
            ScoreResultText.SetText("NEW HIGHSCORE!!!");
        }
    }
    #endregion

    #region Private Methods
    private void OnRetryButtonClicked()
    {
        gameObject.SetActive(false);
        GameManager.Instance.RestartGame();
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    private void OnClearHighScoreButtonClicked()
    {
        GameManager.Instance.ClearScore();
        ScoreResultText.SetText("High Score Cleared!");
        ScoreResultText.alpha = 1.0f;
        //Stop Flashing if it's flashing
    }

    //Need to make GameOver screen pop up when player dies, and call CongratulatePlayer when the high score is beaten
    #endregion
}

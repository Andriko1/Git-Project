using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    #region Fields
    public Button startButton;
    public Button quitButton;
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    void Start()
    {
        startButton = GameObject.Find("StartGame_Button").GetComponent<Button>();
        startButton.onClick.AddListener(OnStartButtonClicked);
        quitButton = GameObject.Find("QuitGame_Button").GetComponent<Button>();
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    void Update()
    {
        
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
    #endregion
}

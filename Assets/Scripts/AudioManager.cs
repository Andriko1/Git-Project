using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Fields
    private AudioSource _audio;
    private AudioClip _playerShoot;
    private AudioClip _enemyShoot;
    private AudioClip _enemyExplode;
    private AudioClip _nuke;
    private AudioClip _enemyDie;
    private AudioClip _playerDie;
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion
}

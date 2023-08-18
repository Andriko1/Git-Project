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
        /*_playerShoot = ;
        _enemyShoot = ;
        _enemyExplode = ;
        _nuke = ;
        _enemyDie = ;
        _playerDie = ;*/
}

    void Start()
    {
        Player.Instance.PlayerShoot += PlayPlayerShootSound;
        Player.Instance.PlayerNuke += PlayNukeSound;
    }

    
    void Update()
    {
        
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void PlayPlayerShootSound()
    {
        //_audio.PlayOneShot(_playerShoot);
    }

    private void PlayNukeSound()
    {
        //_audio.PlayOneShot(_nuke);
    }
    #endregion
}

using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    #region Fields
    private static AudioManager _instance;
    private AudioSource _audio;
    private AudioClip _newHighScore;
    private AudioClip _playerShoot;
    private AudioClip _enemyShoot;
    private AudioClip _enemyMelee;
    private AudioClip[] _enemyExplode;
    private AudioClip _nuke;
    private AudioClip _enemyDie;
    private AudioClip _playerDie;
    private AudioClip _nukePU;
    private AudioClip _healthPU;
    private AudioClip _gunPU;

    public enum Sounds
    {
        NewHighScore,
        PlayerShoot,
        EnemyShoot,
        EnemyMelee,
        EnemyExplode,
        Nuke,
        EnemyDie,
        PlayerDie,
        NukePU,
        HealthPU,
        GunPU
    }
    #endregion

    #region Properties
    public static AudioManager Instance
    {
        get { return _instance; }
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _instance = this;
        _audio = GetComponent<AudioSource>();
        _newHighScore = Resources.Load<AudioClip>("Sounds/points_ticker_bonus_score_reward_single_04");
        _playerShoot = Resources.Load<AudioClip>("Sounds/gun_rifle_sniper_shot_03");
        _enemyShoot = Resources.Load<AudioClip>("Sounds/weapon_fun_pea_shooter_02");
        _enemyMelee = Resources.Load<AudioClip>("Sounds/retro_enemy_attack_03");
        _enemyExplode = new AudioClip[18] {
            Resources.Load<AudioClip>("Sounds/fart_squirt_01"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_02"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_03"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_04"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_05"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_06"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_07"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_08"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_09"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_10"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_11"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_12"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_13"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_14"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_15"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_16"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_17"),
            Resources.Load<AudioClip>("Sounds/fart_squirt_18")
        };
        _nuke = Resources.Load<AudioClip>("Sounds/collect_item_hurry_out_of_time_01");
        _enemyDie = Resources.Load<AudioClip>("Sounds/cartoon_boing_jump_01");
        _playerDie = Resources.Load<AudioClip>("Sounds/game_over_dark_bell_chime_01");
        _nukePU = Resources.Load<AudioClip>("Sounds/collect_coin_02");
        _healthPU = Resources.Load<AudioClip>("Sounds/collect_item_20");
        _gunPU = Resources.Load<AudioClip>("Sounds/collect_item_22");
    }

    void Start()
    {

    }

    
    void Update()
    {
        
    }
    #endregion

    #region Public Methods
    public void PlaySound(Sounds sound)
    {
        switch(sound)
        {
            case Sounds.NewHighScore:
                _audio.PlayOneShot(_newHighScore);
                break;
            case Sounds.PlayerShoot:
                _audio.PlayOneShot(_playerShoot);
                break;
            case Sounds.EnemyShoot:
                _audio.PlayOneShot(_enemyShoot);
                break;
            case Sounds.EnemyMelee:
                _audio.PlayOneShot(_enemyMelee);
                break;
            case Sounds.EnemyExplode:
                _audio.PlayOneShot(_enemyExplode[Random.Range(0,18)]);
                break;
            case Sounds.Nuke:
                _audio.PlayOneShot(_nuke);
                break;
            case Sounds.EnemyDie:
                _audio.PlayOneShot(_enemyDie);
                break;
            case Sounds.PlayerDie:
                _audio.PlayOneShot(_playerDie);
                break;
            case Sounds.NukePU:
                _audio.PlayOneShot(_nukePU);
                break;
            case Sounds.HealthPU:
                _audio.PlayOneShot(_healthPU);
                break;
            case Sounds.GunPU:
                _audio.PlayOneShot(_gunPU);
                break;
            default:
                break;
        }
}
    #endregion

    #region Private Methods
    #endregion
}

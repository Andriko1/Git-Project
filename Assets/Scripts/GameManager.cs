using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;

public class GameManager : MonoBehaviour
{
    #region Fields
    private int _seconds;
    public UnityEvent OnScoreUpdated;

    private static GameManager _instance;
    public CurrentStage currentStage;
    private float _elapsedTime = 0.0f;
    private float _fadeDuration = 3.0f;
    private bool _wasStageClear = true;
    private bool _stageClearedBeforeTimer = false;

    private int _enemiesToSpawn = 4;

    private Color _ogC;

    public enum CurrentStage
    {
        Started,
        Stopped,
        Paused
    }

    private List<GameObject> _enemies = new List<GameObject>();
    private List<GameObject> _pickups = new List<GameObject>();

    private int _score;
    private int _highScore;

    [SerializeField] private float _timer;
    [SerializeField] private float _spawnTime = 10.0f;

    private Camera _cam;
    private Player _player;

    #endregion

    #region Properties
    public List<GameObject> Enemies
    {
        get { return _enemies; }
        set { _enemies = value; }
    }
    public List<GameObject> Pickups
    {
        get { return _pickups; }
        set { _pickups = value; }
    }
    public string Timer { get { return (Mathf.Round((float)_seconds / 60.0f) + "minutes and " + _seconds % 60 + " seconds"); } }
    public int Score { get { return _score; } }
    public int HighScore { get { return _highScore; } }
    public static GameManager Instance { get { return _instance; } }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        OnScoreUpdated.AddListener(UIManager.Instance.UpdateScore);

        _cam = Camera.main;
        _ogC = _cam.backgroundColor;
        _player = Player.Instance.GetComponent<Player>();
        //currentStage = CurrentStage.Paused;   Not needed at this stage, implement later
        

        SpawnEnemy(EnemyType.Melee, new Vector2(8.5f, -5.5f));
        SpawnEnemy(EnemyType.Exploder, new Vector2(8.5f, 5.5f));
        SpawnEnemy(EnemyType.Shooter, new Vector2(-8.5f, 5.5f));
        SpawnEnemy(EnemyType.MachineGun, new Vector2(-8.5f, -5.5f));
        _timer = 0.0f;
        LoadScore();
    }

    
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _spawnTime)
        {
            if (_enemies.Count > 0) 
            {
                _wasStageClear = false;
            }
            else
            {
                _wasStageClear = true;
            }
            SpawnEnemies(_enemiesToSpawn);
            if (_stageClearedBeforeTimer)   //This bool is true when the enemies were cleared 0.5s before the timer
            {
                int r = UnityEngine.Random.Range(0, 5);
                if (r == 4)
                {
                    _enemiesToSpawn++;              //20% chance to increase number of enemies
                }
                else
                {
                    _spawnTime -= 0.5f;             //80% chance to decrease spawn timer
                }
                _stageClearedBeforeTimer = false;
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveScore();
    }
    #endregion

    #region Public Methods
    public void OnEnemyDeath(EnemyType enemyType)
    { 
        if (_enemies.Count == 0 && _spawnTime - _timer > 0.5 && _wasStageClear)
        {
            _stageClearedBeforeTimer = true;
        }
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("HIGH_SCORE", _highScore);
    }

    public void LoadScore()
    {
        _highScore = PlayerPrefs.GetInt("HIGH_SCORE"/*, 0*/);
        OnScoreUpdated?.Invoke();
    }

    public void IncreaseScore()
    {
        if (_player == null) return;
        _score++;
        if (_score > _highScore)
        {
            _highScore = _score;
        }
        OnScoreUpdated?.Invoke();
    }

    public void FlashScreen()
    {
        if (_elapsedTime == 0.0f)
        {
            StartCoroutine(Fade());
        }
        else
        {
            _elapsedTime = 0.0f;
        }
        int ecount = _enemies.Count;
        for (int i = 0; i < ecount; i++) 
        {
            GameObject e = _enemies[0];
            switch (e.GetComponent<Enemy>().SelfType)
            {
                case EnemyType.Melee:
                    e.GetComponent<Enemy_Melee>().Die(e, 0.0f);
                    break;
                case EnemyType.Exploder:
                    e.GetComponent<Enemy_Exploder>().Attack(5.0f);  //Blow up
                    break;
                case EnemyType.Shooter:
                    e.GetComponent<Enemy_Shooter>().Die(e, 0.0f);
                    break;
                case EnemyType.MachineGun:
                    e.GetComponent<Enemy_MachineGun>().Die(e, 0.0f);
                    break;
            }
        }

        int pcount = _pickups.Count;
        for (int i = 0; i < pcount; i++)
        {
            GameObject p = _pickups[0];
            Destroy(p);
            _pickups.RemoveAt(0);
        }

    }

    IEnumerator Fade()
    {
        while (_elapsedTime < _fadeDuration)
        {
            _cam.backgroundColor = Color.Lerp(Color.white, _ogC, _elapsedTime / _fadeDuration);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _cam.backgroundColor = _ogC;
        _elapsedTime = 0.0f;
    }

    #endregion

    #region Private Methods
    private void SpawnEnemy(EnemyType enemyType, Vector2 position)
    {
        switch (enemyType)
        {
            case EnemyType.Melee:
                GameObject enemyMelee = new GameObject("Enemy_Melee", typeof(Enemy_Melee));
                enemyMelee.transform.position = position;
                _enemies.Add(enemyMelee);
                break;
            case EnemyType.Shooter:
                GameObject enemyShooter = new GameObject("Enemy_Shooter", typeof(Enemy_Shooter));
                enemyShooter.transform.position = position;
                _enemies.Add(enemyShooter);
                break;
            case EnemyType.Exploder:
                GameObject enemyExploder = new GameObject("Enemy_Exploder", typeof(Enemy_Exploder));
                enemyExploder.transform.position = position;
                _enemies.Add(enemyExploder);
                break;
            case EnemyType.MachineGun:
                GameObject enemyMachineGun = new GameObject("Enemy_MachineGun", typeof(Enemy_MachineGun));
                enemyMachineGun.transform.position = position;
                _enemies.Add(enemyMachineGun);
                break;
            default:
                Debug.LogError("ERROR: Invalid parameter in GameManager.SpawnEnemy()");
                break;
        }
    }

    private void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int r = UnityEngine.Random.Range(0, Enum.GetNames(typeof(EnemyType)).Length);
            int r0 = UnityEngine.Random.Range(0, 4);
            float r1 = UnityEngine.Random.Range(-8.5f, 8.5f);           //Thought generating both would be easier to read with minimal impact
            float r2 = UnityEngine.Random.Range(-5.5f, 5.5f);
            switch (r0)                 //Random decision on where to spawn
            {
                case 0:                 //Left side
                    SpawnEnemy((EnemyType)r, new Vector2(-8.5f, r2));
                    break;
                case 1:                 //Top side
                    SpawnEnemy((EnemyType)r, new Vector2(r1, 5.5f));
                    break;
                case 2:                 //Right side
                    SpawnEnemy((EnemyType)r, new Vector2(8.5f, r2));
                    break;
                case 3:                 //Bottom side
                    SpawnEnemy((EnemyType)r, new Vector2(r1, -5.5f));
                    break;
                default:
                    Debug.LogError($"ERROR: Random value r0 out of bounds, value is {r0}");
                    break;
            }
        }
        _timer = 0.0f;
    }



    #endregion
}

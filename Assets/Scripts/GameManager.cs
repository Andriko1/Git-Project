using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Fields
    private int _seconds;
    public UnityEvent OnScoreUpdated;

    private static GameManager _instance;
    public CurrentStage currentStage;
    private float _elapsedTime = 0.0f;
    private float _fadeDuration = 3.0f;

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
            SpawnEnemy(EnemyType.Melee, new Vector2(8.5f, -5.5f));
            SpawnEnemy(EnemyType.Exploder, new Vector2(8.5f, 5.5f));
            SpawnEnemy(EnemyType.Shooter, new Vector2(-8.5f, 5.5f));
            SpawnEnemy(EnemyType.MachineGun, new Vector2(-8.5f, -5.5f));
            _timer = 0.0f;
        }
    }
    #endregion

    #region Public Methods
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
                    e.GetComponent<Enemy_Exploder>().Attack(5.0f);
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
                GameObject enemyMelee = new GameObject("Enemy_Melee", typeof(Enemy_Melee), typeof(BoxCollider2D), typeof(Rigidbody2D));
                enemyMelee.GetComponent<Enemy_Melee>().SetEnemyType(enemyType);
                enemyMelee.GetComponent<Rigidbody2D>().isKinematic = true;
                enemyMelee.GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
                enemyMelee.transform.position = position;
                GameObject enemyMeleeChild = new GameObject("Child", typeof(Animator), typeof(SpriteRenderer));
                enemyMeleeChild.GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Square.png");
                enemyMeleeChild.GetComponent<Animator>().runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Animations/Enemy_Melee.controller");
                enemyMeleeChild.transform.localScale = new Vector3(0.75f, 0.75f);
                enemyMeleeChild.transform.SetParent(enemyMelee.transform, false);
                _enemies.Add(enemyMelee);
                break;
            case EnemyType.Shooter:
                GameObject enemyShooter = new GameObject("Enemy_Shooter", typeof(Enemy_Shooter), typeof(PolygonCollider2D), typeof(Rigidbody2D));
                enemyShooter.GetComponent<PolygonCollider2D>().pathCount = 1;
                enemyShooter.GetComponent<PolygonCollider2D>().points = new Vector2[]
                {
                    new Vector2(0f, 0.1875f), new Vector2(-0.625f, 0f), new Vector2(0f, -0.1875f), new Vector2(0.625f, 0f)
                };
                enemyShooter.GetComponent<Enemy_Shooter>().SetEnemyType(enemyType);
                enemyShooter.GetComponent<Rigidbody2D>().isKinematic = true;
                enemyShooter.GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
                enemyShooter.transform.position = position;
                GameObject enemyShooterChild = new GameObject("Child", typeof(Animator), typeof(SpriteRenderer));
                enemyShooterChild.GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/IsometricDiamond.png");
                enemyShooterChild.GetComponent<Animator>().runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Animations/Enemy_Shooter.controller");
                enemyShooterChild.transform.localScale = new Vector3(1.25f, 0.75f);
                enemyShooterChild.transform.SetParent(enemyShooter.transform, false);
                _enemies.Add(enemyShooter);
                break;
            case EnemyType.Exploder:
                GameObject enemyExploder = new GameObject("Enemy_Exploder", typeof(Enemy_Exploder), typeof(CircleCollider2D), typeof(Rigidbody2D), typeof(Animator));
                enemyExploder.GetComponent<Enemy_Exploder>().SetEnemyType(enemyType);
                enemyExploder.GetComponent<CircleCollider2D>().radius = 0.375f;
                enemyExploder.GetComponent<Rigidbody2D>().isKinematic = true;
                enemyExploder.GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
                enemyExploder.GetComponent<Animator>().runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Animations/Enemy_Exploder_Parent.controller");
                enemyExploder.transform.position = position;
                GameObject enemyExploderChild = new GameObject("Child", typeof(Animator), typeof(SpriteRenderer));
                enemyExploderChild.GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Circle.png");
                enemyExploderChild.GetComponent<Animator>().runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Animations/Enemy_Exploder.controller");   //Not sure why, but this line of code prevents localScale from having any effect on the transform, I can print out the correct localScale(0.75f, 0.75f) using Debug.Log, but without adding an extra Scale property in the Idle animation, localScale defaults to 1. This is not true for the other enemies for some reason...they all use the same idle animation(had to create a seperate one for the exploder child)
                enemyExploderChild.transform.localScale = new Vector3(0.75f, 0.75f);
                enemyExploderChild.transform.SetParent(enemyExploder.transform, false);
                _enemies.Add(enemyExploder);
                break;
            case EnemyType.MachineGun:
                GameObject enemyMachineGun = new GameObject("Enemy_MachineGun", typeof(Enemy_MachineGun), typeof(BoxCollider2D), typeof(Rigidbody2D));
                enemyMachineGun.GetComponent<Enemy_MachineGun>().SetEnemyType(enemyType);
                enemyMachineGun.GetComponent<Rigidbody2D>().isKinematic = true;
                enemyMachineGun.GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
                enemyMachineGun.transform.position = position;
                GameObject enemyMachineGunChild = new GameObject("Child", typeof(Animator), typeof(SpriteRenderer));
                enemyMachineGunChild.GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Square.png");
                enemyMachineGunChild.GetComponent<Animator>().runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Animations/Enemy_MachineGun.controller");
                enemyMachineGunChild.transform.localScale = new Vector3(1.0f, 0.5f);
                enemyMachineGunChild.transform.SetParent(enemyMachineGun.transform, false);
                _enemies.Add(enemyMachineGun);
                break;
            default:
                Debug.LogError("ERROR: Invalid parameter in GameManager.SpawnEnemy(EnemyType enemyType)");
                break;
        }
    }
    #endregion
}

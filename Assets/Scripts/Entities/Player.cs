using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : Character
{
    #region Fields
    public enum GunType
    {
        normal,
        rapidfire
    }
    [SerializeField] private Camera _cam;
    private static Player _instance;

    private Rigidbody2D _playerRb;
    private int _health;
    public int NukeCount = 0;
    public float AutoShootLength = 10.0f;
    public float AutoShootPUTimer;
    public float AutoShootInterval = 0.2f;
    public float AutoShootTimer;

    public Action PlayerShoot;
    public Action PlayerNuke;
    public Action PlayerDie;

    #endregion

    #region Properties
    public static Player Instance
    {
        get { return _instance; }
    }
    public int CurHealth
    {
        get { return _health; }
    }
    #endregion

    #region Unity Methods
    public override void Awake()
    {
        base.Awake();
        _instance = this;
        Health = new Health(100f, 100f, 1.0f);
        Weapon = new Weapon();
        _speed = 250.0f;
        _playerRb = GetComponent<Rigidbody2D>();
        _cam = Camera.main;
        AutoShootPUTimer = AutoShootLength;
        AutoShootTimer = AutoShootInterval;
    }

    public override void Start()
    {
        base.Start();
        GameManager.Instance.RestartedGame += OnRestartGame;
    }

    public override void Update()
    {
        base.Update();
        if ( AutoShootPUTimer < AutoShootLength)
        {
            AutoShootPUTimer += Time.deltaTime;
        }

        if (AutoShootTimer < AutoShootInterval)
        {
            AutoShootTimer += Time.deltaTime;
        }
    }

    public void OnCollisionStay2D(Collision2D other)
    {
        if ( IsEnemy(other.gameObject) )
        {
            GetComponent<IDamageable>().TakeDamage(0.1f);
        }
    }
    #endregion

    #region Public Methods
    public override void Move(Vector2 direction, Vector2 target)
    {
        _playerRb.velocity = _speed * Time.deltaTime * direction;
        Vector3 playerScreenPos = _cam.WorldToScreenPoint(transform.position);
        target.x -= playerScreenPos.x;
        target.y -= playerScreenPos.y;

        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override void Shoot(Vector3 direction, float speed)
    {
        GameObject bullet = new GameObject("Bullet", typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(Bullet));
        bullet.GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Circle.png");
        bullet.GetComponent<Rigidbody2D>().isKinematic = true;
        bullet.GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
        bullet.GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
        bullet.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
        bullet.transform.position = transform.Find("Gun").position;
        bullet.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        bullet.GetComponent<Bullet>().IsEnemyBullet = false;

        PlayerShoot?.Invoke();
    }

    public void AutoShoot(Vector3 direction, float speed)
    {
        if (AutoShootPUTimer < AutoShootLength)
        {
            if (AutoShootTimer >= AutoShootInterval)
            {
                Shoot(direction, speed);
                AutoShootTimer = 0;
            }
        }
    }

    public void Nuke()
    {
        if (NukeCount > 0)
        {
            NukeCount--;
            PlayerNuke?.Invoke();
        }
    }

    public override void Attack(float interval)
    {
        //Do nothing, idk why this was an abstract, maybe can use this later for multiple weapons instead of only shooting
    }

    public override void Die(GameObject whoDied, float delay = 0.0f)
    {
        PlayerDie?.Invoke();
        //Destroy(whoDied, delay); Instead of destroying, disable the player so you can enable it later
        whoDied.SetActive(false);
    }

    public void PickedUp(GameObject pickup)
    {
        switch (pickup.name)
        {
            case "NukePU":
                if (NukeCount < 3)
                {
                    NukeCount++;
                    UIManager.Instance.UpdateNukeCount();
                }
                break;
            case "HealthPU":
                Health.AddHealth(25.0f);
                break;
            case "GunPU":
                AutoShootPUTimer = 0;
                break;
        }
    }
    #endregion

    #region Private Methods
    private void OnRestartGame()
    {
        gameObject.SetActive(true);
        Health.SetHealth(100);
        transform.position = Vector2.zero;
    }
    #endregion
}

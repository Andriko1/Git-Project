using System;
using UnityEngine;

public class Enemy : Character
{
    #region Fields
    [SerializeField] protected float AttackRange;
    [SerializeField] protected float AttackTime;
    protected Transform playerTransform;
    public EnemyType SelfType;
    [SerializeField] protected float timer;

    public Action<EnemyType> EnemyDied;
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    public override void Awake()
    {
        base.Awake();
    }


    public override void Start()
    {
        base.Start();

        EnemyDied += GameManager.Instance.OnEnemyDeath;

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) 
        {
            playerTransform = playerObj.transform; 
        }
        
    }

    public override void Update()
    {
        base.Update();
        if (playerTransform != null && playerTransform.gameObject.activeSelf)
        {
            Move(playerTransform.position, AttackRange);
        }
        else
        {
            Move(_speed);
        }
        if (timer <= AttackTime)
        {
            timer += Time.deltaTime;
        }
    }
    #endregion

    #region Public Methods
    public override void Move(Vector2 direction, Vector2 target)
    {
        
    }

    public override void Move(Vector2 target, float attackRange)
    {
        target.x -= transform.position.x;
        target.y -= transform.position.y;

        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;

        if (Vector2.Distance(transform.position, playerTransform.position) > attackRange && timer > AttackTime)
        {
            transform.Translate(_speed * Time.deltaTime * Vector2.right);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }

    public override void Move(float speed)
    {
        if (timer > AttackTime)
        {
            transform.Translate(speed * Time.deltaTime * Vector2.right);
        }
    }

    public override void Shoot(Vector3 direction, float speed)
    {
        AudioManager.Instance.PlaySound(AudioManager.Sounds.EnemyShoot);
        GameObject bullet = new GameObject("Bullet", typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(Bullet));
        bullet.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Circle");
        bullet.GetComponent<Rigidbody2D>().isKinematic = true;
        bullet.GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
        bullet.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
        bullet.transform.position = transform.position;
        bullet.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        bullet.GetComponent<Bullet>().IsEnemyBullet = true;
    }

    public override void Die(GameObject whoDied, float delay = 0.0f)
    {
        base.Die(whoDied, delay);
        if (whoDied != null) 
        {
            AudioManager.Instance.PlaySound(AudioManager.Sounds.EnemyDie);
            PickupSpawner.Instance.SpawnPU(UnityEngine.Random.Range(0.0f, 2.0f), whoDied.transform.position);
            GameManager.Instance.Enemies.Remove(whoDied);
            GameManager.Instance.IncreaseScore();
            Destroy(whoDied, delay);
            EnemyDied?.Invoke(whoDied.GetComponent<Enemy>().SelfType);      //What? Shouldn't this not be called because the object just got destroyed? Idk not complaining
        }
    }

    public override void Attack(float interval)
    {

    }

    public void SetEnemyType(EnemyType type)
    {
        SelfType = type;
    }
    #endregion

    #region Private Methods
    

    #endregion
}

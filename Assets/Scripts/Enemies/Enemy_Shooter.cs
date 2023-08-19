using UnityEngine;

public class Enemy_Shooter : Enemy
{
    #region Fields
    PolygonCollider2D _pCollider;
    Rigidbody2D _rb;
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    public override void Awake()
    {
        base.Awake();
        
        _pCollider = gameObject.AddComponent<PolygonCollider2D>();
        _rb = gameObject.AddComponent<Rigidbody2D>();

        _pCollider.pathCount = 1;
        _pCollider.points = new Vector2[]
        {
            new Vector2(0f, 0.1875f), new Vector2(-0.625f, 0f), new Vector2(0f, -0.1875f), new Vector2(0.625f, 0f)
        };

        _rb.isKinematic = true;
        
        GameObject enemyShooterChild = new GameObject("Child", typeof(Animator), typeof(SpriteRenderer));
        enemyShooterChild.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/IsometricDiamond");
        enemyShooterChild.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimatorControllers/Enemy_Shooter");
        enemyShooterChild.transform.localScale = new Vector3(1.25f, 0.75f);
        enemyShooterChild.transform.SetParent(transform, false);

        SetEnemyType(EnemyType.Shooter);
    }

    public override void Start()
    {
        base.Start();
        AttackRange = 7f;
        AttackTime = 1.5f;
        timer = AttackTime;
        _speed = 2f;
        Health = new Health(1, 1, 0);
    }

    public override void Update()
    {
        base.Update();  //Basically just calls base.Move()
        if (playerTransform != null && playerTransform.gameObject.activeSelf)
        {
            if (Vector2.Distance(transform.position, playerTransform.position) <= AttackRange)
            {
                Aim(playerTransform.position, AttackRange);
                Attack(AttackTime);
            }
        }
    }
    #endregion

    #region Public Methods
    public void Aim(Vector2 target, float attackRange)
    {
        target.x -= transform.position.x;
        target.y -= transform.position.y;
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public override void Attack(float interval)
    {
        if (timer > interval)
        {
            base.Attack(interval);
            timer = 0;
            GetComponentInChildren<Animator>().SetTrigger("Attack");
            Shoot(transform.right, 6.0f);
        }
    }
    #endregion

    #region Private Methods
    #endregion
}

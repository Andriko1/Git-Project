using UnityEngine;

public class Enemy_Shooter : Enemy
{
    #region Fields
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    public override void Start()
    {
        base.Start();
        AttackRange = 7f;
        AttackTime = 1.5f;
        timer = AttackTime;
        _speed = 2f;
        SetEnemyType(EnemyType.Shooter);
        Health = new Health(1, 1, 0);
    }

    public override void Update()
    {
        base.Update();  //Basically just calls base.Move()
        if (playerTransform != null)
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

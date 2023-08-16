using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.GraphicsBuffer;

public class Enemy_MachineGun : Enemy
{
    #region Fields

    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    public override void Start()
    {
        base.Start();
        AttackRange = 5f;
        AttackTime = 0.2f;
        timer = AttackTime;
        _speed = 1.75f;
        SetEnemyType(EnemyType.MachineGun);
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
        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange && timer > AttackTime)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle + Random.Range(-10.0f, 10.0f));
        }
    }

    public override void Attack(float interval)
    {
        if (timer > interval)
        {
            base.Attack(interval);
            timer = 0;
            GetComponentInChildren<Animator>().SetTrigger("Attack");
            Shoot(transform.right, 2.0f);
        }
    }
    #endregion

    #region Private Methods
    #endregion
}

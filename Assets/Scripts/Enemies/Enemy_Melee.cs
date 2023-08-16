using UnityEngine;

public class Enemy_Melee : Enemy
{
    #region Fields
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    public override void Start()
    {
        base.Start();
        AttackRange = 0.933f;
        AttackTime = 1.0f;
        timer = AttackTime;
        _speed = 3.5f;
        SetEnemyType(EnemyType.Melee);
        Health = new Health(1, 1, 0);
    }

    public override void Update()
    {
        base.Update();  //Basically just calls base.Move()
        if (playerTransform != null)
        {
            if (Vector2.Distance(transform.position, playerTransform.position) <= AttackRange)
            {
                Attack(AttackTime);
            }
        }
    }
    #endregion

    #region Public Methods
    public override void Attack(float interval)
    {
        if (timer > interval)
        {
            base.Attack(interval);
            timer = 0;
            GetComponentInChildren<Animator>().SetTrigger("Attack");
            playerTransform.GetComponent<IDamageable>().TakeDamage(10);
        }
    }
    #endregion

    #region Private Methods
    #endregion
}

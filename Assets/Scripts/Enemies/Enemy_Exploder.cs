using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Exploder : Enemy
{
    #region Fields
    public float ExplodeRange;
    private bool _hasAttacked = false;
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    public override void Start()
    {
        base.Start();
        AttackRange = 0.95f;
        AttackTime = 5.0f;
        timer = AttackTime;
        _speed = 1.0f;
        SetEnemyType(EnemyType.Exploder);
        Health = new Health(1, 1, 0);
    }

    public override void Update()                   //Test using the regular Update like in the other classes now that we found out we can change the default value for a parameter
    {
        if (timer <= AttackTime)
        {
            timer += Time.deltaTime;
        }
        //transform.GetChild(0).transform.localPosition = new Vector3();
        //transform.GetChild(0).transform.localRotation = new Quaternion();
        
        if (playerTransform != null)
        {
            Move(playerTransform.position, AttackRange);
            if (Vector2.Distance(transform.position, playerTransform.position) <= AttackRange && _hasAttacked == false)
            {
                _hasAttacked = true;
                Attack(AttackTime);
            }
        }
        else
        {
            Move(_speed);
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
            Die(this.gameObject, 1.25f);    //The attack happens as it dies
        }
    }

    public override void Die(GameObject whoDied, float delay = 1.25f)           //Test using the regular Update like in the other classes now that we found out we can change the default value for a parameter
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        GetComponent<Animator>().SetTrigger("Attack");
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("Attack");
        base.Die(whoDied, delay);
    }

    public override void TakeDamage(float damageValue)
    {
        Health.RemoveHealth(damageValue);
        if (Health.GetHealth() <= 0)
        {
            Attack(AttackTime);
        }
    }
    #endregion

    #region Private Methods
    #endregion
}

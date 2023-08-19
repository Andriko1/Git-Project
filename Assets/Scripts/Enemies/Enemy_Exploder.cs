using UnityEngine;

public class Enemy_Exploder : Enemy
{
    #region Fields
    public float ExplodeRange;
    private bool _hasAttacked = false;

    private CircleCollider2D _cCollider;
    private Rigidbody2D _rb;
    private Animator _anim;
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    public override void Awake()
    {
        base.Awake();

        _cCollider = gameObject.AddComponent<CircleCollider2D>();
        _rb = gameObject.AddComponent<Rigidbody2D>();
        _anim = gameObject.AddComponent<Animator>();

        _cCollider.radius = 0.375f;
        _rb.isKinematic = true;
        _anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimatorControllers/Enemy_Exploder_Parent");
        
        GameObject enemyExploderChild = new GameObject("Child", typeof(Animator), typeof(SpriteRenderer));
        enemyExploderChild.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Circle");
        enemyExploderChild.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimatorControllers/Enemy_Exploder");   //Not sure why, but this line of code prevents localScale from having any effect on the transform, I can print out the correct localScale(0.75f, 0.75f) using Debug.Log, but without adding an extra Scale property in the Idle animation, localScale defaults to 1. This is not true for the other enemies for some reason...they all use the same idle animation(had to create a seperate one for the exploder child)
        enemyExploderChild.transform.localScale = new Vector3(0.75f, 0.75f);
        enemyExploderChild.transform.SetParent(transform, false);
        
        SetEnemyType(EnemyType.Exploder);
    }

    public override void Start()
    {
        base.Start();
        AttackRange = 0.95f;
        AttackTime = 5.0f;
        timer = AttackTime;
        _speed = 1.0f;
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
        
        if (playerTransform != null && playerTransform.gameObject.activeSelf)
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
            AudioManager.Instance.PlaySound(AudioManager.Sounds.EnemyExplode);
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

using System.Threading;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageable
{
    #region Fields
    protected string _name;
    [SerializeField] protected float _speed;
    protected int _lastSplosionIDTouched;

    private GameManager _gameManager;
    public Health Health;
    public Weapon Weapon;
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    public virtual void Awake()
    {

    }

    public virtual void Start()
    {
        _gameManager = GameManager.Instance;
    }
    public virtual void Update()
    {
        Health.RegenHealth();
    }

    public virtual void OnTriggerStay2D(Collider2D other)   //to register touching an Enemy_Exploder's explosions
    {
        if (other.name != "Enemy_Exploder") { return; }     //should be redundant, unless more triggers are added later
        Enemy_Exploder sploder = other.transform.GetComponent<Enemy_Exploder>();
        if (sploder != null && _lastSplosionIDTouched != sploder.GetInstanceID())
        {
            _lastSplosionIDTouched = sploder.GetInstanceID();
            gameObject.GetComponent<IDamageable>().TakeDamage(35.0f);
        }
    }
    #endregion

    #region Public Methods
    public virtual void Move(float speed)
    {

    }

    public virtual void Move(Vector2 target, float attackRange)
    {

    }

    public bool IsEnemy(GameObject obj)
    {
        if (obj.name.Contains("Enemy_"))
        {
            return true;
        }
        return false;
    }

    public abstract void Move(Vector2 direction, Vector2 target);

    public abstract void Shoot(Vector3 direction, float speed);

    public abstract void Die(GameObject whoDied, float delay = 0.0f);

    public abstract void Attack(float interval);
    #endregion

    public virtual void TakeDamage(float damageValue)
    {
        Health.RemoveHealth(damageValue);
        if (Health.GetHealth() <= 0.0f)
        {
            Die(gameObject);
        }
    }
    #region Private Methods
    #endregion
}

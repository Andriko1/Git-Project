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
    private Material[] _explosionsMats;
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    public virtual void Awake()
    {
        _explosionsMats = new Material[3] {
            Resources.Load<Material>("Materials/Impact01"),
            Resources.Load<Material>("Materials/Impact02"),
            Resources.Load<Material>("Materials/Impact03")
        };
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

    public virtual void Die(GameObject whoDied, float delay = 0.0f)
    {
        if (whoDied != null)
        {
            DeathParticle();
        }
    }

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
    private void DeathParticle()
    {
        GameObject splode = new GameObject("ExplosionParticleEffect", typeof(ParticleSystem));
        splode.transform.position = transform.position;
        ParticleSystem ps = splode.GetComponent<ParticleSystem>();
        ps.Stop();
        ParticleSystem.MainModule psm = ps.main;
        psm.duration = 1.5f;
        psm.loop = false;
        psm.startLifetime = 1;
        psm.startSpeed = 0;
        psm.startSize = 5;
        psm.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
        psm.stopAction = ParticleSystemStopAction.Destroy;
        ParticleSystem.MinMaxCurve psmmc = new ParticleSystem.MinMaxCurve(0.0f, Mathf.Deg2Rad * 360.0f);
        psmmc.mode = ParticleSystemCurveMode.TwoConstants;
        psm.startRotation = psmmc;
        ParticleSystem.EmissionModule pse = ps.emission;
        pse.rateOverTime = 0.0f;
        pse.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, 1) });
        ParticleSystem.ShapeModule pss = ps.shape;
        pss.shapeType = ParticleSystemShapeType.Sphere;
        pss.radius = 0.0001f;
        pss.radiusThickness = 0.0f;
        ParticleSystem.TextureSheetAnimationModule pst = ps.textureSheetAnimation;
        pst.enabled = true;             //Wth? Why is this the only module that needs to be enabled?
        pst.numTilesX = 8;
        pst.numTilesY = 8;
        ParticleSystemRenderer psr = ps.GetComponent<ParticleSystemRenderer>();
        psr.material = _explosionsMats[Random.Range(0, 3)];
        psr.maxParticleSize = 1.0f;
        ps.Play();
    }
    #endregion
}

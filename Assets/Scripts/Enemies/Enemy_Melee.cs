using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class Enemy_Melee : Enemy
{
    #region Fields
    private BoxCollider2D _bCollider;
    private Rigidbody2D _rb;
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    public override void Awake()
    {
        base.Awake();
        _bCollider = gameObject.AddComponent<BoxCollider2D>();
        _rb = gameObject.AddComponent<Rigidbody2D>();
        
        _rb.isKinematic = true;

        _bCollider.size = new Vector2(0.75f, 0.75f);

        GameObject enemyMeleeChild = new GameObject("Child", typeof(Animator), typeof(SpriteRenderer));
        enemyMeleeChild.GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Square.png");
        enemyMeleeChild.GetComponent<Animator>().runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Animations/Enemy_Melee.controller");
        enemyMeleeChild.transform.localScale = new Vector3(0.75f, 0.75f);
        enemyMeleeChild.transform.SetParent(transform, false);

        SetEnemyType(EnemyType.Melee);
    }

    public override void Start()
    {
        base.Start();
        AttackRange = 0.933f;
        AttackTime = 1.0f;
        timer = AttackTime;
        _speed = 3.5f;
        Health = new Health(1, 1, 0);
    }

    public override void Update()
    {
        base.Update();  //Basically just calls base.Move()
        if (playerTransform != null && playerTransform.gameObject.activeSelf)
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

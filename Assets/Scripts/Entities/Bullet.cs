using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Fields
    private float _damage = 2.0f;
    private bool _isEnemyBullet;
    private GameManager _gameManager;
    #endregion

    #region Properties
    public bool IsEnemyBullet
    {
        get { return _isEnemyBullet; }
        set { _isEnemyBullet = value; }
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        Destroy(gameObject, 10.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D other = collision.collider;
        IDamageable damageable = other.GetComponent<IDamageable>();
        Character character = other.GetComponent<Character>();
        if ( damageable != null && (other.gameObject.name.Contains("Enemy_") && _isEnemyBullet == false) || (other.gameObject.name == "Player" && _isEnemyBullet == true))
        {
            if ( character != null && character.Health.GetHealth() > 0.0f)
            {
                damageable.TakeDamage(_damage);
                Destroy(this.gameObject);
            }
        }
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion
}
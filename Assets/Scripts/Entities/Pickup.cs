using System;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    #region Fields
    private bool _getBigger;
    private float _ogXScale;
    private float _maxXScale;
    private float _minXScale;
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    private void Start()
    {
        int i = UnityEngine.Random.Range(0, 2);
        _getBigger = Convert.ToBoolean(i);
        _ogXScale = transform.localScale.x;
        _minXScale = _ogXScale * 0.8f;
        _maxXScale = _ogXScale * 1.2f;
    }

    private void Update()
    {
        Breathe();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ( other.gameObject.name == "Player")
        {
            OnPickup();
        }
    }
    #endregion

    #region Public Methods
    public virtual void OnPickup()
    {
        GameManager.Instance.Pickups.Remove(gameObject);
        Player.Instance.PickedUp(this.gameObject);
        Destroy(gameObject);
    }
    #endregion

    #region Private Methods
    private void Breathe()
    {
        if (_getBigger)
        {
            transform.localScale *= 1.0004f;
            if (transform.localScale.x > _maxXScale)
            {
                _getBigger = false;
            }
        }
        else
        {
            transform.localScale *= 0.9996f;
            if (transform.localScale.x < _minXScale)
            {
                _getBigger = true;
            }
        }
    }

    #endregion
}

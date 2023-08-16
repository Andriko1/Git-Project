using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region Fields
    [SerializeField]private Player _player;
    private float _horizontal, _vertical;
    private Vector2 _lookTarget;
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    void Start()
    {
        _player = Player.Instance;
    }

    
    private void Update()
    {
        if ( _player == null ) return;
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(_horizontal) == Mathf.Abs(_vertical) && _horizontal != 0)
        {
            _horizontal *= 0.70710678f;
            _vertical *= 0.70710678f;
        }

        _lookTarget = Input.mousePosition;
        if ( Input.GetMouseButtonDown(0) )
        {
            _player.Shoot(transform.right, 4.0f);
        }

        if ( Input.GetMouseButton(0) )
        {
            _player.AutoShoot(transform.right, 4.0f);
        }

        if ( Input.GetMouseButtonDown(1) )
        {
            _player.Nuke();
        }
    }

    private void FixedUpdate()
    {
        if ( _player != null )
        {
            _player.Move(new Vector2(_horizontal, _vertical), _lookTarget);
        }
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion
}

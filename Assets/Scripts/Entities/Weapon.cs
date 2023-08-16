using UnityEngine;

public class Weapon
{
    #region Fields
    private string _name;
    private float _damageAmount;
    #endregion

    #region Properties
    #endregion

    #region Public Methods
    public void Shoot()
    {
        Debug.Log("Shooting from weapon");
    }
    #endregion

    #region Private Methods
    #endregion
}

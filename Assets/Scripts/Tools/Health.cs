using System;
using UnityEngine;

public class Health
{
    #region Fields
    private float _currentHealth=0f;
    private float _maxHealth;
    private float _healthRegenRate;

    public Action<int> OnHealthUpdate;
    #endregion

    #region Properties
    #endregion

    #region Public Methods
    public Health(float maxHealth = 100f)
    {
        _currentHealth = maxHealth;
        _maxHealth = maxHealth;
        _healthRegenRate = 0f;
        OnHealthUpdate?.Invoke((int)_currentHealth);
    }

    public Health(float maxHealth, float healthRegenRate)
    {
        _currentHealth = maxHealth;
        _maxHealth = maxHealth;
        _healthRegenRate = healthRegenRate;
        OnHealthUpdate?.Invoke((int)_currentHealth);
    }

    public Health(float currentHealth, float maxHealth, float healthRegenRate)
    {
        _currentHealth = currentHealth;
        _maxHealth = maxHealth;
        _healthRegenRate = healthRegenRate;
        OnHealthUpdate?.Invoke((int)_currentHealth);
    }

    public float GetHealth()
    {
        return _currentHealth;
    }

    public void SetHealth(float hp) 
    {
        if (hp > _maxHealth || hp < 0.0f)
        {
            throw new ArgumentException($"A valid range of value for health is between 0 and {_maxHealth}", nameof(hp));
        }
        _currentHealth = hp;
        OnHealthUpdate?.Invoke((int)_currentHealth);
    }

    public void AddHealth(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        OnHealthUpdate?.Invoke((int)_currentHealth);
    }

    public void RemoveHealth(float amount)
    {
        _currentHealth -= amount;
        OnHealthUpdate?.Invoke((int)_currentHealth);
    }

    public void RegenHealth()
    {
        if (_currentHealth < _maxHealth)
        {
            AddHealth(_healthRegenRate * Time.deltaTime);
        }
        OnHealthUpdate?.Invoke((int)_currentHealth);
    }
    #endregion

    #region Private Methods
    #endregion
}

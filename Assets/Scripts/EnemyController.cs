using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using UnityEngine.Scripting.APIUpdating;

public class EnemyController : MonoBehaviour
{
    [SerializeField] SkeletonAnimation _anim;
    [SerializeField] AnimationReferenceAsset _moving, _die;
    [SerializeField] float _moveSpeed;
    [SerializeField] int _maxLife;
    [SerializeField] GameObject _healthBar;
    [SerializeField] Image _health;
    bool isDead;
    public bool IsDead => isDead;
    int _currentLife;

    const string _moveState = "move";
    const string _dieState = "die";
    void Start()
    {
        
    }
    public void Respawn(Vector2 position)
    {
        isDead = false;
        _currentLife = _maxLife;
        UpdateLife();
        transform.position = position;
        gameObject.SetActive(true);
    }
    void Update()
    {
        _healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }
    protected virtual void Move() => SwitchToState(_moveState);
    void SwitchToState(string state)
    {
        if (state == _moveState) SetAnim(_moving, true);
        else if (state == _dieState) SetAnim(_die, false);
    }
    void SetAnim(AnimationReferenceAsset anim, bool loop, float timeScale = 1) => _anim.state.SetAnimation(0, anim, loop).TimeScale = timeScale;

    public void TakeDamage(int damage)
    {
        _currentLife -= damage;
        if (_currentLife <= 0) Die();
        else UpdateLife();
    }

    void Die()
    {
        isDead = true;
        SwitchToState(_dieState);
        Invoke(nameof(Deactive), 2);
        _healthBar.SetActive(false);
    }

    protected virtual void Deactive() => gameObject.SetActive(false);
    void UpdateLife()
    {
        _health.fillAmount = (float)_currentLife / _maxLife;
    }

}

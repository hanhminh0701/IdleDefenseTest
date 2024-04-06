using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using UnityEngine.Scripting.APIUpdating;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected SkeletonAnimation _anim;
    [SerializeField] protected AnimationReferenceAsset _moving, _die, _idle;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected int _maxLife;
    [SerializeField] protected GameObject _healthBar;
    [SerializeField] protected Image _health;
    bool _isDead;
    public bool IsDead => _isDead;
    public Transform Transform => transform;
    int _currentLife;
    bool _isFaceingLeft;
    protected Transform[] _groundPath => GameManager.Instance.GroundPath;
    protected Transform _thronePlace => GameManager.Instance.GroundPath.Last();

    const string _moveState = "move";
    const string _dieState = "die";
    const string _idleState = "idle";

    protected virtual void OnEnable() => GameManager.Instance.ON_GAME_OVER += OnGameOver;
    protected void OnDisable() => GameManager.Instance.ON_GAME_OVER -= OnGameOver;

    private void FixedUpdate()
    {
        if (_isDead) return;
        Move();
        _healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }
    public void Respawn(Vector2 position)
    {
        _isDead = false;
        _currentLife = _maxLife;
        UpdateLife();
        transform.position = position;
        gameObject.SetActive(true);
        _healthBar.SetActive(true);
    }

    protected virtual void Move() => SwitchToState(_moveState);
    protected void Flip(float directionX)
    {
        if (directionX > 0 && _isFaceingLeft) 
        {
            _isFaceingLeft = !_isFaceingLeft;
            Transform.Rotate(0, 180, 0);
        }
        else if (directionX < 0 && !_isFaceingLeft)
        {
            _isFaceingLeft = !_isFaceingLeft;
            Transform.Rotate(0, 180, 0);
        }
    }

    void SwitchToState(string state)
    {
        if (state == _moveState) SetAnim(_moving, true);
        else if (state == _dieState) SetAnim(_die, false);
        else SetAnim(_idle, true);
    }
    void SetAnim(AnimationReferenceAsset anim, bool loop, float timeScale = 1) => _anim.state.SetAnimation(0, anim, loop).TimeScale = timeScale;

    public void TakeDamage(int damage)
    {
        _currentLife -= damage;
        if (_currentLife <= 0) Die();
        else UpdateLife();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Throne")
        {
            gameObject.SetActive(false);
            _healthBar.SetActive(false);
            collision.GetComponent<ThroneHealth>().TakeDamage(1);
        }
    }

    void Die()
    {
        _isDead = true;
        SwitchToState(_dieState);
        Invoke(nameof(Deactive), 2);
        _healthBar.SetActive(false);
    }

    protected virtual void Deactive() => gameObject.SetActive(false);
    void UpdateLife() => _health.fillAmount = (float)_currentLife / _maxLife;

    void OnGameOver()
    {
        SwitchToState(_idleState);
        _isDead = true;
    }
}

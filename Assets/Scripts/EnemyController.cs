using UnityEngine;
using Spine.Unity;
using System.Linq;
using System;

public class EnemyController : MonoBehaviour
{
    public event Action<float> UPDATE_HEALTH_BAR;
    public event Action ON_DEAD;

    [SerializeField] protected SkeletonAnimation _anim;
    [SerializeField] protected AnimationReferenceAsset _moving, _die, _idle;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected int _maxLife;
    [SerializeField] protected HealthBarController _healthBarPrefab;
    HealthBarController _healthBar;
    bool _isDead;
    public Transform Transform => transform;
    int _currentLife;
    bool _isFaceingLeft;
    protected Transform[] _groundPath => GameManager.Instance.GroundPath;
    protected Transform _thronePlace => GameManager.Instance.GroundPath.Last();

    const string _moveState = "move";
    const string _dieState = "die";
    const string _idleState = "idle";

    protected virtual void OnEnable() => GameManager.Instance.ON_GAME_OVER += OnGameOver;
    protected virtual void OnDisable() => GameManager.Instance.ON_GAME_OVER -= OnGameOver;

    private void Update()
    {
        if (_isDead) return;
        Move();
    }

    public void Init()
    {
        _healthBar = Instantiate(_healthBarPrefab, GameManager.Instance.Canvas);
        _healthBar.Init(this);
    }
    public void Respawn(Vector2 position)
    {
        _isDead = false;
        _currentLife = _maxLife;
        UPDATE_HEALTH_BAR?.Invoke((float)_currentLife / _maxLife);
        transform.position = position;
        gameObject.SetActive(true);
        _healthBar.gameObject.SetActive(true);
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
        else if (state == _dieState && _die != null) SetAnim(_die, false);
        else SetAnim(_idle, true);
    }
    void SetAnim(AnimationReferenceAsset anim, bool loop, float timeScale = 1) => _anim.state.SetAnimation(0, anim, loop).TimeScale = timeScale;

    public void TakeDamage(int damage)
    {
        if (_healthBar.gameObject.activeSelf) _healthBar.gameObject.SetActive(true);
        _currentLife -= damage;
        if (_currentLife <= 0) Die();
        else UPDATE_HEALTH_BAR?.Invoke((float)_currentLife/_maxLife);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Throne")
        {
            gameObject.SetActive(false);
            _healthBar.gameObject.SetActive(false);
            collision.GetComponent<ThroneHealth>().TakeDamage(1);
        }
    }

    protected virtual void Die()
    {
        _isDead = true;
        ON_DEAD?.Invoke();
        SwitchToState(_dieState);
        _healthBar.gameObject.SetActive(false);
    }

    void OnGameOver()
    {
        SwitchToState(_idleState);
        _isDead = true;
    }
}

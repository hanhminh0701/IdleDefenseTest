using UnityEngine;
using Spine.Unity;
using System.Linq;
using System;

public class EnemyController : MonoBehaviour
{
    public event Action<float> UPDATE_HEALTH_BAR;
    public event Action ON_DEACTIVE;

    [SerializeField] protected SkeletonAnimation _anim;
    [SerializeField] protected AnimationReferenceAsset _moving, _die, _idle;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected int _maxLife;
    [SerializeField] protected HealthBarController _healthBarPrefab;
    [SerializeField] protected Transform _healthBarPlace;

    HealthBarController _healthBar;
    bool _isDead;
    int _currentLife;
    protected bool _isFacingLeft;

    public bool IsDead => _isDead;
    public Transform Transform => transform;

    protected Transform[] _groundPath => GameManager.Instance.GroundPath;
    protected Transform _thronePlace => GameManager.Instance.GroundPath.Last();
    public Transform HealthBarPlace => _healthBarPlace;

    const string _moveState = "move";
    const string _dieState = "die";
    const string _idleState = "idle";

    void OnEnable() => GameManager.Instance.ON_GAME_OVER += OnGameOver;
    void OnDisable() => GameManager.Instance.ON_GAME_OVER -= OnGameOver;

    void Start()
    {
        SwitchToState(_moveState);
        GameManager.Instance.ON_GAME_OVER += OnGameOver;
    }
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
    public virtual void Respawn(Vector2 position)
    {
        _isDead = false;
        _currentLife = _maxLife;
        transform.position = position;
        SwitchToState(_moveState);
        gameObject.SetActive(true);
        _healthBar.UpdatePosition();
        _healthBar.gameObject.SetActive(true);
    }

    protected virtual void Move() { }
    protected void Flip(float directionX)
    {
        if (directionX > 0 && _isFacingLeft) 
        {
            _isFacingLeft = !_isFacingLeft;
            _anim.transform.Rotate(0, 180, 0);
        }
        else if (directionX < 0 && !_isFacingLeft)
        {
            _isFacingLeft = !_isFacingLeft;
            _anim.transform.Rotate(0, 180, 0);
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

    public void SetOrderInLayer(int index) => _anim.GetComponent<MeshRenderer>().sortingOrder = index;

    protected virtual void Die()
    {
        _isDead = true;
        ON_DEACTIVE?.Invoke();
        SwitchToState(_dieState);
        _healthBar.gameObject.SetActive(false);
    }

    protected virtual void Deactive()
    {
        gameObject.SetActive(false);
        _healthBar.gameObject.SetActive(false);
        ON_DEACTIVE?.Invoke();
    }

    void OnGameOver()
    {
        SwitchToState(_idleState);
        _isDead = true;
    }
}

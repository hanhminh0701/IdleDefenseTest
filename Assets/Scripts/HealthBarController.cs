using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] Image _health;
    Transform _place;

    private void OnEnable() => _health.fillAmount = 1;
    public void Init(EnemyController owner)
    {
        owner.UPDATE_HEALTH_BAR += UpdateHealthBar;
        _place = owner.HealthBarPlace;
    }

    private void Update() => UpdatePosition();

    public void UpdatePosition() => transform.position = Camera.main.WorldToScreenPoint(_place.position);
    void UpdateHealthBar(float percent) => _health.fillAmount = percent;
}

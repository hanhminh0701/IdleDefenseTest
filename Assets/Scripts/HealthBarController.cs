using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] Image _health;
    Transform _place;

    private void OnEnable()
    {
        _health.fillAmount = 1;
        transform.position = _place.transform.position;
    }
    public void Init(EnemyController owner)
    {
        owner.UPDATE_HEALTH_BAR += UpdateHealthBar;
        _place = owner.HealthBarPlace;
        gameObject.SetActive(false);
    }

    private void Update() => transform.position = Camera.main.WorldToScreenPoint(_place.position);

    void UpdateHealthBar(float percent) => _health.fillAmount = percent;
}

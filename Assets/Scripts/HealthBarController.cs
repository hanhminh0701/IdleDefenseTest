using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] Image _health;
    Transform _owner;

    private void OnEnable()
    {
        _health.fillAmount = 1;
        transform.position = _owner.transform.position;
    }
    public void Init(EnemyController owner)
    {
        owner.UPDATE_HEALTH_BAR += UpdateHealthBar;
        _owner = owner.transform;
        gameObject.SetActive(false);
    }

    private void Update() => transform.position = _owner.transform.position;

    void UpdateHealthBar(float percent) => _health.fillAmount = percent;
}

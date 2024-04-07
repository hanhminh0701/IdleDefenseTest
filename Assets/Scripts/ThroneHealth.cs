using UnityEngine;
using UnityEngine.UI;

public class ThroneHealth : MonoBehaviour
{
    [SerializeField] int _maxLife;
    [SerializeField] Image _health;
    int _life;

    private void Start() => _life = _maxLife;
    public void TakeDamage(int damage)
    {
        _life -= damage;
        _health.fillAmount = (float)_life/_maxLife;
        AudioManager.Instance.PlaySFX(SFXType.Throne);
        if (_life <= 0)
        {
            GameManager.Instance.FireGameOver();
            AudioManager.Instance.OnGameOver();
        }
    }
}

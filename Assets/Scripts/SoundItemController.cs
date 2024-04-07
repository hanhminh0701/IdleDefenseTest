using UnityEngine;

public class SoundItemController : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;

    public void UpdateSFX(AudioClip source)
    {
        gameObject.SetActive(true);
        _audioSource.clip = source;
        _audioSource.Play();
        Invoke(nameof(Deactive), _audioSource.clip.length);
    }

    void Deactive()
    {
        _audioSource.clip = null;
        AudioManager.Instance.Sounds.Enqueue(this);
        gameObject.SetActive(false);
    }
}

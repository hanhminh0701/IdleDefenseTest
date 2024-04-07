using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] SoundItemController _soundItemPrefab;
    [SerializeField] Transform _soundPool;
    [SerializeField] AudioClip _slashSFX, _bulletSFX, _groundEnemySFX, _skyEnemySFX, _throneSFX, _gameOverSFX;

    [HideInInspector] public Queue<SoundItemController> Sounds = new();

    public void PlaySFX(SFXType type)
    {
        switch (type)
        {
            case SFXType.Slash: GetSFX(_slashSFX); break;
            case SFXType.Bullet: GetSFX(_bulletSFX); break;
            case SFXType.GroundEnemy: GetSFX(_groundEnemySFX); break;
            case SFXType.SkyEnemy: GetSFX(_skyEnemySFX);break;
            case SFXType.Throne: GetSFX(_throneSFX); break;
        }
    }
    void GetSFX(AudioClip source)
    {
        SoundItemController sound;
        if (Sounds.Count > 0) sound = Sounds.Dequeue();
        else sound = Instantiate(_soundItemPrefab, _soundPool);
        sound.UpdateSFX(source);
    }

}
public enum SFXType { Slash, Bullet, GroundEnemy, SkyEnemy, Throne, GameOver }

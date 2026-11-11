using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; set; }

    // Weapon shooting channel 
    public AudioSource ShootingChannel;

    // Weapons reloading sounds
    public AudioSource reloadingSoundAK47;
    public AudioSource reloadingSoundM1911;


    // audio clips
    public AudioClip AK47Shot;
    public AudioClip PM1911Shot;

    public AudioClip killShot;

    public AudioSource guardChannel;

    // Empty magazine sound
    public AudioSource emptyMagazineSoundM1911;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.PistolM1911:  //play PistolM1911 sound
                ShootingChannel.PlayOneShot(PM1911Shot);
                break;
            case WeaponModel.AK47:
                //play AK47 sound
                ShootingChannel.PlayOneShot(AK47Shot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.PistolM1911: //play PistolM1911 sound
                reloadingSoundM1911.Play();
                break;
            case WeaponModel.AK47:
                //play AK47 sound
                reloadingSoundAK47.Play();
                break;
        }
    }
}

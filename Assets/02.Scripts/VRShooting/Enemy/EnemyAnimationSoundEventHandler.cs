using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationSoundEventHandler : MonoBehaviour
{
    [SerializeField] private AudioClip shootingSound;
    [SerializeField] [Range(0, 1)] private float shootingVolume;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] [Range(0, 1)] private float attackVolume;
    [SerializeField] private AudioClip footstepSound;
    [SerializeField] [Range(0, 1)] private float footstepVolume;
    [SerializeField] private AudioClip throwingSound;
    [SerializeField] [Range(0, 1)] private float throwingVolume;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] [Range(0, 1)] private float dieSoundVolume;
    [SerializeField] private AudioClip[] damageSound;
    [SerializeField] [Range(0, 1)] private float damageSoundVolume;
    [SerializeField] private AudioClip damageVoiceSound;
    [SerializeField] [Range(0, 1)] private float damageVoiceSoundVolume;
    public void ShootingSoundPlay()
    {
        GameManager.AudioManager.PlaySoundEffect(shootingSound, transform.position, shootingVolume);
    }
    public void AttackSoundPlay()
    {
        GameManager.AudioManager.PlaySoundEffect(attackSound, transform.position, attackVolume);
    }
    public void FootStepSoundPlay()
    {
        GameManager.AudioManager.PlaySoundEffect(footstepSound, transform.position, footstepVolume);
    }

    public void ThrowingSoundPlay()
    {
        GameManager.AudioManager.PlaySoundEffect(throwingSound, transform.position, throwingVolume);
    }

    public void DieSoundPlay()
    {
        GameManager.AudioManager.PlaySoundEffect(dieSound, transform.position, dieSoundVolume);
    }

    public void DamageSoundPlay(int index)
    {
        GameManager.AudioManager.PlaySoundEffect(damageSound[index], transform.position, damageSoundVolume);
    }
    public void DamageVoiceSoundPlay(int index)
    {
        GameManager.AudioManager.PlaySoundEffect(damageVoiceSound, transform.position, damageVoiceSoundVolume);
    }
}

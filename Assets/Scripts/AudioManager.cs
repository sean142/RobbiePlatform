using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("環境聲音")]
    public AudioClip ambientClip;
    public AudioClip musicClip;

    [Header("FX音效")]
    public AudioClip deathFXClip;
    public AudioClip orbFXClip;
    public AudioClip doorFXClip;
    public AudioClip startLevelClip;
    public AudioClip winClip;

    [Header("Robbie音效")]
    public AudioClip[] walkStepClips;
    public AudioClip[] crouchStepClips;
    public AudioClip jumpClip;
    public AudioClip deathClip;

    public AudioClip jumpVoiceClip;
    public AudioClip deathVoiceClip;
    public AudioClip orbVoiceClip;

    AudioSource ambientSource;//環境
    AudioSource musicSource;//背景
    AudioSource fxSource;//人聲
    AudioSource playerSource;
    AudioSource voiceSource;

    public AudioMixerGroup ambientGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup FXGroup;
    public AudioMixerGroup playerGroup;
    public AudioMixerGroup voiceGroup;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        //	場景更改時未銷毀的對象
        DontDestroyOnLoad(gameObject);

        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource=  gameObject.AddComponent<AudioSource>();
        playerSource=  gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        ambientSource.outputAudioMixerGroup = ambientGroup;
        musicSource.outputAudioMixerGroup = musicGroup;
        fxSource.outputAudioMixerGroup = FXGroup;
        playerSource.outputAudioMixerGroup = playerGroup;
        voiceSource.outputAudioMixerGroup = voiceGroup;

        StartLevelAudio();
    }

    void StartLevelAudio()
    {
        instance.ambientSource.clip = instance.ambientClip;
        instance.ambientSource.loop = true;
        instance.ambientSource.Play();

        instance.musicSource.clip = instance.musicClip;
        instance.musicSource.loop = true;
        instance.musicSource.Play();

        instance.fxSource.clip = instance.startLevelClip;
        instance.fxSource.Play();
    }

    public static void PlayerWonAudio()
    {
        instance.fxSource.clip = instance.winClip;
        instance.fxSource.Play();
        instance.playerSource.Stop();
    }

    public static void PlayDoorOpenAudio()
    {
        instance.fxSource.clip = instance.doorFXClip;
        instance.fxSource.PlayDelayed(1f);
    }

    public static void PlayFootstrpAudio()
    {
        int index = Random. Range(0, instance.walkStepClips.Length);

        instance.playerSource.clip = instance.walkStepClips[index];
        instance.playerSource.Play();
    }

    public static void PlayCrouchFootstrpAudio()
    {
        int index = Random.Range(0, instance.crouchStepClips.Length);

        instance.playerSource.clip = instance.crouchStepClips[index];
        instance.playerSource.Play();
    }

    public static void PlayerJumpAudio()
    {
        instance.playerSource.clip = instance.jumpClip;
        instance.playerSource.Play();

        instance.voiceSource.clip = instance.jumpVoiceClip;
        instance.voiceSource.Play();
    }

    public static void PlayerDeathAudio()
    {
        instance.playerSource.clip = instance.deathClip;
        instance.playerSource.Play();

        instance.voiceSource.clip = instance.deathVoiceClip;
        instance.voiceSource.Play();

        instance.fxSource.clip = instance.deathFXClip;
        instance.fxSource.Play();
    }

    public static void PlayerOrbAudio()
    {
        instance.fxSource.clip = instance.orbFXClip;
        instance.fxSource.Play();

        instance.voiceSource.clip = instance.orbVoiceClip;
        instance.voiceSource.Play();
    }
}

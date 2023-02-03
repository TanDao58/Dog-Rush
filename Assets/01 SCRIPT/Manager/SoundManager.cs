using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    DestroySound Audio_Prefab = null;
    [SerializeField] AudioClip[] bgSound;
    [SerializeField]
    List<DestroySound> AudioS = new List<DestroySound>();
    [SerializeField]
    List<AudioClip> SoundFiles = new List<AudioClip>();

    void OnEnable()
    {
        Object[] file = Resources.LoadAll("SOUND", typeof(AudioClip));
        foreach (Object o in file)
        {
            SoundFiles.Add((AudioClip)o);
        }
    }
    public void PlaySound(string nameSound, bool isRepeat = false)
    {
        foreach (AudioClip A in SoundFiles)
        {
            if (A.name.ToLower() != nameSound.ToLower())
                continue;
            if (this.gameObject.activeSelf)
            {
                AudioSource source = GetAudioSource();
                source.clip = A;
                source.loop = isRepeat;
                source.gameObject.SetActive(true);
            }
        }
    }
    public void SoundAction(string nameSound, bool isOn = false)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).gameObject.GetComponent<AudioSource>().clip.name == nameSound)
            {
                if (this.transform.GetChild(i).gameObject.activeInHierarchy == isOn) continue;
                this.transform.GetChild(i).gameObject.SetActive(isOn);
                //return;
            }
        }
    }
    public void StopAllSoundExcept(string str = null)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if(str != null)
            {
                if (this.transform.GetChild(i).gameObject.GetComponent<AudioSource>().clip.name == str)
                    continue;
            }
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    AudioSource GetAudioSource()
    {
        foreach (DestroySound D in AudioS)
        {
            if (D.gameObject.activeSelf)
                continue;
            return D.Audio;
        }
        DestroySound D2 = Instantiate(Audio_Prefab, this.transform.position, Quaternion.identity, this.transform).GetComponent<DestroySound>();
        AudioS.Add(D2);
        D2.gameObject.SetActive(false);
        return D2.Audio;
    }
    private void OnDisable()
    {
        for (int i = 1; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
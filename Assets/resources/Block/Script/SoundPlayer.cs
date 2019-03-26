using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioClip Music;
	
	public void Play()
    {
        transform.gameObject.GetComponent<AudioSource>().clip = this.Music;
        transform.gameObject.GetComponent<AudioSource>().Play();
    }

    public void PlayAndDestroyMe()
    {
        transform.gameObject.GetComponent<AudioSource>().clip = this.Music;
        transform.gameObject.GetComponent<AudioSource>().Play();
        StartCoroutine(PlayAndDestroy());
    }

    public void Stop()
    {
        transform.gameObject.GetComponent<AudioSource>().Stop();
    }

    public void Pause()
    {
        transform.gameObject.GetComponent<AudioSource>().Pause();
    }

    public void SetDestroyTime(float Time)
    {
        Invoke("DestroyMe", Time);
    }

    public void StopDestroyTime(float Time)
    {
        CancelInvoke("DestroyMe");
    }

    void DestroyMe()
    {
        Destroy(transform.gameObject);
    }

    IEnumerator PlayAndDestroy()
    {
        while (transform.gameObject.GetComponent<AudioSource>().isPlaying) yield return new WaitForSeconds(0.00001f);
        DestroyMe();
    }

}

using UnityEngine;
using System.Collections;

public class CreditControl : MonoBehaviour
{
    public MovieTexture openingMov;
    bool _hasPlayed = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_hasPlayed && !openingMov.isPlaying)
        {
            openingMov.Stop();
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void PlayCredit()
    {
        renderer.material.mainTexture = openingMov;
        openingMov.loop = false;
        openingMov.Play();
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
            audioSource.Play();
        _hasPlayed = true;
    }
}

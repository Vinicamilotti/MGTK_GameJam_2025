using UnityEngine;

public class SoundManager : MonoBehaviour
{
    bool IsPlaying;
     AudioSource _audioSource;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if(IsPlaying)
        {
            return;
        }

        _audioSource.Play();
        IsPlaying = true;
    }
}

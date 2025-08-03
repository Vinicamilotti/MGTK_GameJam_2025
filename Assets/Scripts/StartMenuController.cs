using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    bool audioIsPlaying = false;
    bool started = false;
    public GameObject libas;
    AudioSource libasAudioSource;
    
    void Start() {
        libasAudioSource = libas.GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        started = true;
    }

    private void Update()
    {

        if (!started)
        {
            return;
        }
        
        if(!audioIsPlaying) {
            libasAudioSource.Play();
            audioIsPlaying = true;
        }
       
        Vector2 movement = new Vector2(1, 0);
        libas.transform.Translate(movement * 20f * Time.deltaTime);

        if (libas.transform.position.x < 26f)
        {
            return;
        }

   
        SceneManager.LoadScene("Level1");
        libasAudioSource.Stop();

    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    bool started = false;
    public GameObject libas;

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
        Vector2 movement = new Vector2(1, 0);
        libas.transform.Translate(movement * 20f * Time.deltaTime);

        if (libas.transform.position.x < 24f)
        {
            return;
        }

        SceneManager.LoadScene("Level1");

    }
}

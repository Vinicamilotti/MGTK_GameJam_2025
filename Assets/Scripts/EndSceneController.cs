using UnityEngine;
using UnityEngine.SceneManagement;


public class EndSceneController : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Restart() {
        SceneManager.LoadScene("Tutorial");
    }
    // Update is called once per frame
}

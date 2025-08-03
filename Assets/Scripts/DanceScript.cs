using UnityEngine;

public class DanceScript : MonoBehaviour
{
    float deltaTime = 0.25f;
    SpriteRenderer sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        if(deltaTime >= 0.5f) 
        {
            deltaTime = 0;
            sprite.flipX = !sprite.flipX;
        }

    }
}

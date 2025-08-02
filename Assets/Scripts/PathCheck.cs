using TMPro;
using UnityEngine;

public class PathCheck : MonoBehaviour
{
    public GameObject player;
    public GameObject path;
    public GameObject startPath;
    public GameObject midPath;
    public GameObject endPath;

    PolygonCollider2D pathCollider;
    PolygonCollider2D startPathCollider;
    PolygonCollider2D endPathCollider;

    public GameObject message;

    TextMeshPro textMessage;
    PlayerMovement playerMovement;

    bool isOnPath = false;

    bool started = false;
    bool canContinue;
    Vector2 closestPointOnPath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pathCollider = path.GetComponent<PolygonCollider2D>();
        startPathCollider = startPath.GetComponent<PolygonCollider2D>();
        endPathCollider = endPath.GetComponent<PolygonCollider2D>();
        playerMovement = player.GetComponent<PlayerMovement>();
        textMessage = message.GetComponent<TextMeshPro>();
    }

    void SetStarted(Vector2 airplanePosition)
    {
        if(started)
        {
            return;
        }
        var startPoint = startPathCollider.ClosestPoint(airplanePosition);
        float distance = Vector2.Distance(airplanePosition, startPoint);

        started = distance == 0 || started;
    }
    bool CheckEnd(Vector2 airplanePosition)
    {
        if(!started)
        {
            return false;
        };

        if(!canContinue)
        {
            return false;
        }
        var endtPoint = endPathCollider.ClosestPoint(airplanePosition);
        float distance = Vector2.Distance(airplanePosition, endtPoint);
    
        return distance == 0;

    }
    // Update is called once per frame
    void Update()
    {

        Vector2 airplanePosition = player.transform.position;
        if (playerMovement.GetState() == PlayerSate.Moving)
        {
            started = false;
            canContinue = false;
            textMessage.SetText("");
            return;
        }

        SetStarted(airplanePosition);
         
        if (!started)
        {
            return;
        }

        textMessage.SetText("Loop!");
        // 2. Find the closest point on the path collider to the airplane
        closestPointOnPath = pathCollider.ClosestPoint(airplanePosition);
        // 3. Calculate the distance between the airplane and that closest point
        float distance = Vector2.Distance(airplanePosition, closestPointOnPath);
        canContinue = true;
        // 4. Check if the distance is within our allowed error margin
        isOnPath = distance <= 1f;
 
        if(CheckEnd(airplanePosition) && canContinue)
        {
            Debug.Log("End of path reached!");
            textMessage.SetText("Congrats!");
            started = false;
            canContinue = false;
            return;
        }

        // You can use this 'isOnPath' boolean for your game logic
        if (!isOnPath)
        {
           textMessage.SetText("Try again");
           canContinue = false;
        }

    }

}

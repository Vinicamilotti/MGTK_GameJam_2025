using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public enum Levels : byte
{
    Tutorial,
    Level1,
    Level2,
    Level3,
    Level4,
    Level5,
    End,
}
public class PathCheck : MonoBehaviour
{
    public Levels nextLevel;
    public GameObject player;
    public GameObject path;
    public GameObject startPath;
    public List<GameObject> midPath;
    public GameObject endPath;

    PolygonCollider2D pathCollider;
    PolygonCollider2D startPathCollider;
    PolygonCollider2D endPathCollider;
    Dictionary<PolygonCollider2D, bool> midPathColliderDict = new();

    public GameObject message;

    TextMeshPro textMessage;
    PlayerMovement playerMovement;
    


    bool middlePointReached = false;

    bool isOnPath = false;

    bool started = false;
    bool canContinue;
    Vector2 closestPointOnPath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
         pathCollider = path.GetComponent<PolygonCollider2D>();
        startPathCollider = startPath.GetComponent<PolygonCollider2D>();
        endPathCollider = endPath.GetComponent<PolygonCollider2D>();
        playerMovement = player.GetComponent<PlayerMovement>();
        textMessage = message.GetComponent<TextMeshPro>();
        foreach(var midpoints in midPath)
        {
            if (midpoints.TryGetComponent(out PolygonCollider2D collider))
            {
                midPathColliderDict.Add(collider, false);
            }
        }

    }
    
    float GetDistance(PolygonCollider2D collider, Vector2 position)
    {
        // 1. Find the closest point on the path collider to the airplane
        Vector2 closestPoint = collider.ClosestPoint(position);
        float distance = Vector2.Distance(position, closestPoint);
        return distance;
    }

    void SetStarted(Vector2 airplanePosition)
    {
        if (started)
        {
            return;
        }
        var distance = GetDistance(startPathCollider, airplanePosition);
        started = distance == 0 || started;
    }

    void MidlePointCheck(Vector2 airplanePosition)
    {
        if (middlePointReached)
        {
            return;
        }
        if (!started)
        {
            return;
        }
        
        foreach (var midPathCollider in midPathColliderDict.ToList())
        {
            if(midPathCollider.Value)
            {
                continue;
            }
            var distance = GetDistance(midPathCollider.Key, airplanePosition);
            if (distance == 0)
            {
                midPathColliderDict[midPathCollider.Key] = true;
                continue;
            }
            midPathColliderDict[midPathCollider.Key] = false;
            
        }

       middlePointReached = midPathColliderDict.Values.All(x => x);
    }
    bool CheckEnd(Vector2 airplanePosition)
    {
        if (!started)
        {
            return false;
        }
        ;

        if (!canContinue)
        {
            return false;
        }
        var distance = GetDistance(endPathCollider, airplanePosition);
        return distance == 0;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 airplanePosition = player.transform.position;
        if (playerMovement.GetState() == PlayerSate.Moving)
        {
            ResetState();
            textMessage.SetText("");
            return;
        }

        SetStarted(airplanePosition);

        if (!started)
        {
            return;
        }

        textMessage.SetText("Loop!");

        MidlePointCheck(airplanePosition);
        // 2. Find the closest point on the path collider to the airplane
        closestPointOnPath = pathCollider.ClosestPoint(airplanePosition);
        // 3. Calculate the distance between the airplane and that closest point
        float distance = Vector2.Distance(airplanePosition, closestPointOnPath);
        canContinue = true;
        // 4. Check if the distance is within our allowed error margin
        isOnPath = distance <= 1f;

        if (CheckEnd(airplanePosition))
        {
            if(!canContinue || !middlePointReached)
            {
                textMessage.SetText("Try Again");
                ResetState();
                return;
            }
            Debug.Log("End of path reached!");
            textMessage.SetText("Congrats!");
            Invoke(nameof(GoToNextLevel), 1f );
            ResetState();
            return;
        }

        // You can use this 'isOnPath' boolean for your game logic
        if (!isOnPath)
        {
            textMessage.SetText("Try again");
            ResetState();
        }

    }

    void GoToNextLevel()
    {
        string lvlName = nextLevel.ToString();
        SceneManager.LoadScene(lvlName);
    }

    private void ResetState()
    {
        foreach(var entry in midPathColliderDict.ToList())
        {
            midPathColliderDict[entry.Key] = false;
        }
        middlePointReached = false;
        canContinue = false;
        started = false;
    }
}

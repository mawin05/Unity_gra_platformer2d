using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField]  GameObject[] waypoints;
    int currentWaypoint = 0;
    [SerializeField]  float speed = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, waypoints[currentWaypoint].transform.position, speed * Time.deltaTime);

        if(Vector2.Distance(this.transform.position, waypoints[currentWaypoint].transform.position) < 0.1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }
}

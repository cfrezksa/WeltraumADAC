using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Movement : MonoBehaviour {

    Waypoint[] waypoints;
    public float velocity = 4f;
    Waypoint targetWaypoint;
    public Waypoint spawnPoint;
    Waypoint lastCheckpoint;
    Waypoint nextCheckpoint;
    public LinkedList<Waypoint> path;

    GameObject temporaryGameObject = null;
	// Use this for initialization
	void Start () {

        // fetch list of all waypoints
        waypoints = FindObjectsOfType<Waypoint>();

        // set current position to given spawn point
        if(spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
            lastCheckpoint = spawnPoint;
            nextCheckpoint = spawnPoint;
        }

    }
	
	// Update is called once per frame
	void Update () {

        // guarantee next checkpoint is set or return
        if(nextCheckpoint == null)
        {
            if(path != null && path.Count > 0)
            {
                nextCheckpoint = path.First.Value;
                path.RemoveFirst();
            } else
            {
                return;
            }
        }


        var pathToGo = nextCheckpoint.transform.position - transform.position;
        var stepLength = velocity * Time.deltaTime;

        if (stepLength >= pathToGo.magnitude)
        {
            //Debug.Log("jump to finish");
            transform.position = nextCheckpoint.transform.position;
            lastCheckpoint = nextCheckpoint;
            nextCheckpoint = null;
        }
        else
        {
            //Debug.Log("Move");
            var direction = pathToGo.normalized;
            transform.position += stepLength * direction;
        }
    }

    // command the robot to move to the target waypoint
    public void setTargetWaypoint(Waypoint target)
    {
        // if target was set to null, just stop
        if (target == null)
        {
            targetWaypoint = null;
            path = new LinkedList<Waypoint>();
            return;
        }

        // if target is the same as current target, don't change anything
        if (target == targetWaypoint)
        {
            return;
        }

        // if we are here, we have a new target and it's not null

        // if we had a target, reset its color
        if(targetWaypoint != null)
        {
            targetWaypoint.GetComponent<Renderer>().material.color = Color.white;
        }

        // set current target to new target
        targetWaypoint = target;

        // set target to red
        target.GetComponent<Renderer>().material.color = Color.red;

        // find shortest path to new target
        path = findShortestWayToTarget(target);
        foreach(Waypoint wp in path)
        {
            Debug.Log(wp.transform.position);
        }
    }

    private LinkedList<Waypoint> findShortestWayToTarget(Waypoint target)
    {

        WaypointMinHeap nodesToExplore;

        // explored nodes ( contain predecessor info)
        HashSet<WaypointMinHeapNode> exploredNodes = new HashSet<WaypointMinHeapNode>();

        // explored Waypoints
        HashSet<Waypoint> exploredWaypoints = new HashSet<Waypoint>();

        // find starting point
        Waypoint startingPoint = null;

        // if we are standing on a waypoint, set this as out starting point
        if (lastCheckpoint != null && transform.position == lastCheckpoint.transform.position) {
            startingPoint = lastCheckpoint;
        } else if(nextCheckpoint != null && transform.position == nextCheckpoint.transform.position)
        {
            startingPoint = nextCheckpoint;
        } else
        // we are between two waypoints, thus create a new one
        {
            if (null != temporaryGameObject)
            {
                Destroy(temporaryGameObject);
            }

            temporaryGameObject = new GameObject();
            startingPoint = temporaryGameObject.AddComponent<Waypoint>();

            if (null == startingPoint) Debug.LogError("statingPoint == null");
            if (startingPoint.transform == null) Debug.LogError("statingPoint.transform == null");

            startingPoint.transform.position = transform.position;
            startingPoint.connections = new Waypoint[2];
            if(lastCheckpoint != null)
            {
                startingPoint.connections[0] = lastCheckpoint;
            }
            if(nextCheckpoint != null)
            {
                startingPoint.connections[1] = nextCheckpoint;
            }
        }

        // add starting point as root of exploredNodes
        nodesToExplore = new WaypointMinHeap(startingPoint);
        exploredWaypoints.Add(startingPoint);

        WaypointMinHeapNode currNode = null;
        do
        {
            currNode = nodesToExplore.getFirst();

            exploredNodes.Add(currNode);
            exploredWaypoints.Add(currNode.waypoint);

            foreach (Waypoint neighbor in currNode.waypoint.connections)
            {
                if (neighbor != null && !exploredWaypoints.Contains(neighbor))
                {
                    nodesToExplore.update(neighbor, currNode);
                }
            }
        } while (currNode.waypoint != target && !nodesToExplore.isEmpty());

        LinkedList<Waypoint> path = new LinkedList<Waypoint>();

        if(currNode.waypoint != target)
        {
            Debug.LogError("no path to target found");
        } else
        {
            WaypointMinHeapNode pathNode = currNode;
            while(pathNode.waypoint != startingPoint)
            {
                path.AddFirst(pathNode.waypoint);
                pathNode = pathNode.predecessorNode;
            }
        }

        return path;
    }
}

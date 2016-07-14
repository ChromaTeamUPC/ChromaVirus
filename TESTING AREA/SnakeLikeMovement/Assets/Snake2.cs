using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Snake2 : MonoBehaviour 
{

    public Transform head;
    public Transform[] segments;
    /*List<Vector3> breadcrumbs;*/

    //public float segmentSpacing; //set controls the spacing between the segments,which is always constant.

    public snakeWaypoint headWP;

    public float headToSegmentDistance;
    public float segmentToSegmentDistance;

    void Start()
    {
        headWP = null;
        //populate the first set of crumbs by the initial positions of the segments.
        /*breadcrumbs = new List<Vector3>();
        breadcrumbs.Add(head.position); //add head first, because that's where the segments will be going.
        for (int i = 0; i < segments.Length; i++) // we have an extra-crumb to mark where the last segment was...
            breadcrumbs.Add(segments[i].position);*/
        for(int i = segments.Length - 1; i >= 0; --i)
        {
            snakeWaypoint swp = new snakeWaypoint();
            swp.position = segments[i].position;
            swp.rotation = segments[i].rotation;

            if (headWP != null)
                swp.next = headWP;

            headWP = swp;
        }

        snakeWaypoint hwp = new snakeWaypoint();
        hwp.position = head.position;
        hwp.rotation = head.rotation;

        if (headWP != null)
            hwp.next = headWP;

        headWP = hwp;
    }

    void Update()
    {
        //Create a new way point if head has moved,and recalculate all segments positions
        Vector3 newHeadPos = head.position;
        if (newHeadPos != headWP.position)
        {
            snakeWaypoint wp = new snakeWaypoint();
            wp.position = newHeadPos;
            wp.rotation = head.transform.rotation;
            wp.next = headWP;
            headWP = wp;

            float totalDistance = headToSegmentDistance;
            float consolidatedDistance = 0f;

            snakeWaypoint current = headWP;
            snakeWaypoint next = headWP.next;
            float distanceBetween = (current.position - next.position).magnitude;

            //for each segment move through the "virtual line"
            for (int i = 0; i < segments.Length; ++i)
            {
                //move through waypoints until we reach desired distance
                while (consolidatedDistance + distanceBetween < totalDistance)
                {
                    consolidatedDistance += distanceBetween;
                    current = next;
                    //if we are in the last waypoint, there is nothing more we can do, so we quit
                    if (next.next == null) return;
                    next = next.next;
                    distanceBetween = (current.position - next.position).magnitude;
                }

                //We reached the waypoints where this segment must be between of
                float remainingDistance = totalDistance - consolidatedDistance;
                Vector3 direction = (next.position - current.position).normalized;
                direction *= remainingDistance;

                segments[i].position = current.position + direction;

                segments[i].rotation = Quaternion.Slerp(current.rotation, next.rotation, remainingDistance / distanceBetween);

                //if it was the final segment, release the oldest waypoints
                if (i == segments.Length - 1)
                    next.next = null; //Remove reference, let garbage collector do its job
                //else add total distance for next iteration
                else
                    totalDistance += segmentToSegmentDistance;
            }
        }   
    }
}

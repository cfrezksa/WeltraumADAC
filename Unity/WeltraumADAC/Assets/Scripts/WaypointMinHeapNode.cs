using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class WaypointMinHeapNode
    {
        public Waypoint waypoint;
        public WaypointMinHeapNode predecessorNode;
        public float pathLength = 0f;


        public WaypointMinHeapNode(Waypoint waypoint, WaypointMinHeapNode predecessorNode)
        {
            this.waypoint = waypoint;
            setPredecessor(predecessorNode);
        }

        public WaypointMinHeapNode(Waypoint waypoint) : this(waypoint, null) {}

        public WaypointMinHeapNode setPredecessor(WaypointMinHeapNode predecessorNode)
        {
            this.predecessorNode = predecessorNode;
            if(predecessorNode != null)
            {
                pathLength = predecessorNode.pathLength + (waypoint.transform.position - predecessorNode.waypoint.transform.position).magnitude;
            }
            return this;
        }
    }
}

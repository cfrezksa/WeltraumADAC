using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Assets.Scripts
{
    class WaypointMinHeap
    {
        ArrayList minHeap;
        Dictionary<Waypoint,WaypointMinHeapNode> waypointToNodeMap;

        public WaypointMinHeap()
        {
            minHeap = new ArrayList();
            waypointToNodeMap = new Dictionary<Waypoint, WaypointMinHeapNode>();
        }

        public WaypointMinHeap(Waypoint root) :this()
        {
            WaypointMinHeapNode node = new WaypointMinHeapNode(root);
            minHeap.Add(node);
            waypointToNodeMap.Add(root, node);
        }

        private Boolean contains(Waypoint waypoint)
        {
            return waypointToNodeMap.ContainsKey(waypoint);
        }

        public WaypointMinHeapNode update(Waypoint waypoint, WaypointMinHeapNode predecessorNode)
        {
            // if waypoint was not added already, add it
            if(!contains(waypoint))
            {
                return add(waypoint, predecessorNode);
            } else
            // otherwise find corresponding node and update it
            {
                WaypointMinHeapNode node = waypointToNodeMap[waypoint];
                node.setPredecessor(predecessorNode);
                bubbleUp(node);
                return node;
            }
        }
        private WaypointMinHeapNode add(Waypoint waypoint, WaypointMinHeapNode predecessorNode)
        {
            // create new node
            WaypointMinHeapNode node = new WaypointMinHeapNode(waypoint, predecessorNode);

            // add node to map
            waypointToNodeMap[waypoint] = node;
            minHeap.Add(node);
            bubbleUp(node);
            return node;
        }

        private void bubbleUp(WaypointMinHeapNode node)
        {
            int currIdx = minHeap.IndexOf(node);
            int parentIdx = getParentIndex(currIdx);
            // termination condition
            if (parentIdx < 0) return;

            WaypointMinHeapNode parentNode = (WaypointMinHeapNode) minHeap[parentIdx];
            
            if(parentNode.pathLength > node.pathLength)
            {
                swap(currIdx, parentIdx);
                bubbleUp(node);
            }
        }

        private int getParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        private void swap(int idx1, int idx2)
        {
            Object temp = minHeap[idx2];
            minHeap[idx2] = minHeap[idx1];
            minHeap[idx1] = temp;
        }

        public WaypointMinHeapNode getFirst()
        {
            if(minHeap.Count > 0)
            {
                WaypointMinHeapNode node = (WaypointMinHeapNode) minHeap[0];
                minHeap.RemoveAt(0);
                return node;
            } else
            {
                return null;
            }
        }

        public Boolean isEmpty()
        {
            return minHeap.Count == 0;
        }
    }
}

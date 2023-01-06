using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PathFindingGraphUpdate : MonoBehaviour
{
    private GridGraph gridGraph;
    private Vector3 position = new Vector3(0, 0, 1);

    private void Start() {
        gridGraph = AstarPath.active.data.gridGraph;
    }

    public void UpdateGraph(Vector3 pos) {
        if (pos == position) {
            return;
        }
        position = pos;
        gridGraph.center = pos;
        AstarPath.active.Scan();
    }

    public void InitializeGraph() {
        position = new Vector3(0, 0, 0);
        gridGraph.center = position;
        AstarPath.active.Scan();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    public Transform player;
    public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake() {
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

	void CreateGrid() {
		grid = new Node[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
				bool openPath = !(Physics2D.OverlapCircle(worldPoint,nodeRadius,unwalkableMask));
				grid[x,y] = new Node(openPath,worldPoint, x, y);
			}
		}
	}

    public List<Node> GetNeighbours(Node node){
    List<Node> neighbour = new List<Node>();
    for (int x=-1; x<=1; x++){
        for(int y=-1; y<=1; y++){
            if(x==0 && y==0){
                continue;
            }

            int checkX = node.gridX + x;
            int checkY = node.gridY + y;

            if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY){
                neighbour.Add(grid[checkX, checkY]);
            }
        }
    }

    return neighbour;
}

	public Node NodeFromWorldPoint(Vector2 worldPosition) {
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
	}

    public List<Node> path;
	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,gridWorldSize.y,1));

	
		if (grid != null) {
            Node playerNode = NodeFromWorldPoint(player.position);
			foreach (Node n in grid) {
				Gizmos.color = (n.openPath)?Color.yellow:Color.red;
                if(playerNode == n){
                    Gizmos.color = Color.cyan;
                }
                if(path != null){
                    if(path.Contains(n)){
                        Gizmos.color = Color.black;
                    }
                }
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
			}
		}
	}
}


// public class Node1 {
	
// 	public bool walkable;
// 	public Vector3 worldPosition;
	
// 	public Node(bool _walkable, Vector3 _worldPos) {
// 		walkable = _walkable;
// 		worldPosition = _worldPos;
// 	}
// }
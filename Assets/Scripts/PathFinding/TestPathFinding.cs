using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathFinding : MonoBehaviour
{

	public Transform Destination;

	public PathFindingGrid Grid;

	private List<Vector3> path = new List<Vector3>();
	
	void Start () {
		
	}
	
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return))
		{
			Debug.Log("StartPathFinding");
			GoToDestination();
		}
	}

	private void GoToDestination()
	{
		path = Grid.FindPathTo(transform.position, Destination.position);
		Debug.Log("GoToDestination Pass, path lenght: "+path.Count);
	}
	
	void OnDrawGizmosSelected() {
		Gizmos.color = new Color(1, 0, 0, 1.0F);
		for (int i = 0; i < path.Count-1; i++)
		{
			Gizmos.DrawLine(path[i],path[i+1]);
		}
			
	}
}

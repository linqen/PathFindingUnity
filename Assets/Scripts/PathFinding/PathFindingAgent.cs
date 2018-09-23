using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingAgent : MonoBehaviour
{
	public PathFindingGrid Grid;

	public List<Vector3> GetPathToDestination(Vector3 destination)
	{
		return Grid.FindPathTo(transform.position, destination);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNode
{
	public Vector3 Position;
	public List<PathFindingNode> Adjacents = new List<PathFindingNode>();
	public float Cost=0;
	public float Heuristic=0;
	public PathFindingNode Parent=null;

	public void Reset()
	{
		Cost=0;
		Heuristic=0;
		Parent=null;
	}

	public float CostPlusHeuristic()
	{
		return Cost + Heuristic;
	}
}

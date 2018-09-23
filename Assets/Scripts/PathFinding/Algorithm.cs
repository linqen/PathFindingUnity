using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour
{
	protected List<PathFindingNode> _openNodes = new List<PathFindingNode>();
	protected List<PathFindingNode> _closedNodes = new List<PathFindingNode>();

	protected List<PathFindingNode> _pathFinal = new List<PathFindingNode>();
	protected bool _debugModeEnabled = false;

	public virtual List<PathFindingNode> FindPath(List<List<PathFindingNode>> grid, PathFindingNode origin,
		PathFindingNode destination){return null;}
	
	public virtual void FindPathToDebug(List<List<PathFindingNode>> grid, PathFindingNode origin,
		PathFindingNode destination){}

	protected void CleanValues(List<List<PathFindingNode>> grid)
	{
		for (int i = 0; i < grid.Count; i++)
		{
			for (int j = 0; j < grid[i].Count; j++)
			{
				grid[i][j].Reset();
			}
		}
		_openNodes.Clear();
		_closedNodes.Clear();
	}
}

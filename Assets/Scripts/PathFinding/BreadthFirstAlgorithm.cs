using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstAlgorithm : Algorithm
{
	public override List<PathFindingNode> FindPath(List<List<PathFindingNode>> grid, PathFindingNode origin, PathFindingNode destination)
	{
		_openNodes.Add(origin);
		while (_openNodes.Count != 0)
		{
			PathFindingNode node = _openNodes[0];
			if (node == destination)
			{
				List<PathFindingNode> path = GetPath(node);
				CleanValues(grid);
				return path;
			}

			for (int i = 0; i < node.Adjacents.Count; i++)
			{
				PathFindingNode actualAdjacentNode = node.Adjacents[i];
				bool isOnClosed = false;
				bool isOnOpen = false;
				for (int j = 0; j < _closedNodes.Count; j++)
				{
					if (actualAdjacentNode == _closedNodes[j])
					{
						isOnClosed = true;
						j = _closedNodes.Count;
					}
				}
				for (int j = 0; j < _openNodes.Count; j++)
				{
					if (actualAdjacentNode == _openNodes[j])
					{
						isOnOpen = true;
						j = _openNodes.Count;
					}
				}

				if (!isOnClosed && !isOnOpen)
				{
					actualAdjacentNode.Parent = node;
					_openNodes.Add(actualAdjacentNode);
				}
			}
			_openNodes.Remove(node);
			_closedNodes.Add(node);
		}
		return null;
	}
	
	
	
	
	
	
	
	//DEBUG///////////////////////////////////////////////////////////
	public override void FindPathToDebug(List<List<PathFindingNode>> grid, PathFindingNode origin,
		PathFindingNode destination)
	{
		_debugModeEnabled = true;
		StartCoroutine(FindPathDebug(grid, origin, destination));
	}
	
	IEnumerator FindPathDebug(List<List<PathFindingNode>> grid, PathFindingNode origin, PathFindingNode destination)
	{
		_openNodes.Add(origin);
		while (_openNodes.Count != 0)
		{
			PathFindingNode node = _openNodes[0];
			if (node == destination)
			{
				Debug.Log("Finded Destination");
				_pathFinal = GetPath(node);
				CleanValues(grid);
				break;
			}

			for (int i = 0; i < node.Adjacents.Count; i++)
			{
				PathFindingNode actualAdjacentNode = node.Adjacents[i];
				bool isOnClosed = false;
				bool isOnOpen = false;
				for (int j = 0; j < _closedNodes.Count; j++)
				{
					if (actualAdjacentNode == _closedNodes[j])
					{
						isOnClosed = true;
						j = _closedNodes.Count;
					}
				}
				for (int j = 0; j < _openNodes.Count; j++)
				{
					if (actualAdjacentNode == _openNodes[j])
					{
						isOnOpen = true;
						j = _openNodes.Count;
					}
				}

				if (!isOnClosed && !isOnOpen)
				{
					actualAdjacentNode.Parent = node;
					_openNodes.Add(actualAdjacentNode);
				}
			}
			_openNodes.Remove(node);
			_closedNodes.Add(node);
			yield return new WaitForSeconds(0.01f);
		}
	}

	private List<PathFindingNode> GetPath(PathFindingNode node)
	{
		List<PathFindingNode>path=new List<PathFindingNode>();
		path.Add(node);
		while (node.Parent!=null)
		{
			path.Add(node.Parent);
			node = node.Parent;
		}

		return path;
	}

	private void OnDrawGizmos()
	{
		if (_debugModeEnabled)
		{
			for (int i = 0; i < _openNodes.Count; i++)
			{
				Gizmos.color = new Color(1, 1, 1, 1);

				Gizmos.DrawWireCube(_openNodes[i].Position, Vector3.one);
			}

			for (int i = 0; i < _pathFinal.Count; i++)
			{
				Gizmos.color = new Color(1, 0, 0, 1);
				Gizmos.DrawWireCube(_pathFinal[i].Position, Vector3.one);

			}
		}
	}
}

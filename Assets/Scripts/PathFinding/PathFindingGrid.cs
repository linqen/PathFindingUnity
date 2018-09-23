using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public enum AlgorithmType
{
	ThetaStar,AStar,Dijkstra,DepthFirst,BreadthFirst
}

[ExecuteInEditMode]
public class PathFindingGrid : MonoBehaviour
{
	public bool DrawQuadOnEditor = true;
	public bool DebugMode = false;
	public int QuadAreaSize;
	public AlgorithmType AlgorithmType;
	[Range(0.1f, 5.0f)] public float DistanceBetweenNodes;

	private List<List<PathFindingNode>> _grid = new List<List<PathFindingNode>>();
	private Algorithm _algorithm;
	
	void Start ()
	{
		CreateGrid();
		SetAdjacents();
		GetAlgorithmByType();
	}

	private void CreateGrid()
	{
		for (int i = 0; i <= QuadAreaSize; i++)
		{
			_grid.Add(new List<PathFindingNode>());
			for (int j = 0; j <= QuadAreaSize; j++)
			{
				PathFindingNode newNode = new PathFindingNode();
				newNode.Position = new Vector3(transform.position.x + (i * DistanceBetweenNodes),
					transform.position.y,
					transform.position.z + (j * DistanceBetweenNodes));
				_grid[i].Add(newNode);
			}
		}
	}
		
	private void SetAdjacents()
	{
		for (int i = 0; i < _grid.Count; i++)
		{
			for (int j = 0; j < _grid[i].Count; j++)
			{
				if (j + 1 < _grid[i].Count && CanReachNode(_grid[i][j], _grid[i][j + 1]))
				{
					_grid[i][j].Adjacents.Add(_grid[i][j + 1]);
				}
				if (j - 1 > 0 && CanReachNode(_grid[i][j], _grid[i][j - 1]))
				{
					_grid[i][j].Adjacents.Add(_grid[i][j-1]);
				}
				if (i + 1 < _grid.Count && CanReachNode(_grid[i][j], _grid[i+1][j]))
				{
					_grid[i][j].Adjacents.Add(_grid[i+1][j]);
				}
				if (i - 1 > 0 && CanReachNode(_grid[i][j], _grid[i-1][j]))
				{
					_grid[i][j].Adjacents.Add(_grid[i-1][j]);
				}
			}
		}
	}
	
	private void GetAlgorithmByType()
	{
		switch (AlgorithmType)
		{
			case AlgorithmType.DepthFirst:
				_algorithm = GetComponent<DepthFirstAlgorithm>();
				break;

			case AlgorithmType.BreadthFirst:
				_algorithm = GetComponent<BreadthFirstAlgorithm>();
				break;

			case AlgorithmType.Dijkstra:
				_algorithm = GetComponent<DijkstraAlgorithm>();
				break;

			case AlgorithmType.AStar:
				_algorithm = GetComponent<AStarAlgorithm>();
				break;

			case AlgorithmType.ThetaStar:
				_algorithm = GetComponent<ThetaAlgorithm>();
				break;

		}
	}


	private bool CanReachNode(PathFindingNode a, PathFindingNode b)
	{
		bool hit = Physics.Linecast(a.Position, b.Position);
		return !hit;
	}

	public List<Vector3> FindPathTo(Vector3 origin, Vector3 destination)
	{
		PathFindingNode originNode = GetNearNode(origin);
		PathFindingNode destinationNode = GetNearNode(destination);
		//For Debug
		if (DebugMode)
		{
			_algorithm.FindPathToDebug(_grid, originNode, destinationNode);
			return null;
		}
		//For Debug
		
		List<Vector3> path = new List<Vector3>();
		List<PathFindingNode> pathNodes = _algorithm.FindPath(_grid,originNode,destinationNode);
		if (pathNodes != null)
		{
			for (int i = 0; i < pathNodes.Count; i++)
			{
				path.Add(pathNodes[pathNodes.Count - 1 - i].Position);
			}
		}
		return path;
	}

	private PathFindingNode GetNearNode(Vector3 position)
	{
		int indexI = 0, indexJ = 0;
		float distance = 99999999;
		for (int i = 0; i < _grid.Count; i++)
		{
			for (int j = 0; j < _grid[i].Count; j++)
			{
				float magnitude = (_grid[i][j].Position - position).magnitude;
				if (magnitude < distance)
				{
					//Ignore Y to avoid linecast hit with terrain or floor
					Vector3 nodePositionXZ = new Vector3(_grid[i][j].Position.x,position.y, _grid[i][j].Position.z);
					bool canReach = !Physics.Linecast(position, nodePositionXZ);
					if (canReach)
					{
						indexI = i;
						indexJ = j;
						distance = magnitude;
					}
				}
			}
		}
		return _grid[indexI][indexJ];
	}
	
	void OnDrawGizmosSelected() {
		if (DrawQuadOnEditor)
		{
			Gizmos.color = new Color(0, 1, 0, 1.0F);
			float halfQuadAreaSize = QuadAreaSize / 2.0f;
			Gizmos.DrawWireCube(new Vector3(transform.position.x+halfQuadAreaSize,transform.position.y,transform.position.z+halfQuadAreaSize),
				new Vector3(QuadAreaSize, transform.position.y, QuadAreaSize));
			
			for (int i = 0; i < _grid.Count; i++)
			{
				for (int j = 0; j < _grid[i].Count; j++)
				{
					Gizmos.DrawLine(_grid[i][j].Position,_grid[i][j].Position+Vector3.up);

					for (int k = 0; k < _grid[i][j].Adjacents.Count; k++)
					{
						Gizmos.DrawLine(_grid[i][j].Position,_grid[i][j].Adjacents[k].Position);
					}
					
				}
			}
			
		}
	}
}

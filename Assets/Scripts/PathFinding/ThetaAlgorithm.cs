using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThetaAlgorithm : AStarAlgorithm
{
	protected override List<PathFindingNode> GetPath(PathFindingNode node)
	{
		List<PathFindingNode>path=new List<PathFindingNode>();
		path.Add(node);
		while (node.Parent!=null)
		{
			path.Add(node.Parent);
			node = node.Parent;
		}
		List<PathFindingNode>optimizedPath=new List<PathFindingNode>();
		optimizedPath.Add(path[0]);
		while (optimizedPath[optimizedPath.Count-1]!=path[path.Count-1])
		{
			for (int i = 0; i < path.Count; i++)
			{
				bool canReach = !Physics.Linecast(optimizedPath[optimizedPath.Count - 1].Position, path[path.Count - 1 - i].Position);
				if (canReach)
				{
					optimizedPath.Add(path[path.Count - 1 - i]);
					i = path.Count;
				}
			}
			
		}

		return optimizedPath;
	}
}

using System.Collections.Generic;

namespace Aladdin.PathFinding
{
	static public class AStar
	{
		static public T[] FindPath<T>(T startNode, T endNode) where T : IAstarNode<T>
		{
			BinHeap<T> openList = new BinHeap<T>();
			List<T> closeList = new List<T>();

			startNode.g = 0;
			startNode.f = startNode.heuristic(endNode);

			T currentNode = startNode;

			while (!ReferenceEquals(currentNode, endNode))
			{
				closeList.Add(currentNode);
				foreach (T testNode in currentNode.neighbours)
				{
					if (null == testNode || closeList.Contains(testNode)){
						continue;
					}
					if (!currentNode.canWalkTo(testNode)){
						continue;
					}

					bool isTestNodeInOpen = openList.has(testNode);
					float g = currentNode.g + currentNode.heuristic(testNode);
					float f = g + testNode.heuristic(endNode);

					if (isTestNodeInOpen && f >= testNode.f){
						continue;
					}

					testNode.parent = currentNode;
					testNode.g = g;
					testNode.f = f;
					if (isTestNodeInOpen){
						openList.update(testNode);
					}else{
						openList.push(testNode);
					}
				}

				if (openList.isEmpty()){
					return null;
				}
				currentNode = openList.shift();
			}

			return BuildPath(startNode, endNode);
		}

		static T[] BuildPath<T>(T startNode, T endNode) where T : IAstarNode<T>
		{
			List<T> path = new List<T>();

			T node = endNode;

			while (!ReferenceEquals(node, startNode))
			{
				path.Add(node);
				node = node.parent;
			}

			path.Add(startNode);
			path.Reverse();

			for(int i=0, n=path.Count; i<n; ++i)
			{
				if (!path[i].walkable)
				{
					path.RemoveRange(i, n-i);
					break;
				}
			}

			return path.ToArray();
		}
	}
}

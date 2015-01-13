using System;
using System.Collections.Generic;

namespace Aladdin.PathFinding
{
	public class AStarGridNode : IAstarNode<AStarGridNode>
	{
		static public AStarGridNode[,] Create(int[,] mapData)
		{
			int numRows = 0;

			var result = new AStarGridNode[mapData.GetLength(0), mapData.GetLength(1)];

			for (int i=0; i<mapData.GetLength(0); ++i)
			{
				for (int j=0; j<mapData.GetLength(1); ++j)
				{
					result[i, j] = new AStarGridNode(i, j, result, mapData);
				}
			}

			return result;
		}

		private int _x;
		private int _y;
		private AStarGridNode[,] _grids;
		private int[,] _mapData;

		public AStarGridNode(int x, int y, AStarGridNode[,] grids, int[,] mapData)
		{
			_x = x;
			_y = y;
		}

		public float x { get { return _x; } }
		public float y { get { return _y; } }

		public bool walkable
		{
			get {
				return true;
			}
		}

		public AStarGridNode parent { get; set; }
		public float g { get; set; }
		public float f { get; set; }

		public AStarGridNode[] neighbours
		{
			get {
				List<AStarGridNode> neighbours = new List<AStarGridNode>();

				int startX = Math.Max(0, _x - 1);
				int startY = Math.Max(0, _y - 1);
				int endX = Math.Min(_mapData.GetLength(0) - 1, _x + 1);
				int endY = Math.Min(_mapData.GetLength(1) - 1, _y + 1);

				for (int i = startX; i <= endX; i++)
				{
					for (int j = startY; j <= endY; j++)
					{
						AStarGridNode node = _grids[i, j];
						if (this != node)
						{
							neighbours.Add(node);
						}
					}
				}

				return neighbours.ToArray();
			}
		}

		public bool canWalkTo(AStarGridNode target)
		{
			//还有检测对角
			return walkable;
		}

		public float heuristic(AStarGridNode target)
		{
			float dx = target.x - x;
			float dy = target.y - y;
			return (float)Math.Sqrt(dx*dx + dy*dy);
		}

		public int CompareTo(AStarGridNode other)
		{
			float d = f - other.f;
			if (d < 0){
				return -1;
			}
			if (d > 0){
				return 1;
			}
			return 0;
		}
	}
}

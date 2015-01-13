using System;

namespace Aladdin.PathFinding
{
	public interface IAstarNode<T> : IComparable<T>
	{
		float x { get; }
		float y { get; }
		bool walkable { get; }
		T[] neighbours { get; }

		T parent { get; set; }
		float g { get; set; }
		float f { get; set; }
		
		bool canWalkTo(T target);
		float heuristic(T target);
	}
}

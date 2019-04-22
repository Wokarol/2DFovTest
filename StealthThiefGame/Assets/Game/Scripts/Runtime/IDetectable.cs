using UnityEngine;

namespace Wokarol
{
    public interface IDetectable
    {
        Vector2 Position { get; }
        Vector2[] GetEdgePoints(Vector2 source);
    }
}
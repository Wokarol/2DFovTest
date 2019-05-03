using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Wokarol
{
    public class ObstacleBuilderEditor : Editor
    {
        [MenuItem("Tools/Rebuild Corners")]
        static void RebuildCorners() {
            ObstacleManager.RebuildCorners();
        }
    } 
}

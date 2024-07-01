using UnityEngine;
using UnityEditor.Tilemaps;

[CreateAssetMenu(fileName = "Prefab Brush", menuName = "Level Building/Prefab Brush")]
[CustomGridBrush(false, true, false, "Prefab Brush")]
public class PrefabBrush : GameObjectBrush
{

}
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TileMap))]
public class TileMapLoader : Editor {
	
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		DrawDefaultInspector();
	}
}

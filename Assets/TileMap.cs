using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour {
	
	List<TileLayer> layers = new List<TileLayer>();
	List<TileSet> sets = new List<TileSet>();
	
	// Use this for initialization
	void Start () {
		//Load File
		//Parse File
		//foreach layer 
		//foreach 
		
	}
	
	void LoadFile(){
	}
	
	void ParseFile(){
	}
	
	void GenerateLayers(){
	}
}

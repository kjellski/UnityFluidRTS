using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

using System.Xml;
using System.Xml.Schema;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap  : MonoBehaviour
{
    private readonly List<TileLayer> _tileLayers = new List<TileLayer>();
    private readonly List<TileSet> _tileSets = new List<TileSet>();
    //private bool _tileSetsLoaded = false;
    //private bool _tileLayersLoaded = false;
    //private bool _mapLoaded = false;

    // Use this for initialization
    void Start()
    {
        LoadFile("level002");
    }

    void LoadFile(string path)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(path);
        
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);
        ParseFile(xmlDoc);
    }

    void ParseFile(XmlDocument xmlData)
    {
        // there is only one map per map file, we'll parse this one
        var xmlNodeList = xmlData.SelectNodes("map");
        if (xmlNodeList != null) ParseMap(xmlNodeList.Item(0));
        else Debug.LogError("Unable to parse the map file, no <map> element found.");
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("TileMap{");
        foreach (var tileset in _tileSets)
        {
            sb.Append(tileset);
        }
        foreach (var tilelayer in _tileLayers)
        {
            sb.Append(tilelayer);
        }
        sb.Append("}");
        return sb.ToString();
    }

    public TileSet GetTileSetByGid(int gid)
    {
        foreach (var tileset in _tileSets.Where(tileset => tileset.Conains(gid)))
        {
            return tileset;
        }

        throw new NotSupportedException("Unknown tile gid, not found in any known TileSet.");
    }

    private void ParseMap(XmlNode mapNode)
    {
        var tilesetsXmlNodeList = mapNode.SelectNodes("tileset");
        if (tilesetsXmlNodeList != null) ParseTileSets(tilesetsXmlNodeList);
        else Debug.LogError("Unable to parse the map file, no <tileset> element found for <map>.");

        var tilellayerXmlNodeList = mapNode.SelectNodes("layer");
        if (tilellayerXmlNodeList != null) ParseTileLayers(tilellayerXmlNodeList);
        else Debug.LogError("Unable to parse the map file, no <layer> element found for <map>.");

        //_mapLoaded = true;
        Debug.Log("Parsed map.");
    }

    private void ParseTileLayers(XmlNodeList tilelayersXmlNodeList)
    {
        foreach (XmlNode tileset in tilelayersXmlNodeList)
        {
            _tileLayers.Add(TileLayer.ParseTileLayer(tileset, this));
        }
        //_tileLayersLoaded = true;
        Debug.Log(this);
        Debug.Log("Parsed TileLayers.");
    }

    private void ParseTileSets(XmlNodeList tilesetsXmlNodeList)
    {
        foreach (XmlNode tileset in tilesetsXmlNodeList)
        {
            _tileSets.Add(TileSet.ParseTileSet(tileset));
        }
        //_tileSetsLoaded = true;
        Debug.Log(this);
        Debug.Log("Parsed TileSets.");
    }

}

using System.Text;
using System.Xml;
using UnityEngine;
using System.Collections;
using Debug = System.Diagnostics.Debug;

public class TileLayer // : MonoBehaviour
{
    private Tile[,] _data;
    private int _width;
    private int _height;
    private string _name;

    /// <summary>
    /// private to prevent instantiation
    /// </summary>
    private TileLayer() { }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("TileLayer{");
        sb.Append(" Name: " + _name);
        sb.Append(", Width: " + _width);
        sb.Append(", Height: " + _height);
        sb.Append(", Data: [");
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                sb.Append(_data[x,y]);
            }
        }   
        sb.Append("] ");
        sb.Append("}");
        return sb.ToString();
    }

    public TileLayer(int width, int height, string name, Tile[,] data)
    {
        _width = width;
        _height = height;
        _name = name;
        _data = data;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Parses the example xml element structure to an object of this type:
    /// <note>
    ///     <layer name="background" width="20" height="20">
    ///         <data>
    ///             <tile gid="1"/>
    ///             ...
    ///         </data>
    ///     </layer>
    /// </note>
    /// </summary>
    /// <param name="tileLayerXmlNode"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static TileLayer ParseTileLayer(XmlNode tileLayerXmlNode, TileMap map)
    {
        int widht = int.Parse(tileLayerXmlNode.Attributes["width"].Value);
        int height = int.Parse(tileLayerXmlNode.Attributes["height"].Value);
        string name = tileLayerXmlNode.Attributes["name"].Value;
        Tile[,] data = ParseTiles(tileLayerXmlNode.SelectNodes("data/tile"), map, widht, height);

        return new TileLayer(widht, height, name, data);
    }

    public static Tile[,] ParseTiles(XmlNodeList tilesXmlNodeList, TileMap map, int width, int height)
    {
        var tiles = new Tile[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tiles[x, y] = Tile.ParseTile(tilesXmlNodeList.Item(x * width + y), map, x, y);
            }
        }

        return tiles;
    }
}

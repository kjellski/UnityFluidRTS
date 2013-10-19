using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UnityEngine;

public class TileSet // : MonoBehaviour
{
    private readonly Dictionary<int, Texture2D> _textureParts = new Dictionary<int, Texture2D>();

    public TileSet(int firstgid,
        Texture2D texture,
        //Dictionary<int, Dictionary<string, string>> tileProperties,
        int imageheight,
        int imagewidth,
        //int margin,
        String name,
        //int spacing,
        int tileheight,
        int tilewidth)
    {
        FirstGid = firstgid;
        Texture = texture;
        //TileProperties = tileProperties;
        ImageHeight = imageheight;
        ImageWidth = imagewidth;
        TileHeight = tileheight;
        //Spacing = spacing;
        Name = name;
        //Margin = margin;
        TileWidth = tilewidth;
    }

    public int FirstGid { get; private set; }

    public Texture2D Texture { get; private set; }
    // tile array for individual properties
    //public Dictionary<int, Dictionary<string, string>> TileProperties { get; private set; }

    public int ImageHeight { get; private set; }
    public int ImageWidth { get; private set; }
    //public int Margin { get; private set; }
    public String Name { get; private set; }
    //public int Spacing { get; private set; }
    public int TileHeight { get; private set; }
    public int TileWidth { get; private set; }

    // cached properties as parts of Texture
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("TileSet{");
        sb.Append(" FirstGid: " + FirstGid);
        sb.Append(", Texture: " + Texture.GetHashCode());
        sb.Append(", ImageHeight: " + ImageHeight);
        sb.Append(", ImageWidth: " + ImageWidth);
        sb.Append(", Name: " + Name);
        sb.Append(", TileHeight: " + TileHeight);
        sb.Append(", TileWidth: " + TileWidth);
        sb.Append("}");
        return sb.ToString();
    }

    public Texture2D GetTilesTexture2D(int gid)
    {
        if (_textureParts.ContainsKey(gid))
            return _textureParts[gid];

        int tileNumber = gid - 1;
        int x = (tileNumber % TileWidth);
        var y = (int)Math.Floor((double)(tileNumber / TileWidth));
        Debug.Log("GetTilesTexture2D(" + gid + ") => (" + x + ", " + y + ")");

        var newTexture = new Texture2D(TileWidth, TileHeight);
        Color[] pixels = Texture.GetPixels(x * TileWidth, y * TileHeight, TileWidth, TileHeight);
        newTexture.SetPixels(pixels);
        newTexture.Apply();

        // memoize for next time;
        _textureParts[gid] = newTexture;

        return newTexture;
    }

    public bool Conains(int gid)
    {
        int upper = (ImageHeight / TileHeight) * (ImageHeight / TileHeight);
        int lower = FirstGid;
        return gid <= upper && gid >= lower;
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }


    /// <summary>
    ///     Parses xml of the following format into an instance of TileSet
    ///     <note>
    ///         <tileset firstgid="1" name="terrain" tilewidth="32" tileheight="32">
    ///             <image source="../Images/terrain.png" width="1024" height="1024" />
    ///             <tile id="0">
    ///                 <properties>
    ///                     <property name="testName" value="testValue" />
    ///                 </properties>
    ///             </tile>
    ///             <tile id="66">
    ///                 <properties>
    ///                     <property name="newName" value="newValue" />
    ///                 </properties>
    ///             </tile>
    ///         </tileset>
    ///     </note>
    /// </summary>
    /// <param name="tilesetXmlNode"></param>
    /// <returns></returns>
    public static TileSet ParseTileSet(XmlNode tilesetXmlNode)
    {
        // read the tileset node itself
        int firstgid = int.Parse(tilesetXmlNode.Attributes["firstgid"].Value);
        string name = tilesetXmlNode.Attributes["name"].Value;
        int tileheight = int.Parse(tilesetXmlNode.Attributes["tileheight"].Value);
        int tilewidth = int.Parse(tilesetXmlNode.Attributes["tileheight"].Value);

        // read the image node
        XmlNode imageXmlNode = tilesetXmlNode.SelectNodes("image").Item(0);
        int imageheight = GetImageHeight(imageXmlNode);
        int imagewidth = GetImageWidth(imageXmlNode);
        Texture2D texture = ParseImage(imageXmlNode, imagewidth, imageheight);

        return new TileSet(firstgid, texture, imageheight, imagewidth, name, tileheight, tilewidth);
    }

    private static Texture2D ParseImage(XmlNode imageXmlNode, int imagewidth, int imageheight)
    {
        string path = imageXmlNode.Attributes["source"].Value;
        path = "file://" + Application.dataPath + "/Resources/"+ path;
        Debug.Log(path);
        var www = new WWW(path);
        
        //var texture = new Texture(imagewidth, imageheight);
        //www.LoadImageIntoTexture(texture);

        //var texture = new Texture(imagewidth, imageheight);
        //texture.LoadImage(imageTextAsset.bytes);
        return www.texture;
    }

    private static int GetImageHeight(XmlNode imageXmlNode)
    {
        return int.Parse(imageXmlNode.Attributes["height"].Value);
    }

    private static int GetImageWidth(XmlNode imageXmlNode)
    {
        return int.Parse(imageXmlNode.Attributes["width"].Value);
    }
}
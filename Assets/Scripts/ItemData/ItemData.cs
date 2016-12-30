using System.Collections.Generic;
using System.Xml.Serialization;

//Contains all data for skill stats
[XmlRoot("ItemData")]
public class ItemData
{
    [XmlArray("Items")]
    [XmlArrayItem("Item")]
    public List<Item> Items = new List<Item>();
}

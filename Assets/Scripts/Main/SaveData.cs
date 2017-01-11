using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;

[Serializable ()]
public class SaveData : ISerializable
{
    public int[] unitID = { 1, -1, -1, -1 };
    public int[] cHp = { 12, 10, 0, 0 };
    public int[] cMp = { 20, 5, 0, 0 };
    public int[] inventory = { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
    public int[] inventoryStackSize = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

    public SaveData() {
        
    }
    public SaveData(SerializationInfo info, StreamingContext ctxt)
    {
        //Get the values from info and assign them to the appropriate properties
        unitID = (int[])info.GetValue("unitID", typeof(int[]));
        cHp = (int[])info.GetValue("cHp", typeof(int[]));
        cMp = (int[])info.GetValue("cMp", typeof(int[]));
        inventory = (int[])info.GetValue("inventory", typeof(int[]));
        inventoryStackSize = (int[])info.GetValue("inventoryStackSize", typeof(int[]));
    }

    //Serialization function.
    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        info.AddValue("unitID", (unitID));
        info.AddValue("cHp", (cHp));
        info.AddValue("cMp", (cMp));
        info.AddValue("inventory", (inventory));
        info.AddValue("inventoryStackSize", (inventoryStackSize));
    }

    public void Save()
    {
        SaveData data = new SaveData();
        Stream stream = File.Open("Assets\\Data\\player.dat", FileMode.Create);
        BinaryFormatter bformatter = new BinaryFormatter();
        bformatter.Binder = new VersionDeserializationBinder();
        Debug.Log("Writing Information");
        bformatter.Serialize(stream, data);
        stream.Close();

    }


    public void Load()
    {
        SaveData data = new SaveData();
        Stream stream = File.Open("Assets\\Data\\player.dat", FileMode.Open);
        BinaryFormatter bformatter = new BinaryFormatter();
        bformatter.Binder = new VersionDeserializationBinder();
        Debug.Log("Reading Data");
        data = (SaveData)bformatter.Deserialize(stream);
        stream.Close();
    }

}

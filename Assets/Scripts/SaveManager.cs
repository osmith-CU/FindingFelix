using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;



public class SaveManager{
    //Singleton
    private PlayerController playerController;
    private saveEntry saveValues = new saveEntry();
    public SaveManager(string stageName, string fileName, PlayerController playerController){
        this.saveValues.stageName = stageName;
        this.saveValues.fileName = fileName;
        this.playerController = playerController;
    }

    public void updateStage(int newNumber){
        this.saveValues.stageName = "Level" + newNumber;
    }

    //base structure for save and load functions from https://www.youtube.com/watch?v=6vl1IYMpwVQ
    public void save(){
        XmlSerializer serializer = new XmlSerializer(typeof(saveEntry));
        FileStream stream = new FileStream(Application.dataPath + "/SaveFiles/data" + saveValues.fileName + ".xml",  FileMode.Create);
        serializer.Serialize(stream, saveValues);
        stream.Close();
    }

    public void load(){
        XmlSerializer serializer = new XmlSerializer(typeof(saveEntry));
        FileStream stream = new FileStream(Application.dataPath + "/SaveFiles/data" + saveValues.fileName + ".xml",  FileMode.Open);
        saveValues = serializer.Deserialize(stream) as saveEntry;
        stream.Close();
        Debug.Log(saveValues.stageName);
        //this.playerController.loadFromSave(saveValues.stageName);
    }
}

public class saveEntry{
    public string stageName;      //tells which stage to load
    public string fileName;       //tells which save file its stored in
}
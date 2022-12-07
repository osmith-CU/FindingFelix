using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

/*-----MEMENTO IMPLEMENTATION-----*/

public class SaveManager{                   //Saving stuff (memento)
    //Singleton
    private saveEntry saveValues = new saveEntry();
    public SaveManager(string fileName){
        this.saveValues.fileName = fileName;
    }

    public void updateStage(string newStage){           //updates the stage name
        this.saveValues.stageName = newStage;
    }

    //base structure for save and load functions from https://www.youtube.com/watch?v=6vl1IYMpwVQ
    public void save(){                 //Saves to xml files

        bool directory = System.IO.Directory.Exists(Application.persistentDataPath + "/SaveFiles");                                 // creates boolean for directory               
        if(!directory){                                                                                                             // if directory has not been created, create it
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/SaveFiles");
        }
        XmlSerializer serializer = new XmlSerializer(typeof(saveEntry));                                                            // create and XML Serializer for save writes
        FileStream stream = new FileStream(Application.persistentDataPath + "/SaveFiles/data" + saveValues.fileName + ".xml",  FileMode.Create);
        serializer.Serialize(stream, saveValues);
        stream.Close();
    }

    public void load(){         
        //Loads from XML file
        XmlSerializer serializer = new XmlSerializer(typeof(saveEntry));
        FileStream stream = new FileStream(Application.persistentDataPath + "/SaveFiles/data" + saveValues.fileName + ".xml",  FileMode.Open);
        saveValues = serializer.Deserialize(stream) as saveEntry;
        stream.Close();
        //loads scene stores in scene manager
        SceneManager.LoadScene(saveValues.stageName);
    }
}

public class saveEntry{           //datastructure for our saves
    public string stageName;      //tells which stage to load
    public string fileName;       //tells which save file its stored in
}
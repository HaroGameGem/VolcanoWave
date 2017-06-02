using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager {
    static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
                instance = new DataManager();
            return instance;
        }
    }

    DataManager()
    {
        
    }

    int level = 1;
    public int Level
    {
        get { return level; }
    }
    public int maxLevel = 1;

    public void ResetData()
    {
        level = 1;
        maxLevel = 1;
    }

    public void AddLevel()
    {
        level++;
    }

    public void ResetLevel()
    {
        level = 1;
    }

}

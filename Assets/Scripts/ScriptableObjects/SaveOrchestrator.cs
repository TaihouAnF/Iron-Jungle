//TODO: holds the active loaded state
//https://github.com/UnityTechnologies/UniteNow20-Persistent-Data/blob/main/FileManager.cs

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[CreateAssetMenu(fileName = "SaveOrchestrator")]
public class SaveOrchestrator : ScriptableObject
{
    public delegate void SaveDataEvent(ref SaveData saveData);
    public SaveData saveData { get { return this._saveData; } }
    public SaveDataEvent onSave;
    public SaveDataEvent onLoad;
    public Action onReset;

    [ReadOnly][SerializeField] SaveData _saveData;

    void OnEnable()
    {
        //TODO: check if this will run
        Load();
    }


    [ButtonMethod]
    public void Save()
    {
        onSave?.Invoke(ref _saveData);
        Debug.Log("Save");
        //TODO: write to file
    }

    [ButtonMethod]
    public void Load()
    {
        Debug.Log("Load");
        //TODO: load file
        //set loadedData
        onLoad?.Invoke(ref _saveData);
    }

    [ButtonMethod]
    public void Reset()
    {
        Debug.Log("Reset");
        onReset?.Invoke();
    }
}

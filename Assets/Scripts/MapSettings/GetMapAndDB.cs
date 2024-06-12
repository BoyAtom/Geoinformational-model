using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetMapAndDB : MonoBehaviour
{
    
    string DBPath;
    DirectoryInfo DataBaseDir;
    string ImagesPath;
    DirectoryInfo ImagesDir;

    [SerializeField]
    TMP_Dropdown Images;
    [SerializeField]
    TMP_Dropdown DataBases;

    List<FileInfo> img = new List<FileInfo>();
    List<FileInfo> db = new List<FileInfo>();

    void Start()
    {
        InitDirs();

        DataBaseDir = new DirectoryInfo(DBPath);
        ImagesDir = new DirectoryInfo(ImagesPath);

        foreach (FileInfo fileInfo in ImagesDir.GetFiles()){
            img.Add(fileInfo);
        }
        SetSettings(Images, img);

        foreach (FileInfo fileInfo in DataBaseDir.GetFiles()){
            db.Add(fileInfo);
        }
        SetSettings(DataBases, db);
    }

    private void InitDirs() {
        if(Application.platform == RuntimePlatform.Android) {
            if (!Directory.Exists(Application.persistentDataPath + "/DataBases")) {
                Directory.CreateDirectory(Application.persistentDataPath + "/DataBases");
            }
            DBPath = Application.persistentDataPath + "/DataBases";
            if (!Directory.Exists(Application.persistentDataPath + "/Images")) {
                Directory.CreateDirectory(Application.persistentDataPath + "/Images");
            }
            ImagesPath = Application.persistentDataPath + "/Images";
        }
        else {
            DBPath = Application.dataPath + "/StreamingAssets/DataBases";
            ImagesPath = Application.dataPath + "/StreamingAssets/Images";
        }
    }

    private void SetSettings(TMP_Dropdown dropdown, List<FileInfo> fileInfos) {
        List<string> names = new List<string>();
        foreach (FileInfo file in fileInfos) {
            print(file.Name);
            names.Add(file.Name);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(names);
    }

    public void OnImageChange() {
        PlayerPrefs.SetString("ImageDIR", GetName(Images, img));
    }

    public void OnDBChange() {
        PlayerPrefs.SetString("DataBaseDIR", GetName(DataBases, db));
    }

    public string GetName(TMP_Dropdown dropdown, List<FileInfo> fileInfos) {
        int selected = dropdown.value;
        string dir = fileInfos[selected].Name;

        print(dir);
        return dir;
    }

    public void OpenDir() {
        if(Application.platform == RuntimePlatform.Android) {
            Application.OpenURL(Application.persistentDataPath);
        }
        else {
            Application.OpenURL(Application.dataPath + "/StreamingAssets");
        }
    }

    public void Return() {
        PlayerPrefs.SetString("ImageDIR", GetName(Images, img));
        PlayerPrefs.SetString("DataBaseDIR", GetName(DataBases, db));

        SceneManager.LoadSceneAsync("OptionsScene");
    }
}

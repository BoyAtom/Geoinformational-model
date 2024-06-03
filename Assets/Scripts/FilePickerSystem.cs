using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilePickerSystem : MonoBehaviour
{

    public string FinalPath;

    public void LoadFile() {
        string FileType = NativeFilePicker.ConvertExtensionToFileType("*");

        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) => {
            if (path == null) Debug.Log("Operation cancelled");
            else {
                FinalPath = path;
                Debug.Log("Picked file: " + FinalPath);
            }
        }, new string[] { FileType });
    }
}

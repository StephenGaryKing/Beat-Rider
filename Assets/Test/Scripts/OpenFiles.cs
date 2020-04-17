using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class OpenFiles : MonoBehaviour {

    string path = null;
    string fileName = null;
    private SongSelectionLoader loader = null;
    string documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic);

    private void Start()
    {
        loader = FindObjectOfType<SongSelectionLoader>();
    }

    public void OpenFolder()
    {
        path = EditorUtility.OpenFilePanel("Songs", documentPath, "");
        fileName = Path.GetFileName(path);
        if (path != null)
        {
            CopyFile();
        }
    }

    void CopyFile()
    {
        FileUtil.CopyFileOrDirectory(path, "Assets/Songs/" + fileName);
        if (loader)
            loader.RefreshFilesFromDefault();
    }
}

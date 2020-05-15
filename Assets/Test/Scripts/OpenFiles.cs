using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System.IO;
using System;
using Microsoft.Win32;
using System.Runtime;
using System.Windows.Forms;
using UnityEngine.Events;
using UnityEngine.UI;

public delegate void ClickEventHandler(object sender, EventArgs e);

public class OpenFiles : MonoBehaviour {

    string fileName = null;
    private SongSelectionLoader loader = null;
    string path = null;
    string documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic);
    string musicFolder = null;

    //Button fileLocatorButton = null;

    //public event ClickEventHandler Clicked;

    protected virtual void OnClicked(object sender, EventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.InitialDirectory = documentPath;
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            path = openFileDialog.FileName;
        }
    }

    private void Awake()
    {
        musicFolder = UnityEngine.Application.dataPath + "/StreamingAssets/Songs/";
        if (!Directory.Exists(musicFolder))
            Directory.CreateDirectory(musicFolder);
        else
            Debug.Log("music folder exist");
    }

    private void Start()
    {

        loader = FindObjectOfType<SongSelectionLoader>();
        string targetPath = UnityEngine.Application.dataPath + "/StreamingAssets/Songs/";
    }

    public void OpenFolder()
    {
        ////SelectDataSourceClick();
        //return;

        //using (OpenFileDialog openFileDialog = new OpenFileDialog())
        //{
        //    openFileDialog.InitialDirectory = "c:\\";
        //    openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        //    openFileDialog.FilterIndex = 2;
        //    openFileDialog.RestoreDirectory = true;

        //    openFileDialog.InitialDirectory = documentPath;
        //    if (openFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        path = openFileDialog.FileName;
        //    }
        //}
        // path = UnityEditor.EditorUtility.OpenFilePanel("Songs", documentPath, "");
        //UnityEngine.Utility
        //Microsoft.Win32.
        ////File.
        ////Application.strea

        OnClicked(null, EventArgs.Empty);

        if (path != null)
        {
            fileName = Path.GetFileName(path);
            CopyFile();
        }
        else
            Debug.Log("Path is null");
    }

    

    //private void SelectDataSourceClick()
    //{
    //    OpenFileDialog ofd = new OpenFileDialog();
    //    ofd.Filter = "Microsoft Access Databases |*.*|Excel Workbooks|*.xls";
    //    ofd.Title = "Select the data source";
    //    ofd.InitialDirectory = documentPath;

    //    if (ofd.ShowDialog() == DialogResult.OK)
    //    {
    //        var FilePath = ofd.FileName.ToString();
    //        var ext = Path.GetExtension(FilePath).ToLower();
    //        Debug.Log(FilePath);
    //    }
    //    else
    //    {
    //        System.Windows.Forms.Application.Exit();
    //    }
    //}

    void CopyFile()
    {
        //UnityEditor.FileUtil.CopyFileOrDirectory(path, "Assets/Songs/" + fileName);
        string destFile = System.IO.Path.Combine(musicFolder, fileName);

        System.IO.File.Copy(path, destFile, true);

        //Debug.Log("Application path: " + UnityEngine.Application.dataPath);

        if (loader)
            loader.RefreshFilesFromDefault();
    }
}

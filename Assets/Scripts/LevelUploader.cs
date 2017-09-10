using System;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System.Collections;
using System.Net;
using System.Text;

// a very simplistic level upload and random name generator script

public class LevelUploader : MonoBehaviour
{
    public Text debug;
    bool isUploaded = false;

    public void UploadLevel()
    {
        var username = "";
        var password = "";
        var url = "/"; // These fields will be filled with custom domain names etc

        var date = String.Format("{0:M-d-yyyy}", DateTime.Now);

        string filepath = Application.persistentDataPath + "/Icons/"; // Change this to where files are being saved too -- important!
        var d = new DirectoryInfo(filepath);
        debug.text = d.ToString();

        WebRequest ftpRequest = WebRequest.Create(url + date);
        ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
        ftpRequest.Credentials = new NetworkCredential(username, password);
        WebResponse response = ftpRequest.GetResponse();

        foreach (var file in d.GetFiles("*.txt"))
        {

            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(username, password);
                debug.text = (file.FullName);
                client.UploadFile(url + date + "//" + file.Name, WebRequestMethods.Ftp.UploadFile, file.FullName);
                isUploaded = true;
            }
        }

        if (isUploaded == true)
        {
            var iconFiles = Directory.GetFiles(filepath);
            for (int i = 0; i < iconFiles.Length; i++)
            {
                File.Delete(iconFiles[i]);
            }
            Directory.Delete(filepath);
            isUploaded = false;
        }
    }
}
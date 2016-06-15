using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class SaveLoad : MonoBehaviour
{

    public void writeStringToFile(string str, string filename)
    {
        string path = pathForDocumentsFile(filename);
        FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
        
        StreamWriter sw = new StreamWriter(file);
        sw.WriteLine(str);

        sw.Close();
        file.Close();
    }

    public string readStringFromFile(string filename)//, int lineIndex )
    {
        string path = pathForDocumentsFile(filename);

        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            string str = null;
            str = sr.ReadLine();

            sr.Close();
            file.Close();

            return str;
        }
        else
        {
            return null;
        }
    }


    public string pathForDocumentsFile(string filename)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var path = Application.dataPath.Substring(0, Application.dataPath.Length - 5) + filename;
            return path;
            /*
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return path + "/Documents/" + filename;
            */
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }

    public void writeImageToFile(Texture2D imageToSave)
    {
        imageToSave.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        imageToSave.Apply();
        byte[] bytes = imageToSave.EncodeToPNG();
        Destroy(imageToSave);
        string result = System.Text.Encoding.UTF8.GetString(bytes);

        string IMAGE = "Image" + DateTime.Now.ToString();
        writeStringToFile(result, IMAGE);
    }
}

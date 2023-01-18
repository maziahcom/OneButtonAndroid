using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadBytesFromFile : MonoBehaviour
{
    public static byte[] ReadBytes(string filename)
    {
        try
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                return bytes;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Exception caught in process: " + ex.Message);
            return null;
        }        
    }
}

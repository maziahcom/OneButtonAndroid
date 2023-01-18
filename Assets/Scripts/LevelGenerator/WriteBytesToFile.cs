using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//
//File format ideas:
//----1. B BBBB B BBBB B BBBB B
//---- The first byte marks the star tof the file data. ASCII 's' for example;
//---- The next 4 bytesTemp are the delta-time from the start of the game until the first keydown_code event.
//---- The next single byte represents a keydown_code event. ASCII d for example.
//---- The next 4 bytesTemp are another float, the deltatime since last key event.
//---- The final byte is an end of file byte 'e' 



public class WriteBytesToFile : MonoBehaviour
{
    public static FileStream CreateFile(string fileName)
    {
        try
        {
            return new FileStream(fileName, FileMode.Create, FileAccess.Write);
        }
        catch (Exception ex)
        {
            Debug.Log("Exception caught in process: " + ex.Message);
            return null;
        }
    }
    public static void FlushAndCloseFile(FileStream fs)
    {
        fs.Flush();
        fs.Close();
    }
    public static bool ByteArrayToFile(FileStream fs, byte[] byteArray)
    {
        try
        {
            fs.Write(byteArray, 0, byteArray.Length);
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log("Exception caught in process: " + ex.Message);
            return false;
        }
    }
}

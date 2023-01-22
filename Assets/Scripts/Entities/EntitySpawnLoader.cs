using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EntitySpawnLoader : MonoBehaviour
{
    public string fileamePublic = "Assets/Resources/Levels/LEVEL1X.bytes";
    public float horizontalSpawnRate = 0.5f;
    private float lastDropped;
    private float deltas;
    private string filename = "";
    private int lineLength;
    private List<string> strings;
    int columnCount = -1;
    public void Init(string filename, int maxLines)
    {
        this.filename = filename;
        lastDropped = 0;
        deltas = 0;
        strings = new List<string>();

        //read the length of the first line (minus whitespace)
//        foreach (string line in System.IO.File.ReadLines(filename))
//        {
//            lineLength = line.Substring(0, line.Length - 1).Trim().Length;
//            break;
//        }
        strings = ReadLines(maxLines);
        for(int i = 0; i < strings.Count; i++)
        {
            Debug.Log(strings[i]);
        }
        lineLength = strings[0].Length;
        columnCount = lineLength - 1;
        Debug.Log("LineLength: " + lineLength.ToString());
        Debug.Log("columnCount: " + columnCount.ToString());
    }

    public List<string> ReadLines(int maxLines)
    {
        List<string> lines = new List<string>();

        if (filename == "" || maxLines < 1)
            return null;

        foreach (string line in System.IO.File.ReadLines(filename))
        {
            lines.Add(line);
            if (lines.Count == maxLines)
                break;
        }
        return lines;
    }

    public byte SpawnNext()
    {
        byte b = 0x0;

        if (horizontalSpawnRate - deltas <= 0)
        {
            if (columnCount >= 0 && columnCount < lineLength)
            {
                if (strings[0][columnCount] == '1')
                {
                    b += 0x1;
                    deltas = 0;
                }
                if (strings[1][columnCount] == '1')
                {
                    b += 0x2;
                    deltas = 0;
                }
                if (strings[2][columnCount] == '1')
                {
                    b += 0x4;
                    deltas = 0;
                }
                if (strings[3][columnCount] == '1')
                {
                    b += 0x8;
                    deltas = 0;
                }              
            }
            columnCount--;
        }
        else
        {
            deltas += Time.deltaTime;
        }
        return b;
    }
}
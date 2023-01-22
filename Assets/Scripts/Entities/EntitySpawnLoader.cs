using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EntitySpawnLoader : MonoBehaviour
{
    public string fileamePublic = "Assets/Resources/Levels/LEVEL1X.bytes";
    public float horizontalSpawnRate = 0.5f;
    private float deltas;
    private string filename = "";
    private int lineLength;
    private List<string> strings;
    int columnCount = -1;
    public void Init(string filename, int maxLines)
    {
        this.filename = filename;
        deltas = 0;
        strings = new List<string>();

        strings = ReadLines(maxLines);
        lineLength = strings[0].Length;
        columnCount = lineLength - 1;
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
            deltas = 0;

            if (columnCount >= 0 && columnCount < lineLength)
            {              
                if (strings[0][columnCount] == '1')
                {
                    b += 0x1;                  
                }
                if (strings[1][columnCount] == '1')
                {
                    b += 0x2;
                }
                if (strings[2][columnCount] == '1')
                {
                    b += 0x4;
                }
                if (strings[3][columnCount] == '1')
                {
                    b += 0x8;
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
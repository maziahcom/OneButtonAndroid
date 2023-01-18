using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.XR;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


//
//File format guide:
//----1. B BBBB B BBBB B BBBB B
//---- The first 1 byte represents a key DOWN event.
//---- The next 4 bytesTemp are a float, representing the delta-time (seconds) from the frame of the previous event
//---- until the frame of the current event (or zero if its the first event);
//---- The next single byte represents a key UP event.
//---- The next 4 bytesTemp are a float, representing the delta-time from the previous event
//---- This sequence of key down, delta, key up, delta will continue for as long as the user is recording the key.
//---- The user can press ESC to place a final byte and end the recording
//---- The final byte is an end of file byte; 
// Bitconverter.GetBytes
//byte[] byteArray = BitConverter.GetBytes(794.328247);
//Note that this produce an array of bytesTemp since a float is 32bit, so it needs 4 bytesTemp to store it. Do the reverse with ToSingle and a ReadOnlySpan.

public class LevelReader : MonoBehaviour
{
    public string filename = "Assets/Resources/Levels/LEVEL1.bytes";
    bool active;
    public Reader reader;
  
    //
    // The recorder class is required for *making* levels in the game.
    // The reader class is needed for reading the data at runtime
    // Both reader and recorder inhert from KeyEvents
    //
    public class KeyEvents
    {
        public const int max_file_size = 500000;
        public byte keyup_code;
        public byte keydown_code;
        public byte file_end_code;
        public int counter;
        public enum NextEvent
        {
            keydown,
            keyup
        }
        public KeyEvents()
        {
            counter = 0;
            keyup_code = 0x01;
            keydown_code = 0x02;
            file_end_code = 0x04;
        }
    }


    public bool LoadLevel1()
    {
        reader = new Reader();
        byte[] newBytes = ReadBytesFromFile.ReadBytes(filename);
        if (newBytes != null)
        {
            string message = CheckBytesIntegrity(newBytes) ? "Integrity: PASS" : "Integrity: FAIL";
            if(message == "Integrity: FAIL")
                return false;
            Debug.Log(message);
            Debug.Log("Bytes read: " + BitConverter.ToString(newBytes));
            return Deserialize(newBytes);
        }
        else
            Debug.Log("No bytes read");
        return false;
    }
        



    // Update is called once per frame
    void UpdateInputReader()
    {
        //on the press of R, read the file back, run integrity checks and deserialise
        if(Input.GetKeyDown(KeyCode.R))
        {

        }

    }

    public class Reader : KeyEvents
    {
        public Structure structure;

        public class Structure
        {
            private List<NextEvent> nextEvents;
            private List<float> deltas;
            private int count;
            

            public Structure()
            {
                nextEvents = new List<NextEvent>();
                deltas = new List<float>();
                count = 0;
            }
            public void SetCount(int c)
            {
                count = c;
            }
            public void AddEvent(NextEvent n)
            {
                nextEvents.Add(n);
            }
            public void AddDelta(float d)
            {
                deltas.Add(d);
            }
            public void PrintResults()
            {
                Debug.Log("Count = " + count.ToString());
                foreach(NextEvent n in nextEvents)
                    Debug.Log(n.ToString());
                foreach (float f in deltas)
                    Debug.Log(f.ToString());
            }

            public List<float> GetTimestamps()
            {
                return deltas;
            }
            public List<NextEvent> GetNextEvents()
            {
                return nextEvents;
            }
        }

        public Reader()
        {
            counter = 0;
        }

        public bool Deserialize(byte[] bytes)
        {
            structure = new Structure();
            
            int length = bytes.Length;
            counter = 0;

            //loop 5 bytes at a time, flipping nextevent between keyup and keydown
            bool b = false;
            for (int i = 1; i < length; i+=5)
            {
                //alternate between keyup and keydown
                if(!b)
                    structure.AddEvent(NextEvent.keydown);
                else
                    structure.AddEvent(NextEvent.keyup);
                b = !b;
                //take next 4 bytes and convert to float (our delta time)
                ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(bytes, i, 4);
                float f = BitConverter.ToSingle(s);
                structure.AddDelta(f); 

                //increment the counter so we know how many events there are
                counter++;
            }
            structure.SetCount(counter);
            structure.PrintResults();
            return true;
        }
    }

    bool CheckBytesIntegrity(byte[] bytes)
    {
        int length = bytes.Length;

        if (bytes == null)
            return false;
        if (bytes.Length == 0)
            return false;
                
        //last byte always end-file byte
        if (bytes[length-1] != reader.file_end_code)
            return false;

        //0th and every 10th byte onwards always key down (except when reading the final byte in the file)
        for(int i = 0; i < length; i+=10)
        {
            if (bytes[i] != reader.keydown_code)
                if (i != length - 1)
                    return false;
        }
        //5th and every 10th byte onwards always key up (except when reading the final byte in the file)        
        for (int i = 5; i < length; i += 10)
        {
            if (bytes[i] != reader.keyup_code)
                if (i != length - 1)
                    return false;
        }

        //no faults found
        return true;
    }

    bool Deserialize(byte[] bytes)
    {
        if(!CheckBytesIntegrity(bytes))
            return false;
        return reader.Deserialize(bytes);
    }
}

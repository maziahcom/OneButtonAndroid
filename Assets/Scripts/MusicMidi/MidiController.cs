using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SmfLite;

public class MidiController : MonoBehaviour
{
    // Source MIDI file asset.
    public TextAsset sourceFile;
    public float startTime = 5.0f;
    // Test settings.
    public float bpm;

    // MIDI objects.
    MidiFileContainer song;
    List<MidiTrackSequencer> sequencer;// = new List<MidiTrackSequencer>;

    private bool readyToStart = false;

    // Start function (MonoBehaviour).
    public void InitMidi()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {

        //sequencer = new List<MidiTrackSequencer>();
        // Load the MIDI song.
       // song = MidiFileLoader.Load(sourceFile.bytes);
        //Debug.Log("Midi file has " + song.tracks.Count.ToString() + " tracks");
        // Wait for one second to avoid stuttering.
        yield return new WaitForSeconds(1.0f);
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().time = startTime;
        // Start sequencing.

        // ResetAndPlay(0);
    }

    // Reset and start sequecing.
    void ResetAndPlay(float startTime)
    {
        sequencer.Clear();
        // Start the sequencer and dispatch events at the beginning of the track.
        for (int i = 0; i < song.tracks.Count; i++)
        {
            sequencer.Add(new MidiTrackSequencer(song.tracks[i], song.division, bpm));
        }
        Debug.Log("Sequencer count is: " + sequencer.Count.ToString());
        for (int i = 0; i < sequencer.Count; i++)
        {
            DispatchEvents(sequencer[i].Start(startTime), i);
        }
        // Play the audio clip.
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().time = startTime;
        readyToStart = true;
    }

    // Update function (MonoBehaviour).
    public void UpdateMidi()
    {
        if (!readyToStart)
            return;
        for (int i = 0; i < sequencer.Count; i++)
        {
            if (sequencer[i] != null && sequencer[i].Playing)
            {
                // Update the sequencer and dispatch incoming events.
                DispatchEvents(sequencer[i].Advance(Time.deltaTime), i);
            }
        }
    }

    // Dispatch incoming MIDI events. 
    void DispatchEvents(List<MidiEvent> events, int trackNumber)
    {
        if (events != null)
        {
            foreach (var e in events)
            {
                //log track instrument names 
                if ((e.status) == 0xFF)
                {
                    Debug.Log("Track " + trackNumber.ToString() + ": " + e.data3);
                }
                if ((e.status & 0xf0) >= 0x90 && (e.status & 0xf0) <= 0x9F)
                {
                    //log number of events per track
                    Debug.Log("Track: " + trackNumber.ToString() + " midi event");// + "   Note: " + e.data1.ToString());
                    /*if (e.data1 == 0x24)
                    {
                        GameObject.Find("Kick").SendMessage("OnNoteOn");
                    }
                    else if (e.data1 == 0x2a)
                    {
                        GameObject.Find("Hat").SendMessage("OnNoteOn");
                    }
                    else if (e.data1 == 0x2e)
                    {
                        GameObject.Find("OHat").SendMessage("OnNoteOn");
                    }
                    else if (e.data1 == 0x26 || e.data1 == 0x27 || e.data1 == 0x28)
                    {
                        GameObject.Find("Snare").SendMessage("OnNoteOn");
                    }*/
                }
            }
        }
    }

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SmfLite;

public class MidiController : MonoBehaviour
{
    // Source MIDI file asset.
    public TextAsset sourceFile;
    public float audioSkipToSeconds = 1.0f;
    public float audioStartDelay = 7.2f;
    public float midiStartDelay = 0.01f;
    public int[] enabledChannels = { 2 };
    private int bpm;
    private List<MidiJob> midiJobs;

    // MIDI objects.
    MidiFileContainer song;
    List<MidiTrackSequencer> sequencer;// = new List<MidiTrackSequencer>;

    private bool audioStarted = false;
    private bool midiStarted = false;
    private float awakeTime;
    // Start function (MonoBehaviour).
    public void InitMidi()
    {
        awakeTime = Time.unscaledTime;
        sequencer = new List<MidiTrackSequencer>();
        // Load the MIDI song.
        song = MidiFileLoader.Load(sourceFile.bytes);
        bpm = song.bpm;
        Debug.Log("Midi file has " + song.tracks.Count.ToString() + " tracks");
        midiJobs = new List<MidiJob>();
    }

    // Reset and start sequecing.
    void MidiResetAndPlay(float startTime)
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

    }
    void AudioResetAndPlay(float startTime)
    {
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().time = startTime;
    }

    // Update function (MonoBehaviour).
    public void UpdateMidi()
    {
        //Allow time for the first entities to move toward the player before starting the audio
        //the audio should sync to the visual moment that the first entity arrives at the player
        if(Time.unscaledTime - awakeTime >= audioStartDelay)
        {
            //The resetAndPlay should run only once
            if (!audioStarted)
            {
                audioStarted = true;
                AudioResetAndPlay(audioSkipToSeconds);
                Debug.Log("audio play now: " + Time.unscaledTime.ToString());
            }
        }
        if (Time.unscaledTime - awakeTime >= midiStartDelay)
        {
            //The resetAndPlay should run only once
            if (!midiStarted)
            {
                midiStarted = true;
                MidiResetAndPlay(audioSkipToSeconds);
                Debug.Log("midi play now: " + Time.unscaledTime.ToString());
            }
        }
        if (!midiStarted)
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
                //e.status 0x90-0x9F is NoteOn
                //the 9 is the NoteOn, the 0-F is the channel
                //e.data1 is the note number 0-127
                //e.data2 is note velocity. A zero velocity should be interpretted as NoteOff
                //Be mindful that tracks and channels are not the same thing.
                //Each track can have 16 channels
                //Each channel can have 127 notes
                //In this app we are combining the tracks into one track
                //and maintaining the channels. Example: track 1 channel 5 and track 4 channel 5 will become simply "channel 5".

                //how this fits with the bigger game:
                //the game manager will ask for a list of midi jobs every frame
                //this loop will add jobs to the list for every midi event that happens
                //when the game manager receives the list, the list will be cleared.
                //Its the game managers business to act on the list (i.e., dispatch entities per midi event)

                byte channel = (byte)(e.status & 0x0F);
                byte note = (byte)(e.data1 & 0x0F);

                if ((e.status & 0xf0) == 0x90)
                {

                    if (e.data2 != 0)
                    {
                        //we dont want to dispatch jobs down the line for every midi channel.
                        //therefore only dispatch jobs if the channel number is on our list
                        for (int i = 0; i < enabledChannels.Length; i++)
                        {
                            if(channel == enabledChannels[i])
                                midiJobs.Add(new MidiJob(true, channel, note));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < enabledChannels.Length; i++)
                        {
                            if (channel == enabledChannels[i])
                                midiJobs.Add(new MidiJob(false, channel, note));
                        }
                    }
                }
                if ((e.status & 0xf0) == 0x80)
                {
                    for (int i = 0; i < enabledChannels.Length; i++)
                    {
                        if (channel == enabledChannels[i])
                            midiJobs.Add(new MidiJob(false, channel, note));
                    }
                }
            }
        }
    }

    public List<MidiJob> GetAndClearMidiJobs()
    {

        //we cant return the midiJobs list because it has to be cleared first.
        //copy to tmp list, then clear midiJobs, then return the tmp list

        List<MidiJob> whatever = new List<MidiJob>();
        whatever.AddRange(midiJobs);
        midiJobs.Clear();
        return whatever;
    }

    public class MidiJob
    {
        public bool on;
        public byte channel;
        public byte note;

        public MidiJob(bool on, byte channel, byte note)
        {
            this.on = on;
            this.channel = channel;
            this.note = note;
        }
    }
}
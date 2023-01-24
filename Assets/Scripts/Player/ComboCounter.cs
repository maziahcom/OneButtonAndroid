using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    //combo counter will take information from entity controller (deadcount) and player comtroller (swingState)
    //it will analyse this info to count hits and misses and update the combo meter

    public GameObject comboMeter;
    public string _meterRef = "_Meter";
    public string _colorRef = "_Color";
    public string _intensityRef = "_Intensity";
    public AudioClip[] levelupClips;

    private bool was_ascending;
    private bool was_descending;
    private bool was_static;
    private int has_hit_during_action;
    private int combo_count;
    private const int meter_max = 10;
    private bool doFlashNow;
    private const float flashTime = 2.2f;
    private float flashTimeRemaining;
    private float flashCycleSpeed;
    private float intensity;
    private const float max_intensity = 50;
    private AudioSource audioSourceFX;
    private List<AudioClip> comboFX = new List<AudioClip>();
    private int fullPowerupsCounted;

    public void Init()
    {
        was_ascending = false;
        was_descending = false;
        was_static = false;
        has_hit_during_action = 0;
        combo_count = 0;
        doFlashNow = false;
        flashTimeRemaining = 0;
        flashCycleSpeed = 440;
        intensity = 0;
        fullPowerupsCounted = 0;
        InitSoundFX();
    }

    private void InitSoundFX()
    {
        audioSourceFX = gameObject.AddComponent<AudioSource>();
        audioSourceFX.loop = false;
        audioSourceFX.bypassEffects = true;
        audioSourceFX.bypassListenerEffects = true;
        audioSourceFX.bypassReverbZones = true;
        audioSourceFX.playOnAwake = false;
        audioSourceFX.priority = 5;
        audioSourceFX.volume = 1.0f;
        audioSourceFX.pitch = 1.0f;

        for (int i = 0; i < levelupClips.Length; i++)
        {
            string displayString = "zapsplat_multimedia_notification_bell_chime_ring_alert_" + i.ToString("000");
            comboFX.Add(levelupClips[i]);
        }
        //comboFX.Add((AudioClip)Resources.Load("SoundFX/ComboMeter/" + "zapsplat_fm_synth_047"));
        //Debug.Log(comboFX.Count);
    }

    public void _Update(int deadCount, bool _ascending, bool _descending, bool _static, out int powerupLevel)
    {
        FlashOnFull();

        powerupLevel = fullPowerupsCounted;

        if (deadCount > 0)
            has_hit_during_action += deadCount;

        if (was_ascending)
        {
            if (!_ascending)
            {
                if (has_hit_during_action > 0)
                    combo_count+= has_hit_during_action;
                else combo_count = 0;
            }
        }
        else if (was_descending)
        {
            if (!_descending)
            {
                if (has_hit_during_action > 0)
                    combo_count+= has_hit_during_action;
                else combo_count = 0;
            }
        }
        else if (was_static)
        {
            if (!_static)
            {
                has_hit_during_action = 0;
            }

        }

        was_ascending = _ascending;
        was_descending= _descending;
        was_static= _static;

        comboMeter.GetComponent<MeshRenderer>().material.SetFloat(_meterRef, combo_count);

        //sanity check (these should never occur)
        /*if (_ascending && _descending)
            Debug.Log("PROBLEM 1823");
        if (_ascending && _static)
            Debug.Log("PROBLEM 1824");
        if (_static && _descending)
            Debug.Log("PROBLEM 1825"); */

    }

    private void FlashOnFull()
    {
        if(combo_count >= meter_max)
        {
            if(!doFlashNow)
            {
                doFlashNow = true;
                flashTimeRemaining = flashTime;
                audioSourceFX.clip = comboFX[Random.Range(0, levelupClips.Length-1)];
                audioSourceFX.Play();
                fullPowerupsCounted += 1;
                if(fullPowerupsCounted >= comboFX.Count)
                    fullPowerupsCounted = comboFX.Count - 1;
                Time.timeScale = 0.25f;
            }
            
        }
        if(doFlashNow)
        {
            intensity += flashCycleSpeed * Time.deltaTime;
            if(intensity > max_intensity)
            {
                intensity = max_intensity;
                flashCycleSpeed *= -1;
            }
            else if (intensity < -max_intensity)
            {
                intensity = -max_intensity;
                flashCycleSpeed *= -1;
            }
            comboMeter.GetComponent<MeshRenderer>().material.SetFloat(_intensityRef, intensity);
        }
        if(flashTimeRemaining > 0)
        {
            flashTimeRemaining -= Time.deltaTime;
            if (flashTimeRemaining < 0)
            {
                flashTimeRemaining = 0;
                doFlashNow = false;
                intensity = 0;
                comboMeter.GetComponent<MeshRenderer>().material.SetFloat(_intensityRef, intensity);
                combo_count -= meter_max;
                Time.timeScale = 1;
            }
        }
    }
}

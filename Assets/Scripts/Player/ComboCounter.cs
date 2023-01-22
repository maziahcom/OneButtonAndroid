using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    //combo counter will take information from entity controller (deadcount) and player comtroller (swingState)
    //it will analyse this info to count hits and misses and update the combo meter

    public GameObject comboMeter;
    public string _floatRef = "_Meter";

    private bool was_ascending;
    private bool was_descending;
    private bool was_static;
    private int has_hit_during_action;
    private int combo_count;
    public void Init()
    {
        was_ascending = false;
        was_descending = false;
        was_static = false;
        has_hit_during_action = 0;
        combo_count = 0;
    }


    public void _Update(int deadCount, bool _ascending, bool _descending, bool _static)
    {
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

        comboMeter.GetComponent<MeshRenderer>().material.SetFloat(_floatRef, combo_count);

        //sanity check
        if (_ascending && _descending)
            Debug.Log("PROBLEM 1823");
        if (_ascending && _static)
            Debug.Log("PROBLEM 1824");
        if (_static && _descending)
            Debug.Log("PROBLEM 1825");
    }
}

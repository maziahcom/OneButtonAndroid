using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using System;
using Random = System.Random;
using static TMPro.TMP_Compatibility;

namespace TMPro.Examples
{

    public class TMP_ExampleScript_01 : MonoBehaviour
    {
        public bool isStatic;

        private TMP_Text m_text;

        private Random random = new Random();

        //private TMP_InputField m_inputfield;


        private const string k_label = "Starting in <#0080ff>{00.0}</color>";
        private float count;



        void Update()
        {
            if (!isStatic)
            {
                m_text.SetText(k_label, count);
            }
            count -= 1.0f * Time.deltaTime;
            if (count <= 0.0f)
            {
                count = 0.0f;
            }
        }

    }
}

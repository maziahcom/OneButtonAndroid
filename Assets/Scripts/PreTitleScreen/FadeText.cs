using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine;
//using System.Collections;


namespace TMPro.Examples
{

    public class FadeText : MonoBehaviour
    {
        public float fadeSpeed = 1.0f;
        public float delaySeconds = 2.0f;
        private TMP_Text m_TextComponent;


        void Awake()
        {
            m_TextComponent = gameObject.GetComponent<TMP_Text>();
        }


        void Start()
        {
            StartCoroutine(WarpText());
        }

        IEnumerator WarpText()
        {
            //apply start delay
            yield return new WaitForSeconds(2);

            while (m_TextComponent.color.a > 0)
            { 
                //wait for one frame
                yield return null;

                //decrement the alpha
                m_TextComponent.color = new Color(m_TextComponent.color.r, m_TextComponent.color.g, m_TextComponent.color.b, m_TextComponent.color.a - (fadeSpeed * Time.deltaTime));
            }
        }
    }
}


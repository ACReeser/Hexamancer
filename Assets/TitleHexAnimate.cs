using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleHexAnimate : MonoBehaviour {
    public RectTransform[] titleCharacters;

    private const float duration = .5f;
    private float time = 0f;
    private const float holdDuration = 10f;
    public bool pulsing = false;
    
	
	void OnEnable() {
        foreach(var t in titleCharacters)
            t.localScale = Vector3.zero;

        pulsing = true;
        StartCoroutine(Pulse());
	}

    private IEnumerator Pulse()
    {
        float time = 0f;
        while (time < 2f)
        {
            for (int i = 0; i < titleCharacters.Length; i++)
            {
                float delay = i * .1f;
                if (i > 2)
                    delay += .2f;
                if (i > 3)
                    delay += .2f;

                titleCharacters[i].localScale = Vector3.Lerp(Vector3.zero, Vector3.one, (time - delay) / .5f);
            }

            yield return null;
            time += Time.deltaTime;
        }
        yield return new WaitForSeconds(10f);
        time = 0f;
        while (time < 2f)
        {
            for (int i = 0; i < titleCharacters.Length; i++)
            {
                float delay = i * .1f;
                if (i > 2)
                    delay += .2f;
                if (i > 3)
                    delay += .2f;

                titleCharacters[i].localScale = Vector3.Lerp(Vector3.one, Vector3.zero, (time - delay) / .5f);
            }

            yield return null;
            time += Time.deltaTime;
        }
        pulsing = false;
    }

    // Update is called once per frame
    void Update () {
        if (!pulsing)
        {
            pulsing = true;
            StartCoroutine(Pulse());
        }
	}
}

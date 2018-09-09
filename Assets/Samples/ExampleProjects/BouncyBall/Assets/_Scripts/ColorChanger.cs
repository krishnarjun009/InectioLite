using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    Color[] colors = new Color[6];

    [SerializeField]
    private Image renderer;

    [SerializeField]
    private float timer = 0.0f;

    float t = 0f;
    float duration = 5f;

    void Start()
    {
        colors[0] = Color.cyan;
        colors[1] = Color.red;
        colors[2] = Color.green;
        colors[3] = new Color(255, 165, 0);
        colors[4] = Color.yellow;
        colors[5] = Color.magenta;

        renderer = GetComponent<Image>();
    }
    void Update()
    {

            // pick a random color
            Color newColor = colors[UnityEngine.Random.Range(0, colors.Length)];
            // apply it on current object's material
            renderer.color = Color.Lerp(renderer.color, newColor, Time.time);

        if (t < 1)
        { // while t below the end limit...
          // increment it at the desired rate every update:
            t += Time.deltaTime / duration;
        }



    }
}

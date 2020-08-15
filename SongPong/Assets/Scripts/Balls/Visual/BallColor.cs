using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorType {None, Main, Light, Dark};

public class BallColor : MonoBehaviour
{
    public Color color;
    private Color lighterColor;
    private Color darkColor;

    private float intensity = 5.0f;

    private ParticleEffect[] effects;

    // Start is called before the first frame update
    void Start()
    {
        if(color.a != 1.0f)
        {
            Debug.LogError("This ParticleSystem has a color with alpha not equal to 1. May appear funky!");
        }

        // Set Color Values
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        lighterColor = Color.HSVToRGB(H, (S - 0.5f), V);
        darkColor = Color.HSVToRGB(H, (S + 0.5f), (V - 0.2f));

        // Set all Particle Effect children
        effects = GetComponentsInChildren<ParticleEffect>();
        foreach(ParticleEffect effect in effects)
        {
            ParticleSystem system = effect.GetComponent<ParticleSystem>();
            var main = system.main;
            if(effect.colorType == ColorType.Main)
            {
                main.startColor = color;
            }
            else if(effect.colorType == ColorType.Light)
            {
                main.startColor = lighterColor;
            }
            else if(effect.colorType == ColorType.Dark)
            {
                main.startColor = darkColor;
            }
            system.Clear();
            system.Play();
        }
        
    }
}

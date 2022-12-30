using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

public enum CarLightType
{
    none,
    Headlight,
    Taillight,
    LeftBlinker,
    RightBlinker,
    BrakeLight,
    FogLight,
}
[System.Serializable]
public class LightClass
{
    public string name;
    
    public CarLightType type;
    public Material material;
    public MeshRenderer meshRenderer;
    public float Emission;
    public int matIndex;
    public bool isOn = false;
    [HideInInspector]
    public bool isDone = true;
    public KeyCode Key;

}
public class Lighting : MonoBehaviour
{
    public List<LightClass> lights = new List<LightClass>();

    public float LEDDelay = 0.05f;
    public float HalogenDelay = 0.1f;
    public float lightUpdateDelay = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        lights.ForEach(light =>
        {
            if (Input.GetKeyDown(light.Key) && light.isDone)
            {
                SendMessage(light.type.ToString(), light.isOn);
            }
        });
    }
    private void Init()
    {
        lights.ForEach(light =>
        {
            Material mat = new Material(light.material);
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", Color.black * light.Emission);
            //mat.SetColor("_EmissionColor", new Color(light.material.GetColor("_EmissionColor").r, light.material.GetColor("_EmissionColor").g, light.material.GetColor("_EmissionColor").b));
            light.meshRenderer.materials[light.matIndex] = mat;
            light.meshRenderer.materials[light.matIndex].SetColor("_EmissionColor", Color.black * light.Emission);
            light.isOn = false;
        });
    }
    public void Headlight(bool enable)
    {
        var light = lights.Where(x => x.type == CarLightType.Headlight).First();
        if (light != null)
        {
            StartCoroutine(LightingCoroutine(light, light.material.name.Contains("LED") ? LEDDelay : HalogenDelay));
        }
        else
        {
            Debug.LogError("Can't find the headlight");
            return;
        }
    }

    IEnumerator LightingCoroutine(LightClass light, float delay)
    {
        light.isDone = false;
        float t;

        if (!light.isOn) //켜질때 when the light turns on
        {
            t = 0;
            while(t <= delay)
            {
                light.meshRenderer.materials[light.matIndex].SetColor("_EmissionColor", new Color
                    (Mathf.Lerp(0, light.material.GetColor("_EmissionColor").r, t/delay ),
                    Mathf.Lerp(0, light.material.GetColor("_EmissionColor").g, t / delay),
                    Mathf.Lerp(0, light.material.GetColor("_EmissionColor").b, t / delay)) * light.Emission);

                yield return new WaitForSeconds(lightUpdateDelay);
                t += lightUpdateDelay;
            }
        }

        else //꺼질때 when the light turns off
        {
            t = 0;
            while (t <= delay)
            {
                light.meshRenderer.materials[light.matIndex].SetColor("_EmissionColor", new Color
                    (Mathf.Lerp(light.material.GetColor("_EmissionColor").r, 0, t / delay),
                    Mathf.Lerp(light.material.GetColor("_EmissionColor").g, 0, t / delay),
                    Mathf.Lerp(light.material.GetColor("_EmissionColor").b, 0, t / delay)) * light.Emission);

                yield return new WaitForSeconds(lightUpdateDelay);
                t += lightUpdateDelay;
            }
        }

        light.isOn = light.isOn?false:true;
        light.isDone = true;
    }
    
}

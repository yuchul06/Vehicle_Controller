using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int matIndex;
    public KeyCode Key;

}
public class Lighting : MonoBehaviour
{
    public List<LightClass> lights = new List<LightClass>();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

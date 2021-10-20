using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public WaterZone[] waterZones;
    static LevelManager _singleton;
    public static LevelManager singleton
    {
        get
        {
            return _singleton;
        }
    }


    private void Awake()
    {
        if (singleton)
        {
            throw new System.Exception("LevelManager already has singleton");
        }
        else
        {
            _singleton = this;
        }
        waterZones = FindObjectsOfType<WaterZone>();
    }
}

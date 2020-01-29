﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class LightmapsSwap : MonoBehaviour
{
    public bool toggle;
    public string nightpath = "";

    private LightmapData[] Daydata;
    private LightmapData[] Nightdata;
    private LightmapData[][] allLightmaps;
    private Texture[] DayReflections;
    private Texture[] NightReflections;
    private Texture[][] allReflections;
    private ReflectionProbe[] allReflectionProbes;

    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nightpath));
        Daydata = LightmapSettings.lightmaps;

        allLightmaps = new LightmapData[2][];
        allLightmaps[0] = Daydata;

        Nightdata = new LightmapData[Daydata.Length];

        for (int i = 0; i < Daydata.Length; i++)
        {
            Nightdata[i] = new LightmapData();
            Nightdata[i].lightmapColor = Resources.Load(nightpath + "/" + Daydata[i].lightmapColor.name) as Texture2D;
            Nightdata[i].lightmapDir = Resources.Load(nightpath + "/" + Daydata[i].lightmapDir.name) as Texture2D;
        }

        allLightmaps[1] = Nightdata;

        allReflectionProbes = FindObjectsOfType<ReflectionProbe>();

        DayReflections = new Texture[allReflectionProbes.Length];
        NightReflections = new Texture[allReflectionProbes.Length];

        for (int i = 0; i < allReflectionProbes.Length; i++)
        {
            DayReflections[i] = allReflectionProbes[i].bakedTexture;
            NightReflections[i] = Resources.Load(nightpath + "/" + DayReflections[i].name) as Texture;
            allReflectionProbes[i].mode = ReflectionProbeMode.Custom;
            allReflectionProbes[i].customBakedTexture = DayReflections[i];
        }

        allReflections = new Texture[2][];
        allReflections[0] = DayReflections;
        allReflections[1] = NightReflections;
    }

    public void SwapLightmaps(int option = 1)
    {

        LightmapSettings.lightmaps = allLightmaps[option];
        for (int i = 0; i < allReflectionProbes.Length; i++)
        {
            allReflectionProbes[i].customBakedTexture = allReflections[option][i];
        }
    }
}
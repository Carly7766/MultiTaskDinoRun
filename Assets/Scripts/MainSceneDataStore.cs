using System;
using UnityEngine;

[Serializable]
public class MainSceneDataStore : SingletonMonoBehaviour<MainSceneDataStore>
{
    public float highScore = 0;
    public float score = 0;
}
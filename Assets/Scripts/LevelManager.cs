using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : Singleton<LevelManager>
{
    
    public int calloriesValue;

    public int startEnergyValue;

    public float synthMultiplerValue;

    public int energyToDivideValue;

    GameObject newCell;

    public int mutations;
    public TextMesh mutationsText;
    public TextMesh fpsText;

    public List<Transform> StartPositions;
    public StartConfiguration Configuration;

    protected override void Init()
    {
        Application.runInBackground = true;

        if (Configuration)
            Configuration.Place();
        else
            Debug.LogError("Please setup cell configuration");
    }

    // Update is called once per frame
    void Update()
    {
        mutationsText.text = mutations.ToString();
        fpsText.text = (1 / Time.deltaTime).ToString();
    }
}

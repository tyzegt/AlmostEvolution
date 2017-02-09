using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : Singleton<LevelManager>
{

    public int Callories { get { return Configuration.Callories; } }

    public int StartEnergy { get { return Configuration.StartEnergy; } }

    public float SynthMultipler { get { return Configuration.SynthMultipler; } }

    public int EnergyToDivide { get { return Configuration.EnergyToDivide; } }

    public int mutations;
    public TextMesh mutationsText;
    public TextMesh fpsText;
    public TextMesh cellsCountText;
    public TextMesh corpseCountText;

    public List<Transform> StartPositions;
    public StartConfiguration Configuration;

    protected void Start()
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
        cellsCountText.text = Registry.Instance.GetCellsCount().ToString();
        corpseCountText.text = Registry.Instance.GetCorpsesCount().ToString();
    }
}

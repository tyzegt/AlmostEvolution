using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public int calloriesValue;
    public static int callories;
        
    public int startEnergyValue;
    public static int startEnergy;

    public float synthMultiplerValue;
    public static float synthMultipler;

    public int energyToDivideValue;
    public static int energyToDivide;

    GameObject newCell;

    public int mutations;
    public TextMesh mutationsText;
    public TextMesh fpsText;
    
    // Use this for initialization
    void Start () {
        callories = calloriesValue;
        startEnergy = startEnergyValue;
        synthMultipler = synthMultiplerValue;
        energyToDivide = energyToDivideValue;
        Application.runInBackground = true;
    }
	
	// Update is called once per frame
	void Update () {        
        mutationsText.text = mutations.ToString();
        fpsText.text = (1 / Time.deltaTime).ToString();
    }
   
}

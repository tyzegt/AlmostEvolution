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


    public GameObject cell;

    Bot[] cells;

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

        cells = FindObjectsOfType<Bot>();
        if (cells.Length == 0)
        {
            GameObject[] corpses = GameObject.FindGameObjectsWithTag("food");
            for (int i = 0; i < corpses.Length; i++)
            {
                Destroy(corpses[i]);
            }
            Instantiate(cell, new Vector2(48, 56), transform.rotation);
        }
        //Debug.Log(Time.deltaTime);

        mutationsText.text = mutations.ToString();
        fpsText.text = (1 / Time.deltaTime).ToString();
    }
   
}

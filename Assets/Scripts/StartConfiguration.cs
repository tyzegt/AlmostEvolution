using UnityEngine;
using System;
using System.Collections.Generic;

public class StartConfiguration : MonoBehaviour
{
    public int Callories = 200;

    public int StartEnergy = 50;

    public float SynthMultipler = 1;

    public int EnergyToDivide = 1000;

    [Serializable]
    public struct Slot
    {
        public Vector2 Position;
        public Bot Cell;
    }

    public List<Slot> Slots;
    public string ProductedCellName = "Cell";
    public void Place()
    {
        foreach (var slot in Slots)
        {
            Registry.Instance.Add(slot.Position, Quaternion.identity, slot.Cell).name = ProductedCellName;
            //var go = Instantiate(slot.Cell, slot.Position, Quaternion.identity);
        }
    }
}

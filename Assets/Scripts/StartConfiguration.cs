using UnityEngine;
using System;
using System.Collections.Generic;

public class StartConfiguration : MonoBehaviour
{
    [Serializable]
    public struct Slot
    {
        public Vector2 Position;
        public GameObject Cell;
    }

    public List<Slot> Slots;
    public string ProductedCellName = "Cell";
    public void Place()
    {
        foreach (var slot in Slots)
        {
            var go = Instantiate(slot.Cell, slot.Position, Quaternion.identity);
            go.name = ProductedCellName;
        }
    }
}

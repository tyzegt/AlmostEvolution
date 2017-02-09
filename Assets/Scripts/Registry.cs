using UnityEngine;
using System.Collections.Generic;

public class Registry : Singleton<Registry>
{
    public GameObject DeadCell;

    Dictionary<int, GameObject> Cells = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> Corpses = new Dictionary<int, GameObject>();

    Vector2 ValidatePos(Vector3 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }

    public GameObject Add(Vector3 position, Quaternion rotation, Bot prefab)
    {
        var inst = (Bot)Instantiate(prefab, ValidatePos(position), rotation);

        Cells.Add(inst.gameObject.GetInstanceID(), inst.gameObject);
        return inst.gameObject;
    }

    public void Kill(GameObject deadmen)
    {
        if (deadmen)
        {
            if (IsCell(deadmen))
                MakeCorpse(deadmen.transform.position, deadmen.transform.rotation);
            Remove(deadmen);
        }
    }

    public void MakeCorpse(Vector2 position, Quaternion rotation)
    {
        var corpse = (GameObject)Instantiate(DeadCell, ValidatePos(position), rotation);
        Corpses.Add(corpse.GetInstanceID(), corpse);
    }

    public void Remove(GameObject bot)
    {
        if (bot)
        {
            if (IsCell(bot))
                Cells.Remove(bot.GetInstanceID());
            else if (IsCorpse(bot))
                Corpses.Remove(bot.GetInstanceID());
            Destroy(bot.gameObject);
        }
    }
    public int GetCorpsesCount()
    {
        return Corpses.Count;
    }

    public int GetCellsCount()
    {
        return Cells.Count;
    }
    //public Bot Get(int id)
    //{
    //    Bot bot;
    //    if (Cells.TryGetValue(id, out bot))
    //        return bot;
    //    else
    //        return null;
    //}

    public bool IsCorpse(GameObject obj)
    {
        return Corpses.ContainsKey(obj.GetInstanceID());
    }

    public bool IsCell(GameObject obj)
    {
        return Cells.ContainsKey(obj.GetInstanceID());
    }

    List<GameObject> GetInRadius(Vector3 position, float radius, Dictionary<int, GameObject> dict)
    {
        var ret = new List<GameObject>();

        var sqrRadius = radius * radius;
        foreach (var obj in dict)
        {
            if ((obj.Value.transform.position - position).sqrMagnitude < sqrRadius)
                ret.Add(obj.Value);

        }
        return ret;
    }

    public List<GameObject> GetInRadius(Vector3 position, float radius)
    {
        var ret = GetInRadius(position, radius, Cells);
        ret.AddRange(GetInRadius(position, radius, Corpses));
        return ret;
    }
}

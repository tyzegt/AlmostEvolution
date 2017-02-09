using UnityEngine;
using System.Collections.Generic;

public class Registry : Singleton<Registry>
{
    public GameObject DeadCell;

    Dictionary<int, Bot> Cells = new Dictionary<int, Bot>();

    Vector2 ValidatePos(Vector3 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }

    public void Add(Vector3 position, Quaternion rotation, Bot prefab)
    {
        var inst = (Bot)Instantiate(prefab, ValidatePos(position), rotation);

        Cells.Add(inst.Id, inst);
    }

    public void Kill(int id)
    {
        var deadmen = Get(id);
        if (deadmen)
        { 
            MakeCorpse(deadmen.transform.position, deadmen.transform.rotation);
            Remove(id);
        }
    }

    public void MakeCorpse(Vector2 position, Quaternion rotation)
    {
        Instantiate(DeadCell, ValidatePos(position), rotation);
    }

    public void Remove(int id)
    {
        var bot = Get(id);
        if (bot)
            Destroy(bot.gameObject);
    }

    public Bot Get(int id)
    {
        Bot bot;
        if (Cells.TryGetValue(id, out bot))
            return bot;
        else
            return null;
    }
}

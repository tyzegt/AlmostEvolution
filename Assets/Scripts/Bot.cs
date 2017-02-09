using UnityEngine;
using System.Collections;

public class Bot : MonoBehaviour
{

    public Transform[] sensors;
    private Transform sensor;

    public LayerMask side;
    public LayerMask wall;
    public LayerMask food;
    public LayerMask bot;
    public Collider2D[] colliders;
    [Space()]
    public int[] genome;
    public int energy;
    [Space()]
    public int controller;
    [Space()]
    public float red;
    public float green;
    public float blue;
    private float rot;

    public GameObject deadCell;
    public SpriteRenderer Color;

    // Use this for initialization
    void Start()
    {
        //genome = new int[64];
        controller = 0;
        if (LevelManager.Instance.startEnergyValue > 0) energy = LevelManager.Instance.startEnergyValue;
        if (Random.Range(0, 3) == 1)
        {
            LevelManager.Instance.mutations++;
            genome[Random.Range(0, 63)] = Random.Range(0, 63);
        }
        ChangeColor();

        for (int i = 0; i < genome.Length; i++)
        {
            //genome[i] = Random.Range(0,63);
            //genome[i] = 10;
        }

        sensor = sensors[0];
    }

    public void ChangeColor()
    {
        Color.color = new Color(red, green, blue);
    }

    public void Step()
    {
        colliders = Physics2D.OverlapCircleAll(sensor.position, 0.1f);
        if (controller > 63) controller -= 64;

        energy--;

        if (energy > LevelManager.Instance.energyToDivideValue)
        {
            TryToDivide();
        }
        if (energy <= 0)
        {
            Destroy(gameObject);
            return;
            //Debug.Log("starved");
        }
        // смотрит
        if (genome[controller] == 0)
        {
            Profiler.BeginSample("Look");
            Look();
            Profiler.EndSample();
            return;
        }
        // поворачивается
        else if (genome[controller] > 0 && genome[controller] < 8)
        {
            Turn();
            return;
        }
        // жрёт
        else if (genome[controller] == 8)
        {
            Profiler.BeginSample("Look"); 
            Eat();
            Profiler.EndSample();
            return;
        }
        // жрёт
        else if (genome[controller] == 9)
        {
            Profiler.BeginSample("Move");
            Move();
            Profiler.EndSample();

            return;
        }
        // фотосинтез
        else if (genome[controller] == 10)
        {
            Synth();
            return;
        }
        // проверяет здоровье
        else if (genome[controller] == 11)
        {
            CheckEnergy();
            return;
        }
        // рожает
        else if (genome[controller] == 12)
        {
            if (energy > LevelManager.Instance.energyToDivideValue)
            {
                TryToDivide();
                return;
            }
        }
        // переход
        else if (genome[controller] > 12)
        {
            controller += genome[controller];
            return;
        }

    }

    void TryToDivide()
    {
        for (int i = sensors.Length - 1; i >= 0; i--)
        {
            if (Physics2D.OverlapCircleAll(sensors[i].position, 0.1f).Length == 0)
            {
                Divide(sensors[i]);
                return;
            }
        }
        Die();
    }

    void Divide(Transform tr)
    {

        Instantiate(this, new Vector2(Mathf.Round(tr.position.x), Mathf.Round(tr.position.y)), tr.rotation);
        energy /= 2;
    }

    void Look()
    {
        if (colliders.Length == 0)                                                  // Пусто
        {
            controller++;
            return;
        }

        if (colliders[0].gameObject.layer == LayerMask.NameToLayer("side"))         // Край
        {
            controller++;
            return;
        }
        else if (colliders[0].gameObject.layer == LayerMask.NameToLayer("wall"))    // стена
        {
            controller += 2;
            return;
        }
        else if (colliders[0].gameObject.layer == LayerMask.NameToLayer("food"))    // еда
        {
            controller += 3;
            return;
        }
        else if (colliders[0].gameObject.layer == LayerMask.NameToLayer("bot"))     // бот
        {
            if (CheckRelations()) controller += 4;
            else controller += 5;
            return;
        }

    }

    void Turn()
    {
        rot += 45 * genome[controller];
        if (rot > 360) rot -= 360;
        transform.rotation = Quaternion.Euler(0, 0, rot);
        controller++;
    }

    void Eat()
    {
        if (colliders.Length == 0)                                                  // Пусто
        {
            controller++;
            return;
        }

        if (colliders[0].gameObject.layer == LayerMask.NameToLayer("side"))         // Край
        {
            controller++;
            return;
        }
        else if (colliders[0].gameObject.layer == LayerMask.NameToLayer("wall"))    // стена
        {
            controller += 2;
            return;
        }
        else if (colliders[0].gameObject.layer == LayerMask.NameToLayer("food"))    // еда
        {
            energy += LevelManager.Instance.calloriesValue;
            controller += 3;
            Destroy(colliders[0].gameObject);

            red += 1f;
            if (red > 1) red = 1;
            green -= 1f;
            if (green < 0) green = 0;
            blue -= 1f;
            if (blue < 0) blue = 0;
            ChangeColor();

            return;
        }
        else if (colliders[0].gameObject.layer == LayerMask.NameToLayer("bot"))     // бот
        {
            if (CheckRelations())
            {
                controller += 4;
                energy += LevelManager.Instance.calloriesValue;
            }
            else
            {
                controller += 5;
                energy += LevelManager.Instance.calloriesValue;
            }
            Destroy(colliders[0].gameObject);

            red += 1f;
            if (red > 1) red = 1;
            green -= 1f;
            if (green < 0) green = 0;
            blue -= 1f;
            if (blue < 0) blue = 0;
            ChangeColor();
            return;
        }

    }

    void Move()
    {
        if (colliders.Length == 0)                                                  // Пусто
        {
            transform.position = new Vector2(Mathf.Round(sensor.position.x), Mathf.Round(sensor.position.y));
            controller++;
            return;
        }

        if (colliders[0].gameObject.layer == LayerMask.NameToLayer("side"))         // Край
        {
            float newx = Mathf.Round(sensor.position.x);
            if (newx < 0) newx = 99;
            if (newx > 99) newx = 0;
            transform.position = new Vector2(newx, Mathf.Round(sensor.position.y));
            controller++;
            return;
        }
        else if (colliders[0].gameObject.layer == LayerMask.NameToLayer("wall"))    // стена
        {
            controller += 2;
            return;
        }
        else if (colliders[0].gameObject.layer == LayerMask.NameToLayer("food"))    // еда
        {
            energy += LevelManager.Instance.calloriesValue;
            controller += 3;
            Destroy(colliders[0].gameObject);

            transform.position = new Vector2(Mathf.Round(sensor.position.x), Mathf.Round(sensor.position.y));

            red += 1f;
            if (red > 1) red = 1;
            green -= 1f;
            if (green < 0) green = 0;
            blue -= 1f;
            if (blue < 0) blue = 0;
            ChangeColor();
            return;
        }
        else if (colliders[0].gameObject.layer == LayerMask.NameToLayer("bot"))     // бот
        {
            if (CheckRelations())
            {
                controller += 4;
                energy += LevelManager.Instance.calloriesValue;
            }
            else
            {
                controller += 5;
                energy += LevelManager.Instance.calloriesValue;
            }
            Destroy(colliders[0].gameObject);

            transform.position = new Vector2(Mathf.Round(sensor.position.x), Mathf.Round(sensor.position.y));

            red += 1f;
            if (red > 1) red = 1;
            green -= 1f;
            if (green < 0) green = 0;
            blue -= 1f;
            if (blue < 0) blue = 0;
            ChangeColor();
            return;
        }

    }

    void Synth()
    {
        energy += (int)Mathf.Round(transform.position.y * LevelManager.Instance.synthMultiplerValue);
        controller++;


        green += 0.1f;
        if (green > 1) green = 1;
        blue -= 0.1f;
        if (blue < 0) blue = 0;
        red -= 0.1f;
        if (red < 0) red = 0;
        ChangeColor();
    }

    void CheckEnergy()
    {
        int cnt = controller + 1;
        if (cnt > 63) cnt -= 64;
        if (energy < genome[cnt] * 15)
        {
            controller += 2;
        }
        if (energy >= genome[cnt] * 15)
        {
            controller += 3;
        }
    }

    public void Die()
    {
        Instantiate(deadCell, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    bool CheckRelations()
    {
        int dismatches = 0;

        for (int i = 0; i < genome.Length; i++)
        {
            if (dismatches < 2)
            {
                if (genome[i] != colliders[0].GetComponent<Bot>().genome[i]) dismatches++;
            }
            else return false;
        }
        return true;
    }

    void Update()
    {
        Step();
    }
}

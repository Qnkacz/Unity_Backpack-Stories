using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodBoyPoints : MonoBehaviour
{
    public UnityEngine.UI.Text time;
    public static GoodBoyPoints instance;
    public int points;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        time.text = points.ToString();
    }
}

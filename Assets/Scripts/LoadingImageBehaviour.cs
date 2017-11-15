using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingImageBehaviour : MonoBehaviour
{

    private Color color_cycle;
    private Color start;
    private Color end;
    bool op;

    private void Start()
    {
        start = new Color(26,49,32);
        end = new Color(28,92,45);

        color_cycle = new Color(26,49,32);
    }


    // Update is called once per frame
    void Update()
    {
        // loop until load
        //if (!load)
        if (color_cycle == start)
            op = true;
        else if (color_cycle == end)
            op = false;
        

        if (op)
        {
            // go towards end
            color_cycle = Color.Lerp(start, end, 0.1f);
            gameObject.GetComponent<Image>().color = color_cycle;
        }
        else
        {
            color_cycle = Color.Lerp(end, start, 0.1f);
            gameObject.GetComponent<Image>().color = color_cycle;
            // go towards start
        }

    }
}

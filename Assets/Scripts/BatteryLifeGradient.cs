using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryLifeGradient : MonoBehaviour
{
    public Gradient gradient;
    private Image image;

    private void Start()
    {
        image = this.GetComponent<Image>();
    }

    public void SetFill(float val)
    {
        image.fillAmount = val;
        image.color = gradient.Evaluate(val);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryLifeGradient : MonoBehaviour
{
    public Gradient gradient;
    private Image image;

    [SerializeField] private Animator batterAlertAnim = null;
    private bool isLow = false;

    private void Start()
    {
        image = this.GetComponent<Image>();
    }

    public void SetFill(float val)
    {
        image.fillAmount = val;
        image.color = gradient.Evaluate(val);

        if (!isLow && val < 0.25)
        {
            isLow = true;
            batterAlertAnim.SetBool("BatteryLow", true);
        } else if (isLow && val > 0.25)
        {
            isLow = false;
            batterAlertAnim.SetBool("BatteryLow", false);
        }
    }
}

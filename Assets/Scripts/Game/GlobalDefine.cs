using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class GlobalDefine
{
    public static float startSpeed = 8;//起始速度
    public static float startLife = 1;//起始生命
    public static float maxSpeed = 20;//最大速度
    public static float addedSpeed = 0.5f;//每次增加的速度
    public static float addSpeedEveryDistance = 100;//每多少距离增加一次速度

    public static float defaultBuildingInterval = 4;//建筑间隔距离(z轴向)

    public static void SetImageColorAlpha(Image image, float a)
    {
        Color color = image.color;
        color.a = a;
        image.color = color;
    }
    public static void SetAllImageColorAlpha(GameObject obj, float a)
    {
        //self
        SetImageColorAlpha(obj.GetComponent<Image>(), a);

        //child
        Image[] images = obj.GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            SetImageColorAlpha(img, a);
        }
    }
}
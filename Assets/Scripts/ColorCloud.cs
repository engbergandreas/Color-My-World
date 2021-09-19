using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCloud : DrawableObject
{
    private const float min = 5, max = 15;

    [Range(min, max)]
    public float lifeTime = 15.0f;

    private float endTime, t;
    Vector3 startvalue;

    private bool isShrinking = true;


    protected override void Start()
    {
        base.Start();
        endTime = Time.timeSinceLevelLoad + lifeTime;
        startvalue = transform.localScale;
        t = 0;
    }

    private void Update()
    {
        if (isShrinking)
        {
            t = Time.timeSinceLevelLoad / endTime;
            transform.localScale = Vector3.Lerp(startvalue, Vector3.zero, t);
        }
    }

    protected override void FullyColored()
    {
        if (canGivePoints)
        {
            int bonus = Mathf.RoundToInt(pointsToGive * (1 - (lifeTime / (max + min)) )); // extra points given at points * [0.25, 0.75]
            int points = Mathf.RoundToInt(pointsToGive * (1 - t)) + bonus;
            PointSystem.Instance.AddPoints(points);
            canGivePoints = false;
            isShrinking = false;
            Destroy(gameObject, 5);
        }
    }
}

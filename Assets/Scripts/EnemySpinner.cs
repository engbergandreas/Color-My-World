using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpinner : MonoBehaviour
{
    public int nrOfArms = 1;
    [Range(-0.5f, 0.5f)]
    public float rotationSpeed = 0.3f;

    public GameObject arm;


    private void Start()
    {
        float angleBetweenArms = 2 * Mathf.PI / nrOfArms;
        for(int i = 0; i < nrOfArms; i++)
        {
            float angle = i * angleBetweenArms;
            float angleindeg = angle * 180 / Mathf.PI;
            Vector2 pos = PolarToCartesian(angle, transform.localScale.x);
            var obj = Instantiate(arm, transform);
            obj.SetActive(true);
            obj.transform.localPosition = new Vector3(pos.x, 0, pos.y);
            obj.transform.eulerAngles = new Vector3(0, -angleindeg, 90);
        }
    }

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * 360 * Time.deltaTime);
    }

    Vector2 PolarToCartesian(float angle, float radius)
    {
        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }

}

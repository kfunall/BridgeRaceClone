using UnityEngine;

public class Collactable : MonoBehaviour
{
    int index;
    bool collected = false;
    Color color;

    private void Awake()
    {
        color = GetComponent<Renderer>().material.color;
    }
    public void Collect(int value, Transform parent, string objectTag)
    {
        index = value;
        collected = true;
        transform.tag = objectTag;
        transform.SetParent(parent);
        transform.localPosition = Vector3.up * index * transform.localScale.y;
        transform.localEulerAngles = Vector3.zero;
    }
    public void Drop()
    {
        Destroy(gameObject);
    }
    public bool CollectSituation()
    {
        return collected;
    }
    public Color GetColor()
    {
        return color;
    }
}
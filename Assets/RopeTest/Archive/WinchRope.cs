using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class WinchRope : MonoBehaviour
{
    [SerializeField]
    GameObject boomTarget;
    [SerializeField]
    bool deletePart;
    [SerializeField]
    bool addPart;

    [SerializeField]
    GameObject partPrefab;

    [SerializeField]
    Transform lastPart;

    Transform[] ropeParts;

    private Vector3 ropeLocalOrigin = new Vector3(-3.8f, .5f, 0f);

    void Update()
    {
        ropeParts = transform.GetComponentsInChildren<Transform>();

        FollowBoom();

        if (deletePart)
        {
            DeleteRopePart();
            deletePart = false;
        }

        if (addPart)
        {
            AddRopePart();
            addPart = false;
        }

        Transform origin = ropeParts[1];
        var dist = (origin.position - boomTarget.transform.position).magnitude - (origin.localScale.y / 2);
        Debug.Log($"dist {dist} / {origin.localScale.y}");
    }

    void FollowBoom()
    {
        if (boomTarget && lastPart)
        {
            lastPart.transform.position = boomTarget.transform.position;
        }
    }

    void AddRopePart()
    {
        Transform a = ropeParts[1]; // todo get dinamically
        Transform b = ropeParts[2]; // todo get dinamically
        Transform c = ropeParts[3]; // todo get dinamically

        Vector3 pos = a.position;
        Quaternion rot = a.rotation;

        GameObject tmp;
        tmp = Instantiate(partPrefab, pos, Quaternion.identity, transform);
        tmp.transform.SetSiblingIndex(0);
        tmp.transform.position = pos;
        tmp.transform.rotation = rot;
        tmp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        CharacterJoint tmpComponent = tmp.GetComponent<CharacterJoint>();
        CharacterJoint aComponent = a.gameObject.AddComponent<CharacterJoint>();
        CopyComponentValuesFromSourceToTarget(tmpComponent, aComponent);
        Destroy(tmpComponent);

        aComponent.connectedBody = tmp.gameObject.GetComponent<Rigidbody>();
        a.GetComponent<CharacterJoint>().connectedAnchor = c.GetComponent<CharacterJoint>().connectedAnchor;

        tmp.transform.position = pos;
        tmp.transform.rotation = rot;
        a.position = b.position;
        a.rotation = b.rotation;
    }

    void DeleteRopePart()
    {
        Transform a = ropeParts[1]; // todo get dinamically
        Transform b = ropeParts[2];

        Debug.Log($"{a.gameObject.name} / {b.gameObject.name}");

        Destroy(a.gameObject);

        b.GetComponent<CharacterJoint>().connectedBody = a.gameObject.GetComponent<Rigidbody>();
        b.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        b.position = a.position;
        b.rotation = a.rotation;

    }


    void CopyComponentValuesFromSourceToTarget(CharacterJoint sourceComponent, CharacterJoint targetComponent)
    {
        FieldInfo[] fields = typeof(CharacterJoint).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (FieldInfo field in fields)
        {
            field.SetValue(targetComponent, field.GetValue(sourceComponent));
        }
    }

    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }
}
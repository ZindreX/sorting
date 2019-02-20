using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListVisual : MonoBehaviour {

    [SerializeField]
    private GameObject listObjPrefab;

    [SerializeField]
    private TextMesh listTypeTitle;

    [SerializeField]
    private Transform spawnPointList, outPoint;

    private GameObject outElement;
    private List<GameObject> listObjects;

    private void Awake()
    {
        listObjects = new List<GameObject>();
    }


    public void SetListType(string listType)
    {
        listTypeTitle.text = listType;
    }

    public void AddListObject(char id)
    {
        Vector3 pos = spawnPointList.position + new Vector3(0f, 1f, 0f) * listObjects.Count;
        GameObject listObject = Instantiate(listObjPrefab, pos, Quaternion.identity);
        listObject.GetComponent<TextHolder>().SetSurfaceText(id.ToString());
        listObject.GetComponent<MoveObject>().SetDestination(pos);
        listObjects.Add(listObject);
    }

    public void RemoveAndMoveElementOut()
    {
        switch (listTypeTitle.text)
        {
            case Util.QUEUE:
                outElement = listObjects[0];
                listObjects.RemoveAt(0);
                MoveObjectOut(outElement);
                foreach (GameObject obj in listObjects)
                {
                    obj.GetComponent<MoveObject>().SetDestination(obj.transform.position - new Vector3(0f, 1f, 0f));
                }
                break;

            case Util.STACK:
                outElement = listObjects[listObjects.Count - 1];
                listObjects.RemoveAt(listObjects.Count - 1);
                MoveObjectOut(outElement);
                break;
        }
    }

    private void MoveObjectOut(GameObject obj)
    {
        obj.GetComponent<MoveObject>().SetDestination(outPoint.position);
    }

    public void DestroyOutElement()
    {
        Destroy(outElement);
    }

}

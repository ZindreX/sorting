using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.Windows;

public class Video : MonoBehaviour {

    [SerializeField]
    private string title;

    [SerializeField]
    private string url;

    [SerializeField]
    private GetVideoFrom getVideoFrom;
    private enum GetVideoFrom { Streaming, LocalPath }

    [SerializeField]
    private TitleColor titleColor;
    private enum TitleColor { Green, Blue, Red }

    [SerializeField]
    private Material[] textMaterials;

    //
    private string baseFolderPath = "D:\\sindrw\\Videos\\", format = ".mp4";

    private Vector3 startPos, insertEjectMovement = new Vector3(-0.164f, 0f, 0f);

    private Rigidbody rb;
    private Animator animator;
    private readonly string VHS_INSERT_ANIMATION = "Insert", VHS_EJECT_ANIMATION = "Eject";

    private bool releasedFromHand;

    private WaitForSeconds enterDuration = new WaitForSeconds(1f);

    private void Awake()
    {
        startPos = transform.position;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        releasedFromHand = true;

        // Fix filepath, destroy if it doesn't exists
        if ((int)getVideoFrom == 1)
        {
            url = baseFolderPath + url + format;

            if (!File.Exists(url))
            {
                title = "Bad tape";
                Destroy(gameObject, 5f);
                Debug.Log("Destroying VHS, check path: " + url);
            }
        }

        // >>> Set title
        Component[] components = GetComponentsInChildren<TextMeshPro>();
        foreach (Component comp in components)
        {
            TextMeshPro temp = (TextMeshPro)comp;
            temp.text = title;

            int useColor = (int)titleColor;
            if (useColor < textMaterials.Length)
                temp.fontSharedMaterial = textMaterials[useColor];

        }
    }

    private void Update()
    {
        if (transform.position.y < 0)
            transform.position = startPos;
    }

    public string Title
    {
        get { return title; }
    }

    public string Url
    {
        get { return url; }
    }

    public bool ReleasedFromHand
    {
        get { return releasedFromHand; }
        set { releasedFromHand = value; }
    }

    public IEnumerator Insert()
    {
        transform.rotation = Quaternion.identity;
        transform.Rotate(0, -90, 0);
        animator.SetBool(VHS_INSERT_ANIMATION, true);
        yield return enterDuration;
        transform.position += insertEjectMovement;
        animator.SetBool(VHS_INSERT_ANIMATION, false);
    }

    public IEnumerator Eject(Vector3 ejectPos)
    {
        transform.position = ejectPos;
        animator.SetBool(VHS_EJECT_ANIMATION, true);
        yield return enterDuration;
        animator.SetBool(VHS_EJECT_ANIMATION, false);
        transform.position -= insertEjectMovement;
        rb.constraints = RigidbodyConstraints.None;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Util.FLOOR_TAG))
        {
            transform.position = startPos;
        }
    }
}

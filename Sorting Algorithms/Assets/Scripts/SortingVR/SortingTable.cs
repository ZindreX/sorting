using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SortingTable : StartExchangePosition {

    [SerializeField]
    private Transform returnPlate;

    private Color originalColor;
    private Renderer plateRenderer;

    [SerializeField]
    private TextMeshPro returnText;

    private WaitForSeconds showMessageDuration = new WaitForSeconds(3f);

    private Vector3 switchingPos = new Vector3(UtilSort.SPACE_BETWEEN_HOLDERS / 2, 0f, 0f), aboveTable = new Vector3(0f, 1f, 0f);
    private bool alternate, canUpdateText;

    protected override void Awake()
    {
        base.Awake();
        canUpdateText = true;

        plateRenderer = returnPlate.gameObject.GetComponent<Renderer>();
        originalColor = plateRenderer.material.color;

        Section sortingTableSection = GetComponentInChildren<Section>();
        sortingTableSection.SectionManager = FindObjectOfType<SortSettings>();

        InScenePosition = new Vector3(0f, 0.3f, 0f);
    }

    // The return position is used by sorting elements which hit the floor, which are then returned to this spot
    public Vector3 ReturnPosition
    {
        get {
            alternate = !alternate;

            if (canUpdateText)
            {
                canUpdateText = false;
                StartCoroutine(TextUpdate());
            }

            Vector3 temp = returnPlate.position + aboveTable;
            return alternate ? temp - switchingPos : temp + switchingPos; }
    }

    public IEnumerator TextUpdate()
    {
        plateRenderer.material.color = Util.STANDARD_COLOR;
        returnText.text = "Returned dropped element";
        yield return showMessageDuration;
        plateRenderer.material.color = originalColor;
        canUpdateText = true;
        returnText.text = "";
        
    }
}

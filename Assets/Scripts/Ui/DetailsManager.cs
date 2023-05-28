using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject detailsParent;

    [SerializeField]
    private TextMeshProUGUI gradeDomain;

    [SerializeField]
    private TextMeshProUGUI cluster;

    [SerializeField]
    private TextMeshProUGUI idDescription;


    private int blockLayer;

    private BlockDAO[][] blocks;


    private void Awake()
    {
        blockLayer = 1 << LayerMask.NameToLayer("Block");
        StackManager.OnQueryComplete += CacheBlocks;

        gradeDomain.text = "Please select a block (by right-clicking) to view its details.";
        cluster.text = idDescription.text = string.Empty;

        GetComponent<Toggle>().onValueChanged.AddListener(ShowDetails);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(1))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, 100f, blockLayer))
            return;

        // Yeah, I can manage this better - I swear haha
        Stacker hitStack = hit.transform.GetComponentInParent<Stacker>();
        Stacker currentStack = CameraController.Instance.Target.GetComponentInParent<Stacker>();

        if (hitStack != currentStack)
            return;

        int blockIndex = hit.transform.GetSiblingIndex();
        SetDetails(currentStack.BlockData[blockIndex]);
    }

    public void ShowDetails(bool show) => detailsParent.SetActive(show);

    private void CacheBlocks(BlockDAO[][] blocks) => this.blocks = blocks;

    public void SetDetails(BlockDAO block)
    {
        gradeDomain.text = $"{block.grade[0]}: {block.domain}";
        cluster.text = block.cluster;
        idDescription.text = $"{block.standardid}: {block.standarddescription}";
    }
}

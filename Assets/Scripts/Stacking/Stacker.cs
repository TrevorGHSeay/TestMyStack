using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stacker : MonoBehaviour
{
    [SerializeField]
    private BlockSettings settings;

    [SerializeField]
    private TextMeshProUGUI label;

    [SerializeField]
    private int blocksCreatedPerFrame = 1;

    [SerializeField]
    private Transform blockParent;

    [SerializeField]
    private Transform cameraTarget;
    public Transform CameraTarget => cameraTarget;

    public BlockDAO[] BlockData { get; private set; }

    private (GameObject, BlockDAO)[] blocks;

    private bool tested = false;

    public string Text
    {
        get => label.text;
        set => label.text = value;
    }

    private int row;
    private int column;

    private void Awake()
    {
        row = 0;
        column = 0;
    }

    public void Stack(BlockDAO[] blocks)
    {
        if (blocks is null || blocks.Length < 1)
        {
            blocks = new BlockDAO[0];
            return;
        }

        this.BlockData = blocks;
        this.blocks = new (GameObject, BlockDAO)[blocks.Length];

        string grade = blocks[0].grade;

        this.Text = grade;
        StartCoroutine(CreatingBlocks(blocks));
    }

    private IEnumerator CreatingBlocks(IEnumerable<BlockDAO> blocks)
    {
        int i = 0;
        int j = 0;
        foreach(var block in blocks)
        {
            var newBlock = CreateBlock(block);
            this.blocks[j++] = (newBlock, block);

            i += 1;
            if (i < blocksCreatedPerFrame)
                continue;

            yield return null;
            i = 0;
        }
    }

    private GameObject CreateBlock(BlockDAO block)
    {
        float x = settings.ColumnWidth * -0.5f + settings.BlockDimensions.x * column + settings.BlockDimensions.x * 0.5f;
        float y = settings.BlockDimensions.y * row;

        Vector3 localPos = new Vector3(x, y, 0f);

        Vector3 globalPos;
        Vector3 globalRot;

        // Every even row will be rotated
        if (row % 2 == 0)
        {
            transform.eulerAngles += new Vector3(0f, 90f, 0f);

            globalPos = transform.TransformPoint(localPos);
            globalRot = new Vector3(0f, 90f, 0f);

            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            globalPos = transform.TransformPoint(localPos);
            globalRot = new Vector3();
        }

        var newBlock = Instantiate(settings.BlockPrefab, globalPos, Quaternion.Euler(globalRot), blockParent);

        foreach (var renderer in newBlock.GetComponentsInChildren<Renderer>())
            renderer.materials = renderer.sharedMaterials = new Material[] { MaterialFor(block) };

        newBlock.GetComponentInChildren<Rigidbody>().isKinematic = true;

        IncrementBlockPlacement();

        return newBlock;
    }

    public void TestMyStack()
    {
        if (tested)
            return;

        tested = true;

        // Tuples are gross, but they work in a pinch (crunch time!)
        for(int i = 0; i < this.blocks.Length; i += 1)
            if (this.blocks[i].Item2.mastery == 0)
                this.blocks[i].Item1.SetActive(false); // cheaper than destroying, and maybe we'd still want them

        // Should really cache these, but time is of the essense!
        foreach (var rigidbody in gameObject.GetComponentsInChildren<Rigidbody>())
            rigidbody.isKinematic = false;
    }

    private void IncrementBlockPlacement()
    {
        if (column < 2)
        {
            column += 1;
            return;
        }

        column = 0;
        row += 1;
    }

    private Material MaterialFor(BlockDAO block)
    {
        switch(block.mastery)
        {
            case 0: return settings.Glass;
            case 1: return settings.Wood;
            case 2: return settings.Stone;

            default: throw new ArgumentException($"Parameter named '{nameof(block.mastery)}' of value '{block.mastery}' not yet accounted for.");
        }
    }
}

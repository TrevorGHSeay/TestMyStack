using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockSettings", menuName = "Assets/Stacking/Block Settings")]
public class BlockSettings : ScriptableObject
{
    [SerializeField]
    private Material glass;
    public Material Glass => glass;

    [SerializeField]
    private Material wood;
    public Material Wood => wood;

    [SerializeField]
    private Material stone;
    public Material Stone => stone;

    [SerializeField]
    private GameObject blockPrefab;
    public GameObject BlockPrefab => blockPrefab;

    [SerializeField]
    private Vector3 blockDimensions;
    public Vector3 BlockDimensions => blockDimensions;


    public float ColumnWidth => blockDimensions.x * 3f;
}

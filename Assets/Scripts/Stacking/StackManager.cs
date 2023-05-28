using System;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    [SerializeField]
    private Stacker[] stackers;

    public delegate void QueryCompletionEvent(BlockDAO[][] blocks);

    public static event QueryCompletionEvent OnQueryComplete;

    public static readonly string[] grades = new string[]
    {
        "6th Grade",
        "7th Grade",
        "8th Grade",
    };


    private void Awake()
    {
        // Give a wee bit of time to let subscribers hook in
        Invoke(nameof(Initialize), 0.2f);
    }

    private void Initialize()
    {
        DAL.QueryStack(StackQuerySuccessful, StackQueryFailed);
    }


    private void StackQuerySuccessful(BlockDAO[] blocks)
    {
        Split(blocks, out var grades);

        var queryArguments = new BlockDAO[grades.Count][];

        for (int i = 0; i < grades.Count; i += 1)
            stackers[i].Stack(queryArguments[i] = grades[i].ToArray()); // Yeah, linq's not so efficient for this but we're being timed!

        OnQueryComplete(queryArguments);
    }

    private static void Split(BlockDAO[] blocks, out List<List<BlockDAO>> grades) 
    {
        grades = new List<List<BlockDAO>>();

        for (int i = 0; i < StackManager.grades.Length; i += 1)
            grades.Add(new List<BlockDAO>());

        foreach (var block in blocks)
        {
            int gradeIndex = Array.IndexOf(StackManager.grades, block.grade);
            if (gradeIndex == -1)
                continue;
            grades[gradeIndex].Add(block);
        }
    }

    private void StackQueryFailed(string error)
    {
        Debug.LogError(error);
    }
}

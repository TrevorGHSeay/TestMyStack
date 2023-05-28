using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;

public static class DAL
{
    // One extra layer of encapsulation for good measure
    private static readonly Func<string> URL = () => "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";

    public delegate void SuccessCallback<T>(T[] result) where T : DAO;
    public delegate void FailureCallback(string errorMessage);

    /// <summary>
    /// Query for a collection of <see cref="BlockDAO"/> and return the result.
    /// </summary>
    /// <param name="success">Callback on successful query.</param>
    /// <param name="failure">Callback on unsuccessful query.</param>
    public static void QueryStack(SuccessCallback<BlockDAO> success, FailureCallback failure)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL());
        www.downloadHandler = new DownloadHandlerBuffer();

        var asyncOperation = www.SendWebRequest();

        asyncOperation.completed += (ao) => OnCompleted(www, success, failure);
    }

    private static void OnCompleted<T>(UnityWebRequest www, SuccessCallback<T> success, FailureCallback failure) where T : DAO
    {
        if (www.result != UnityWebRequest.Result.Success)
        {
            failure(www.error);
            return;
        }

        var result = JsonHelper.FromJson<T>(JsonHelper.FixJsonArray(www.downloadHandler.text));

        if (result is null)
        {
            failure(string.Format(DAO.InvalidFromJson, typeof(T).Name));
            return;
        }

        success(result);
    }
}

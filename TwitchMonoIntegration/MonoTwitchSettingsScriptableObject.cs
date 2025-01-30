using System.Collections.Generic;
using System.Linq;
using TwitchSDK;
using UnityEngine;

public class MonoTwitchSettingsScriptableObject : ScriptableObject
{
    [Header("Channel")]
    public bool channelManagePredictions;
    public bool channelManageBroadcast;
    public bool channelManageRedemptions;
    public bool channelReadHypeTrain;
    [Header("Clips")]
    public bool clipEdit;
    [Header("User")]
    public bool userReadSubscriptions;
    [Header("Bits")]
    public bool bitsRead;

    public string[] otherScopes;

    public TwitchOAuthScope[] GetAuthScope =>
        otherScopes.Select(x => new TwitchOAuthScope(x))
            .Concat(GetInternalAuth())
            .ToArray();

    private List<TwitchOAuthScope> GetInternalAuth()
    {
        var result = new List<TwitchOAuthScope>();
        if(channelManagePredictions)
            result.Add(new TwitchOAuthScope("channel:manage:predictions"));
        if(channelManageBroadcast)
            result.Add(new TwitchOAuthScope("channel:manage:broadcast"));
        if(channelManageRedemptions)
            result.Add(new TwitchOAuthScope("channel:manage:redemptions"));
        if(channelReadHypeTrain)
            result.Add(new TwitchOAuthScope("channel:read:hype_train"));
        if(clipEdit)
            result.Add(new TwitchOAuthScope("clips:edit"));
        if(userReadSubscriptions)
            result.Add(new TwitchOAuthScope("user:read:subscriptions"));
        if(bitsRead)
            result.Add(new TwitchOAuthScope("bits:read"));
        return result;
    }
}
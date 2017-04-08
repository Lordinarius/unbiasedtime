# Unbiased Time Manager

Unbiased Time Manager combines Unity3D's Time.unscaledTime and NTP server times to report more reliable device independent Times. 
It returns time with ulong format in miliseconds unit.

# How to use ?

It is pretty straight forward but before getting time from it. It needs to be initialized. To do that, call.
```cs
UnbiasedTime.Init();
```
If everything is ok. Depending on your connection speed. It will be ready in miliseconds. Now you can get it with.

```cs
UnbiasedTime.Instance.time;
```

It will return a ulong value in millisecond format. If you want it in more readable format you can have DateTime with calling.
```cs
UnbiasedTime.Instance.dateTime;
```

By default the manager updates time from server in every 64 seconds and everytime on application focused. But if you want to do it manually just call.
```cs
UnbiasedTime.Instance.Initialize();
```

You can subscribe onTimeReceive event to get notified when time updated

```cs
 private void Start()
{
    UnbiasedTime.Instance.onTimeReceive += OnTimeReceive;
}

private void OnTimeReceive(bool isSucceed, ulong time)
{
    if (isSucceed)
    {
        Debug.Log(time);
        Debug.Log(UnbiasedTime.GetDateTime(time));
    }
    else
    {
        //No reliable time 
    }
}
```

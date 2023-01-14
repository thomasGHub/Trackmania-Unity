using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignMapUiBlock : LocalMapUIBlock
{
    public override void Init(MapInfo mapInfo)
    {
        base.Init(mapInfo);
    }

    protected override IEnumerator UploadingMap(ListJsonData listBlock, bool alreadyPublished)
    {
        RequestData requestDataInfoMap;
        RequestData requestDataBlocksMap;
        Coroutine first;
        Coroutine second;

        if (!alreadyPublished)
        {
            requestDataInfoMap = new NewData(Database.TrackmaniaCampaign, Source.TrackmaniaDB, Collection.MapInfo, _mapInfo);
            requestDataBlocksMap = new NewData(Database.TrackmaniaCampaign, Source.TrackmaniaDB, Collection.MapData, listBlock);

            first = StartCoroutine(RequestManager.SendingNewData(requestDataInfoMap));
            second = StartCoroutine(RequestManager.SendingNewData(requestDataBlocksMap));
        }
        else
        {
            requestDataInfoMap = new UpdatingData(Database.TrackmaniaCampaign, Source.TrackmaniaDB, Collection.MapInfo, _mapInfo, new FilterID(_mapInfo.ID));
            requestDataBlocksMap = new UpdatingData(Database.TrackmaniaCampaign, Source.TrackmaniaDB, Collection.MapData, listBlock, new FilterID(_mapInfo.ID));

            first = StartCoroutine(RequestManager.UpdatingdData(requestDataInfoMap));
            second = StartCoroutine(RequestManager.UpdatingdData(requestDataBlocksMap));
        }

        yield return first;
        yield return second;

        Published();
    }
}

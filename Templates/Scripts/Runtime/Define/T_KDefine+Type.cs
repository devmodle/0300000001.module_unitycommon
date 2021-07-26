using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

#if NEVER_USE_THIS
#region 기본
//! 게임 속성
[System.Serializable]
public struct STGameConfig {

}

//! 아이템 정보
[System.Serializable]
public struct STItemInfo {
	public int m_nNumItems;
	public EItemKinds m_eItemKinds;
}

//! 타입 랩퍼
[MessagePackObject]
public struct STTypeWrapper {
	[Key(31)] public List<long> m_oLevelIDList;
	[Key(201)] public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> m_oLevelInfoDictContainer;
}
#endregion			// 기본
#endif			// #if NEVER_USE_THIS

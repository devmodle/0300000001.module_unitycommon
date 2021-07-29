using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

#region 조건부 타입
#if UNITY_EDITOR || UNITY_STANDALONE
//! 서브 에디터 레벨 생성 정보
public class CSubEditorLevelCreateInfo : CEditorLevelCreateInfo {

}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
#endregion			// 조건부 타입
#endif			// #if NEVER_USE_THIS

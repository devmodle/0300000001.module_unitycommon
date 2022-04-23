using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#region 기본
/** 설명 정보 */
[System.Serializable]
public struct STDescInfo {
	public string m_oName;
	public string m_oDesc;

	#region 함수
	/** 생성자 */
	public STDescInfo(SimpleJSON.JSONNode a_oDescInfo) {
		m_oName = a_oDescInfo[KCDefine.U_KEY_NAME].ExIsValid() ? a_oDescInfo[KCDefine.U_KEY_NAME] : string.Empty;
		m_oDesc = a_oDescInfo[KCDefine.U_KEY_DESC].ExIsValid() ? a_oDescInfo[KCDefine.U_KEY_DESC] : string.Empty;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 에피소드 정보를 생성한다 */
	public void MakeDescInfo(SimpleJSON.JSONClass a_oOutDescInfo) {
		a_oOutDescInfo.Add(KCDefine.U_KEY_NAME, m_oName ?? string.Empty);
		a_oOutDescInfo.Add(KCDefine.U_KEY_DESC, m_oDesc ?? string.Empty);
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 아이템 개수 정보 */
[System.Serializable]
public struct STNumItemsInfo {
	public long m_nNumItems;
	public EItemKinds m_eItemKinds;
}

/** 게임 속성 */
[System.Serializable]
public struct STGameConfig {
	// Do Something
}

/** 타입 랩퍼 */
[MessagePackObject]
public struct STTypeWrapper {
	[Key(51)] public List<long> m_oUniqueLevelIDList;

	[Key(161)] public Dictionary<int, Dictionary<int, int>> m_oNumLevelInfosDictContainer;
	[Key(162)] public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> m_oLevelInfoDictContainer;
}
#endregion			// 기본
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY

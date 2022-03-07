using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
#region 기본
/** 기록 정보 */
public struct STRecordInfo {
	public bool m_bIsSuccess;
	public long m_nIntRecord;
	public double m_dblRealRecord;
}

/** 게임 속성 */
[System.Serializable]
public struct STGameConfig {
	// Do Something
}

/** 아이템 정보 */
[System.Serializable]
public struct STItemInfo {
	public long m_nNumItems;
	public EItemKinds m_eItemKinds;
}

/** 공용 에피소드 정보 */
[System.Serializable]
public struct STCommonEpisodeInfo {
	public string m_oName;
	public string m_oDesc;

	public EDifficulty m_eDifficulty;
	public ERewardKinds m_eRewardKinds;
	public ETutorialKinds m_eTutorialKinds;

	public List<int> m_oRecordList;
	public Dictionary<ETargetKinds, int> m_oNumTargetsDict;
	public Dictionary<ETargetKinds, int> m_oNumUnlockTargetsDict;
}

/** 타입 랩퍼 */
[MessagePackObject]
public struct STTypeWrapper {
	[Key(51)] public List<long> m_oUniqueLevelIDList;

	[Key(161)] public Dictionary<int, Dictionary<int, int>> m_oNumLevelInfosDictContainer;
	[Key(162)] public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> m_oLevelInfoDictContainer;
}
#endregion			// 기본

#region 추가 타입

#endregion			// 추가 타입
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY

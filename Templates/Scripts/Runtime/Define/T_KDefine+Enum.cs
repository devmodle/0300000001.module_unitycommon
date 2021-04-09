using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
/*
Kinds 값 지정 방식
:
- Type 별로 1,000,000 단위로 값 지정
- Kinds Type 별로 10,000 단위로 값 지정
- Sub Kinds Type 별로 100 단위로 값 지정
*/
#region 기본
//! 아이템 타입
public enum EItemType {
	NONE = -1,
	GOODS,
	CONSUMABLE,
	NON_CONSUMABLE,
	MAX_VALUE
}

//! 아이템 종류
public enum EItemKinds {
	NONE = -1,
	GOODS_COIN,

	// 부스터 1,000,000
	CONSUMABLE_BOOSTER = EItemKinds.GOODS_COIN + 1000000,

	// 게임 아이템 1,010,000
	CONSUMABLE_GAME_ITEM = EItemKinds.CONSUMABLE_BOOSTER + 10000,

	// 광고 제거 2,000,000
	NON_CONSUMABLE_REMOVE_ADS = EItemKinds.CONSUMABLE_BOOSTER + 1000000,

	MAX_VALUE
}

//! 보상 타입
public enum ERewardType {
	NONE = -1,
	CLEAR,
	FREE,
	DAILY,
	MAX_VALUE
}

//! 보상 종류
public enum ERewardKinds {
	NONE = -1,
	CLEAR_REWARD,

	// 무료 보상 1,000,000
	FREE_REWARD = ERewardKinds.CLEAR_REWARD + 1000000,

	// 일일 보상 2,000,000
	DAILY_REWARD = ERewardKinds.FREE_REWARD + 1000000,

	MAX_VALUE
}

//! 판매 아이템 타입
public enum ESaleItemType {
	NONE = -1,
	MAX_VALUE
}

//! 판매 아이템 종류
public enum ESaleItemKinds {
	NONE = -1,
	MAX_VALUE
}

//! 가격 타입
public enum EPriceType {
	NONE = -1,
	ADS,
	GOODS,
	PURCHASE,
	MAX_VALUE
}

//! 가격 종류
public enum EPriceKinds {
	NONE = -1,
	ADS_REWARD,

	// 코인 1,000,000
	GOODS_COIN = EPriceKinds.ADS_REWARD + 1000000,
	
	MAX_VALUE
}

//! 보상 수준
public enum ERewardQuality {
	NONE = -1,
	NORM,
	HIGH,
	SPECIAL,
	MAX_VALUE
}

//! 보상 팝업 타입
public enum ERewardPopupType {
	NONE = -1,
	CLEAR,
	FREE,
	DAILY,
	MAX_VALUE
}

//! 튜토리얼 타입
public enum ETutorialType {
	NONE = -1,
	PLAY,
	TOOL_TIP,
	MAX_VALUE
}

//! 튜토리얼 종류
public enum ETutorialKinds {
	NONE = -1,
	MAX_VALUE
}

//! 레벨 모드
public enum ELevelMode {
	NONE = -1,
	EASY,
	NORM,
	HARD,
	MAX_VALUE
}
#endregion			// 기본
#endif			// #if NEVER_USE_THIS

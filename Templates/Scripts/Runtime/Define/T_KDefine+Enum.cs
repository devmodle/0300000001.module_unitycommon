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
	NON_CONSUMABLE_REMOVE_ADS = 2000000,
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
	FREE_REWARD = 1000000,
	DAILY_REWARD = 2000000,
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
	GOODS_COIN = 2000000,
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

//! 레벨 모드
public enum ELevelMode {
	NONE = -1,
	NORM,
	HARD,
	MAX_VALUE
}
#endregion			// 기본
#endif			// #if NEVER_USE_THIS

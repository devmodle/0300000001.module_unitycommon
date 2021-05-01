using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#region 기본
//! 가격 타입
public enum EPriceType {
	NONE = -1,
	ADS,
	GOODS,
	PURCHASE,
	MAX_VAL
}

//! 가격 종류
public enum EPriceKinds {
	NONE = -1,

	// 보상 광고 0 (광고)
	ADS_REWARD,

	// 코인 10,000,000 (재화)
	GOODS_COIN = EPriceKinds.ADS_REWARD + KCDefine.B_UNIT_TYPE_TO_KINDS,
	
	MAX_VAL
}

//! 아이템 타입
public enum EItemType {
	NONE = -1,
	GOODS,
	CONSUMABLE,
	NON_CONSUMABLE,
	MAX_VAL
}

//! 아이템 종류
public enum EItemKinds {
	NONE = -1,

	// 코인 0 (재화)
	GOODS_COIN,

	// 부스터 10,000,000 (소모 아이템)
	CONSUMABLE_BOOSTER = EItemKinds.GOODS_COIN + KCDefine.B_UNIT_TYPE_TO_KINDS,

	// 게임 아이템 10,010,000 (소모 아이템)
	CONSUMABLE_GAME_ITEM = EItemKinds.CONSUMABLE_BOOSTER + KCDefine.B_UNIT_TYPE_TO_KINDS_TYPE,

	// 광고 제거 20,000,000 (비소모 아이템)
	NON_CONSUMABLE_REMOVE_ADS = EItemKinds.CONSUMABLE_BOOSTER + KCDefine.B_UNIT_TYPE_TO_KINDS,

	MAX_VAL
}

//! 판매 아이템 타입
public enum ESaleItemType {
	NONE = -1,
	BOOSTER,
	GAME_ITEM,
	MAX_VAL
}

//! 판매 아이템 종류
public enum ESaleItemKinds {
	NONE = -1,

	// 부스터 0 (부스터 판매 아이템)
	BOOSTER_SALE_ITEM,

	// 게임 아이템 10,000,000 (게임 아이템 판매 아이템)
	GAME_ITEM_SALE_ITEM = ESaleItemKinds.BOOSTER_SALE_ITEM + KCDefine.B_UNIT_TYPE_TO_KINDS,

	MAX_VAL
}

//! 판매 상품 타입
public enum ESaleProductType {
	NONE = -1,
	PKG,
	SINGLE,
	MAX_VAL
}

//! 판매 상품 종류
public enum ESaleProductKinds {
	NONE = -1,

	// 패키지 상품 0 (패키지 상품)
	PKG_PRODUCT,

	// 잔돈 상품 10,000,000 (단일 상품)
	SINGLE_CHANGES = ESaleProductKinds.PKG_PRODUCT + KCDefine.B_UNIT_TYPE_TO_KINDS,

	// 광고 제거 상품 10,010,000 (단일 상품)
	SINGLE_REMOVE_ADS = ESaleProductKinds.SINGLE_CHANGES + KCDefine.B_UNIT_TYPE_TO_KINDS_TYPE,

	MAX_VAL
}

//! 미션 타입
public enum EMissionType {
	NONE = -1,
	FREE,
	DAILY,
	MAX_VAL
}

//! 미션 종류
public enum EMissionKinds {
	NONE = -1,

	// 자유 미션 0 (자유 미션)
	FREE_MISSION,

	// 일일 미션 10,000,000 (일일 미션)
	DAILY_MISSION = EMissionKinds.FREE_MISSION + KCDefine.B_UNIT_TYPE_TO_KINDS,

	MAX_VAL
}

//! 보상 타입
public enum ERewardType {
	NONE = -1,
	FREE,
	DAILY,
	CLEAR,
	MAX_VAL
}

//! 보상 종류
public enum ERewardKinds {
	NONE = -1,

	// 무료 보상 0 (무료 보상)
	FREE_REWARD,

	// 일일 보상 10,000,000 (일일 보상)
	DAILY_REWARD = ERewardKinds.FREE_REWARD + KCDefine.B_UNIT_TYPE_TO_KINDS,

	// 클리어 보상 20,000,000 (클리어 보상)
	CLEAR_REWARD = ERewardKinds.DAILY_REWARD + KCDefine.B_UNIT_TYPE_TO_KINDS,

	MAX_VAL
}

//! 보상 수준
public enum ERewardQuality {
	NONE = -1,
	NORM,
	HIGH,
	SPECIAL,
	MAX_VAL
}

//! 보상 팝업 타입
public enum ERewardPopupType {
	NONE = -1,
	CLEAR,
	FREE,
	DAILY,
	MAX_VAL
}

//! 튜토리얼 타입
public enum ETutorialType {
	NONE = -1,
	PLAY,
	HELP,
	MAX_VAL
}

//! 튜토리얼 종류
public enum ETutorialKinds {
	NONE = -1,
	MAX_VAL
}

//! 레벨 모드
public enum ELevelMode {
	NONE = -1,
	EASY,
	NORM,
	HARD,
	MAX_VAL
}
#endregion			// 기본
#endif			// #if NEVER_USE_THIS

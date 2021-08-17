using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#region 기본
//! 플레이 모드
public enum EPlayMode {
	NONE = -1,
	NORM,
	TUTORIAL,
	TEST,
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

	#region 광고
	// 보상 광고 0
	ADS_REWARD, [HideInInspector] ADS_PRICE = EPriceKinds.ADS_REWARD,
	#endregion			// 광고

	#region 재화
	// 코인 10,000,000
	GOODS_COINS = EPriceKinds.ADS_PRICE + KCDefine.B_UNIT_KINDS_PER_TYPE, [HideInInspector] GOODS_PRICE = EPriceKinds.GOODS_COINS,
	#endregion			// 재화

	#region 결제
	// 결제 20,000,000
	PURCHASE = EPriceKinds.GOODS_PRICE + KCDefine.B_UNIT_KINDS_PER_TYPE, [HideInInspector] PURCHASE_PRICE = EPriceKinds.PURCHASE,
	#endregion			// 결제

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

	#region 재화
	// 코인 0
	GOODS_COINS, [HideInInspector] GOODS_ITEM = EItemKinds.GOODS_COINS,
	#endregion			// 재화

	#region 소모
	// 샘플 10,000,000
	CONSUMABLE_BOOSTER_SAMPLE = EItemKinds.GOODS_ITEM + KCDefine.B_UNIT_KINDS_PER_TYPE, [HideInInspector] CONSUMABLE_BOOSTER = EItemKinds.CONSUMABLE_BOOSTER_SAMPLE,

	// 샘플 10,010,000
	CONSUMABLE_GAME_ITEM_SAMPLE = EItemKinds.CONSUMABLE_BOOSTER + KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE, [HideInInspector] CONSUMABLE_GAME_ITEM = EItemKinds.CONSUMABLE_GAME_ITEM_SAMPLE,
	#endregion			// 소모

	#region 비소모
	// 광고 제거 20,000,000
	NON_CONSUMABLE_REMOVE_ADS = EItemKinds.CONSUMABLE_BOOSTER + KCDefine.B_UNIT_KINDS_PER_TYPE, [HideInInspector] NON_CONSUMABLE_ITEM = EItemKinds.NON_CONSUMABLE_REMOVE_ADS,
	#endregion			// 비소모

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

	#region 부스터
	// 샘플 0
	BOOSTER_SAMPLE, [HideInInspector] BOOSTER_SALE_ITEM = ESaleItemKinds.BOOSTER_SAMPLE,
	#endregion			// 부스터

	#region 게임 아이템
	// 이어하기 10,000,000
	GAME_ITEM_CONTINUE = ESaleItemKinds.BOOSTER_SALE_ITEM + KCDefine.B_UNIT_KINDS_PER_TYPE, [HideInInspector] GAME_ITEM_SALE_ITEM = ESaleItemKinds.GAME_ITEM_CONTINUE,
	#endregion			// 게임 아이템

	MAX_VAL
}

//! 판매 상품 타입
public enum ESaleProductType {
	NONE = -1,
	PKGS,
	SINGLE,
	MAX_VAL
}

//! 판매 상품 종류
public enum ESaleProductKinds {
	NONE = -1,

	#region 패키지
	// 샘플 0
	PKGS_SAMPLE, [HideInInspector] PKGS_SALE_PRODUCT = ESaleProductKinds.PKGS_SAMPLE,
	#endregion			// 패키지

	#region 단일
	// 판매 코인 10,000,000
	SINGLE_SALE_COINS = ESaleProductKinds.PKGS_SALE_PRODUCT + KCDefine.B_UNIT_KINDS_PER_TYPE, [HideInInspector] SINGLE_SALE_PRODUCT = ESaleProductKinds.SINGLE_SALE_COINS,

	// 광고 제거 10,010,000
	SINGLE_REMOVE_ADS = ESaleProductKinds.SINGLE_SALE_COINS + KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE
	#endregion			// 단일

	MAX_VAL
}

//! 미션 타입
public enum EMissionType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	MAX_VAL
}

//! 미션 종류
public enum EMissionKinds {
	NONE = -1,

	#region 자유
	// 샘플 0
	FREE_SAMPLE, [HideInInspector] FREE_MISSION = EMissionKinds.FREE_SAMPLE,
	#endregion			// 자유

	#region 일일
	// 샘플 10,000,000
	DAILY_SAMPLE = EMissionKinds.FREE_MISSION + KCDefine.B_UNIT_KINDS_PER_TYPE, [HideInInspector] DAILY_MISSION = EMissionKinds.DAILY_SAMPLE,
	#endregion			// 일일

	#region 이벤트
	// 샘플 20,000,000
	EVENT_SAMPLE = EMissionKinds.DAILY_MISSION + KCDefine.B_UNIT_KINDS_PER_TYPE, [HideInInspector] EVENT_MISSION = EMissionKinds.EVENT_SAMPLE,
	#endregion			// 이벤트

	MAX_VAL
}

//! 보상 타입
public enum ERewardType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	CLEAR,
	MAX_VAL
}

//! 보상 종류
public enum ERewardKinds {
	NONE = -1,

	#region 무료
	// 샘플 0
	FREE_SAMPLE, [HideInInspector] FREE_REWARD = ERewardKinds.FREE_SAMPLE,
	#endregion			// 무료

	#region 일일
	// 샘플 10,000,000
	DAILY_SAMPLE = ERewardKinds.FREE_REWARD + KCDefine.B_UNIT_KINDS_PER_TYPE, [HideInInspector] DAILY_REWARD = ERewardKinds.DAILY_SAMPLE,
	#endregion			// 일일

	#region 이벤트
	// 샘플 20,000,000
	EVENT_SAMPLE = ERewardKinds.DAILY_REWARD + KCDefine.B_UNIT_KINDS_PER_TYPE, [HideInInspector] EVENT_REWARD = ERewardKinds.EVENT_SAMPLE,
	#endregion			// 이벤트

	#region 클리어
	// 샘플 30,000,000
	CLEAR_SAMPLE = ERewardKinds.EVENT_REWARD + KCDefine.B_UNIT_KINDS_PER_TYPE, [HideInInspector] CLEAR_REWARD = ERewardKinds.CLEAR_SAMPLE,
	#endregion			// 클리어

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

//! 보상 획득 팝업 타입
public enum ERewardAcquirePopupType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	CLEAR,
	MAX_VAL
}

//! 레벨 타입
public enum ELevelType {
	NONE = -1,
	NORM,
	TUTORIAL,
	MAX_VAL
}

//! 레벨 종류
public enum ELevelKinds {
	NONE = -1,

	#region 일반
	// 샘플 0
	NORM_SAMPLE, [HideInInspector] NORM_LEVEL = ELevelKinds.NORM_SAMPLE,
	#endregion			// 일반

	#region 튜토리얼
	// 샘플 10,000,000
	TUTORIAL_SAMPLE = ELevelKinds.NORM_LEVEL + KCDefine.B_UNIT_KINDS_PER_TYPE, [HideInInspector] NORM_TUTORIAL = ELevelKinds.TUTORIAL_SAMPLE,
	#endregion			// 튜토리얼

	MAX_VAL
}

//! 스테이지 타입
public enum EStageType {
	NONE = -1,
	NORM,
	MAX_VAL
}

//! 스테이지 타입
public enum EStageKinds {
	NONE = -1,

	#region 일반
	// 샘플 0
	NORM_SAMPLE, [HideInInspector] NORM_STAGE = EStageKinds.NORM_SAMPLE,
	#endregion			// 일반

	MAX_VAL
}

//! 챕터 타입
public enum EChapterType {
	NONE = -1,
	NORM,
	MAX_VAL
}

//! 챕터 타입
public enum EChapterKinds {
	NONE = -1,

	#region 일반
	// 샘플 0
	NORM_SAMPLE, [HideInInspector] NORM_CHAPTER = EChapterKinds.NORM_SAMPLE,
	#endregion			// 일반

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

	#region 플레이
	// 샘플 0
	PLAY_SAMPLE, [HideInInspector] PLAY_TUTORIAL = ETutorialKinds.PLAY_SAMPLE,
	#endregion			// 플레이

	#region 도움말
	// 샘플 10,000,000
	HELP_SAMPLE = ETutorialKinds.PLAY_TUTORIAL + KCDefine.B_UNIT_KINDS_PER_TYPE, [HideInInspector] HELP_TUTORIAL = ETutorialKinds.HELP_SAMPLE,
	#endregion			// 도움말

	MAX_VAL
}
#endregion			// 기본
#endif			// #if NEVER_USE_THIS

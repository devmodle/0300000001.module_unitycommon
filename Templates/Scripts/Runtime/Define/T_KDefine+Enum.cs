using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if RUNTIME_TEMPLATES_MODULE_ENABLE
#region 기본
/** 플레이 모드 */
public enum EPlayMode {
	NONE = -1,
	NORM,
	TUTORIAL,
	TEST,
	[HideInInspector] MAX_VAL
}

/** 레벨 모드 */
public enum ELevelMode {
	NONE = -1,
	EASY,
	NORM,
	HARD,
	VERY_HARD,
	SUPER_HARD,
	[HideInInspector] MAX_VAL
}

/** 가격 타입 */
public enum EPriceType {
	NONE = -1,
	ADS,
	GOODS,
	PURCHASE,
	[HideInInspector] MAX_VAL
}

/** 가격 종류 */
public enum EPriceKinds {
	NONE = -1,

	#region 광고
	// 보상 광고 0
	ADS_REWARD,
	#endregion			// 광고

	#region 재화
	// 코인 10,000,000
	GOODS_COINS = EPriceKinds.ADS_REWARD + (KCDefine.B_UNIT_KINDS_PER_TYPE * 1) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),
	#endregion			// 재화

	#region 결제
	// 결제 20,000,000
	PURCHASE = EPriceKinds.ADS_REWARD + (KCDefine.B_UNIT_KINDS_PER_TYPE * 2) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),
	#endregion			// 결제

	[HideInInspector] MAX_VAL
}

/** 아이템 타입 */
public enum EItemType {
	NONE = -1,
	GOODS,
	CONSUMABLE,
	NON_CONSUMABLE,
	[HideInInspector] MAX_VAL
}

/** 아이템 종류 */
public enum EItemKinds {
	NONE = -1,

	#region 재화
	// 코인 0
	GOODS_COINS,
	#endregion			// 재화

	#region 소모
	// 부스터 10,000,000
	[HideInInspector] CONSUMABLE_BOOSTER = EItemKinds.GOODS_COINS + (KCDefine.B_UNIT_KINDS_PER_TYPE * 1) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),

	// 샘플 10,000,100
	CONSUMABLE_BOOSTER_SAMPLE = EItemKinds.CONSUMABLE_BOOSTER + (KCDefine.B_UNIT_KINDS_PER_SUB_KINDS_TYPE * 1),

	// 게임 아이템 10,010,000
	[HideInInspector] CONSUMABLE_GAME_ITEM = EItemKinds.GOODS_COINS + (KCDefine.B_UNIT_KINDS_PER_TYPE * 1) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 1),

	// 샘플 10,010,100
	CONSUMABLE_GAME_ITEM_SAMPLE = EItemKinds.CONSUMABLE_GAME_ITEM + (KCDefine.B_UNIT_KINDS_PER_SUB_KINDS_TYPE * 1),
	#endregion			// 소모

	#region 비소모
	// 광고 제거 20,000,000
	NON_CONSUMABLE_REMOVE_ADS = EItemKinds.GOODS_COINS + (KCDefine.B_UNIT_KINDS_PER_TYPE * 2) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),
	#endregion			// 비소모

	[HideInInspector] MAX_VAL
}

/** 판매 아이템 타입 */
public enum ESaleItemType {
	NONE = -1,
	GOODS,
	CONSUMABLE,
	NON_CONSUMABLE,
	[HideInInspector] MAX_VAL
}

/** 판매 아이템 종류 */
public enum ESaleItemKinds {
	NONE = -1,

	#region 재화
	// 샘플 0
	GOODS_SALE_ITEM_SAMPLE,
	#endregion			// 재화

	#region 소모
	// 힌트 10,000,000
	CONSUMABLE_HINT = ESaleItemKinds.GOODS_SALE_ITEM_SAMPLE + (KCDefine.B_UNIT_KINDS_PER_TYPE * 1) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),

	// 이어하기 10,010,000
	CONSUMABLE_CONTINUE = ESaleItemKinds.GOODS_SALE_ITEM_SAMPLE + (KCDefine.B_UNIT_KINDS_PER_TYPE * 1) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 1),
	#endregion			// 소모

	#region 비소모

	#endregion			// 비소모

	[HideInInspector] MAX_VAL
}

/** 판매 상품 타입 */
public enum ESaleProductType {
	NONE = -1,
	PKGS,
	SINGLE,
	[HideInInspector] MAX_VAL
}

/** 판매 상품 종류 */
public enum ESaleProductKinds {
	NONE = -1,

	#region 패키지
	// 초보자 0
	PKGS_START,

	// 숙련자 10,000
	PKGS_EXPERT = ESaleProductKinds.PKGS_START + (KCDefine.B_UNIT_KINDS_PER_TYPE * 1) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),

	// 전문가 20,000
	PKGS_PRO = ESaleProductKinds.PKGS_START + (KCDefine.B_UNIT_KINDS_PER_TYPE * 1) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 1),
	#endregion			// 패키지

	#region 단일
	// 판매 코인 10,000,000
	SINGLE_SALE_COINS = ESaleProductKinds.PKGS_START + (KCDefine.B_UNIT_KINDS_PER_TYPE * 2) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),

	// 광고 제거 10,010,000
	SINGLE_REMOVE_ADS = ESaleProductKinds.PKGS_START + (KCDefine.B_UNIT_KINDS_PER_TYPE * 2) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 1),
	#endregion			// 단일

	[HideInInspector] MAX_VAL
}

/** 미션 타입 */
public enum EMissionType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	[HideInInspector] MAX_VAL
}

/** 미션 종류 */
public enum EMissionKinds {
	NONE = -1,

	#region 자유
	// 샘플 0
	FREE_MISSION_SAMPLE,
	#endregion			// 자유

	#region 일일
	// 샘플 10,000,000
	DAILY_MISSION_SAMPLE = EMissionKinds.FREE_MISSION_SAMPLE + (KCDefine.B_UNIT_KINDS_PER_TYPE * 1) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),
	#endregion			// 일일

	#region 이벤트
	// 샘플 20,000,000
	EVENT_MISSION_SAMPLE = EMissionKinds.FREE_MISSION_SAMPLE + (KCDefine.B_UNIT_KINDS_PER_TYPE * 2) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),
	#endregion			// 이벤트

	[HideInInspector] MAX_VAL
}

/** 보상 타입 */
public enum ERewardType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	CLEAR,
	MISSION,
	TUTORIAL,
	[HideInInspector] MAX_VAL
}

/** 보상 종류 */
public enum ERewardKinds {
	NONE = -1,

	#region 무료
	// 코인 0
	FREE_COINS,
	#endregion			// 무료

	#region 일일
	// 샘플 10,000,000
	DAILY_REWARD_SAMPLE = ERewardKinds.FREE_COINS + (KCDefine.B_UNIT_KINDS_PER_TYPE * 1) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),
	#endregion			// 일일

	#region 이벤트
	// 샘플 20,000,000
	EVENT_REWARD_SAMPLE = ERewardKinds.FREE_COINS + (KCDefine.B_UNIT_KINDS_PER_TYPE * 2) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),
	#endregion			// 이벤트

	#region 클리어
	// 샘플 30,000,000
	CLEAR_REWARD_SAMPLE = ERewardKinds.FREE_COINS + (KCDefine.B_UNIT_KINDS_PER_TYPE * 3) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),
	#endregion			// 클리어

	#region 미션
	// 샘플 40,000,000
	MISSION_REWARD_SAMPLE = ERewardKinds.FREE_COINS + (KCDefine.B_UNIT_KINDS_PER_TYPE * 4) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),
	#endregion			// 미션

	#region 튜토리얼
	// 샘플 50,000,000
	TUTORIAL_REWARD_SAMPLE = ERewardKinds.FREE_COINS + (KCDefine.B_UNIT_KINDS_PER_TYPE * 5) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),
	#endregion			// 튜토리얼

	[HideInInspector] MAX_VAL
}

/** 보상 수준 */
public enum ERewardQuality {
	NONE = -1,
	NORM,
	HIGH,
	ULTRA,
	[HideInInspector] MAX_VAL
}

/** 보상 획득 팝업 타입 */
public enum ERewardAcquirePopupType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	CLEAR,
	[HideInInspector] MAX_VAL
}

/** 레벨 타입 */
public enum ELevelType {
	NONE = -1,
	NORM,
	TUTORIAL,
	[HideInInspector] MAX_VAL
}

/** 레벨 종류 */
public enum ELevelKinds {
	NONE = -1,

	#region 일반
	// 샘플 0
	NORM_LEVEL_SAMPLE,
	#endregion			// 일반

	#region 튜토리얼
	// 샘플 10,000,000
	TUTORIAL_LEVEL_SAMPLE = ELevelKinds.NORM_LEVEL_SAMPLE + (KCDefine.B_UNIT_KINDS_PER_TYPE * 1) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),
	#endregion			// 튜토리얼

	[HideInInspector] MAX_VAL
}

/** 스테이지 타입 */
public enum EStageType {
	NONE = -1,
	NORM,
	[HideInInspector] MAX_VAL
}

/** 스테이지 타입 */
public enum EStageKinds {
	NONE = -1,

	#region 일반
	// 샘플 0
	NORM_STAGE_SAMPLE,
	#endregion			// 일반

	[HideInInspector] MAX_VAL
}

/** 챕터 타입 */
public enum EChapterType {
	NONE = -1,
	NORM,
	[HideInInspector] MAX_VAL
}

/** 챕터 타입 */
public enum EChapterKinds {
	NONE = -1,

	#region 일반
	// 샘플 0
	NORM_CHAPTER_SAMPLE,
	#endregion			// 일반

	[HideInInspector] MAX_VAL
}

/** 튜토리얼 타입 */
public enum ETutorialType {
	NONE = -1,
	PLAY,
	HELP,
	[HideInInspector] MAX_VAL
}

/** 튜토리얼 종류 */
public enum ETutorialKinds {
	NONE = -1,

	#region 플레이
	// 샘플 0
	PLAY_TUTORIAL_SAMPLE,
	#endregion			// 플레이

	#region 도움말
	// 샘플 10,000,000
	HELP_TUTORIAL_SAMPLE = ETutorialKinds.PLAY_TUTORIAL_SAMPLE + (KCDefine.B_UNIT_KINDS_PER_TYPE * 1) + (KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE * 0),
	#endregion			// 도움말

	[HideInInspector] MAX_VAL
}

/** 타겟 타입 */
public enum ETargetType {
	NONE = -1,
	[HideInInspector] MAX_VAL
}

/** 타겟 종류 */
public enum ETargetKinds {
	NONE = -1,
	[HideInInspector] MAX_VAL
}
#endregion			// 기본

#region 추가 상수

#endregion			// 추가 상수
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS

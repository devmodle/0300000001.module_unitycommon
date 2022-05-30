using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#region 기본
/** 플레이 모드 */
public enum EPlayMode {
	NONE = -1,
	NORM,
	TUTORIAL,
	TEST,
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
	// 0
	ADS_REWARD = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 광고

	#region 재화
	// 10,000,000
	GOODS_COINS = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 재화

	#region 결제
	// 20,000,000
	IN_APP_PURCHASE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	// 0
	GOODS_COINS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 재화

	#region 소모
	// 10,000,000
	CONSUMABLE_BOOSTER_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 10,010,000
	CONSUMABLE_GAME_ITEM_HINT = EItemKinds.CONSUMABLE_BOOSTER_SAMPLE + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 10,010,100
	CONSUMABLE_GAME_ITEM_CONTINUE = EItemKinds.CONSUMABLE_BOOSTER_SAMPLE + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 1),
	#endregion			// 소모

	#region 비소모
	// 20,000,000
	NON_CONSUMABLE_REMOVE_ADS = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 비소모

	[HideInInspector] MAX_VAL
}

/** 아이템 판매 타입 */
public enum EItemSaleType {
	NONE = -1,
	GOODS,
	CONSUMABLE,
	NON_CONSUMABLE,
	[HideInInspector] MAX_VAL
}

/** 아이템 판매 종류 */
public enum EItemSaleKinds {
	NONE = -1,

	#region 재화
	// 0
	GOODS_ITEM_SALE_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 재화

	#region 소모
	// 10,000,000
	CONSUMABLE_BOOSTER_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 10,010,000
	CONSUMABLE_GAME_ITEM_HINT = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 10,010,100
	CONSUMABLE_GAME_ITEM_CONTINUE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 1),
	#endregion			// 소모

	#region 비소모
	// 20,000,000
	NON_CONSUMABLE_ITEM_SALE_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 비소모

	[HideInInspector] MAX_VAL
}

/** 상품 판매 타입 */
public enum EProductSaleType {
	NONE = -1,
	PKGS,
	SINGLE,
	[HideInInspector] MAX_VAL
}

/** 상품 판매 종류 */
public enum EProductSaleKinds {
	NONE = -1,

	#region 패키지
	// 0
	PKGS_SPECIAL_BEGINNER = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100
	PKGS_SPECIAL_EXPERT = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),

	// 200
	PKGS_SPECIAL_PRO = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),
	#endregion			// 패키지

	#region 단일
	// 10,000,000
	SINGLE_COINS_BOX = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 10,010,000
	SINGLE_REMOVE_ADS = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	// 0
	FREE_MISSION_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 자유

	#region 일일
	// 10,000,000
	DAILY_MISSION_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 일일

	#region 이벤트
	// 20,000,000
	EVENT_MISSION_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	// 0
	FREE_COINS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 무료

	#region 일일
	// 10,000,000
	DAILY_REWARD_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 일일

	#region 이벤트
	// 20,000,000
	EVENT_REWARD_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 이벤트

	#region 클리어
	// 30,000,000
	CLEAR_REWARD_SAMPLE = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 클리어

	#region 미션
	// 40,000,000
	MISSION_REWARD_SAMPLE = (EEnumVal.TYPE * 4) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 미션

	#region 튜토리얼
	// 50,000,000
	TUTORIAL_REWARD_SAMPLE = (EEnumVal.TYPE * 5) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	// 0
	NORM_LEVEL_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 일반

	#region 튜토리얼
	// 10,000,000
	TUTORIAL_LEVEL_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	// 0
	NORM_STAGE_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	// 0
	NORM_CHAPTER_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	// 0
	PLAY_TUTORIAL_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 플레이

	#region 도움말
	// 10,000,000
	HELP_TUTORIAL_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 도움말

	[HideInInspector] MAX_VAL
}

/** 타겟 타입 */
public enum ETargetType {
	NONE = -1,
	OBJ,
	RECORD,
	[HideInInspector] MAX_VAL
}

/** 타겟 종류 */
public enum ETargetKinds {
	NONE = -1,

	#region 객체
	// 0
	OBJ_TARGET_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 객체

	#region 기록
	// 10,000,000
	RECORD_MARK = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 기록

	[HideInInspector] MAX_VAL
}

/** 효과 타입 */
public enum EFXType {
	NONE = -1,
	[HideInInspector] MAX_VAL
}

/** 효과 종류 */
public enum EFXKinds {
	NONE = -1,
	[HideInInspector] MAX_VAL
}

/** 스킬 타입 */
public enum ESkillType {
	NONE = -1,
	[HideInInspector] MAX_VAL
}

/** 스킬 종류 */
public enum ESkillKinds {
	NONE = -1,
	[HideInInspector] MAX_VAL
}

/** 객체 타입 */
public enum EObjType {
	NONE = -1,
	BG,
	NORM,
	OVERLAY,
	[HideInInspector] MAX_VAL
}

/** 객체 종류 */
public enum EObjKinds {
	NONE = -1,

	#region 배경
	// 0
	BG_EMPTY = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 배경

	#region 일반
	// 10,000,000
	NORM_OBJ_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 일반

	#region 중첩
	// 20,000,000
	OVERLAY_OBJ_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 중첩

	[HideInInspector] MAX_VAL
}

/** 리소스 타입 */
public enum EResType {
	NONE = -1,
	SND,
	FONT,
	SPRITE,
	TEXTURE,
	[HideInInspector] MAX_VAL
}

/** 리소스 종류 */
public enum EResKinds {
	NONE = -1,

	#region 사운드
	// 0
	SND_BG_SCENE_TITLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100
	SND_BG_SCENE_MAIN = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),

	// 200
	SND_BG_SCENE_GAME = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),

	// 10,000
	SND_FX_TOUCH_BEGIN = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 10,100
	SND_FX_TOUCH_END = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 1),

	// 20,000 
	SND_FX_POPUP_SHOW = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 20,100
	SND_FX_POPUP_CLOSE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 1),
	#endregion			// 사운드

	#region 폰트
	// 10,000,000
	FONT_KOREAN = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 10,010,000
	FONT_ENGLISH = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 폰트

	#region 스프라이트
	// 20,000,000
	[HideInInspector] SPRITE_DEF = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	SPRITE_WHITE,
	SPRITE_SPLASH,
	SPRITE_INDICATOR,
	#endregion			// 스프라이트

	#region 텍스처
	// 30,000,000
	[HideInInspector] TEXTURE_DEF = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	TEXTURE_WHITE,
	TEXTURE_SPLASH,
	TEXTURE_INDICATOR,
	#endregion			// 텍스처

	[HideInInspector] MAX_VAL
}
#endregion			// 기본
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY

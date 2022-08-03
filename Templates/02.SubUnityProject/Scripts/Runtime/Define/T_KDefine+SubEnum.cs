#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE
/** 서브 열거형 값 */
public static partial class KEnumVal {
	#region 기본
	// 타겟 종류
	public const int LV_TARGET_SUB_KINDS_TYPE_VAL = 1;
	public const int EXP_TARGET_SUB_KINDS_TYPE_VAL = 2;
	public const int NUMS_TARGET_SUB_KINDS_TYPE_VAL = 3;
	public const int ENHANCE_TARGET_SUB_KINDS_TYPE_VAL = 4;
	#endregion			// 기본
}

#region 기본
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

/** 수식 타입 */
public enum ECalcType {
	NONE = -1,
	ABILITY,
	[HideInInspector] MAX_VAL
}

/** 수식 종류 */
public enum ECalcKinds {
	NONE = -1,

	#region 어빌리티
	// 0
	ABILITY_CALC_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 어빌리티

	[HideInInspector] MAX_VAL
}

/** 미션 타입 */
public enum EMissionType {
	NONE = -1,
	MAIN,
	FREE,
	DAILY,
	EVENT,
	[HideInInspector] MAX_VAL
}

/** 미션 종류 */
public enum EMissionKinds {
	NONE = -1,

	#region 메인
	// 0
	MAIN_MISSION_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 메인

	#region 자유
	// 100,000,000
	FREE_MISSION_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 자유

	#region 일일
	// 200,000,000
	DAILY_MISSION_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 일일

	#region 이벤트
	// 300,000,000
	EVENT_MISSION_SAMPLE = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	// 100,000,000
	DAILY_REWARD_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 일일

	#region 이벤트
	// 200,000,000
	EVENT_REWARD_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 이벤트

	#region 클리어
	// 300,000,000
	CLEAR_REWARD_SAMPLE = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 클리어

	#region 미션
	// 400,000,000
	MISSION_REWARD_SAMPLE = (EEnumVal.TYPE * 4) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 미션

	#region 튜토리얼
	// 50,000,000
	TUTORIAL_REWARD_SAMPLE = (EEnumVal.TYPE * 5) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 튜토리얼

	[HideInInspector] MAX_VAL
}

/** 에피소드 타입 */
public enum EEpisodeType {
	NONE = -1,
	LEVEL,
	STAGE,
	CHAPTER,
	[HideInInspector] MAX_VAL
}

/** 에피소드 종류 */
public enum EEpisodeKinds {
	NONE = -1,

	#region 레벨
	// 0
	LEVEL_NORM = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	LEVEL_NORM_BOSS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),

	// 100,000
	LEVEL_TUTORIAL = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	LEVEL_TUTORIAL_BOSS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 1),
	#endregion			// 레벨

	#region 스테이지
	// 100,000,000
	STAGE_NORM = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,100,000
	STAGE_TUTORIAL = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 스테이지

	#region 챕터
	// 200,000,000
	CHAPTER_NORM = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 200,100,000
	CHAPTER_TUTORIAL = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 챕터

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
	// 100,000,000
	HELP_TUTORIAL_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 도움말

	[HideInInspector] MAX_VAL
}

/** 리소스 타입 */
public enum EResType {
	NONE = -1,
	SND,
	FONT,
	IMG,
	SPRITE,
	TEXTURE,
	[HideInInspector] MAX_VAL
}

/** 리소스 종류 */
public enum EResKinds {
	NONE = -1,

	#region 사운드
	// 0
	SND_BG_SCENE_TITLE_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	SND_BG_SCENE_MAIN_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	SND_BG_SCENE_GAME_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),

	// 100,000
	SND_FX_TOUCH_BEGIN_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	SND_FX_TOUCH_END_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 1),

	// 200,000
	SND_FX_POPUP_SHOW_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 0),
	SND_FX_POPUP_CLOSE_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 1),
	#endregion			// 사운드

	#region 폰트
	// 100,000,000
	FONT_KOREAN_01 = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	FONT_ENGLISH_01 = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	#endregion           // 폰트

	#region 이미지
	// 200,000,000
	IMG_WHITE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	IMG_SPLASH,
	IMG_INDICATOR,
	#endregion			// 이미지

	#region 스프라이트
	// 300,000,000
	SPRITE_WHITE = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	SPRITE_SPLASH,
	SPRITE_INDICATOR,
	#endregion         // 스프라이트

	#region 텍스처
	// 400,000,000
	TEXTURE_RES_SAMPLE = (EEnumVal.TYPE * 4) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 텍스처

	[HideInInspector] MAX_VAL
}

/** 아이템 타입 */
public enum EItemType {
	NONE = -1,
	GOODS,
	CONSUMABLE,
	NON_CONSUMABLE,
	WEAPON,
	ARMOR,
	ACCESSORY,
	ATTACH,
	[HideInInspector] MAX_VAL
}

/** 아이템 종류 */
public enum EItemKinds {
	NONE = -1,

	#region 재화
	// 0
	GOODS_COINS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,000
	GOODS_COINS_BOX_COINS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 재화

	#region 소모
	// 100,000,000
	CONSUMABLE_BOOSTER_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,100,000
	CONSUMABLE_GAME_ITEM_HINT = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	CONSUMABLE_GAME_ITEM_CONTINUE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 1),
	CONSUMABLE_GAME_ITEM_SHUFFLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 2),
	#endregion			// 소모

	#region 비소모
	// 200,000,000
	NON_CONSUMABLE_REMOVE_ADS = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 비소모

	#region 무기
	// 300,000,000
	WEAPON_ITEM_SAMPLE = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 무기

	#region 방어구
	// 400,000,000
	ARMOR_ITEM_SAMPLE = (EEnumVal.TYPE * 4) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 방어구

	#region 악세서리
	// 50,000,000
	ACCESSORY_ITEM_SAMPLE = (EEnumVal.TYPE * 5) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 악세서리

	#region 장식
	// 60,000,000
	ATTACH_ITEM_SAMPLE = (EEnumVal.TYPE * 6) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 장식

	[HideInInspector] MAX_VAL
}

/** 스킬 타입 */
public enum ESkillType {
	NONE = -1,
	ACTION,
	ACTIVE,
	PASSIVE,
	[HideInInspector] MAX_VAL
}

/** 스킬 종류 */
public enum ESkillKinds {
	NONE = -1,

	#region 액션
	// 0
	ACTION_ATK_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 액션

	#region 액티브
	// 100,000,000
	ACTIVE_SKILL_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 액티브

	#region 패시브
	// 200,000,000
	PASSIVE_SKILL_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 패시브

	[HideInInspector] MAX_VAL
}

/** 객체 타입 */
public enum EObjType {
	NONE = -1,
	BG,
	NORM,
	OVERLAY,
	PLAYABLE,
	NON_PLAYABLE,
	ENEMY,
	[HideInInspector] MAX_VAL
}

/** 객체 종류 */
public enum EObjKinds {
	NONE = -1,

	#region 배경
	// 0
	BG_EMPTY_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 배경

	#region 일반
	// 100,000,000
	NORM_OBJ_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 일반

	#region 중첩
	// 200,000,000
	OVERLAY_OBJ_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 중첩

	#region 플레이 가능
	// 300,000,000
	PLAYABLE_COMMON_CHARACTER_01 = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 플레이 가능

	#region 플레이 불가능
	// 400,000,000
	NON_PLAYABLE_OBJ_SAMPLE = (EEnumVal.TYPE * 4) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 플레이 불가능

	#region 적
	// 500,000,000
	ENEMY_OBJ_SAMPLE = (EEnumVal.TYPE * 5) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 적

	[HideInInspector] MAX_VAL
}

/** 효과 타입 */
public enum EFXType {
	NONE = -1,
	HIT,
	BUFF,
	DEBUFF,
	[HideInInspector] MAX_VAL
}

/** 효과 종류 */
public enum EFXKinds {
	NONE = -1,

	#region 타격
	// 0
	HIT_FX_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 타격

	#region 버프
	// 100,000,000
	BUFF_FX_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 버프

	#region 디버프
	// 200,000,000
	DEBUFF_FX_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 디버프

	[HideInInspector] MAX_VAL
}

/** 어빌리티 타입 */
public enum EAbilityType {
	NONE = -1,
	STAT,
	BUFF,
	DEBUFF,
	UPGRADE,
	[HideInInspector] MAX_VAL
}

/** 어빌리티 종류 */
public enum EAbilityKinds {
	NONE = -1,

	#region 스탯
	// 0
	STAT_LV = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	STAT_EXP = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	STAT_NUMS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),
	STAT_ENHANCE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 3),

	// 100,000 {
	STAT_HP_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	STAT_HP_02,

	STAT_MP_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 1),
	STAT_MP_02,

	STAT_SP_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 2),
	STAT_SP_02,
	// 100,000 }

	// 200,000 {
	STAT_RECOVERY_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 0),
	STAT_RECOVERY_02,

	STAT_HP_RECOVERY_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 1),
	STAT_HP_RECOVERY_02,

	STAT_MP_RECOVERY_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 2),
	STAT_MP_RECOVERY_02,

	STAT_SP_RECOVERY_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 3),
	STAT_SP_RECOVERY_02,
	// 200,000 }

	// 300,000 {
	STAT_ATK_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 3) + (EEnumVal.SUB_KINDS_TYPE * 0),
	STAT_ATK_02,

	STAT_P_ATK_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 3) + (EEnumVal.SUB_KINDS_TYPE * 1),
	STAT_P_ATK_02,

	STAT_M_ATK_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 3) + (EEnumVal.SUB_KINDS_TYPE * 2),
	STAT_M_ATK_02,
	// 300,000 }

	// 400,000 {
	STAT_DEF_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 4) + (EEnumVal.SUB_KINDS_TYPE * 0),
	STAT_DEF_02,

	STAT_P_DEF_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 4) + (EEnumVal.SUB_KINDS_TYPE * 1),
	STAT_P_DEF_02,

	STAT_M_DEF_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 4) + (EEnumVal.SUB_KINDS_TYPE * 2),
	STAT_M_DEF_02,
	// 400,000 }

	// 500,000 {
	STAT_RANGE_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 5) + (EEnumVal.SUB_KINDS_TYPE * 0),
	STAT_RANGE_02,

	STAT_VIEW_RANGE_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 5) + (EEnumVal.SUB_KINDS_TYPE * 1),
	STAT_VIEW_RANGE_02,

	STAT_ATK_RANGE_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 5) + (EEnumVal.SUB_KINDS_TYPE * 2),
	STAT_ATK_RANGE_02,
	// 500,000 }

	// 600,000 {
	STAT_SPEED_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 6) + (EEnumVal.SUB_KINDS_TYPE * 0),
	STAT_SPEED_02,

	STAT_ATK_SPEED_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 6) + (EEnumVal.SUB_KINDS_TYPE * 1),
	STAT_ATK_SPEED_02,

	STAT_MOVE_SPEED_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 6) + (EEnumVal.SUB_KINDS_TYPE * 2),
	STAT_MOVE_SPEED_02,
	// 600,000 }

	// 700,000 {
	STAT_RATE_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 7) + (EEnumVal.SUB_KINDS_TYPE * 0),
	STAT_RATE_02,

	STAT_HIT_RATE_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 7) + (EEnumVal.SUB_KINDS_TYPE * 1),
	STAT_HIT_RATE_02,

	STAT_AVOID_RATE_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 7) + (EEnumVal.SUB_KINDS_TYPE * 2),
	STAT_AVOID_RATE_02,

	STAT_CRITICAL_RATE_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 7) + (EEnumVal.SUB_KINDS_TYPE * 3),
	STAT_CRITICAL_RATE_02,
	// 700,000 }

	// 800,000 {
	STAT_DELAY_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 8) + (EEnumVal.SUB_KINDS_TYPE * 0),
	STAT_DELAY_02,

	STAT_ATK_DELAY_01 = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 8) + (EEnumVal.SUB_KINDS_TYPE * 1),
	STAT_ATK_DELAY_02,
	// 800,000 }
	#endregion			// 스탯

	#region 버프
	// 100,000,000
	BUFF_ABILITY_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 버프

	#region 디버프
	// 200,000,000
	DEBUFF_ABILITY_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 디버프

	#region 업그레이드
	// 300,000,000
	UPGRADE_ABILITY_SAMPLE = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 업그레이드

	[HideInInspector] MAX_VAL
}

/** 상품 타입 */
public enum EProductType {
	NONE = -1,
	PKGS,
	SINGLE,
	[HideInInspector] MAX_VAL
}

/** 상품 종류 */
public enum EProductKinds {
	NONE = -1,

	#region 패키지
	// 0
	PKGS_SPECIAL_BEGINNER = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	PKGS_SPECIAL_EXPERT = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	PKGS_SPECIAL_PRO = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),
	#endregion			// 패키지

	#region 단일
	// 100,000,000
	SINGLE_COINS_BOX = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	SINGLE_REMOVE_ADS = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	#endregion			// 단일

	[HideInInspector] MAX_VAL
}

/** 타겟 타입 */
public enum ETargetType {
	NONE = -1,
	ITEM,
	SKILL,
	OBJ,
	ABILITY,
	[HideInInspector] MAX_VAL
}

/** 타겟 종류 */
public enum ETargetKinds {
	NONE = -1,

	#region 아이템
	// 0
	ITEM = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	ITEM_LV = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	ITEM_EXP = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),
	ITEM_NUMS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 3),
	ITEM_ENHANCE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 4),
	#endregion			// 아이템

	#region 스킬
	// 100,000,000
	SKILL = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	SKILL_LV = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	SKILL_EXP = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),
	SKILL_NUMS = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 3),
	SKILL_ENHANCE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 4),
	#endregion			// 스킬

	#region 객체
	// 200,000,000
	OBJ = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	OBJ_LV = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	OBJ_EXP = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),
	OBJ_NUMS = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 3),
	OBJ_ENHANCE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 4),
	#endregion			// 객체

	#region 어빌리티
	// 300,000,000
	ABILITY = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 어빌리티

	[HideInInspector] MAX_VAL
}

/** 스킬 타겟 타입 */
public enum ESkillTargetType {
	NONE = -1,
	MULTI,
	SINGLE,
	[HideInInspector] MAX_VAL
}

/** 스킬 타겟 종류 */
public enum ESkillTargetKinds {
	NONE = -1,

	#region 다수
	// 0
	MULTI = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 다수

	#region 단일
	// 0
	SINGLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 단일

	[HideInInspector] MAX_VAL
}
#endregion			// 기본
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY

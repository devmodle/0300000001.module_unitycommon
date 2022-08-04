#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 상수 */
	public static partial class KDefine {
		#region 기본
		// 식별자 {
		public const string E_KEY_ITEM_OBJS_POOL = "ItemObjsPool";
		public const string E_KEY_SKILL_OBJS_POOL = "SkillObjsPool";
		public const string E_KEY_OBJ_OBJS_POOL = "ObjObjsPool";
		public const string E_KEY_FX_OBJS_POOL = "FXObjsPool";

		public const string E_KEY_PLAYER_OBJ_OBJS_POOL = "PlayerObjsPool";
		public const string E_KEY_ENEMY_OBJ_OBJS_POOL = "EnemyObjsPool";
		// 식별자 }

		// 이름 {
		public const string E_OBJ_N_ITEM = "ITEM";
		public const string E_OBJ_N_SKILL = "SKILL";
		public const string E_OBJ_N_OBJ = "OBJ";
		public const string E_OBJ_N_FX = "FX";

		public const string E_OBJ_N_PLAYER_OBJ = "PLAYER_OBJ";
		public const string E_OBJ_N_ENEMY_OBJ = "ENEMY_OBJ";
		// 이름 }
		#endregion			// 기본

		#region 런타임 상수
		// 경로 {
		public static readonly string E_OBJ_P_ITEM = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}E_Item";
		public static readonly string E_OBJ_P_SKILL = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}E_Skill";
		public static readonly string E_OBJ_P_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}E_Obj";
		public static readonly string E_OBJ_P_FX = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}E_FX";

		public static readonly string E_OBJ_P_PLAYER_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}E_PlayerObj";
		public static readonly string E_OBJ_P_ENEMY_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}E_EnemyObj";
		// 경로 }
		#endregion			// 런타임 상수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY

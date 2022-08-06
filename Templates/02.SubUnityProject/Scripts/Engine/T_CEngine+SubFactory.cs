#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 - 팩토리 */
	public partial class CEngine : CComponent {
		#region 함수
		
		#endregion			// 함수
	}

	/** 서브 엔진 - 팩토리 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 플레이어 객체를 생성한다 */
		public CEObj CreatePlayerObj(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEComponent a_oOwner = null, bool a_bIsEnableController = false) {
			var oObj = CSceneManager.ActiveSceneManager.SpawnObj<CEObj>(KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL, KDefine.E_OBJ_N_PLAYER_OBJ);
			var oController = a_bIsEnableController ? oObj.gameObject.AddComponent<CEPlayerObjController>() : null;

			oObj.Init(Factory.MakeObjParams(this, a_stObjInfo, a_oObjTargetInfo, a_oOwner, oController, KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL));
			oController.Init(Factory.MakePlayerObjControllerParams(this, oObj));
			
			return oObj;
		}

		/** 적 객체를 생성한다 */
		public CEObj CreateEnemyObj(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEComponent a_oOwner = null, bool a_bIsEnableController = false) {
			var oObj = CSceneManager.ActiveSceneManager.SpawnObj<CEObj>(KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL, KDefine.E_OBJ_N_ENEMY_OBJ);
			var oController = a_bIsEnableController ? oObj.gameObject.AddComponent<CEEnemyObjController>() : null;

			oObj.Init(Factory.MakeObjParams(this, a_stObjInfo, a_oObjTargetInfo, a_oOwner, oController, KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL));
			oController.Init(Factory.MakeEnemyObjControllerParams(this, oObj));

			return oObj;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY

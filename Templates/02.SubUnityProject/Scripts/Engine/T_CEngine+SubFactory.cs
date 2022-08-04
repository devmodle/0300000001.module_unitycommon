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
		public CEObj CreatePlayerObj(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEComponent a_oOwner = null) {
			var oObj = CSceneManager.ActiveSceneManager.SpawnObj<CEObj>(KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL, KDefine.E_OBJ_P_PLAYER_OBJ);
			oObj.Init(Factory.MakeObjParams(this, a_stObjInfo, a_oObjTargetInfo, a_oOwner, KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL));

			return oObj;
		}

		/** 적 객체를 생성한다 */
		public CEObj CreateEnemyObj(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEComponent a_oOwner = null) {
			var oObj = CSceneManager.ActiveSceneManager.SpawnObj<CEObj>(KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL, KDefine.E_OBJ_P_ENEMY_OBJ);
			oObj.Init(Factory.MakeObjParams(this, a_stObjInfo, a_oObjTargetInfo, a_oOwner, KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL));

			return oObj;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY

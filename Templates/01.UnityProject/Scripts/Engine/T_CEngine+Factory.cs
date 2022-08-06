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
		/** 아이템을 생성한다 */
		public CEItem CreateItem(STItemInfo a_stItemInfo, CItemTargetInfo a_oItemTargetInfo, CEComponent a_oOwner = null) {
			var oItem = CSceneManager.ActiveSceneManager.SpawnObj<CEItem>(KDefine.E_KEY_ITEM_OBJS_POOL, KDefine.E_OBJ_N_ITEM);
			oItem.Init(Factory.MakeItemParams(this, a_stItemInfo, a_oItemTargetInfo, a_oOwner, null, KDefine.E_KEY_ITEM_OBJS_POOL));

			return oItem;
		}

		/** 스킬을 생성한다 */
		public CESkill CreateSkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, CEComponent a_oOwner = null, bool a_bIsEnableController = false) {
			var oSkill = CSceneManager.ActiveSceneManager.SpawnObj<CESkill>(KDefine.E_KEY_SKILL_OBJS_POOL, KDefine.E_OBJ_N_SKILL);
			var oController = a_bIsEnableController ? oSkill.gameObject.AddComponent<CESkillController>() : null;

			oSkill.Init(Factory.MakeSkillParams(this, a_stSkillInfo, a_oSkillTargetInfo, a_oOwner, oController, KDefine.E_KEY_SKILL_OBJS_POOL));
			oController.Init(Factory.MakeSkillControllerParams(this, oSkill));

			return oSkill;
		}

		/** 객체를 생성한다 */
		public CEObj CreateObj(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEComponent a_oOwner = null) {
			var oObj = CSceneManager.ActiveSceneManager.SpawnObj<CEObj>(KDefine.E_KEY_OBJ_OBJS_POOL, KDefine.E_OBJ_N_OBJ);
			oObj.Init(Factory.MakeObjParams(this, a_stObjInfo, a_oObjTargetInfo, a_oOwner, null, KDefine.E_KEY_OBJ_OBJS_POOL));

			return oObj;
		}

		/** 효과를 생성한다 */
		public CEFX CreateFX(STFXInfo a_stFXInfo, CEComponent a_oOwner = null, bool a_bIsEnableController = false) {
			var oFX = CSceneManager.ActiveSceneManager.SpawnObj<CEFX>(KDefine.E_KEY_FX_OBJS_POOL, KDefine.E_OBJ_N_FX);
			var oController = a_bIsEnableController ? oFX.gameObject.AddComponent<CEFXController>() : null;

			oFX.Init(Factory.MakeFXParams(this, a_stFXInfo, a_oOwner, oController, KDefine.E_KEY_FX_OBJS_POOL));
			oController.Init(Factory.MakeFXControllerParams(this, oFX));

			return oFX;
		}		
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY

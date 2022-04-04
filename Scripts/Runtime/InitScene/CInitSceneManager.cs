using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

#if UNITY_IOS
using UnityEngine.iOS;
#endif			// #if UNITY_IOS

namespace InitScene {
	/** 초기화 씬 관리자 */
	public abstract partial class CInitSceneManager : CSceneManager {
		#region 클래스 변수
		/** =====> 객체 <===== */
		private static GameObject m_oBlindUIs = null;
		#endregion			// 클래스 변수

		#region 프로퍼티
		public override string SceneName => KCDefine.B_SCENE_N_INIT;

#if UNITY_EDITOR
		public override int ScriptOrder => KCDefine.U_SCRIPT_O_INIT_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public sealed override void Start() {
			base.Start();

			// 초기화가 필요 할 경우
			if(!CSceneManager.IsInit) {
				StartCoroutine(this.OnStart());
			}
		}

		/** 씬을 설정한다 */
		protected virtual void Setup() {
			this.SetupBlindUIs();
			DOTween.SetTweensCapacity(KCDefine.U_SIZE_DOTWEEN_ANI, KCDefine.U_SIZE_DOTWEEN_SEQUENCE_ANI);

			// 테이블을 로드한다
			CValTable.Inst.LoadValsFromRes(KCDefine.U_TABLE_P_G_COMMON_VAL);
			CStrTable.Inst.LoadStrsFromRes(KCDefine.U_TABLE_P_G_COMMON_STR);

			// 저장소를 로드한다 {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			CCommonAppInfoStorage.Inst.LoadAppInfo();
			CCommonUserInfoStorage.Inst.LoadUserInfo();
			CCommonGameInfoStorage.Inst.LoadGameInfo();
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
			// 저장소를 로드한다 }
			
			// 사운드 관리자를 설정한다 {
#if MODE_2D_ENABLE
			CSndManager.Inst.IsIgnoreBGSndEffects = true;
			CSndManager.Inst.IsIgnoreFXSndsEffects = true;

			CSndManager.Inst.IsIgnoreBGSndReverbZones = true;
			CSndManager.Inst.IsIgnoreFXSndsReverbZones = true;

			CSndManager.Inst.IsIgnoreBGSndListenerEffects = true;
			CSndManager.Inst.IsIgnoreFXSndsListenerEffects = true;
#else
			CSndManager.Inst.IsIgnoreBGSndEffects = false;
			CSndManager.Inst.IsIgnoreFXSndsEffects = false;

			CSndManager.Inst.IsIgnoreBGSndReverbZones = false;
			CSndManager.Inst.IsIgnoreFXSndsReverbZones = false;

			CSndManager.Inst.IsIgnoreBGSndListenerEffects = false;
			CSndManager.Inst.IsIgnoreFXSndsListenerEffects = false;
#endif			// #if MODE_2D_ENABLE

#if NEWTON_SOFT_JSON_MODULE_ENABLE
			CSndManager.Inst.BGSndVolume = CCommonGameInfoStorage.Inst.GameInfo.BGSndVolume;
			CSndManager.Inst.FXSndsVolume = CCommonGameInfoStorage.Inst.GameInfo.FXSndsVolume;
			
			CSndManager.Inst.IsMuteBGSnd = CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd;
			CSndManager.Inst.IsMuteFXSnds = CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds;
			CSndManager.Inst.IsDisableVibrate = CCommonGameInfoStorage.Inst.GameInfo.IsDisableVibrate;
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
			// 사운드 관리자를 설정한다 }

#if UNITY_STANDALONE
#if DEBUG || DEVELOPMENT_BUILD
			Screen.SetResolution((int)CAccess.CorrectDesktopScreenSize.x, (int)CAccess.CorrectDesktopScreenSize.y, FullScreenMode.Windowed);
#else
			Screen.SetResolution((int)CAccess.DesktopScreenSize.x, (int)CAccess.DesktopScreenSize.y, FullScreenMode.FullScreenWindow);
#endif			// #if DEBUG || DEVELOPMENT_BUILD
#endif			// #if UNITY_STANDALONE
		}

		/** 초기화 */
		private IEnumerator OnStart() {
			// iOS 를 설정한다 {
#if UNITY_IOS
			Device.hideHomeButton = false;

			Device.SetNoBackupFlag(KCDefine.B_DIR_P_WRITABLE);
			Device.SetNoBackupFlag(KCDefine.U_IMG_P_SCREENSHOT);
#endif			// #if UNITY_IOS
			// iOS 를 설정한다 }

			// 관리자를 생성한다 {
			CSndManager.Create();
			CResManager.Create();
			CTaskManager.Create();
			CScheduleManager.Create();
			CNavStackManager.Create();
			CIndicatorManager.Create();
			CCollectionManager.Create();
			
#if ADS_MODULE_ENABLE
			CAdsManager.Create();
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
			CFlurryManager.Create();
#endif			// #if FLURRY_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
			CFacebookManager.Create();
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
			CFirebaseManager.Create();
#endif			// #if FIREBASE_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
			CGameCenterManager.Create();
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			CPurchaseManager.Create();
#endif			// #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
			CNotiManager.Create();
#endif			// #if NOTI_MODULE_ENABLE
			// 관리자를 생성한다 }

			// 로더를 생성한다
			CSceneLoader.Create();

			// 디바이스 연동 객체를 생성한다
			CUnityMsgSender.Create();
			CDeviceMsgReceiver.Create();

			// 테이블을 생성한다 {
			CValTable.Create();
			CStrTable.Create();

			COptsInfoTable.Create(KCDefine.U_ASSET_P_G_OPTS_INFO_TABLE);
			CBuildInfoTable.Create(KCDefine.U_ASSET_P_G_BUILD_INFO_TABLE);
			CDefineSymbolInfoTable.Create(KCDefine.U_ASSET_P_G_DEFINE_SYMBOL_INFO_TABLE);
			CProjInfoTable.Create(KCDefine.U_ASSET_P_G_PROJ_INFO_TABLE);
			CDeviceInfoTable.Create(KCDefine.U_ASSET_P_G_DEVICE_INFO_TABLE);
			CLocalizeInfoTable.Create(KCDefine.U_ASSET_P_G_LOCALIZE_INFO_TABLE);
			CStorageInfoTable.Create(KCDefine.U_ASSET_P_G_STORAGE_INFO_TABLE);

#if ADS_MODULE_ENABLE || FLURRY_MODULE_ENABLE || FIREBASE_MODULE_ENABLE || APPS_FLYER_MODULE_ENABLE
			CPluginInfoTable.Create(KCDefine.U_ASSET_P_G_PLUGIN_INFO_TABLE);
#endif			// #if ADS_MODULE_ENABLE || FLURRY_MODULE_ENABLE || FIREBASE_MODULE_ENABLE || APPS_FLYER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			CProductInfoTable.Create(KCDefine.U_ASSET_P_G_PRODUCT_INFO_TABLE);
#endif			// #if PURCHASE_MODULE_ENABLE
			// 테이블을 생성한다 }

			// 저장소를 생성한다 {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			CCommonAppInfoStorage.Create();
			CCommonUserInfoStorage.Create();
			CCommonGameInfoStorage.Create();
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
			// 저장소를 생성한다 }

			this.Setup();
			yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);
			
			CSceneManager.IsInit = true;
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_SPLASH, false);
		}

		/** 블라인드 이미지를 생성한다 */
		protected virtual Image CreateBlindImg(string a_oName, GameObject a_oParent) {
			return CFactory.CreateCloneObj<Image>(a_oName, CResManager.Inst.GetRes<GameObject>(KCDefine.IS_OBJ_P_SCREEN_BLIND_IMG), a_oParent);
		}
		#endregion			// 함수
	}
}

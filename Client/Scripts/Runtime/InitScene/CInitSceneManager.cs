using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using DG.Tweening;

#if UNITY_IOS
using UnityEngine.iOS;
#endif // #if UNITY_IOS

namespace InitScene
{
	/** 초기화 씬 관리자 */
	public abstract partial class CInitSceneManager : CSceneManager
	{
		#region 추상
		/** 스플래시를 출력한다 */
		protected abstract void ShowSplash();
		#endregion // 추상

		#region 프로퍼티
		protected List<string> SpriteAtlasPathList { get; } = new List<string>();

#if UNITY_EDITOR
		public override int OrderScript => KCDefine.G_SCRIPT_O_MANAGER_SCENE_INIT;
#endif // #if UNITY_EDITOR
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake()
		{
			base.Awake();

			for(int i = 0; i < KCDefine.U_ASSET_P_SPRITE_ATLAS_LIST.Count; ++i)
			{
				this.SpriteAtlasPathList.ExAddVal(KCDefine.U_ASSET_P_SPRITE_ATLAS_LIST[i]);
			}

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			for(int i = 0; i < KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_LIST.Count; ++i)
			{
				this.SpriteAtlasPathList.ExAddVal(KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_LIST[i]);
			}
#endif // #if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		}

		/** 초기화 */
		public sealed override void Start()
		{
			base.Start();

			for(int i = 0; i < this.SpriteAtlasPathList.Count; ++i)
			{
				CManagerRes.Inst.LoadSpriteAtlas(this.SpriteAtlasPathList[i]);
			}

			StartCoroutine(this.CoStart());
		}

		/** 씬을 설정한다 */
		protected virtual void Setup()
		{
			this.SetupBlindUIs();
			DOTween.SetTweensCapacity(KCDefine.U_SIZE_DOTWEEN_ANI, KCDefine.U_SIZE_DOTWEEN_SEQUENCE_ANI);

			// 테이블을 로드한다 {
			CStrTable.Inst.LoadEnumStrs(typeof(EGridType));
			CStrTable.Inst.LoadEnumStrs(typeof(EDifficulty));

			CValTable.Inst.LoadValsFromRes(KCDefine.U_TABLE_P_G_COMMON_VAL);
			CStrTable.Inst.LoadStrsFromRes(KCDefine.U_TABLE_P_G_COMMON_STR);
			// 테이블을 로드한다 }

			// 저장소를 로드한다
			CStorageInfoAppCommon.Inst.LoadAppInfo();
			CStorageInfoUserCommon.Inst.LoadInfoUser();
			CStorageInfoGameCommon.Inst.LoadInfoGame();

			// 사운드 관리자를 설정한다 {
#if ENABLE_MODE_2D
			CManagerSnd.Inst.SetIsBypassBGSndEffects(true);
			CManagerSnd.Inst.SetIsBypassFXSndsEffects(true);

			CManagerSnd.Inst.SetIsBypassBGSndReverbZones(true);
			CManagerSnd.Inst.SetIsBypassFXSndsReverbZones(true);

			CManagerSnd.Inst.SetIsBypassBGSndListenerEffects(true);
			CManagerSnd.Inst.SetIsBypassFXSndsListenerEffects(true);
#else
			CManagerSnd.Inst.SetIsBypassBGSndEffects(false);
			CManagerSnd.Inst.SetIsBypassFXSndsEffects(false);

			CManagerSnd.Inst.SetIsBypassBGSndReverbZones(false);
			CManagerSnd.Inst.SetIsBypassFXSndsReverbZones(false);

			CManagerSnd.Inst.SetIsBypassBGSndListenerEffects(false);
			CManagerSnd.Inst.SetIsBypassFXSndsListenerEffects(false);
#endif // #if ENABLE_MODE_2D

			CManagerSnd.Inst.SetVolumeSndBG(CStorageInfoGameCommon.Inst.GameInfo.VolumeSndBG);
			CManagerSnd.Inst.SetVolumeSndsFX(CStorageInfoGameCommon.Inst.GameInfo.VolumeSndsFX);

			CManagerSnd.Inst.SetIsMuteSndBG(CStorageInfoGameCommon.Inst.GameInfo.IsMuteSndBG);
			CManagerSnd.Inst.SetIsMuteSndsFX(CStorageInfoGameCommon.Inst.GameInfo.IsMuteSndsFX);
			CManagerSnd.Inst.SetIsEnableVibrate(CStorageInfoGameCommon.Inst.GameInfo.IsEnableVibrate);
			// 사운드 관리자를 설정한다 }

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			Screen.SetResolution((int)CAccess.AdjustDesktopScreenSize.x,
				(int)CAccess.AdjustDesktopScreenSize.y, FullScreenMode.Windowed);
#else
			Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow);
#endif // #if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

			// 디바이스 정보를 설정한다 {
			var oTargetFrameInfoDict = new Dictionary<RuntimePlatform, (long, long)>()
			{
				// 모바일
				[RuntimePlatform.Android] = (CValTable.Inst.GetInt(KCDefine.G_VT_KEY_MOBILE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.G_VT_KEY_MOBILE_TARGET_FRAME_RATE)),
				[RuntimePlatform.IPhonePlayer] = (CValTable.Inst.GetInt(KCDefine.G_VT_KEY_MOBILE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.G_VT_KEY_MOBILE_TARGET_FRAME_RATE)),

				// 데스크 탑 {
				[RuntimePlatform.OSXEditor] = (CValTable.Inst.GetInt(KCDefine.G_VT_KEY_DESKTOP_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.G_VT_KEY_DESKTOP_TARGET_FRAME_RATE)),
				[RuntimePlatform.OSXPlayer] = (CValTable.Inst.GetInt(KCDefine.G_VT_KEY_DESKTOP_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.G_VT_KEY_DESKTOP_TARGET_FRAME_RATE)),

				[RuntimePlatform.WindowsEditor] = (CValTable.Inst.GetInt(KCDefine.G_VT_KEY_DESKTOP_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.G_VT_KEY_DESKTOP_TARGET_FRAME_RATE)),
				[RuntimePlatform.WindowsPlayer] = (CValTable.Inst.GetInt(KCDefine.G_VT_KEY_DESKTOP_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.G_VT_KEY_DESKTOP_TARGET_FRAME_RATE)),
				// 데스크 탑 }

				// 콘솔
				[RuntimePlatform.PS4] = (CValTable.Inst.GetInt(KCDefine.G_VT_KEY_CONSOLE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.G_VT_KEY_CONSOLE_TARGET_FRAME_RATE)),
				[RuntimePlatform.PS5] = (CValTable.Inst.GetInt(KCDefine.G_VT_KEY_CONSOLE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.G_VT_KEY_CONSOLE_TARGET_FRAME_RATE)),
				[RuntimePlatform.XboxOne] = (CValTable.Inst.GetInt(KCDefine.G_VT_KEY_CONSOLE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.G_VT_KEY_CONSOLE_TARGET_FRAME_RATE)),

				// 휴대용 콘솔
				[RuntimePlatform.Switch] = (CValTable.Inst.GetInt(KCDefine.G_VT_KEY_HANDHELD_CONSOLE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.G_VT_KEY_HANDHELD_CONSOLE_TARGET_FRAME_RATE))
			};

			bool bIsValid = oTargetFrameInfoDict.TryGetValue(Application.platform, out (long, long) stTargetFrameInfo);
			long nTargetFrameRate = bIsValid ? stTargetFrameInfo.Item2 : CValTable.Inst.GetInt(KCDefine.G_VT_KEY_DEF_TARGET_FRAME_RATE);
			long nDefTargetFrameRate = Application.isEditor ? KCDefine.B_EDITOR_TARGET_FRAME_RATE : KCDefine.B_DEF_TARGET_FRAME_RATE;

			float fRefreshRate = (float)Screen.currentResolution.refreshRateRatio.ExGetVal(nDefTargetFrameRate).ExGetMinVal(nTargetFrameRate);
			CAccess.SetTargetFrameRate(Mathf.RoundToInt(fRefreshRate));

#if ENABLE_MULTITOUCH
			Input.multiTouchEnabled = true;
#else
			Input.multiTouchEnabled = false;
#endif // #if ENABLE_MULTITOUCH

#if UNITY_EDITOR
			CSceneManager.SetupQuality(COptsInfoTable.Inst.InfoOptsQuality.m_eLevelQuality, true);
#else
			CSceneManager.SetupQuality(bIsValid ? 
				(ELevelQuality)stTargetFrameInfo.Item1 : (ELevelQuality)CValTable.Inst.GetInt(KCDefine.G_VT_KEY_DEF_QUALITY_LEVEL), true);
#endif // #if UNITY_EDITOR
			// 디바이스 정보를 설정한다 }
		}

		/** 다음 씬을 로드한다 */
		protected void LoadNextScene()
		{
#if SCENE_TEMPLATES_MODULE_ENABLE
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_SETUP, false);
#endif // #if SCENE_TEMPLATES_MODULE_ENABLE
		}

		/** 블라인드 이미지를 생성한다 */
		protected virtual Image CreateBlindImg(string a_oName, GameObject a_oParent)
		{
			return CFactory.CreateCloneGameObj<Image>(a_oName,
				CManagerRes.Inst.GetRes<GameObject>(KCDefine.IS_OBJ_P_SCREEN_BLIND_IMG), a_oParent);
		}

		/** 블라인드 UI 를 설정한다 */
		private void SetupBlindUIs()
		{
			// UI 설정이 불가능 할 경우
			if(CInitSceneManager.m_oBlindUIs != null)
			{
				return;
			}

			CInitSceneManager.m_oBlindUIs = CFactory.CreateCloneGameObj(KCDefine.U_OBJ_N_SCREEN_BLIND_UIS,
				CManagerRes.Inst.GetRes<GameObject>(KCDefine.IS_OBJ_P_SCREEN_BLIND_UIS), null);

			DontDestroyOnLoad(CInitSceneManager.m_oBlindUIs);
			CFunc.SetupScreenUIs(CInitSceneManager.m_oBlindUIs, KCDefine.G_SORTING_O_UIS_BLIND_SCREEN);

			var oScreenBlindUIs = CInitSceneManager.m_oBlindUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_BLIND_UIS, false);
			CSceneManager.SetScreenBlindUIs(oScreenBlindUIs);

			// 블라인드 이미지를 설정한다 {
			var oImgList = new List<Image>()
			{
				this.CreateBlindImg(KCDefine.U_OBJ_N_UP_BLIND_IMG, CSceneManager.ScreenBlindUIs),
				this.CreateBlindImg(KCDefine.U_OBJ_N_DOWN_BLIND_IMG, CSceneManager.ScreenBlindUIs),
				this.CreateBlindImg(KCDefine.U_OBJ_N_LEFT_BLIND_IMG, CSceneManager.ScreenBlindUIs),
				this.CreateBlindImg(KCDefine.U_OBJ_N_RIGHT_BLIND_IMG, CSceneManager.ScreenBlindUIs)
			};

			for(int i = 0; i < oImgList.Count; ++i)
			{
				oImgList[i].color = KCDefine.B_COLOR_TRANSPARENT;
				oImgList[i].raycastTarget = false;
			}
			// 블라인드 이미지를 설정한다 }
		}
		#endregion // 함수
	}

	/** 초기화 씬 관리자 - 코루틴 */
	public abstract partial class CInitSceneManager : CSceneManager
	{
		#region 함수
		/** 초기화 */
		private IEnumerator CoStart()
		{
			yield return CAccess.CoGetWaitForSecs(KCDefine.U_DELAY_INIT);

			// 관리자를 생성한다 {
			CManagerSnd.Create();
			CManagerRes.Create();
			CManagerTask.Create();
			CScheduleManager.Create();
			CNavStackManager.Create();
			CIndicatorManager.Create();

#if ADS_MODULE_ENABLE
			CAdsManager.Create();
#endif // #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
			CFlurryManager.Create();
#endif // #if FLURRY_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
			CFacebookManager.Create();
#endif // #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
			CFirebaseManager.Create();
#endif // #if FIREBASE_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
			CGameCenterManager.Create();
#endif // #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			CPurchaseManager.Create();
#endif // #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
			CNotiManager.Create();
#endif // #if NOTI_MODULE_ENABLE
			// 관리자를 생성한다 }

			// 전역 객체를 생성한다
			CGSingleton.Create();
			CSceneLoader.Create();
			CUnityMsgSender.Create();
			CDeviceMsgReceiver.Create();

			// 테이블을 생성한다 {
			CValTable.Create();
			CStrTable.Create();

			COptsInfoTable.Create(KCDefine.U_ASSET_P_G_OPTS_INFO_TABLE);
			CBuildInfoTable.Create(KCDefine.U_ASSET_P_G_BUILD_INFO_TABLE);
			CProjInfoTable.Create(KCDefine.U_ASSET_P_G_PROJ_INFO_TABLE);
			CLocalizeInfoTable.Create(KCDefine.U_ASSET_P_G_LOCALIZE_INFO_TABLE);
			CDefineSymbolInfoTable.Create(KCDefine.U_ASSET_P_G_DEFINE_SYMBOL_INFO_TABLE);
			CDeviceInfoTable.Create(KCDefine.U_ASSET_P_G_DEVICE_INFO_TABLE);

#if ADS_MODULE_ENABLE || FLURRY_MODULE_ENABLE || APPS_FLYER_MODULE_ENABLE
			CPluginInfoTable.Create(KCDefine.U_ASSET_P_G_PLUGIN_INFO_TABLE);
#endif // #if ADS_MODULE_ENABLE || FLURRY_MODULE_ENABLE || APPS_FLYER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			CProductInfoTable.Create(KCDefine.U_ASSET_P_G_PRODUCT_INFO_TABLE);
#endif // #if PURCHASE_MODULE_ENABLE
			// 테이블을 생성한다 }

			// 저장소를 생성한다
			CStorageInfoAppCommon.Create();
			CStorageInfoUserCommon.Create();
			CStorageInfoGameCommon.Create();

			// 디바이스를 설정한다 {
#if UNITY_IOS
			Device.hideHomeButton = false;

			Device.SetNoBackupFlag(KCDefine.B_DIR_P_WRITABLE);
			Device.SetNoBackupFlag(KCDefine.U_IMG_P_SCREENSHOT);
#endif // #if UNITY_IOS
			// 디바이스를 설정한다 }

			this.Setup();
			yield return CAccess.CoGetWaitForSecs(KCDefine.U_DELAY_INIT);

			this.ShowSplash();
			CSceneManager.SetEnableInit(true);
		}
		#endregion // 함수
	}
}

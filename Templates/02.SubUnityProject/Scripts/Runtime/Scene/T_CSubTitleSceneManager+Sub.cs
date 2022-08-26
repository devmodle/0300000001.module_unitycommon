#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
using GoogleSheetsToUnity;
#endif			// #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)

namespace TitleScene {
	/** 서브 타이틀 씬 관리자 */
	public partial class CSubTitleSceneManager : CTitleSceneManager {
		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.AwakeSetup();
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.StartSetup();
				this.UpdateUIsState();

				Func.PlayBGSnd(EResKinds.SND_BG_SCENE_TITLE_01);

				// 최초 시작 일 경우
				if(CCommonAppInfoStorage.Inst.IsFirstStart) {
					this.UpdateFirstStartState();
				}

				// 최초 플레이 일 경우
				if(CCommonAppInfoStorage.Inst.AppInfo.IsFirstPlay) {
					this.UpdateFirstPlayState();
				}

				// 로그인 되었을 경우
				if(CUserInfoStorage.Inst.UserInfo.LoginType != ELoginType.NONE) {
					this.OnLogin(CUserInfoStorage.Inst.UserInfo.LoginType, true);
				}
			}
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					foreach(var stKeyVal in m_oAniDict) {
						stKeyVal.Value?.Kill();
					}
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CSubGameSceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 씬을 설정한다 */
		private void AwakeSetup() {
			// 텍스트를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
				(EKey.TOUCH_TEXT, $"{EKey.TOUCH_TEXT}", this.UIsBase)
			}, m_oTextDict, false);

			m_oTextDict.GetValueOrDefault(EKey.TOUCH_TEXT)?.gameObject.SetActive(false);
			// 텍스트를 설정한다 }

			// 버튼을 설정한다
			CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
				(EKey.PLAY_BTN, $"{EKey.PLAY_BTN}", this.UIsBase, this.OnTouchPlayBtn),
				(EKey.GUEST_LOGIN_BTN, $"{EKey.GUEST_LOGIN_BTN}", this.UIsBase, this.OnTouchGuestLoginBtn),
				(EKey.APPLE_LOGIN_BTN, $"{EKey.APPLE_LOGIN_BTN}", this.UIsBase, this.OnTouchAppleLoginBtn),
				(EKey.FACEBOOK_LOGIN_BTN, $"{EKey.FACEBOOK_LOGIN_BTN}", this.UIsBase, this.OnTouchFacebookLoginBtn)
			}, m_oBtnDict, false);

			#region 추가
			this.SubAwakeSetup();
			#endregion			// 추가
		}

		/** 씬을 설정한다 */
		private void StartSetup() {
			// 업데이트가 가능 할 경우
			if(!CAppInfoStorage.Inst.IsIgnoreUpdate && CCommonAppInfoStorage.Inst.IsEnableUpdate()) {
				CAppInfoStorage.Inst.IsIgnoreUpdate = true;
				this.ExLateCallFunc((a_oSender) => Func.ShowUpdatePopup(this.OnReceiveUpdatePopupResult));
			}

			#region 추가
			this.SubStartSetup();
			#endregion			// 추가
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			// 버튼을 갱신한다 {
#if UNITY_IOS && APPLE_LOGIN_ENABLE
			m_oBtnDict.GetValueOrDefault(EKey.APPLE_LOGIN_BTN)?.gameObject.SetActive(true);
#else
			m_oBtnDict.GetValueOrDefault(EKey.APPLE_LOGIN_BTN)?.gameObject.SetActive(false);
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE

			for(int i = 0; i < m_oLoginBtnKeyList.Count; ++i) {
				// 로그인 되었을 경우
				if(CUserInfoStorage.Inst.UserInfo.LoginType != ELoginType.NONE) {
					m_oBtnDict.GetValueOrDefault(m_oLoginBtnKeyList[i])?.gameObject.SetActive(false);
				}
			}
			// 버튼을 갱신한다 }

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
			m_oGoogleSheetHandlerDict.TryAdd(Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ETC_INFO), this.OnLoadEtcInfoGoogleSheet);
			m_oGoogleSheetHandlerDict.TryAdd(Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_MISSION_INFO), this.OnLoadMissionInfoGoogleSheet);
			m_oGoogleSheetHandlerDict.TryAdd(Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_REWARD_INFO), this.OnLoadRewardInfoGoogleSheet);
			m_oGoogleSheetHandlerDict.TryAdd(Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_RES_INFO), this.OnLoadResInfoGoogleSheet);
			m_oGoogleSheetHandlerDict.TryAdd(Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ITEM_INFO), this.OnLoadItemInfoGoogleSheet);
			m_oGoogleSheetHandlerDict.TryAdd(Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_SKILL_INFO), this.OnLoadSkillInfoGoogleSheet);
			m_oGoogleSheetHandlerDict.TryAdd(Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_OBJ_INFO), this.OnLoadObjInfoGoogleSheet);
			m_oGoogleSheetHandlerDict.TryAdd(Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ABILITY_INFO), this.OnLoadAbilityInfoGoogleSheet);
			m_oGoogleSheetHandlerDict.TryAdd(Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_PRODUCT_INFO), this.OnLoadProductInfoGoogleSheet);
#endif			// #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)

			#region 추가
			this.SubUpdateUIsState();
			#endregion			// 추가
		}
		#endregion			// 함수
	}

	/** 서브 타이틀 씬 관리자 - 서브 */
	public partial class CSubTitleSceneManager : CTitleSceneManager {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

#if DEBUG || DEVELOPMENT_BUILD
		/** 서브 테스트 UI */
		[System.Serializable]
		private struct STSubTestUIs {
			// Do Something
		}
#endif			// #if DEBUG || DEVELOPMENT_BUILD

		#region 변수
		/** =====> UI <===== */
#if DEBUG || DEVELOPMENT_BUILD
		[SerializeField] private STSubTestUIs m_stSubTestUIs;
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		#endregion			// 변수

		#region 프로퍼티

		#endregion			// 프로퍼티

		#region 함수
		/** 씬을 설정한다 */
		private void SubAwakeSetup() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SetupSubTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 씬을 설정한다 */
		private void SubStartSetup() {
			// Do Something
		}

		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
			this.UpdateSubTestUIsState();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 터치 모드가 아닐 경우
			if(!m_oBoolDict.GetValueOrDefault(EKey.IS_TOUCH) && CUserInfoStorage.Inst.UserInfo.LoginType != ELoginType.NONE) {
				m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, true);

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
				var oGoogleSheetInfoList = new List<(string, int)>() {
					($"{EUserType.A}", KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS),
					($"{EUserType.B}", KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS),
					(KCDefine.B_KEY_COMMON, KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS)
				};

				Func.LoadGoogleSheet(KDefine.G_ID_VER_INFO_GOOGLE_SHEET, oGoogleSheetInfoList, this.OnLoadVerInfoGoogleSheet);
#endif			// #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
			}
		}
		#endregion			// 함수

		#region 조건부 함수
#if DEBUG || DEVELOPMENT_BUILD
		/** 서브 테스트 UI 를 설정한다 */
		private void SetupSubTestUIs() {
			// Do Something
		}

		/** 서브 테스트 UI 상태를 갱신한다 */
		private void UpdateSubTestUIsState() {
			// Do Something
		}

#if GOOGLE_SHEET_ENABLE
		/** 구글 시트 정보를 설정한다 */
		private void SetupGoogleSheetInfos(string a_oName, Dictionary<string, List<string>> a_oGoogleSheetInfoDictContainer, List<(string, int, System.Action<CServicesManager, GstuSpreadSheet, string, Dictionary<string, SimpleJSON.JSONNode>, bool>)> a_oOutGoogleSheetInfoList) {
			foreach(var stKeyVal in a_oGoogleSheetInfoDictContainer) {
				for(int i = 0; i < stKeyVal.Value.Count; ++i) {
					a_oOutGoogleSheetInfoList.ExAddVal((stKeyVal.Value[i], KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS, m_oGoogleSheetHandlerDict.GetValueOrDefault(a_oName)));
				}
			}
		}

		/** 구글 시트가 로드 되었을 경우 */
		private void OnLoadGoogleSheets(CServicesManager a_oSender, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				for(int i = 0; i < m_oVerInfos.Count; ++i) {
					CAppInfoStorage.Inst.AppInfo.m_oTableSysVerDict.ExReplaceVal(m_oVerInfos[i][KCDefine.U_KEY_NAME], System.Version.Parse(m_oVerInfos[i][KCDefine.U_KEY_VER]));
				}

				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
			}

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
		}

		/** 버전 정보 구글 시트를 로드했을 경우 */
		private void OnLoadVerInfoGoogleSheet(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, string a_oID, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
#if AB_TEST_ENABLE
				m_oVerInfos = a_oJSONNodeInfoDict.ExToJSONNode()[(CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? $"{EUserType.B}" : $"{EUserType.A}"];
#else
				m_oVerInfos = a_oJSONNodeInfoDict.ExToJSONNode()[KCDefine.B_KEY_COMMON];
#endif			// #if AB_TEST_ENABLE

				for(int i = 0; i < m_oVerInfos.Count; ++i) {
					var oVer = CAppInfoStorage.Inst.AppInfo.m_oTableSysVerDict.GetValueOrDefault(m_oVerInfos[i][KCDefine.U_KEY_NAME], KCDefine.U_VER_DEF);
					
					// 버전이 다를 경우
					if(oVer.CompareTo(System.Version.Parse(m_oVerInfos[i][KCDefine.U_KEY_VER])) < KCDefine.B_COMPARE_EQUALS) {
						foreach(var stKeyVal in KDefine.G_KEY_TABLE_DICT_CONTAINER[m_oVerInfos[i][KCDefine.U_KEY_NAME]]) {
							this.SetupGoogleSheetInfos(m_oVerInfos[i][KCDefine.U_KEY_NAME], stKeyVal.Value, CAppInfoStorage.Inst.GoogleSheetInfoDictContainer.GetValueOrDefault(m_oVerInfos[i][KCDefine.U_KEY_NAME]).Item2);
						}
					}
				}
			}

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
		}

		/** 기타 정보 구글 시트를 로드했을 경우 */
		private void OnLoadEtcInfoGoogleSheet(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, string a_oID, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				CEtcInfoTable.Inst.SaveEtcInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString());
			}

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
		}

		/** 미션 정보 구글 시트를 로드했을 경우 */
		private void OnLoadMissionInfoGoogleSheet(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, string a_oID, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				CMissionInfoTable.Inst.SaveMissionInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString());
			}

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
		}

		/** 보상 정보 구글 시트를 로드했을 경우 */
		private void OnLoadRewardInfoGoogleSheet(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, string a_oID, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				CRewardInfoTable.Inst.SaveRewardInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString());
			}

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
		}

		/** 리소스 정보 구글 시트를 로드했을 경우 */
		private void OnLoadResInfoGoogleSheet(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, string a_oID, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				CResInfoTable.Inst.SaveResInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString());
			}

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
		}

		/** 아이템 정보 구글 시트를 로드했을 경우 */
		private void OnLoadItemInfoGoogleSheet(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, string a_oID, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				CItemInfoTable.Inst.SaveItemInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString());
			}

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
		}

		/** 스킬 정보 구글 시트를 로드했을 경우 */
		private void OnLoadSkillInfoGoogleSheet(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, string a_oID, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				CSkillInfoTable.Inst.SaveSkillInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString());
			}

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
		}

		/** 객체 정보 구글 시트를 로드했을 경우 */
		private void OnLoadObjInfoGoogleSheet(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, string a_oID, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				CObjInfoTable.Inst.SaveObjInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString());
			}

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
		}

		/** 어빌리티 정보 구글 시트를 로드했을 경우 */
		private void OnLoadAbilityInfoGoogleSheet(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, string a_oID, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				CAbilityInfoTable.Inst.SaveAbilityInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString());
			}

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
		}

		/** 상품 정보 구글 시트를 로드했을 경우 */
		private void OnLoadProductInfoGoogleSheet(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, string a_oID, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				CProductTradeInfoTable.Inst.SaveProductTradeInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString());
			}

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
		}
#endif			// #if GOOGLE_SHEET_ENABLE
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		#endregion			// 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY

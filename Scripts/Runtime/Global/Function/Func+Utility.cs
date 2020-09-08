using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif			// #if UNITY_EDITOR

#if UNITY_IOS
using UnityEngine.iOS;
#endif			// #if UNITY_IOS

//! 유틸리티 함수
public static partial class Func {
	#region 클래스 함수
	//! 약관 동의 필요 여부를 검사한다
	public static bool IsNeedAgreement(string a_oCountryCode) {
		string oCountryCode = a_oCountryCode.ToUpper();
		return oCountryCode.ExIsEuropeanUnion() || oCountryCode.ExIsEquals(KDefine.B_KOREA_COUNTRY_CODE);
	}

	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdate(string a_oLatestVersion) {
#if UNITY_ANDROID
		return Func.IsNeedUpdateByBuildNumber(a_oLatestVersion);
#else
		return Func.IsNeedUpdateByBuildVersion(a_oLatestVersion);
#endif			// #if UNITY_ANDROID
	}

	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdate(string a_oLatestBuildNumber, string a_oLatestBuildVersion) {
		bool bIsNeedUpdate = Func.IsNeedUpdateByBuildNumber(a_oLatestBuildNumber);
		return bIsNeedUpdate || Func.IsNeedUpdateByBuildVersion(a_oLatestBuildVersion);
	}

	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdateByBuildNumber(string a_oLatestNumber) {
		Func.Assert(a_oLatestNumber.ExIsValid());

		bool bIsValidNumberA = int.TryParse(a_oLatestNumber, out int nBuildNumberA);
		bool bIsValidNumberB = int.TryParse(CProjectInfoTable.Instance.ProjectInfo.m_oBuildNumber, out int nBuildNumberB);

		Func.Assert(bIsValidNumberA && bIsValidNumberB);
		return nBuildNumberA > nBuildNumberB;
	}

	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdateByBuildVersion(string a_oLatestVersion) {
		Func.Assert(a_oLatestVersion.ExIsValid());

		var bIsValidVersionA = System.Version.TryParse(a_oLatestVersion, out System.Version oVersionA);
		var bIsValidVersionB = System.Version.TryParse(CProjectInfoTable.Instance.ProjectInfo.m_oBuildVersion, out System.Version oVersionB);

		Func.Assert(bIsValidVersionA && bIsValidVersionB);
		return oVersionA.CompareTo(oVersionB) >= KDefine.B_COMPARE_RESULT_GREATE;
	}

	//! 안전 영역을 반환한다
	public static Rect GetSafeArea(bool a_bIsRuntime = true) {
		if(a_bIsRuntime) {
			return Screen.safeArea;
		}

		return new Rect(0.0f, 0.0f, Camera.main.pixelWidth, Camera.main.pixelHeight);
	}

	//! 디바이스 화면 크기를 반환한다
	public static Vector2 GetDeviceScreenSize(bool a_bIsRuntime = true) {
		if(a_bIsRuntime) {
#if UNITY_EDITOR
			return new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
#else
			return new Vector2(Screen.width, Screen.height);
#endif			// #if UNITY_EDITOR			
		}

		return new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
	}

	//! 해상도를 반환한다
	public static Vector2 GetResolution(bool a_bIsRuntime = true) {
		float fScale = Func.GetResolutionScale(a_bIsRuntime);
		return new Vector2(KDefine.B_SCREEN_WIDTH, KDefine.B_SCREEN_HEIGHT) * fScale;
	}

	//! 해상도 비율을 반환한다
	public static float GetResolutionScale(bool a_bIsRuntime = true) {
		float fScale = 1.0f;
		float fAspect = KDefine.B_SCREEN_WIDTH / (float)KDefine.B_SCREEN_HEIGHT;

		float fScreenWidth = Func.GetDeviceScreenSize(a_bIsRuntime).x;
		float fScreenHeight = Func.GetDeviceScreenSize(a_bIsRuntime).y;

		// 화면을 벗어났을 경우
		if(fScreenWidth.ExIsLess(fScreenHeight * fAspect)) {
			fScale = fScreenWidth / (fScreenHeight * fAspect);
		}
		
		return fScale;
	}

	//! 왼쪽 화면 비율을 반환한다
	public static float GetLeftScreenScale(bool a_bIsRuntime = true) {
		var stSafeArea = Func.GetSafeArea(a_bIsRuntime);
		return stSafeArea.x / Func.GetDeviceScreenSize(a_bIsRuntime).x;
	}

	//! 오른쪽 화면 비율을 반환한다
	public static float GetRightScreenScale(bool a_bIsRuntime = true) {
		var stSafeArea = Func.GetSafeArea(a_bIsRuntime);
		float fScreenWidth = Func.GetDeviceScreenSize(a_bIsRuntime).x;

		return (fScreenWidth - (stSafeArea.x + stSafeArea.width)) / fScreenWidth;
	}

	//! 상단 화면 비율을 반환한다
	public static float GetTopScreenScale(bool a_bIsRuntime = true) {
		var stSafeArea = Func.GetSafeArea(a_bIsRuntime);
		float fScreenHeight = Func.GetDeviceScreenSize(a_bIsRuntime).y;

		return (fScreenHeight - (stSafeArea.y + stSafeArea.height)) / fScreenHeight;
	}

	//! 하단 화면 비율을 반환한다
	public static float GetBottomScreenScale(bool a_bIsRuntime = true) {
		var stSafeArea = Func.GetSafeArea(a_bIsRuntime);
		return stSafeArea.y / Func.GetDeviceScreenSize(a_bIsRuntime).y;
	}
	
	//! 메세지를 전송한다
	public static void SendMsg(string a_oName, string a_oMsg, object a_oParams) {
		var oObj = Func.FindObj(a_oName);
		oObj?.SendMessage(a_oMsg, a_oParams, SendMessageOptions.DontRequireReceiver);
	}

	//! 메세지를 전파한다
	public static void BroadcastMsg(string a_oMsg, object a_oParams) {
		Func.EnumerateScenes((a_stScene) => {
			a_stScene.ExBroadcastMsg(a_oMsg, a_oParams);
		});
	}

	//! 객체를 탐색한다
	public static GameObject FindObj(string a_oName) {
		Func.Assert(a_oName.ExIsValid());
		GameObject oObj = null;

		Func.EnumerateScenes((a_stScene) => {
			oObj = (oObj != null) ? oObj : a_stScene.ExFindChild(a_oName);
		});

		return oObj;
	}

	//! 객체를 탐색한다
	public static List<GameObject> FindObjs(string a_oName) {
		Func.Assert(a_oName.ExIsValid());
		var oObjList = new List<GameObject>();

		Func.EnumerateScenes((a_stScene) => {
			var oChildObjList = a_stScene.ExFindChildren(a_oName);

			if(oChildObjList != null) {
				oObjList.AddRange(oChildObjList);
			}
		});
		
		return oObjList;
	}

	//! 씬을 순회한다
	public static void EnumerateScenes(System.Action<Scene> a_oCallback) {
		for(int i = 0; i < SceneManager.sceneCount; ++i) {
			a_oCallback?.Invoke(SceneManager.GetSceneAt(i));
		}
	}

	//! 추가 씬을 로드한다
	public static void LoadAdditiveScene(string a_oName, bool a_bIsStartActivityIndicator = false) {
		Func.Assert(a_oName.ExIsValid());
		CSceneLoader.Instance.LoadScene(a_oName, a_bIsStartActivityIndicator, false, false, 0.0f, LoadSceneMode.Additive);
	}

	//! 추가 씬을 로드한다
	public static void LoadAdditiveScenes(string[] a_oNames, bool a_bIsStartActivityIndicator = false) {
		Func.Assert(a_oNames.ExIsValid());

		for(int i = 0; i < a_oNames.Length; ++i) {
			Func.LoadAdditiveScene(a_oNames[i], a_bIsStartActivityIndicator);
		}
	}

	//! 추가 씬을 로드한다
	public static void LoadAdditiveScenes(List<string> a_oNameList, bool a_bIsStartActivityIndicator = false) {
		Func.Assert(a_oNameList.ExIsValid());

		for(int i = 0; i < a_oNameList.Count; ++i) {
			Func.LoadAdditiveScene(a_oNameList[i], a_bIsStartActivityIndicator);
		}
	}

	//! 객체를 생선한다
	public static GameObject CreateObj(string a_oName, 
		GameObject a_oParent, bool a_bIsStayWorldState = false) {
		var oObj = new GameObject(a_oName);
		oObj.transform.SetParent(a_oParent?.transform, a_bIsStayWorldState);

		return oObj;
	}

	//! 사본 객체를 생성한다
	public static GameObject CreateCloneObj(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, bool a_bIsStayWorldState = false) {
		Func.Assert(a_oOrigin != null);

		var oObj = Object.Instantiate(a_oOrigin, a_oOrigin.transform.position, a_oOrigin.transform.rotation);
		oObj.name = a_oName;
		oObj.transform.localScale = a_oOrigin.transform.localScale;

		oObj.transform.SetParent(a_oParent?.transform, a_bIsStayWorldState);
		return oObj;
	}

	//! 터치 응답자를 생성한다
	public static GameObject CreateTouchResponder(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, Vector2 a_stSize, Vector2 a_stPos, Color a_stColor) {
		var oObj = Func.CreateCloneObj(a_oName, a_oOrigin, a_oParent);
		var oImg = oObj.GetComponentInChildren<Image>();

		Func.Assert(oImg != null);

		var oTransform = oObj.transform as RectTransform;
		oTransform.sizeDelta = a_stSize;
		oTransform.anchoredPosition = a_stPos;

		oImg.color = a_stColor;
		return oObj;
	}
	
	//! 객체 풀을 생성한다
	public static ObjectPool CreateObjPool(GameObject a_oOrigin, GameObject a_oParent, int a_nNumObjs = 0) {
		Func.Assert(a_oOrigin != null);
		return new ObjectPool(a_oOrigin, a_oParent?.transform, a_nNumObjs);
	}
	#endregion			// 클래스 함수

	#region 제네릭 클래스 함수
	//! 리소스 존재 여부를 검사한다
	public static bool IsExistsRes<T>(string a_oFilepath) where T : Object {
		return Resources.Load<T>(a_oFilepath) != null;
	}

	//! 컴포넌트를 탐색한다
	public static T FindComponent<T>(string a_oName) where T : Component {
		var oObj = Func.FindObj(a_oName);
		return oObj?.GetComponentInChildren<T>();
	}

	//! 컴포넌트를 탐색한다
	public static T[] FindComponents<T>(string a_oName) where T : Component {
		var oObj = Func.FindObj(a_oName);
		return oObj?.GetComponentsInChildren<T>();
	}

	//! 객체를 생선한다
	public static T CreateObj<T>(string a_oName,
		GameObject a_oParent, bool a_bIsStayWorldState = false) where T : Component {
		var oObj = Func.CreateObj(a_oName, a_oParent, a_bIsStayWorldState);
		return oObj.ExAddComponent<T>();
	}

	//! 객체 사본을 생성한다
	public static T CreateCloneObj<T>(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, bool a_bIsStayWorldState = false) where T : Component {
		var oObj = Func.CreateCloneObj(a_oName, a_oOrigin, a_oParent, a_bIsStayWorldState);
		return oObj?.GetComponentInChildren<T>();
	}

	//! 팝업을 생성한다
	public static T CreatePopup<T>(string a_oName, 
		GameObject a_oOrigin, GameObject a_oParent, Vector2 a_stPos) where T : CPopup {
		var oPopup = Func.CreateCloneObj<T>(a_oName, a_oOrigin, a_oParent);

		if(oPopup != null) {
			oPopup.m_oRectTransform.anchoredPosition = a_stPos;
		}

		return oPopup;
	}

	//! 알림 팝업을 생성한다
	public static T CreateAlertPopup<T>(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, Dictionary<string, string> a_oDataList, System.Action<CAlertPopup, bool> a_oCallback) where T : CAlertPopup {
		var oAlertPopup = Func.CreatePopup<T>(a_oName, a_oOrigin, a_oParent, KDefine.B_POS_MIDDLE_CENTER);
		oAlertPopup?.Init(a_oDataList, a_oCallback);

		return oAlertPopup;
	}

	//! 토스트 팝업을 생성한다
	public static T CreateToastPopup<T>(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, string a_oMsg, float a_fDuration) where T : CToastPopup {
		var oToastPopup = Func.CreatePopup<T>(a_oName, a_oOrigin, a_oParent, KDefine.B_POS_MIDDLE_CENTER);
		oToastPopup?.Init(a_oMsg, a_fDuration);

		return oToastPopup;
	}

	//! 터치 응답자를 생성한다
	public static T CreateTouchResponder<T>(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, Vector2 a_stSize, Vector2 a_stPos, Color a_stColor) where T : Component {
		var oObj = Func.CreateTouchResponder(a_oName, a_oOrigin, a_oParent, a_stSize, a_stPos, a_stColor);
		return oObj?.GetComponentInChildren<T>();
	}
	#endregion			// 제네릭 클래스 함수

	#region 조건부 클래스 함수
#if UNITY_EDITOR
	//! 스크립트 순서를 변경한다
	public static void SetScriptOrder(MonoScript a_oScript, int a_nOrder) {
		Func.Assert(a_oScript != null && (a_nOrder >= short.MinValue && a_nOrder <= short.MaxValue));
		int nOrder = MonoImporter.GetExecutionOrder(a_oScript);

		if(nOrder != a_nOrder) {
			MonoImporter.SetExecutionOrder(a_oScript, a_nOrder);
		}
	}

	//! 객체를 선택한다
	public static void SelectObj(GameObject a_oObj, bool a_bIsEnablePing = false) {
		Func.Assert(a_oObj != null);
		Selection.activeGameObject = a_oObj;

		if(a_bIsEnablePing) {
			EditorGUIUtility.PingObject(a_oObj);
		}
	}
#endif			// #if UNITY_EDITOR

#if UNITY_IOS
	//! 애플 로그인 지원 여부를 검사한다
	public static bool IsSupportLoginWithApple() {
		if(!Func.IsMobilePlatform()) {
			return false;
		}

		var oVersion = new System.Version(Device.systemVersion);
		return oVersion.CompareTo(KDefine.U_MIN_VERSION_LOGIN_WITH_APPLE) >= KDefine.B_COMPARE_RESULT_EQUALS;
	}
#endif			// UNITY_IOS

#if HAPTIC_FEEDBACK_ENABLE && (UNITY_IOS || UNITY_ANDROID)
	//! 햅틱 피드백 지원 여부를 검사한다
	public static bool IsSupportHapticFeedback() {
#if UNITY_IOS
		string oModel = Device.generation.ToString();
		bool bIsiPhone = oModel.Contains(KDefine.U_MODEL_NAME_IPHONE);

		int nIndex = KDefine.U_HAPTIC_FEEDBACK_SUPPORT_MODELS.ExFindValue((a_eDeviceGeneration) => {
			return bIsiPhone && a_eDeviceGeneration == Device.generation;
		});

		return nIndex > KDefine.B_INDEX_INVALID;
#else
		return true;
#endif			// #if UNITY_IOS
	}
#endif			// #if HAPTIC_FEEDBACK_ENABLE && (UNITY_IOS || UNITY_ANDROID)

#if MESSAGE_PACK_ENABLE
	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdate() {
		bool bIsEnable = CAppInfoStorage.Instance.IsLoadStoreVersion && 
			CAppInfoStorage.Instance.IsValidStoreVersion;

		return bIsEnable && Func.IsNeedUpdate(CAppInfoStorage.Instance.StoreVersion);
	}

	//! 버전 정보를 생성한다
	public static STVersionInfo MakeDefVersionInfo(string a_oVersion) {
		return new STVersionInfo() {
			m_oVersion = a_oVersion,
			m_oExtraInfoList = new Dictionary<string, string>()
		};
	}
#endif			// #if MESSAGE_PACK_ENABLE
	#endregion			// 조건부 클래스 함수
}

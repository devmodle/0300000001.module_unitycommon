using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

//! 기본 객체 생성자
public static partial class CObjCreator {
	#region 클래스 함수
	//! 게임 객체를 생성한다
	[MenuItem("GameObject/Create/GameObject %#u")]
	public static void CreateGameObj() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_GAME_OBJ, string.Empty, false, true);
	}

	//! 게임 객체를 생성한다
	[MenuItem("GameObject/Create/ChildGameObject %#i")]
	public static void CreateChildGameObj() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_GAME_OBJ, string.Empty);
	}

	//! 텍스트를 생성한다
	[MenuItem("GameObject/Create/Prefab/Text/Text")]
	public static void CreateText() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_TEXT, KDefine.U_OBJ_PATH_TEXT);
	}

	//! 텍스트를 생성한다
	[MenuItem("GameObject/Create/Prefab/Text/LocalizeText")]
	public static void CreateLocalizeText() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT, KDefine.U_OBJ_PATH_LOCALIZE_TEXT);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create/Prefab/Button/Text + Button/TextButton")]
	public static void CreateTextBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_TEXT_BTN, KDefine.U_OBJ_PATH_TEXT_BTN);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create/Prefab/Button/Text + Button/TextScaleButton")]
	public static void CreateTextScaleBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_TEXT_SCALE_BTN);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create/Prefab/Button/LocalizeText + Button/LocalizeTextButton")]
	public static void CreateLocalizeTextBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_BTN, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_BTN);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create/Prefab/Button/LocalizeText + Button/LocalizeTextScaleButton")]
	public static void CreateLocalizeTextScaleBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_SCALE_BTN);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create/Prefab/Button/Image + Button/ImageButton")]
	public static void CreateImgBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_IMG_BTN, KDefine.U_OBJ_PATH_IMG_BTN);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create/Prefab/Button/Image + Button/ImageScaleButton")]
	public static void CreateImgScaleBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_IMG_SCALE_BTN, KDefine.U_OBJ_PATH_IMG_SCALE_BTN);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create/Prefab/Button/Image + Text + Button/ImageTextButton")]
	public static void CreateImgTextBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_IMG_TEXT_BTN, KDefine.U_OBJ_PATH_IMG_TEXT_BTN);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create/Prefab/Button/Image + Text + Button/ImageTextScaleButton")]
	public static void CreateImgTextScaleBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_IMG_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_IMG_TEXT_SCALE_BTN);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create/Prefab/Button/Image + LocalizeText + Button/ImageLocalizeTextButton")]
	public static void CreateImgLocalizeTextBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_IMG_LOCALIZE_TEXT_BTN, KDefine.U_OBJ_PATH_IMG_LOCALIZE_TEXT_BTN);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create/Prefab/Button/Image + LocalizeText + Button/ImageLocalizeTextScaleButton")]
	public static void CreateImgLocalizeTextScaleBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_IMG_LOCALIZE_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_IMG_LOCALIZE_TEXT_SCALE_BTN);
	}

	//! 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create/Prefab/ScrollView/ScrollView")]
	public static void CreateScrollView() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_SCROLL_VIEW, KDefine.U_OBJ_PATH_SCROLL_VIEW);
	}

	//! 페이지 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create/Prefab/ScrollView/PageScrollView")]
	public static void CreatePageScrollView() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_PAGE_SCROLL_VIEW, KDefine.U_OBJ_PATH_PAGE_SCROLL_VIEW);
	}

	//! 터치 응답자를 생성한다
	[MenuItem("GameObject/Create/Prefab/Responder/TouchResponder")]
	public static void CreateTouchResponder() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_TOUCH_RESPONDER, KDefine.U_OBJ_PATH_TOUCH_RESPONDER);
	}

	//! 드래그 응답자를 생성한다
	[MenuItem("GameObject/Create/Prefab/Responder/DragResponder")]
	public static void CreateDragResponder() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_DRAG_RESPONDER, KDefine.U_OBJ_PATH_DRAG_RESPONDER);
	}

	//! 원본 텍스트를 생성한다
	[MenuItem("GameObject/Create/Original/Text/Text")]
	public static void CreateOriginText() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_TEXT, KDefine.U_OBJ_PATH_TEXT, false);
	}

	//! 원본 텍스트를 생성한다
	[MenuItem("GameObject/Create/Original/Text/LocalizeText")]
	public static void CreateOriginLocalizeText() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT, KDefine.U_OBJ_PATH_LOCALIZE_TEXT, false);
	}
	
	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create/Original/Button/Text + Button/TextButton")]
	public static void CreateOriginTextBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_TEXT_BTN, KDefine.U_OBJ_PATH_TEXT_BTN, false);
	}

	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create/Original/Button/Text + Button/TextScaleButton")]
	public static void CreateOriginTextScaleBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_TEXT_SCALE_BTN, false);
	}

	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create/Original/Button/LocalizeText + Button/LocalizeTextButton")]
	public static void CreateOriginLocalizeTextBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_BTN, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_BTN, false);
	}

	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create/Original/Button/LocalizeText + Button/LocalizeTextScaleButton")]
	public static void CreateOriginLocalizeTextScaleBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_SCALE_BTN, false);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create/Original/Button/Image + Button/ImageButton")]
	public static void CreateOriginImgBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_IMG_BTN, KDefine.U_OBJ_PATH_IMG_BTN, false);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create/Original/Button/Image + Button/ImageScaleButton")]
	public static void CreateOriginImgScaleBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_IMG_SCALE_BTN, KDefine.U_OBJ_PATH_IMG_SCALE_BTN, false);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create/Original/Button/Image + Text + Button/ImageTextButton")]
	public static void CreateOriginImgTextBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_IMG_TEXT_BTN, KDefine.U_OBJ_PATH_IMG_TEXT_BTN, false);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create/Original/Button/Image + Text + Button/ImageTextScaleButton")]
	public static void CreateOriginImgTextScaleBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_IMG_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_IMG_TEXT_SCALE_BTN, false);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create/Original/Button/Image + LocalizeText + Button/ImageLocalizeTextButton")]
	public static void CreateOriginImgLocalizeTextBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_IMG_LOCALIZE_TEXT_BTN, KDefine.U_OBJ_PATH_IMG_LOCALIZE_TEXT_BTN, false);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create/Original/Button/Image + LocalizeText + Button/ImageLocalizeTextScaleButton")]
	public static void CreateOriginImgLocalizeTextScaleBtn() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_IMG_LOCALIZE_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_IMG_LOCALIZE_TEXT_SCALE_BTN, false);
	}

	//! 원본 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create/Original/ScrollView/ScrollView")]
	public static void CreateOriginScrollView() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_SCROLL_VIEW, KDefine.U_OBJ_PATH_SCROLL_VIEW, false);
	}

	//! 원본 페이지 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create/Original/ScrollView/PageScrollView")]
	public static void CreateOriginPageScrollView() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_PAGE_SCROLL_VIEW, KDefine.U_OBJ_PATH_PAGE_SCROLL_VIEW, false);
	}

	//! 원본 터치 응답자를 생성한다
	[MenuItem("GameObject/Create/Original/Responder/TouchResponder")]
	public static void CreateOriginTouchResponder() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_TOUCH_RESPONDER, KDefine.U_OBJ_PATH_TOUCH_RESPONDER, false);
	}

	//! 원본 드래그 응답자를 생성한다
	[MenuItem("GameObject/Create/Original/Responder/DragResponder")]
	public static void CreateOriginDragResponder() {
		CObjCreator.CreateObj(KEditorDefine.B_OBJ_NAME_DRAG_RESPONDER, KDefine.U_OBJ_PATH_DRAG_RESPONDER, false);
	}

	//! 객체를 생성한다
	private static GameObject CreateObj(string a_oName, string a_oFilepath, bool a_bIsPrefab = true, bool a_bIsIgnoreParent = false) {
		var oParent = a_bIsIgnoreParent ? null : EditorFunc.GetActiveObj();
		GameObject oObj = null;

		if(a_oFilepath.ExIsValid()) {
			var oOrigin = Resources.Load<GameObject>(a_oFilepath);
			
			if(a_bIsPrefab) {
				oObj = EditorFunc.CreatePrefabInstance(a_oName, oOrigin, oParent);
			} else {
				oObj = Func.CreateCloneObj(a_oName, oOrigin, oParent);
			}
		} else {
			oObj = Func.CreateObj(a_oName, oParent);
		}

		Func.SelectObj(oObj);
		EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

		// 부모 객체가 존재 할 경우
		if(oParent != null && oParent.GetComponent<RectTransform>() != null) {
			var oTransform = oObj.ExAddComponent<RectTransform>();
			oTransform.sizeDelta = Vector2.zero;

#if !MODE_CENTER_ENABLE
			bool bIsRightUIRoot = oParent.name.ExIsEquals(KDefine.U_OBJ_NAME_SCENE_RIGHT_UI_ROOT);
			bool bIsTopUIRoot = oParent.name.ExIsEquals(KDefine.U_OBJ_NAME_SCENE_TOP_UI_ROOT);

			Vector2 stAnchor = KDefine.B_ANCHOR_BOTTOM_LEFT;
			
			if(bIsRightUIRoot) {
				stAnchor = KDefine.B_ANCHOR_BOTTOM_RIGHT;
			} else if(bIsTopUIRoot) {
				stAnchor = KDefine.B_ANCHOR_TOP_LEFT;
			}

			oTransform.anchorMin = stAnchor;
			oTransform.anchorMax = stAnchor;
#endif			// #if !MODE_CENTER_ENABLE
		}

		Undo.RegisterCreatedObjectUndo(oObj, a_oName);
		return oObj;
	}
	#endregion			// 클래스 함수
}

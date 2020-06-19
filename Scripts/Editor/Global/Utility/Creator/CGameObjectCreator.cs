using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//! 기본 게임 객체 생성자
public static partial class CGameObjectCreator {
	#region 클래스 함수
	//! 텍스트를 생성한다
	[MenuItem("GameObject/Create Other/Text/Text")]
	public static void CreateText() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_TEXT, KDefine.U_OBJ_PATH_TEXT);
	}

	//! 텍스트를 생성한다
	[MenuItem("GameObject/Create Other/Text/LocalizeText")]
	public static void CreateLocalizeText() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT, KDefine.U_OBJ_PATH_LOCALIZE_TEXT);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Text + Button/TextButton")]
	public static void CreateTextButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_TEXT_BUTTON, KDefine.U_OBJ_PATH_TEXT_BUTTON);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Text + Button/TextScaleButton")]
	public static void CreateTextScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_TEXT_SCALE_BUTTON);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/LocalizeText + Button/LocalizeTextButton")]
	public static void CreateLocalizeTextButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_BUTTON, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_BUTTON);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/LocalizeText + Button/LocalizeTextScaleButton")]
	public static void CreateLocalizeTextScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_SCALE_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Button/ImageButton")]
	public static void CreateImageButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Button/ImageScaleButton")]
	public static void CreateImageScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_SCALE_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Text + Button/ImageTextButton")]
	public static void CreateImageTextButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_TEXT_BUTTON, KDefine.U_OBJ_PATH_IMAGE_TEXT_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Text + Button/ImageTextScaleButton")]
	public static void CreateImageTextScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_TEXT_SCALE_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + LocalizeText + Button/ImageLocalizeTextButton")]
	public static void CreateImageLocalizeTextButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_LOCALIZE_TEXT_BUTTON, KDefine.U_OBJ_PATH_IMAGE_LOCALIZE_TEXT_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + LocalizeText + Button/ImageLocalizeTextScaleButton")]
	public static void CreateImageLocalizeTextScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_LOCALIZE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_LOCALIZE_TEXT_SCALE_BUTTON);
	}

	//! 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create Other/ScrollView/ScrollView")]
	public static void CreateScrollView() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_SCROLL_VIEW, KDefine.U_OBJ_PATH_SCROLL_VIEW);
	}

	//! 페이지 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create Other/ScrollView/PageScrollView")]
	public static void CreatePageScrollView() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_PAGE_SCROLL_VIEW, KDefine.U_OBJ_PATH_PAGE_SCROLL_VIEW);
	}

	//! 터치 응답자를 생성한다
	[MenuItem("GameObject/Create Other/Responder/TouchResponder")]
	public static void CreateTouchResponder() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_TOUCH_RESPONDER, KDefine.U_OBJ_PATH_TOUCH_RESPONDER);
	}

	//! 드래그 응답자를 생성한다
	[MenuItem("GameObject/Create Other/Responder/DragResponder")]
	public static void CreateDragResponder() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_DRAG_RESPONDER, KDefine.U_OBJ_PATH_DRAG_RESPONDER);
	}

	//! 게임 객체를 생성한다
	private static GameObject CreateGameObject(string a_oName, string a_oFilepath) {
		var oGameObject = EditorFunction.CreatePrefabInstance(a_oName,
			Resources.Load<GameObject>(a_oFilepath), EditorFunction.GetActiveGameObject());

		Function.SelectGameObject(oGameObject);
		return oGameObject;
	}
	#endregion			// 클래스 함수
}

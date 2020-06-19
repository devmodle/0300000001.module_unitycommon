using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//! 에셋 추출자
public static partial class CAssetExporter {
	#region 클래스 함수
	//! 빌드 설정 파일을 추출한다
	[MenuItem("Utility/Export/BuildOption")]
	public static void ExportBuildOption() {
		//! iOS 설정 파일을 복사한다
		Function.CopyFile(KEditorDefine.B_IOS_DEST_MONO_MODULES_REGISTER_PATH, KEditorDefine.B_IOS_SRC_MONO_MODULES_REGISTER_PATH);

		// 안드로이드 빌드 설정 파일을 복사한다
		Function.CopyFile(KEditorDefine.B_ANDROID_DEST_MANIFEST_PATH, KEditorDefine.B_ANDROID_SRC_MANIFEST_PATH);
		Function.CopyFile(KEditorDefine.B_ANDROID_DEST_MAIN_TEMPLATE_PATH, KEditorDefine.B_ANDROID_SRC_MAIN_TEMPLATE_PATH);
		Function.CopyFile(KEditorDefine.B_ANDROID_DEST_PROGUARD_PATH, KEditorDefine.B_ANDROID_SRC_PROGUARD_PATH);

		var oStringBuilder = new System.Text.StringBuilder();
		oStringBuilder.AppendLine("빌드 설정 파일을 추출했습니다.");
		oStringBuilder.AppendLine(KEditorDefine.B_IOS_SRC_MONO_MODULES_REGISTER_PATH);
		oStringBuilder.AppendLine(KEditorDefine.B_ANDROID_SRC_MANIFEST_PATH);
		oStringBuilder.AppendLine(KEditorDefine.B_ANDROID_SRC_MAIN_TEMPLATE_PATH);
		oStringBuilder.AppendLine(KEditorDefine.B_ANDROID_SRC_PROGUARD_PATH);

		CAssetExporter.ShowExportSuccessPopup(oStringBuilder.ToString());
	}

	//! 텍스처 -> PNG 이미지로 추출한다
	[MenuItem("Utility/Export/Texture to PNGImage")]
	public static void ExportTextureToPNGImage() {
		var oTextureList = Selection.objects.ExToTypes<Texture2D>();

		if(!oTextureList.ExIsValid()) {
			CAssetExporter.ShowExportFailPopup(KEditorDefine.B_ALERT_P_EXPORT_TEXTURE_FAIL_MESSAGE);
		} else {
			for(int i = 0; i < oTextureList.Count; ++i) {
				string oFilepath = string.Format(KEditorDefine.B_IMG_PATH_FORMAT_TEXTURE_TO_IMAGE, oTextureList[i].name);
				CAssetExporter.SaveTexture(oFilepath, oTextureList[i]);
			}

			CAssetExporter.ShowExportSuccessPopup(KEditorDefine.B_ALERT_P_EXPORT_IMAGE_SUCCESS_MESSAGE);
		}
	}

	//! 기본 텍스처 -> PNG 이미지로 추출한다
	[MenuItem("Utility/Export/DefTexture to PNGImage")]
	public static void ExportDefTextureToPNGImage() {
		string oFilepath = string.Format(KEditorDefine.B_IMG_PATH_FORMAT_TEXTURE_TO_IMAGE, Texture2D.whiteTexture.name);
		CAssetExporter.SaveTexture(oFilepath, Texture2D.whiteTexture);

		CAssetExporter.ShowExportSuccessPopup(KEditorDefine.B_ALERT_P_EXPORT_IMAGE_SUCCESS_MESSAGE);
	}

	//! 스프라이트 -> PNG 이미지로 추출한다
	[MenuItem("Utility/Export/Sprite to PNGImage")]
	public static void ExportSpriteToPNGImage() {
		var oSpriteList = Selection.objects.ExToTypes<Sprite>();

		if(!oSpriteList.ExIsValid()) {
			CAssetExporter.ShowExportFailPopup(KEditorDefine.B_ALERT_P_EXPORT_SPRITE_FAIL_MESSAGE);
		} else {
			for(int i = 0; i < oSpriteList.Count; ++i) {
				var oTexture = new Texture2D((int)oSpriteList[i].textureRect.width,
					(int)oSpriteList[i].textureRect.height, TextureFormat.RGBA32, 1, true);

				oTexture.SetPixels(oSpriteList[i].texture.GetPixels((int)oSpriteList[i].textureRect.x,
					(int)oSpriteList[i].textureRect.y, (int)oSpriteList[i].textureRect.width, (int)oSpriteList[i].textureRect.height, 0));

				oTexture.Apply();

				var oBytes = oTexture.EncodeToPNG();
				Function.WriteBytes(string.Format(KEditorDefine.B_IMG_PATH_FORMAT_TEXTURE_TO_IMAGE, oSpriteList[i].name), oBytes);
			}

			CAssetExporter.ShowExportSuccessPopup(KEditorDefine.B_ALERT_P_EXPORT_IMAGE_SUCCESS_MESSAGE);
		}
	}

	//! 텍스처를 저장한다
	private static void SaveTexture(string a_oFilepath, Texture2D a_oTexture) {
		var oTexture = new Texture2D(a_oTexture.width,
			a_oTexture.height, TextureFormat.RGBA32, 1, true);

		oTexture.SetPixels(a_oTexture.GetPixels(0));
		oTexture.Apply();

		var oBytes = oTexture.EncodeToPNG();
		Function.WriteBytes(a_oFilepath, oBytes);		
	}

	//! 추출 성공 팝업을 출력한다
	private static void ShowExportSuccessPopup(string a_oMessage) {
		EditorFunction.ShowAlertPopup(KEditorDefine.B_ALERT_P_TITLE,
			a_oMessage, KEditorDefine.B_ALERT_P_OK_BUTTON_TEXT, string.Empty);
	}

	//! 추출 에러 팝업을 출력한다
	private static void ShowExportFailPopup(string a_oMessage) {
		EditorFunction.ShowAlertPopup(KEditorDefine.B_ALERT_P_TITLE,
			a_oMessage, KEditorDefine.B_ALERT_P_OK_BUTTON_TEXT, string.Empty);
	}
	#endregion			// 클래스 함수
}

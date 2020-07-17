using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

//! 리소스 관리자
public class CResManager : CSingleton<CResManager> {
	#region 변수
	private Dictionary<string, Mesh> m_oMeshList = new Dictionary<string, Mesh>();
	private Dictionary<string, Shader> m_oShaderList = new Dictionary<string, Shader>();
	private Dictionary<string, Sprite> m_oSpriteList = new Dictionary<string, Sprite>();
	private Dictionary<string, Texture> m_oTextureList = new Dictionary<string, Texture>();
	private Dictionary<string, Material> m_oMaterialList = new Dictionary<string, Material>();
	private Dictionary<string, AudioClip> m_oAudioClipList = new Dictionary<string, AudioClip>();
	private Dictionary<string, TextAsset> m_oTextAssetList = new Dictionary<string, TextAsset>();
	private Dictionary<string, GameObject> m_oPrefabList = new Dictionary<string, GameObject>();
	private Dictionary<string, SpriteAtlas> m_oSpriteAtlasList = new Dictionary<string, SpriteAtlas>();
	private Dictionary<string, AssetBundle> m_oAssetBundleList = new Dictionary<string, AssetBundle>();
	private Dictionary<string, ScriptableObject> m_oScriptableObjList = new Dictionary<string, ScriptableObject>();
	#endregion			// 변수
	
	#region 프로퍼티
	public Dictionary<System.Type, System.Func<string, Object>> ResCreatorList { get; private set; } = new Dictionary<System.Type, System.Func<string, Object>>() {
		[typeof(Mesh)] = Resources.Load<Mesh>,
		[typeof(Shader)] = Resources.Load<Shader>,
		[typeof(Sprite)] = Resources.Load<Sprite>,
		[typeof(Texture)] = Resources.Load<Texture>,
		[typeof(Material)] = Resources.Load<Material>,
		[typeof(AudioClip)] = Resources.Load<AudioClip>,
		[typeof(TextAsset)] = Resources.Load<TextAsset>,
		[typeof(GameObject)] = Resources.Load<GameObject>,
		[typeof(ScriptableObject)] = Resources.Load<ScriptableObject>
	};
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		this.SetupDefaultReses();
	}

	//! 메시를 반환한다
	public Mesh GetMesh(string a_oKey) {
		return this.GetRes<Mesh>(m_oMeshList, a_oKey, false);
	}

	//! 쉐이더를 반환한다
	public Shader GetShader(string a_oKey, bool a_bIsAutoCreate = true) {
		return this.GetRes<Shader>(m_oShaderList, a_oKey, a_bIsAutoCreate);
	}

	//! 스프라이트를 반환한다
	public Sprite GetSprite(string a_oKey, bool a_bIsAutoCreate = true) {
		return this.GetRes<Sprite>(m_oSpriteList, a_oKey, a_bIsAutoCreate);
	}

	//! 텍스처를 반환한다
	public Texture GetTexture(string a_oKey, bool a_bIsAutoCreate = true) {
		return this.GetRes<Texture>(m_oTextureList, a_oKey, a_bIsAutoCreate);
	}

	//! 재질을 반환한다
	public Material GetMaterial(string a_oKey, bool a_bIsAutoCreate = true) {
		return this.GetRes<Material>(m_oMaterialList, a_oKey, a_bIsAutoCreate);
	}

	//! 오디오 클립을 반환한다
	public AudioClip GetAudioClip(string a_oKey, bool a_bIsAutoCreate = true) {
		return this.GetRes<AudioClip>(m_oAudioClipList, a_oKey, a_bIsAutoCreate);
	}

	//! 텍스트 에셋을 반환한다
	public TextAsset GetTextAsset(string a_oKey, bool a_bIsAutoCreate = true) {
		return this.GetRes<TextAsset>(m_oTextAssetList, a_oKey, a_bIsAutoCreate);
	}

	//! 프리팹을 반환한다
	public GameObject GetPrefab(string a_oKey, bool a_bIsAutoCreate = true) {
		return this.GetRes<GameObject>(m_oPrefabList, a_oKey, a_bIsAutoCreate);
	}

	//! 메시를 추가한다
	public void AddMesh(string a_oKey, Mesh a_oMesh) {
		m_oMeshList.ExAddValue(a_oKey, a_oMesh);
	}

	//! 쉐이더를 추가한다
	public void AddShader(string a_oKey, Shader a_oShader) {
		m_oShaderList.ExAddValue(a_oKey, a_oShader);
	}

	//! 스프라이트를 추가한다
	public void AddSprite(string a_oKey, Sprite a_oSprite) {
		m_oSpriteList.ExAddValue(a_oKey, a_oSprite);
	}

	//! 텍스처를 추가한다
	public void AddTexture(string a_oKey, Texture a_oTexture) {
		m_oTextureList.ExAddValue(a_oKey, a_oTexture);
	}

	//! 재질을 추가한다
	public void AddMaterial(string a_oKey, Material a_oMaterial) {
		m_oMaterialList.ExAddValue(a_oKey, a_oMaterial);
	}

	//! 오디오 클립을 추가한다
	public void AddAudioClip(string a_oKey, AudioClip a_oAudioClip) {
		m_oAudioClipList.ExAddValue(a_oKey, a_oAudioClip);
	}

	//! 텍스트 에셋을 추가한다
	public void AddTextAsset(string a_oKey, TextAsset a_oTextAsset) {
		m_oTextAssetList.ExAddValue(a_oKey, a_oTextAsset);
	}

	//! 객체를 추가한다
	public void AddPrefab(string a_oKey, GameObject a_oPrefab) {
		m_oPrefabList.ExAddValue(a_oKey, a_oPrefab);
	}

	//! 스크립트 객체를 추가한다
	public void AddScriptableObj(string a_oKey, ScriptableObject a_oScriptableObj) {
		m_oScriptableObjList.ExAddValue(a_oKey, a_oScriptableObj);
	}

	//! 리소스 생성자를 추가한다
	public void AddResCreator(System.Type a_oType, System.Func<string, Object> a_oCreator) {
		Func.Assert(a_oType != null && a_oCreator != null);
		this.ResCreatorList.ExReplaceValue(a_oType, a_oCreator);
	}

	//! 메시를 제거한다
	public void RemoveMesh(string a_oKey) {
		this.RemoveRes(m_oMeshList, a_oKey, false);
	}

	//! 쉐이더를 제거한다
	public void RemoveShader(string a_oKey, bool a_bIsAutoUnload = false) {
		this.RemoveRes(m_oShaderList, a_oKey, a_bIsAutoUnload);
	}

	//! 스프라이트를 제거한다
	public void RemoveSprite(string a_oKey, bool a_bIsAutoUnload = false) {
		this.RemoveRes(m_oSpriteList, a_oKey, a_bIsAutoUnload);
	}

	//! 텍스처를 제거한다
	public void RemoveTexture(string a_oKey, bool a_bIsAutoUnload = false) {
		this.RemoveRes(m_oTextureList, a_oKey, a_bIsAutoUnload);
	}

	//! 재질을 제거한다
	public void RemoveMaterial(string a_oKey, bool a_bIsAutoUnload = false) {
		this.RemoveRes(m_oMaterialList, a_oKey, a_bIsAutoUnload);
	}

	//! 오디오 클립을 제거한다
	public void RemoveAudioClip(string a_oKey, bool a_bIsAutoUnload = false) {
		this.RemoveRes(m_oAudioClipList, a_oKey, a_bIsAutoUnload);
	}

	//! 텍스트 에셋을 제거한다
	public void RemoveTextAsset(string a_oKey, bool a_bIsAutoUnload = false) {
		this.RemoveRes(m_oTextAssetList, a_oKey, a_bIsAutoUnload);
	}

	//! 프리팹을 제거한다
	public void RemovePrefab(string a_oKey, bool a_bIsAutoUnload = false) {
		this.RemoveRes(m_oPrefabList, a_oKey, a_bIsAutoUnload);
	}

	//! 스크립트 객체를 제거한다
	public void RemoveScriptableObj(string a_oKey, bool a_bIsAutoUnload = false) {
		this.RemoveRes(m_oScriptableObjList, a_oKey, a_bIsAutoUnload);
	}

	//! 다중 스프라이트를 로드한다
	public void LoadMultiSprite(string a_oFilepath) {
		Func.Assert(a_oFilepath.ExIsValid());
		var oSprites = Resources.LoadAll<Sprite>(a_oFilepath);

		for(int i = 0; i < oSprites.Length; ++i) {
			this.AddSprite(oSprites[i].name, oSprites[i]);
		}
	}

	//! 에셋 번들을 로드한다
	public void LoadAssetBundle(string a_oURL) {
		
	}

	//! 스프라이트 아틀라스를 로드한다
	public void LoadSpriteAtlas(string a_oFilepath) {
		Func.Assert(a_oFilepath.ExIsValid());

		if(!m_oSpriteAtlasList.ContainsKey(a_oFilepath)) {
			var oSpriteAtlas = Resources.Load<SpriteAtlas>(a_oFilepath);
			m_oSpriteAtlasList.Add(a_oFilepath, oSpriteAtlas);
		}

		if(m_oSpriteAtlasList[a_oFilepath]?.spriteCount >= 1) {
			var oSprites = new Sprite[m_oSpriteAtlasList[a_oFilepath].spriteCount];
			m_oSpriteAtlasList[a_oFilepath].GetSprites(oSprites);

			for(int i = 0; i < m_oSpriteAtlasList[a_oFilepath].spriteCount; ++i) {
				string oKey = oSprites[i].name.ExGetReplaceString(KDefine.U_IMG_NAME_SPRITE_CLONE, 
					string.Empty);

				this.AddSprite(oKey, oSprites[i]);
			}
		}
	}

	//! 기본 리소스를 설정한다
	private void SetupDefaultReses() {
		this.SetupDefaultMeshes();
		this.SetupDefaultSprites();
	}

	//! 기본 메시를 설정한다
	private void SetupDefaultMeshes() {
		var oMesh = new Mesh();
		oMesh.name = KDefine.U_MESH_NAME_DEF_MESH;

		oMesh.SetVertices(new List<Vector3> {
			new Vector3(-0.5f, -0.5f, 0.0f),
			new Vector3(-0.5f, 0.5f, 0.0f),
			new Vector3(0.5f, 0.5f, 0.0f),
			new Vector3(0.5f, -0.5f, 0.0f)
		});

		oMesh.SetIndices(new int[] {
			0, 1, 2, 0, 2, 3
		}, MeshTopology.Triangles, 0);

		oMesh.SetUVs(0, new List<Vector2>() {
			new Vector2(0.0f, 0.0f),
			new Vector2(0.0f, 1.0f),
			new Vector2(1.0f, 1.0f),
			new Vector2(1.0f, 0.0f)
		});

		oMesh.Optimize();
		this.AddMesh(KDefine.U_MESH_NAME_DEF_MESH, oMesh);
	}

	//! 기본 스프라이트를 설정한다
	private void SetupDefaultSprites() {
		var oSprite = Sprite.Create(Texture2D.whiteTexture, Rect.MinMaxRect(0.0f, 0.0f, 1.0f, 1.0f), KDefine.B_ANCHOR_MIDDLE_CENTER, 1.0f);
		oSprite.name = KDefine.U_IMG_NAME_DEF_SPRITE;

		this.AddSprite(KDefine.U_IMG_NAME_DEF_SPRITE, oSprite);
	}
	#endregion			// 함수

	#region 제네릭 함수
	//! 스크립트 객체를 반환한다
	public T GetScriptableObj<T>(string a_oKey, bool a_bIsAutoCreate = true) where T : ScriptableObject {
		return this.GetRes<ScriptableObject>(m_oScriptableObjList, a_oKey, a_bIsAutoCreate) as T;
	}

	//! 리소스를 반환한다
	private T GetRes<T>(Dictionary<string, T> a_oResList, string a_oKey, bool a_bIsAutoCreate) where T : Object {
		Func.Assert(a_oResList != null && a_oKey.ExIsValid());
		Func.Assert(a_bIsAutoCreate || a_oResList.ContainsKey(a_oKey));

		if(a_bIsAutoCreate && !a_oResList.ContainsKey(a_oKey)) {
			var oType = typeof(T);
			bool bIsContainsCreator = this.ResCreatorList.ContainsKey(oType);

			Func.Assert(bIsContainsCreator && this.ResCreatorList[oType] != null);

			var oRes = this.ResCreatorList[oType](a_oKey) as T;
			a_oResList.ExAddValue(a_oKey, oRes);
		}

		return a_oResList[a_oKey];
	}

	//! 리소스를 제거한다
	private void RemoveRes<T>(Dictionary<string, T> a_oResList, string a_oKey, bool a_bIsAutoUnload) where T : Object {
		Func.Assert(a_oResList != null && a_oKey.ExIsValid());

		if(a_oResList.ContainsKey(a_oKey)) {
			var oRes = a_oResList[a_oKey];
			a_oResList.Remove(a_oKey);

			if(a_bIsAutoUnload) {
				Resources.UnloadAsset(oRes);
			}
		}
	}
	#endregion			// 제네릭 함수
}

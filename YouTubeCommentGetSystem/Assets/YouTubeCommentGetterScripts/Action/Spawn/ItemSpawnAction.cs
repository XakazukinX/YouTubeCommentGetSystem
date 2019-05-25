using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemSpawnAction : YoutubeCommentActionBase
{
    //スポーンしたオブジェクトが消えるまでの時間
    public float destroyTime = 3f;
    
    //名前とスポーンするオブジェクトの定義
    [Serializable]
    public class SpawnSetting
    {
        public string targetComment;
        public GameObject spawnGameObject;
        
    }
    
    //SpawnSettingの内容をそのまま辞書にしたもの
    private Dictionary<string, GameObject> spawnDictionary;

    [SerializeField] private List<SpawnSetting> spawnSettings = new List<SpawnSetting>();

    //スポーンの中心になるオブジェクト
    [Header("スポーンの中心になるオブジェクトをアタッチしてください。")]
    [Header("座標で指定する場合は空にしてください。")]
    [SerializeField] private GameObject targetGameObject;

    [Header("スポーンの中心になる座標を入力してください。")] 
    [SerializeField] private Vector3 targetPos;
    
    
    [Header("スポーンの中心からの出現範囲")] 
    [SerializeField] private Vector2 spawnWidth_MIN_MAX = new Vector2(-1,1);
    [SerializeField] private Vector2 spawnDepth_MIN_MAX = new Vector2(-1,1);
    [SerializeField] private float spawnHeight = 1.7f;


    private void Awake()
    {
        //辞書の初期化
        spawnDictionary = new Dictionary<string, GameObject>();
        
        for (int i = 0; i < spawnSettings.Count; i++)
        {
            var _targetComment = spawnSettings[i].targetComment;
            var _spawnGameObject = spawnSettings[i].spawnGameObject;
            spawnDictionary.Add(_targetComment,_spawnGameObject);
        }
    }



    public override void applyCommentAction(string comment, string target ,bool isSuperChat)
    {
        commentAction(target);
    }

    public void commentAction(string targetComment)
    {
        //例外
        if (spawnDictionary[targetComment] == null)
        {
            return;
        }
        spawn(spawnDictionary[targetComment]);
    }


    public void spawn(GameObject spawn)
    {

        //基準となる座標を決定する。
        Vector3 standardPos = (targetGameObject == null) ? targetPos : targetGameObject.transform.localPosition;  
        
        //スポーンする座標をランダムに指定
        var spawnPos = new Vector3(standardPos.x + UnityEngine.Random.Range(spawnWidth_MIN_MAX.x, spawnWidth_MIN_MAX.y), standardPos.y + spawnHeight,  
            standardPos.z + UnityEngine.Random.Range(spawnDepth_MIN_MAX.x, spawnDepth_MIN_MAX.y));
        
        //スポーンするオブジェクトの向きをランダムに指定。
        var spawnRot = new Vector3(UnityEngine.Random.Range(-90, 90), UnityEngine.Random.Range(-90, 90),
            UnityEngine.Random.Range(-90, 90));
			
        GameObject _spawn = Instantiate(spawn, spawnPos, Quaternion.Euler(spawnRot));
        _spawn.AddComponent<CommentItemFalling>();

        
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            Destroy(_spawn, destroyTime);
        }
        Undo.RecordObject(_spawn , "spawnObject");
#else
			if (Application.isPlaying)
			{
				Destroy(_spawn,destroyTime);	
			}
#endif
    }

    [SerializeField] private string TestComment;
    [SerializeField] private string TestTarget;


    [ContextMenu("testSpawn")]
    public void testSpawn()
    {
        Awake();
        applyCommentAction(TestComment,TestTarget,false);

    }
    
    
}

[CustomEditor(typeof(ItemSpawnAction))] //拡張するクラスを指定
[CanEditMultipleObjects]
public class ItemSpawnActionTestScriptEditor : Editor
{
    private ItemSpawnAction _itemSpawnAction;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        _itemSpawnAction = target as ItemSpawnAction;

        if (GUILayout.Button("SpawnTest!!"))
        {
            _itemSpawnAction.testSpawn();
			
        }
    }
}

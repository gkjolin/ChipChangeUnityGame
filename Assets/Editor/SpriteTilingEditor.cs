using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SpriteTiling))]

public class SpriteTilingEditor : Editor {
	
	private SpriteTiling spriteTiling;
	private GameObject originSprite;
	
	public override void OnInspectorGUI() {

		spriteTiling = (SpriteTiling)target;
		GUILayout.BeginVertical("box");
		EditorGUILayout.LabelField("TILING",EditorStyles.boldLabel);
		spriteTiling.tileSize = EditorGUILayout.Vector2Field("Tile Size: ", spriteTiling.tileSize);
		if(GUILayout.Button("Apply!"))
			ApplyTileSettings();
		GUILayout.EndHorizontal();
		
		GUILayout.BeginVertical("box");
		EditorGUILayout.LabelField("ROTATION",EditorStyles.boldLabel);
		if(GUILayout.Button("Flip Horizontal")) FlipHorizontal(!spriteTiling.flipHorizontal);
		EditorGUILayout.LabelField("", spriteTiling.flipHorizontal.ToString(), EditorStyles.boldLabel);
		if(GUILayout.Button("Flip Vertical")) FlipVertical(!spriteTiling.flipVertical);
		EditorGUILayout.LabelField("", spriteTiling.flipVertical.ToString(), EditorStyles.boldLabel);
		GUILayout.EndHorizontal();
	}
	
	private void ApplyTileSettings() {
		
		originSprite = spriteTiling.transform.GetChild(0).gameObject;
		
		List<GameObject> children = new List<GameObject>();
		foreach(Transform child in spriteTiling.transform) {
			if(child.gameObject == originSprite) continue;
			children.Add(child.gameObject);
		}
		children.ForEach(child => DestroyImmediate(child));
		
		
		if(spriteTiling.tileSize.x > 1 && spriteTiling.tileSize.y > 1) {
			for(int x = 0; x < spriteTiling.tileSize.x; x++) {
				for(int y = 0; y < spriteTiling.tileSize.y; y++) {
					if(x==0 && y==0) continue;
					Object prefabRoot = PrefabUtility.GetPrefabParent(originSprite);
					GameObject newTiledSprite = (GameObject)PrefabUtility.InstantiatePrefab(prefabRoot);
					GameObject newTiledSpriteParent = newTiledSprite.transform.parent.gameObject;
					newTiledSprite.transform.position = new Vector3(originSprite.transform.position.x+x, originSprite.transform.position.y+y, 0);
					newTiledSprite.transform.parent = spriteTiling.transform;
					DestroyImmediate(newTiledSpriteParent);
				}
			}
		}
		else if(spriteTiling.tileSize.x == 1) {
			for(int y = 1; y < spriteTiling.tileSize.y; y++) {
				Object prefabRoot = PrefabUtility.GetPrefabParent(originSprite);
				GameObject newTiledSprite = (GameObject)PrefabUtility.InstantiatePrefab(prefabRoot);
				GameObject newTiledSpriteParent = newTiledSprite.transform.parent.gameObject;
				newTiledSprite.transform.position = new Vector3(originSprite.transform.position.x, originSprite.transform.position.y+y, 0);
				newTiledSprite.transform.parent = spriteTiling.transform;
				DestroyImmediate(newTiledSpriteParent);
				
			}
		}
		else if(spriteTiling.tileSize.y == 1) {
			for(int x = 1; x < spriteTiling.tileSize.x; x++) {
				Object prefabRoot = PrefabUtility.GetPrefabParent(originSprite);
				GameObject newTiledSprite = (GameObject)PrefabUtility.InstantiatePrefab(prefabRoot);
				GameObject newTiledSpriteParent = newTiledSprite.transform.parent.gameObject;
				newTiledSprite.transform.position = new Vector3(originSprite.transform.position.x+x, originSprite.transform.position.y, 0);
				newTiledSprite.transform.parent = spriteTiling.transform;
				DestroyImmediate(newTiledSpriteParent);
			}
		}
	}
	
	private void FlipHorizontal(bool b) {
		if(spriteTiling.flipHorizontal) spriteTiling.transform.Rotate(0, -180, 0);
		else if(!spriteTiling.flipHorizontal) spriteTiling.transform.Rotate(0, 180, 0);
		spriteTiling.flipHorizontal = b;
	}
	private void FlipVertical(bool b) {
		if(spriteTiling.flipHorizontal) spriteTiling.transform.Rotate(0, -180, -180);
		else if(!spriteTiling.flipHorizontal) spriteTiling.transform.Rotate(0, 180, 180);
		spriteTiling.flipVertical = b;
	}
}
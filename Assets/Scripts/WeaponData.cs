using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UpgradeLevel {
    public int cost;
    public GameObject visualization;
  
}

public class WeaponData : MonoBehaviour {

	public List<UpgradeLevel> levels;
	private UpgradeLevel currentLevel;
    [HideInInspector]
    public int curr_level;

    private void Awake()
    {
        CurrentLevel = levels[0];
        curr_level = 0;
    }

    //1
    public UpgradeLevel CurrentLevel {
		//2
		get {
			return currentLevel;
		}
		//3
		set {
			currentLevel = value;
			int currentLevelIndex = levels.IndexOf(currentLevel);
 
			GameObject levelVisualization = levels[currentLevelIndex].visualization;
			for (int i = 0; i < levels.Count; i++) {
				if (levelVisualization != null) {
					if (i == currentLevelIndex) {
						levels[i].visualization.SetActive(true);
					} else {
						levels[i].visualization.SetActive(false);
					}
				}
			}
		}
	}


	public UpgradeLevel getNextLevel() {
		int currentLevelIndex = levels.IndexOf (currentLevel);
		int maxLevelIndex = levels.Count - 1;
		if (currentLevelIndex < maxLevelIndex) {
			return levels[currentLevelIndex+1];
		} else {
			return null;
		}
	}
	
	public void upgrade() {
		int currentLevelIndex = levels.IndexOf(currentLevel);
		if (currentLevelIndex < levels.Count - 1) {
			CurrentLevel = levels[currentLevelIndex + 1];
            curr_level++;
		}
	}
}

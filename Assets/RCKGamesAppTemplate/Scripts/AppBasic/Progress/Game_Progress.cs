using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Game_Progress
{
	[Header("** Parameters to Show the Inn App Review")]
	public int numero_de_lecturas_para_mostrar_InAppReview;
	public int numero_de_ARscans_para_mostrar_InAppReview;
	public int veces_que_se_ha_mostrado_InAppReview;
	public bool volver_a_mostrar_InAppReview;


	/*
    public GameProgress[] game;

	[Header("Currency")]
	public int blueCristals;					// Moneda dificil de conseguir

	[Header("Game Data")]
	public int allStarsMedals;					// Numero de veces que se ha completado por primera vez un nivel con las 3 estrellas
	public LevelsProgress[] levelsProgress;		
	public ChallengeLevelsProgress[] challengeLevelProgress;
	public CharactersProgress[] heroesProgress;					// 0 = Bruno,  1 = Mila,  2 = Abuelo
	public TowersProgress[] towersProgress;
	public EnemiesProgress[] enemyProgress;
	public bool isGameLocked = true;
	*/
}

// ===================================
/*
[System.Serializable]
public class GameProgress
{
    public bool isUnlocked = false;
    public bool isCompleted = false;
}

[System.Serializable]
public class LevelsProgress 
{
	public bool isUnlocked = false;
	public bool isCompleted = false;
	public int score = 0;
	public bool firstTimeUnlocked = false;
	[Range(0,3)]
	public int numberOfStars = 0;
}

[System.Serializable]
public class ChallengeLevelsProgress 
{
	public bool isUnlocked = false;
	public bool isCompleted = false;
}

[System.Serializable]
public class CharactersProgress
{
	[Range(0,99)]	// 99 niveles
	public int currentLevel = 1;
    public int currentXP = 0;
}

[System.Serializable]
public class TowersProgress 	// 0 = fuego,  1 = hielo,  2 = planta,  3 = trueno,  4 = viento
{
	public bool isUnlocked = false;
	[Range(1,3)]
	public int maxLevelUnlocked = 1;
}

[System.Serializable]
public class EnemiesProgress
{
	public bool isUnlocked = false;
}
*/



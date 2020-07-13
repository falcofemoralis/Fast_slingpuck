﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BaseStructures;

public class Speed : MonoBehaviour, Mode
{
    public GameObject AI;
    public GameObject capperField;
    public GameObject gameMenu;
    public GameObject downBorderHolder;
    public Game game;

    public Text upCountText, downCountText, gameCounterText;

    // Счетчики
    public int score;
    public short upCount = 0, downCount = 0; // константы (4) необходимо заменить

    // Цели
    public int winTarget; // На случай если нету ИИ, победа начисляется за количество фишек забитых за время
    public int targetCheckers; // Вторая цель - количество забитых фишек, для получения следующей звезды
    public bool missOrLag; // true - если случился промах в игре без ИИ или игрок отстал по очкам хотя бы раз за игру от ИИ 

    // Start is called before the first frame update
    void Start()
    {
        initScene();
        StartCoroutine(delayBeforeStart(3));
    }

    public void initScene()
    {
        game = GetComponent<Game>();

        // Текст счетчиков
        upCountText = game.upCountText;
        downCountText = game.downCountText;
        gameCounterText = game.gameCounter;
        upCountText.text = upCount.ToString();

        downBorderHolder = game.downBorderHolder;

        // Бот
        AI = game.AI;
        if (!GameRule.AI)
        {
            AI.SetActive(false);
            downCountText.text = downCount.ToString();
        }

        // Меню
        gameMenu = game.gameMenu;

        // Заглушка
        capperField = game.capperField;
        game.checkersSpeed.SetActive(true);

        // Установка целей
        initTargets();
    }


    /* Установка целей для режима Normal
     * Порядок целей для режима (номер цели = номеру цели в GameRule)
     * 1 - Победа (Или достигнута цель winTarget)
     * 2 - Количество фишек для получения следующей звезды
     * 3 - Без промахов (нету ИИ), не отставать от бота в течении всей игры
     */
    public void initTargets()
    {
        winTarget = GameRule.target1;
        targetCheckers = GameRule.target2;
    }

    // Очко добавляется тому игроку с чьей стороны прилетела шайба
    public void changeCount(GameObject obj)
    {
        if (obj.GetComponent<Checker>().field == Checker.Field.Down)
            downCountText.text = (++downCount).ToString();
        else
            upCountText.text = (++upCount).ToString();

        if (upCount < downCount)
            missOrLag = true;
    }

    public void gameOver()
    {
        AI.GetComponent<AI>().active = false;
        gameMenu.GetComponent<GameMenu>().gameOver("Game Over !", downCount);
    }

    public void calculateResult()
    {
        if ((GameRule.AI && downCount < winTarget) || (!GameRule.AI && downCount <= upCount))
            game.countStars = 0;
        else
        {
            if (downCount < targetCheckers)
                --game.countStars;

            if (missOrLag)
                --game.countStars;
        }

        Debug.Log(game.countStars);
    }

    // Задержка перед началом игры
    IEnumerator delayBeforeStart(int sec)
    {
        for (int i = sec; i >= 1; --i)
        {
            gameCounterText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        gameCounterText.text = "GO!"; // Заменить и локализовать
        capperField.SetActive(false);
        AI.GetComponent<AI>().active = true;    
        yield return new WaitForSeconds(1);
        StartCoroutine(counter(60));
    }

    // Таймер игры
    IEnumerator counter(int sec)
    {
        for (int i = sec; i >= 0; --i)
        {
            gameCounterText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        AI.GetComponent<AI>().active = false;
        gameOver();
    }

}

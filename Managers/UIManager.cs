using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine;
using TMPro;

using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    [Header("UI Component")]
    public GameObject tutorialManager;
    public GameObject gameoverManager;
    public Volume volume;
    public Image skilChargeImage;
    public Image hpBarImage;
    public Animator UI_HPanime;
    public RectTransform[] ItemList = new RectTransform[5];
    public GameObject puzzleEnterLabel;
    public TMP_Text PuzzleLabel;

}

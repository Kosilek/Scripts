using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioContainer : MonoBehaviour
{
    [Foldout("Door")]
    public AudioClip openDoor;
    [Foldout("Door")]
    public AudioClip closeDoor;

    [Foldout("MainClip")]
    public AudioClip menuClip;
    [Foldout("MainClip")]
    public AudioClip gameClip;

    [Foldout("Game")]
    public AudioClip comboOrNewMaxMonstr;
    [Foldout("Game")]
    public AudioClip updateCell;
    [Foldout("Game")]
    public AudioClip destoyHummer;
    [Foldout("Game")]
    public AudioClip echangesCell;
    [Foldout("Game")]
    public AudioClip addMoneyCombo;
    [Foldout("Game")]
    public AudioClip addDNKCombo;
    [Foldout("Game")]
    public AudioClip prizeCanvas;

    [Foldout("Button")]
    public AudioClip tublerButton;
    [Foldout("Button")]
    public AudioClip clickButon;
}

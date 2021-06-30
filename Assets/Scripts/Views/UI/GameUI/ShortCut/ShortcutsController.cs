using System;
using System.Collections;
using System.Collections.Generic;
using ActionPool;
using Domain.Services.IService;
using Items;
using Scripts;
using UnityEngine;

public class ShortcutsController : MonoBehaviour
{
    private static ShortcutsController instance;
    public static ShortcutsController Instance => instance;
    
    [SerializeField] 
    private TypedUITypes type;
    [SerializeField]
    private GameObject shortCutPrefab;
    
    [SerializeField] 
    private GameObject shortCutParent;
    
    private Dictionary<int,ShortCutButtonController> shortCutButtonScripts;

    private int[] keyCodeBinds;
    private CharacterService _characterService;
    private PlayerData _player;

    public PlayerData Player
    {
        get => _player;
        set 
        {
            _player = value;
            foreach (ShortCutButtonController inventoryButtonController in shortCutButtonScripts.Values)
            {
                inventoryButtonController.Player = _player;
            }
        }   
    }

    private void Awake()
    {
        instance = this;
        keyCodeBinds=new int[6];
        keyCodeBinds[0] = 1;
        keyCodeBinds[1] = 2;
        keyCodeBinds[2] = 3;
        keyCodeBinds[3] = 5;
        keyCodeBinds[4] = 6;
        keyCodeBinds[5] = 7;
        EventCenter.AddListener<long>("UI:"+type.ToString()+":Initial",Initial);
    }

    public void Initial(long uid)
    {
        /*Dictionary<int, ItemInGame> shortCutItems = CharactersManager.Instance.Get(uid).ShortCuts;
        if(shortCutItems==null) return;
        shortCutButtonScripts=new Dictionary<int, ShortCutButtonScript>();
        for (int i = 0; i < keyCodeBinds.Length; i++)
        {
            GameObject obj = Instantiate(shortCutPrefab, shortCutParent.transform);
            shortCutButtonScripts[keyCodeBinds[i]] = obj.GetComponent<ShortCutButtonScript>();
            if (shortCutItems[keyCodeBinds[i]] != null)
            {
                shortCutButtonScripts[keyCodeBinds[i]].Icon.enabled = true;
                shortCutButtonScripts[keyCodeBinds[i]].Icon.sprite = shortCutItems[keyCodeBinds[i]].Icon;
                shortCutButtonScripts[keyCodeBinds[i]].ItemInGame = shortCutItems[keyCodeBinds[i]];
            }
            shortCutButtonScripts[keyCodeBinds[i]].BindKey.enabled = true;
            shortCutButtonScripts[keyCodeBinds[i]].BindKey.text = keyCodeBinds[i].ToString();
        }*/
            
    }
    public void SetPlayer(PlayerData playerData)
    {
        
    }
    public void OutUse(int keyCode)
    {
        shortCutButtonScripts[keyCode].ItemUse();
    }
    
}

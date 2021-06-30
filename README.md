# <center> 目录说明
##  Attribute-属性目录

保存游戏中的所有属性

* Character：角色所拥有的属性，如攻击力、法术强度、攻击速度等。

## Commons-常量枚举目录

* Constants：游戏中全局常量，包括字符串、整数等
* Enums：游戏中枚举变量

## Domain-领域目录

* Data：数据目录
    
    游戏中的每一个数据都将被附加上一个Data类（包括地板、角色等等）。其中，GameData为基类，其余类必须继承于此类。
    ```
    例如：
        FloorData：地板数据类，此类附加对象可供角色移动，包括地面、地板、桥梁等。
        PlayerData：角色数据类，此类附加对象为角色所控制对象，其中包括角色的全部属性，如：攻击力、防御力、生命值等，还包含附加在角色身上的所有脚本。
    ```
* MessageEntities：消息通信目录

    广播通信之间传递的消息类，根据各频道的不同，通信类也不同。

* State：状态目录

    标记游戏中物体所处的不同状态。

    * Player：玩家的状态
    ```
    例如：
        AnimationState：玩家操控角色的动作状态
        MovementState：玩家操控角色的移动状态
        NormalAttackState：玩家操控角色的普通攻击状态
    ```

## Infrastructure-基础设施目录
* Core：核心目录

    处理游戏中的大部分逻辑

* Managers：管理目录

    管理游戏中引用的资源，根据资源类别进行管理。
    ```
    例如：
        CombatTextManager：战斗文本管理，用于管理游戏中伤害值、恢复值等战斗相关文本。
        GameManager：对整个游戏逻辑进行管理
        GameObjectManager：对场景中的对象的引用，目前手动添加，后期将通过代码生成引用。
        PrefabsManager：预制体管理，管理所需使用的预制体，预制体访问将会被重新生成。
        SoundManager：游戏音频管理
    ```
* Utils-工具目录

    使用到的全局方法。
    
    * EventPoolUtils：通过委托实现的事件池子，由于每次事件的唤醒必须在下一帧才进行调用，目前已经被框架代替。 

    * Tools：通用工具类

## Views-视图层

收集外部状态、修改UI的层级

* Character：角色相关视图
* Player：接收用户输入
* UI：UI控制
    
    * GaneUI：普通UI，直接显示在界面上的
    * SceneUI：场景UI，在场景中进行渲染的UI


# 游戏路线图（代码架构）

本文档基于前期的游戏设计文档，将整体游戏功能划分为若干模块与类，并按照开发阶段提出代码层面的实现与测试方案。
目标是确保架构清晰、模块解耦、易扩展且易于测试。

---


## 阶段 1: 核心系统开发（路径、计数器、资源管理）

### 1.1 类结构

**路径与 Tile 系统**
- `PathSystem`
  - 功能：管理路径编辑，路径验证，触发计数器增量事件。
  - 依赖：`Tile` 对象列表、`EventBus`
- `Tile`
  - 属性：
    - `TileType`
      - 可通过的：Road, Forest, 
      - 不可通过的：HighMountain,Building... 
      - 空地：Empty
    - `IsBuilt`
    - `BuildCost`（资源消耗）
    - `WalkCost`（步数消耗）
  - 方法：`CanPlace()`, `OnPlaced()`
- `TilePlacer`
  - 功能：在路径上放置Tile，并触发路径验证。
- `TilePalette`
  - 功能：提供Tile的列表，供玩家选择。
  - 操作逻辑
    - UI面板上有Tile的图标，供玩家选择。
    - 在玩家点击对应Tile时，才会激活TilePlacer，用来放置选择的Tile。
    - 如果玩家成功放置Tile，则TilePlacer不再激活。
- `PathInitializer`
  - 功能：生成初始化路径
  - 初始路径是一个闭环路径，长度可调，默认长度为30。
  - 使用策略模式，针对不同职业定义不同的路径生成逻辑（目前先暂定三个相同的职业）
  - 将路径初始化逻辑与`PathSystem`解耦，使用单独的`PathInitializer`进行路径初始化。

**计数器** 
- 创建一个通用的计数器接口，所有的计数器，都实现这个接口
- `LoopCounter`（已完成）
  - 属性：`CurrentValue`, `Threshold`
  - 方法：`Increment()`, `Reset()`
  - 当 `CurrentValue >= Threshold` 时通过 `EventBus` 触发 `OnBossTriggered` 事件。
- `StepCounter` （未完成）
  - 属性：`CurrentValue`, `Threshold`
  - 方法：`Increment()`, `Reset()`
  - 当 `CurrentValue >= Threshold` 时通过 `EventBus` 触发 `OnBossTriggered` 事件。

**资源与经济基础** （待拓展）
- `ResourceManager`
  - 属性：`Dictionary<ResourceType, int>`（资源类型与数量映射）、`MaxStorage`、`OnResourceChanged`事件
  - 方法：`AddResource(type, amount)`, `ConsumeResource(type, amount)`
- `IResourceService`
  - 方法：`GetResourceCount(type)`, `TryConsumeResource(type, amount)`, `AddResource(type, amount)`
  - `ResourceManager`将通过实现该接口进行资源操作。

**调试控制台** （已完成/使用IngameDebugConsole插件）

- `DebugConsole`
  - 功能：
    - 资源状态和系统运行状态。
  - 要求：
    - 可拓展未来添加更多调试命令。
    - 可以解析长命令，并执行命令。

### 1.1.1 随机数生成种子


#### 2.2 测试用例
- `PathSystemTests`
  - 测试添加/删除 Tile 的逻辑正确性
  - 测试路径连续性检测
  - 测试Tile放置和路径验证逻辑
- `LoopCounterTests`
  - 测试计数器增量与重置
  - 测试计数器到达阈值后事件触发
- `ResourceManagerTests`
  - 测试资源添加、消耗与边界条件（溢出、不足）


---

### 阶段 2: 战斗与 RPG 系统开发（战斗逻辑、角色属性、技能树）

#### 2.1 类结构

**角色与属性**
- `Character`
  - 属性：`Health`, `Attack`, `Defense`, `Skills`（List<Skill>）
  - 方法：`TakeDamage(amount)`, `Heal(amount)`
- `Skill`
  - 属性：`Name`, `Cooldown`, `Effect`
  - 方法：`Activate(target)`
- `Enemy`
   - 可以由特定的Building生成
   - 可以由特定的事件生成
   - 可以由特定的敌人生成

**战斗系统**
- `CombatSystem`
  - 方法：`StartBattle(Character player, Character enemy)`
  - 内部调用`RoundProcessor`进行战斗回合管理。

- `RoundProcessor`
  - 方法：`ProcessRound(player, enemy)`
  - 逻辑：计算攻击伤害、应用技能效果、更新状态（如冷却）

**技能树**
- `SkillTree`
  - 属性：`UnlockedSkills`（HashSet<string>）
  - 方法：`UnlockSkill(skillName)`, `IsUnlocked(skillName)`

#### 2.2 测试用例
- `CharacterTests`
  - 测试伤害计算、治疗、死亡状态
- `CombatSystemTests`
  - 测试单回合战斗结果正确性
  - 测试使用技能、冷却时间正确处理
- `SkillTreeTests`
  - 测试解锁技能与查询技能是否已解锁的逻辑

---

### 阶段 3: 城建与经济系统扩展（建筑与资源生产）

#### 3.1 类结构

**建筑管理**
- `Building`
  - 属性：`BuildingType`、`Level`、`OutputResourceType`、`OutputRate`
  - 方法：`Upgrade()`, `Produce()`
- `BuildingManager`
  - 属性：`List<Building>`
  - 方法：`AddBuilding(type)`, `UpgradeBuilding(buildingId)`, `CollectResources()`

**资源生产逻辑**
- `ResourceProductionSystem`
  - 定期（通过Unity Update或Coroutine）从`BuildingManager`收集产出，更新`ResourceManager`
  
#### 3.2 测试用例
- `BuildingTests`
  - 测试升级逻辑、产出资源正确性
  - 测试不同建筑类型的逻辑
- `ResourceProductionSystemTests`
  - 测试在指定时间内正确产出资源

---

### 阶段 4: Boss 战与随机事件

#### 4.1 类结构

**Boss 战机制**
- `Boss`
  - 继承`Character`或实现相似接口
  - 增加特殊技能/阶段变化的属性和逻辑
- `BossFightSystem`
  - 方法：`StartBossFight(player)`
  - 内部逻辑考虑boss特殊机制（阶段性增援、特殊事件）

**随机事件系统**
- `RandomEvent`
  - 属性：`EventType`（枚举：ResourceBonus, EliteFight, Trap）
  - 方法：`Trigger()`
- `EventManager`
  - 方法：`RollEvent()`, `ExecuteEvent(randomEvent)`
  - 使用`RandomGenerator`进行随机决定
  
**随机生成器**
- `RandomGenerator`
  - 方法：`GetRandomInt(min, max)`, `GetRandomFloat(min, max)`
  - 通过依赖注入方便测试中使用假随机数据（Mock）

#### 4.2 测试用例
- `BossFightSystemTests`
  - 测试Boss特殊技能阶段
- `EventManagerTests`
  - 测试随机事件触发概率与结果的稳定性（使用Mock Random）

---

### 阶段 5: 竞技场与联网功能

#### 5.1 类结构

**网络服务层**
- `INetworkService`
  - 方法：`UploadCharacterData(data)`, `FetchOpponentData()`
- `NetworkService` (实现 `INetworkService`)
  - 使用 `HttpClient` 或 UnityWebRequest 实现数据上传/下载

**角色上传和匹配系统**
- `ArenaManager`
  - 方法：`UploadPlayerBuild()`, `FindMatch()`, `SimulateBattle(opponentData)`
  
**服务器数据模型**
- `PlayerDataModel`
  - 属性：`Level`, `EquippedItems`, `SkillTreeState`
  
#### 5.2 测试用例
- `ArenaManagerTests`
  - 使用Mock `INetworkService`测试数据上传和匹配逻辑
- `NetworkServiceTests`
  - 测试网络请求的正确性与错误处理（用Unity Test Runner的Mock或本地Server Stub）

---

### 阶段 6: 优化与扩展

- 增加新Tile类型、新敌人AI逻辑、新建筑物功能时，可在对应模块中新增子类或新枚举值，无需大幅修改原有逻辑。
- 扩展测试：对新增类编写单元测试，并通过集成测试检查系统协作。

---

## 3. 测试策略与持续集成

### 3.1 测试策略
- 单元测试（NUnit）：针对核心逻辑类（如`Character`、`ResourceManager`、`CombatSystem`）。
- 集成测试：在Unity的测试模式下模拟玩家操作，对路径编辑和战斗流程进行集成验证。
- 持续集成工具：GitHub Actions，在每次代码提交和Pull Request时运行自动化测试。

### 3.2 Mock与依赖注入
- 对随机数生成器、网络请求等外部依赖进行Mock，确保测试可重复且稳定。
- 利用接口与依赖注入（DI）使系统容易进行替换与测试。

---

## 4. 扩展性与维护

- 使用接口(`IResourceService`, `INetworkService`)和依赖注入简化单元测试与后续扩展。
- 模块化开发，将路径、战斗、经济、事件、竞技场分别封装，新增特性在相应模块内完成。
- 通过统一的事件总线（`EventBus`）解耦模块间通讯，减少直接依赖。

---

**总结**：本路线图.md 提供了从架构层面到类级设计的参考，以及相应的测试策略。您可在开发过程中根据此路线图为蓝本，细化类实现并确保架构清晰、可维护、可扩展。

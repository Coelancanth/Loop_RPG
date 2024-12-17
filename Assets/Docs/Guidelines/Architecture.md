## 1. 架构概览

### 1.1.1 分层与模块
整体采用分层与模块化架构，将游戏逻辑与表现层分离，并通过 Manager 类、Service 类和数据层进行管理。示意图如下：

```plaintext
+--------------------------------------------------------+
|                      Presentation                      |
|               (Unity Scenes, UI Prefabs)               |
+--------------------------+-----------------------------+
|         Managers         |           Systems           |
|  (GameManager, ... )     | (CombatSystem, PathSystem,  |
|                          |  ResourceSystem, ...)       |
+--------------------------------------------------------+
|        Data Access & Services (Data Models, Repos)     |
| (PlayerDataService, ResourceService, NetworkService)   |
+--------------------------------------------------------+
|                  Utility & Common Modules              |
|    (EventBus, Logging, Configuration, RandomGen)       |
+--------------------------------------------------------+

```

### 1.1.2 核心架构模式
- 采用 MVC + Service 分层架构
- 使用事件系统实现模块间解耦
- 采用单例模式管理全局服务
- 使用策略模式处理不同类型的游戏实体
- 采用观察者模式处理游戏事件
- 使用随机数管理系统，基于种子来生成固定场景，所有与随机相关的内容都应该依赖于`RandomManager.cs`



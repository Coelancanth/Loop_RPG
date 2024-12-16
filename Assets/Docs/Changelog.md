# Changelog

## 2024-12-17 06:04:58

### Added
- 基础事件系统 (EventBus)
- 资源管理系统 (ResourceManager)
- 路径系统 (PathSystem)
- 循环计数系统 (LoopCounter)
- 路径验证系统 (PathValidator)
- UI显示系统 (ResourceUI, LoopCounterUI)

### 架构设计
#### 事件系统设计
采用观察者模式实现的事件总线(EventBus)，用于解耦各系统间的通信：
- 使用单例模式确保全局唯一性
- 通过事件委托实现松耦合的消息传递
- 主要事件包括：
  - OnLoopCountChanged: 循环计数变化
  - OnBossTriggered: 触发Boss战
  - OnResourceChanged: 资源变化
  - OnTilePlaced: 放置Tile
  - OnTileRemoved: 移除Tile

#### 系统职责划分
1. PathSystem
   - 职责：管理Tile的放置和移除
   - 不直接处理游戏逻辑，只负责基础操作
   - 通过事件通知其他系统Tile的变化

2. PathValidator
   - 职责：验证路径有效性并触发计数器
   - 订阅OnTilePlaced事件
   - 与PathSystem解耦，专注于路径验证逻辑

3. LoopCounter
   - 职责：管理游戏循环计数
   - 提供Increment和Reset功能
   - 触发OnLoopCountChanged和OnBossTriggered事件

#### 事件流转路径
1. Tile放置流程：
```
TilePlacer (用户输入)
→ PathSystem.PlaceTile()
→ EventBus.TriggerTilePlaced
→ PathValidator.ValidatePathOnTilePlaced
→ LoopCounter.Increment
→ EventBus.TriggerLoopCountChanged
→ LoopCounterUI.UpdateUI
```

2. 资源变化流程：
```
ResourceManager (内部逻辑)
→ EventBus.TriggerResourceChanged
→ ResourceUI.UpdateUI
```

### 设计原则
1. 单一职责原则：
   - 每个系统只负责一个特定功能
   - UI显示与逻辑处理分离

2. 开闭原则：
   - 通过事件系统支持功能扩展
   - 新功能可以通过订阅事件实现，无需修改现有代码

3. 依赖倒置原则：
   - 系统间通过事件通信，而不是直接依赖
   - 使用接口（如IResourceService）进行解耦

4. 接口隔离原则：
   - UI组件只订阅需要的事件
   - 系统间的接口精确定义

### 测试策略
- 为核心系统提供单元测试
- 资源管理测试
- 路径系统测试
- 事件系统测试

### 后续优化方向
1. 路径验证算法的实现
2. Boss战触发机制的完善
3. 资源平衡性调整
4. UI交互优化


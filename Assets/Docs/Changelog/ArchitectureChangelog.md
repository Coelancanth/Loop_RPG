# 2D Loop 游戏架构文档

## 1. 系统架构图
### 1.1 高层架构图

项目采用模块化架构，主要由以下核心系统组成：
- Core Systems
  - GameManager：游戏主控制器
  - SceneSetup：场景初始化和引用管理
  - ResourceManager：资源管理系统
  - TileSystem：地块系统
  - RandomManager：随机数管理器
  - LoopCounter：循环计数系统
  - EventBus：事件总线系统

## 2. 模块和组件描述

### 2.1 核心系统（Core Systems）
#### 2.1.1 功能
- 游戏管理和初始化
- 资源管理
- 地块系统
- 随机数生成
- 事件系统
- 循环计数

#### 2.1.2 接口
- IResourceService：资源管理接口
  - GetResourceCount
  - TryConsumeResource
  - AddResource
  - GetMaxStorage

#### 2.1.3 依赖关系
- GameManager 依赖于：
  - ResourceManager
  - TileSystem
  - LoopCounter
  - TilePlacer
- SceneSetup 负责初始化和管理所有核心系统的引用

### 2.2 资源系统（Resource System）
#### 2.2.1 功能
- 资源计数管理
- 资源消耗验证
- 资源上限管理
- 资源UI显示

#### 2.2.2 组件
- ResourceManager：核心资���管理器
- ResourceUI：资源界面显示
- LoopRewardSystem：循环奖励系统

### 2.3 地块系统（Tile System）
#### 2.3.1 功能
- 地块放置和移除
- 网格坐标转换
- 地块预制体管理

#### 2.3.2 组件
- TileSystem：核心地块管理器
- TilePlacer：地块放置控制器
- Tile：地块基础类

### 2.4 路径系统（Path System）
#### 2.4.1 功能
- 路径生成
- 角色类型特定路径策略
- 路径可视化

#### 2.4.2 组件
- PathInitializer：路径初始化器
- PathVisualizer：路径可视化
- IPathInitializationStrategy：路径生成策略接口

### 2.5 调试系统（Debug System）
#### 2.5.1 功能
- 调试信息显示
- 测试控制器
- 单元测试

#### 2.5.2 组件
- DebugUI：调试界面
- PathTestController：路径测试控制器
- 各类测试类（RandomManagerTests, PathSystemTests, ResourceManagerTests）

## 3. 数据架构
### 3.1 数据模型
- ResourceType：资源类型枚举
- CharacterClass：角色类型枚举
- Vector2Int：网格坐标
- TileType：地块类型枚举

### 3.2 数据流
1. 用户交互 -> TilePlacer -> TileSystem
2. 资源变化 -> ResourceManager -> EventBus -> ResourceUI
3. 循环计数变化 -> LoopCounter -> EventBus -> LoopRewardSystem
4. 路径生成 -> PathInitializer -> PathVisualizer

## 4. 设计决策记录
### 4.1 单例模式使用
- RandomManager 采用单例模式确保随机数生成的一致性
- EventBus 采用单例模式实现全局事件系统

### 4.2 模块化设计
- 采用接口分离原则（IResourceService）
- 使用策略模式处理不同角色类型的路径生成
- 使用观察者模式（EventBus）处理系统间通信

### 4.3 测试策略
- 包含单元测试确保核心功能正确性
- 实现专门的测试控制器（PathTestController）
- 提供调试UI支持开发和测试

# TileSystem架构更新文档

## 1. 系统架构图
### 1.1 TileSystem组件关系
```
TileSystem
├── PrefabMap (预制体管理)
│   ├── RoadTilePrefab
│   ├── ForestTilePrefab
│   ├── MountainTilePrefab
│   └── EmptyTilePrefab
├── TileContainer (实例管理)
│   └── Dictionary<Vector2Int, BaseTile>
└── GridSystem (网格管理)
    ├── GridToWorld
    └── WorldToGrid
```

## 2. 模块和组件描述
### 2.1 预制体管理模块
#### 2.1.1 功能
- 管理和验证Tile预制体
- 提供类型安全的预制体访问
- 处理预制体实例化错误

#### 2.1.2 接口
- InitializePrefabMap(): 初始化预制体映射
- ValidatePrefab(): 验证预制体组件完整性

#### 2.1.3 依赖关系
- 依赖Unity预制体系统
- 依赖BaseTile及其子类
- 依赖SpriteRenderer组件

### 2.2 网格管理模块
#### 2.2.1 功能
- 维护网格坐标系统
- 提供世界坐标转换
- 支持网格可视化

#### 2.2.2 接口
- GridToWorld(): 网格坐标转世界坐标
- WorldToGrid(): 世界坐标转网格坐标
- DrawGrid(): 绘制网格辅助线

#### 2.2.3 依赖关系
- 依赖Unity Gizmos系统
- 依赖Vector2/Vector3转换

### 2.3 Tile实例管理模块
#### 2.3.1 功能
- 管理Tile游戏对象
- 处理Tile的生命周期
- 维护位置映射关系

#### 2.3.2 接口
- PlaceTile(): 放置Tile
- RemoveTile(): 移除Tile
- SetupReferences(): 设置系统引用

#### 2.3.3 依赖关系
- 依赖ResourceManager
- 依赖EventBus系统
- 依赖BaseTile组件

## 3. 数据架构
### 3.1 数据模型
- TileType: Tile类型枚举
- Vector2Int: 网格坐标
- Dictionary<Vector2Int, BaseTile>: Tile实例映射

### 3.2 数据流
1. Tile放置流程：
```
用户输入 → PlaceTile()
→ 预制体实例化
→ 位置转换
→ Tile初始化
→ 事件通知
```

2. Tile移除流程：
```
RemoveTile()
→ Tile清理
→ 实例销毁
→ 映射更新
→ 事件通知
```

## 4. 设计决策记录
### 4.1 预制体验证机制
- 决策：在编辑器中实现预制体验证
- 原因：
  - 提前发现预制体配置错误
  - 减少运行时错误
  - 提供清晰的错误提示
- 影响：
  - 增加了编辑器开发时间
  - 提高了系统稳定性
  - 改善了开发体验

### 4.2 网格可视化系统
- 决策：实现编辑器网格可视化
- 原因：
  - 便于关卡设计
  - 提供直观的空间参考
  - 辅助调试开发
- 影响：
  - 提升了开发效率
  - 增强了编辑器功能
  - 优化了调试体验

### 4.3 Tile实例管理策略
- 决策：使用字典存储Tile实例
- 原因：
  - O(1)的查找效率
  - 便于管理Tile生命周期
  - 简化位置映射
- 影响：
  - 提高了性能
  - 简化了代码逻辑
  - 便于扩展功能

## 关键设计原则
- 依赖注入原则
- 单一职责原则
- 开闭原则
- 接口隔离原则
- 依赖倒置原则
- 组合优于继承原则
- 迪米特法则
- KISS原则
- 健壮性原则
- 数据驱动
- 拓展性设计
- 单一入口（Centralized Entry Point）
- 模块化开发

###  数据驱动的模块开发
引入更多 **数据驱动** 的设计思想，将游戏的核心逻辑通过配置文件或脚本定义：

-  **路径和 Tile 的属性**：通过 JSON 文件定义 Tile 类型及其效果（如资源消耗、增益）。
-  **敌人和 Boss 的行为**：将敌人 AI、攻击模式等从代码中剥离出来，使用数据配置增强灵活性。

###  模块间依赖优化

- 使用依赖注入
- 通过依赖注入，减少模块间的直接依赖，增强模块的可替代性

###  依赖倒置原则
核心思想：
- 高层模块（如系统逻辑）不应该直接依赖于低层模块（如具体实现），二者都应该依赖于抽象。
- 抽象（接口或基类）不应该依赖于具体实现，而具体实现应该依赖于抽象。

###  组合优于继承（Composition over Inheritance）
核心思想：
- 优先通过组合（将功能分解为独立组件并组合到对象中）实现功能，而不是通过继承（将所有功能集成到一个复杂的类层次中）。

###  迪米特法则（Law of Demeter, LoD）
核心思想：
- 一个模块或类不应该直接依赖过多的其他模块，只与其直接需要交互的模块通信。
- 简化模块间的依赖关系，降低耦合度。

###  测试驱动开发（Test-Driven Development, TDD）
核心思想：先写测试用例，再实现功能，确保代码符合预期。

- 核心模块（路径、战斗、资源管理）必须有覆盖率较高的单元测试。
- 例如，测试路径系统是否能够正确验证路径连续性。

### 日志与监控

- 统一日志系统：为调试提供标准化的日志输出。
- 错误监控：为关键模块（如资源系统、战斗系统）引入异常处理机制，避免运行时崩溃。

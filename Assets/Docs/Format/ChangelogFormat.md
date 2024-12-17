# Changelog Template

#### 1. **总述**
   - 明确列出当前更改的版本号或迭代编号。
   - 简要概述本次更新的目标、影响的模块及整体改动的意义。

#### 2. **改动内容**
   - **新增功能**：
     - 详细说明新增的功能或模块，包括功能描述、实现思路及关键依赖。
     - 提供涉及的主要类、方法或 API。
     - 并给出如何使用的例子。
   - **修复内容**：
     - 列出修复的 Bug 或问题，描述问题的表现及解决方案。
     - 涉及的代码路径（如：`ClassName.cs`、`MethodName()`）。
   - **优化内容**：
     - 描述性能或代码优化的部分，包括改进的原因、思路和测试结果。
     - 如有性能提升的数据，请一并提供。
   - **调整与重构**：
     - 说明重构或调整的模块，列出原有设计的不足与优化后的设计。
     - 提供涉及的类结构或方法重写概述。

#### 3. **依赖关系**
   - 明确此次改动是否引入新依赖：
     - 外部库、包或服务的版本号。
     - 影响的模块与耑合度。
   - 如果修改了依赖版本，请附上旧版本与新版本的差异说明。

#### 4. **改动思路与设计原则**
   - 列出此次改动的核心设计原则，例如：
     - **解耦设计**：通过引入接口或事件总线减少依赖。
     - **单一责职**：模块或类拆分的逻辑。
     - **代码重用**：提取公共方法或模块。
   - 提供改动前后的示意图或代码示例（如有）。

#### 5. **测试验证**
   - 列出本次改动的测试内容与结果：
     - **单元测试**：说明测试的类与方法，测试通过率。
     - **集成测试**：验证系统整体行为，特别是交互部分。
     - **性能测试**：列出性能改进数据。

#### 6. **风险与注意事项**
   - 列出本次改动可能带来的风险：
     - 新增功能的稳定性。
     - 依赖调整可能影响的模块。
   - 提供相应的回滚方案与应急措施。

#
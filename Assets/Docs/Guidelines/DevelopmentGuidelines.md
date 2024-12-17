## 命名规范与约定
- 类名以大写开头，使用 PascalCase（如：`PathTile`、`ResourceManager`）。
- 接口名以 `I` 开头（如 `IResourceService`）。
- 脚本文件与类名一致，一个文件定义一个类。
- 测试类文件命名为 `{ClassName}Tests.cs`，存放于 `Tests` 目录内。
- 方法命名：

   - 使用 PascalCase。
   - 动词开头，表示具体的操作，例如：
        GetResourceCount、ValidatePath、StartBattle
   - 事件处理方法以 On 开头，例如：
        OnTilePlaced、OnResourceChanged

- 变量命名：

    - 局部变量和参数：camelCase。
        int resourceCount;
    - 类级字段：_camelCase（下划线开头），例如：
        _currentHealth、_tileList
    - 常量：全大写，单词之间用下划线分隔，例如：
        MAX_PLAYER_LEVEL、DEFAULT_TILE_COST


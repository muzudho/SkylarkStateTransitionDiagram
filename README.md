# YukaiLarkStateTransitionDiagram

MonoGame で作る、AI エージェントとの相談に使いやすい状態遷移図作成ツールです。

## 状態遷移図を読み書きしたい人

アプリを起動して、状態と遷移を並べたい人向けの入口です。

- [使い方説明書](YukaiLarkStateTransitionDiagram/Docs/使い方説明書.md) - 起動方法、画面の見方、保存・読込、ショートカット一覧
- [Docs README](YukaiLarkStateTransitionDiagram/Docs/README.md) - 説明書フォルダーの案内板

すぐ起動する場合は、リポジトリー直下で次を実行します。

```powershell
dotnet run --project .\YukaiLarkStateTransitionDiagram\YukaiLarkStateTransitionDiagram.csproj
```

## このツールを作りたい人

アプリの実装、設計、今後の作業を見たい人向けの入口です。

- [YukaiLarkStateTransitionDiagram.slnx](YukaiLarkStateTransitionDiagram.slnx) - Visual Studio / .NET 用ソリューション
- [YukaiLarkStateTransitionDiagram/](YukaiLarkStateTransitionDiagram/) - アプリケーション本体
- [YukaiLarkStateTransitionDiagram.csproj](YukaiLarkStateTransitionDiagram/YukaiLarkStateTransitionDiagram.csproj) - C# プロジェクト設定
- [Program.cs](YukaiLarkStateTransitionDiagram/Program.cs) - アプリ起動入口
- [Game1.cs](YukaiLarkStateTransitionDiagram/Game1.cs) - MonoGame のメイン実装
- [Content/](YukaiLarkStateTransitionDiagram/Content/) - MonoGame コンテンツ管理
- [開発日誌 2026-06-21](YukaiLarkStateTransitionDiagram/Docs/開発/開発日誌_2026-06-21.md) - 今日進んだ作業の記録
- [続きはここから](YukaiLarkStateTransitionDiagram/Docs/続きはここから.md) - 作業再開用メモ
- [【これは人間専用】メモ](YukaiLarkStateTransitionDiagram/Docs/【これは人間専用】/【これは人間専用】メモ.md) - 人間向けの自由メモ

# YukaiLarkStateTransitionDiagram

YukaiLarkStateTransitionDiagram は、状態と遷移を画面上に並べて、AI エージェントとの相談に使いやすい状態遷移図を作るための無料オープンソースアプリです。

<img src="YukaiLarkStateTransitionDiagram/Docs/images/icon-thumbnail-128.png" alt="YukaiLarkStateTransitionDiagram のアプリアイコン" width="96">

![サンプル状態遷移図の画面例](YukaiLarkStateTransitionDiagram/Docs/images/screenshot-sample.png)

## できること

- 日本語ラベル付きの状態ノードを作れます。
- 状態から状態への遷移を矢印でつなげます。
- 自己ループも作れます。
- 遷移ラベル、矢印の曲がり方、接点位置を調整できます。
- 状態遷移図を JSON として保存・読込できます。
- 図を PNG 画像として出力できます。
- ユカイラークという鳥のアシスタントが、次に作れそうな部品を提案します。

## こんな人向け

- ゲームやアプリの画面遷移を整理したい人
- AI エージェントへ状態遷移を説明するための図を作りたい人
- 日本語で状態名やイベント名を書きたい人
- 軽く動かせる状態遷移図ツールを探している人

## 起動方法

リポジトリー直下で次のコマンドを実行します。

```powershell
dotnet run --project .\YukaiLarkStateTransitionDiagram\YukaiLarkStateTransitionDiagram.csproj
```

Visual Studio から起動する場合は、`YukaiLarkStateTransitionDiagram.slnx` を開いて実行します。

## 基本操作

| やりたいこと | 操作 |
| --- | --- |
| 状態を追加 | `N` |
| 状態や遷移のラベルを編集 | 選択して `F2` または `Enter` |
| 遷移を作成 | `Shift` + 状態から状態へ左ドラッグ |
| 自己ループを作成 | `Shift` + 同じ状態上で左ドラッグして離す |
| 保存 | `Ctrl + S` |
| 読込 | `Ctrl + O` |
| PNG画像として出力 | `Ctrl + P` |

詳しい操作は [使い方説明書](YukaiLarkStateTransitionDiagram/Docs/使い方説明書.md) を見てください。

## ユカイラークについて

画面に出てくる鳥の名前は「ユカイラーク」です。

ユカイラークは、図が空のときは開始ノード、状態が足りないときは次の状態、遷移が足りないときは矢印、といったように、今の図に合わせて作図を手伝います。吹き出しが出ているときに `Enter` を押すか、ユカイラークをクリックすると、提案された部品を追加できます。

## 必要なもの

- Windows
- .NET SDK
- Visual Studio 2022 または `dotnet` CLI

## ドキュメント

- [使い方説明書](YukaiLarkStateTransitionDiagram/Docs/使い方説明書.md)
- [Docs README](YukaiLarkStateTransitionDiagram/Docs/README.md)
- [開発日誌 2026-06-21](YukaiLarkStateTransitionDiagram/Docs/開発/開発日誌_2026-06-21.md)

## ライセンス

このリポジトリーは [MIT License](LICENSE.txt) です。

# README.md

## 環境準備

* Dotnet Core 2.1

* Docker


## 1.安裝環境

Docker : [Download Docker for Windows](https://docs.docker.com/docker-for-windows/install/)

DotNet Core 2.1 : [Download .Net Core SDK](https://www.microsoft.com/net/download)  



## 2.準備Chrome Binary

請用Power Shell執行以下指令

```bash
docker run -dt --rm --name headless-chromium adieuadieu/headless-chromium-for-aws-lambda:stable
docker cp headless-chromium:/bin/headless-chromium ./
docker stop headless-chromium
```

參考網址 : [Chrome/Chromium on AWS Lambda](https://github.com/adieuadieu/serverless-chrome/blob/master/docs/chrome.md)



將產生的headless-chromium加入到專案中，並且設定**屬性** >> **複製到輸出目錄**為永遠複製



## 3.Docker中測試

1.**修改程式**

```csharp
var record = sqsEvent.Records.First();
var parameter = JsonConvert.DeserializeObject<SqsEventBodyEntity>(record.Body);
snapshotService.Do(parameter.SnapshotUrl);
```



2.**Publish**

請用Power Shell並將目錄移到專案有 `.proj` 的那層目錄，並執行

```bash
dotnet publish
```



3.**修改 test.json 中的 SnapshotUrl**

```
...
"body": "{\"SnapshotUrl\": \"https://www.91app.com\"}"
...
```



4.**執行拍照程式**

```bash
cat test.json | docker run --env-file ./env.variable --rm -v ${PWD}/bin/Debug/netcoreapp2.1/publish:/var/task -i -e DOCKER_LAMBDA_USE_STDIN=1 lambci/lambda:dotnetcore2.1 SnapshotDemo::SnapshotDemo.SqsHandler::Snapshot
```

如果成功可以在根目錄找到 `YourSnapshotImage.png`
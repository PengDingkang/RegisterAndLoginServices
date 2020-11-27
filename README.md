# Register And Login Services
登录和注册服务

## 开发环境
- .NET Core 5.0
- C# 9.0
- Microsoft Visual Studio Professional 2019 v16.8.1

## API 文档
https://docs.apipost.cn/view/b18a5c685bc214c8#3301654

## 部署文档
> OS: Debian GNU/Linux 10 (buster) x86_64  
> Kernel: 4.19.0-12-amd64

**安装 .NET 5.0 SDK** 

> 安装 .NET 之前，请运行以下命令，将 Microsoft 包签名密钥添加到受信任密钥列表，并添加包存储库。

```shell
wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
```

使用 `apt-get` 安装

```shell
sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-5.0
```

下载本项目

```shell
git clone https://github.com/Huangdu-Mental-Health-Center/RegisterAndLoginServices.git
cd ./RegisterAndLoginServices
```

**手动添加配置文件**

- 在项目根目录下新建 app.config，添加以下内容，并手动替换其中值字段

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <add key="Secret" value="你的 JWT Secret"/>
    <add key="Domain" value="你的 JWT Domain"/>
  </appSettings>
  <connectionStrings>
    <add name="UserDB" connectionString="你的数据库连接字符串"/>
  </connectionStrings>
</configuration>
```

编译并运行

```shell
dotnet build -c release
./bin/Release/net5.0/RegisterAndLoginServices
```


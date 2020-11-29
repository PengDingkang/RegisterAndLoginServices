# Register And Login Services
Huangdu Mental Health Center 登录和注册服务  
![build_status](https://github.com/Huangdu-Mental-Health-Center/RegisterAndLoginServices/workflows/.NET%20Core/badge.svg)
![language](https://img.shields.io/badge/language-C%23%209.0-blue.svg)
![env](https://img.shields.io/badge/-.NET%20Core%205.0-blueviolet)
## 1. 开发环境

- OS: Debian GNU/Linux 10 (buster) x86_64  
- Kernel: 4.19.0-12-amd64
- Microsoft Visual Studio Professional 2019 v16.8.1

## 2. [API 文档](https://docs.apipost.cn/view/b18a5c685bc214c8#3301654)


## 3. 部署文档
### **1.安装 .NET 5.0 SDK** 

0. 安装 .NET 之前，请根据自身系统环境将 Microsoft 包签名密钥添加到受信任密钥列表，并添加包存储库。  
[MS-Document](https://docs.microsoft.com/en-us/windows-server/administration/linux-package-repository-for-microsoft-software)

1. 安装 .Net 5.0  
  [MS-Document](https://docs.microsoft.com/en-us/dotnet/core/install/linux)

2. clone 本项目

```shell
git clone https://github.com/Huangdu-Mental-Health-Center/RegisterAndLoginServices.git
cd ./RegisterAndLoginServices
```

### **2. 手动添加配置文件**

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

- 编译并运行

```shell
dotnet run -c release
```


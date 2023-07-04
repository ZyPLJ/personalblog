# 个人博客系统

# 介绍

个人博客系统，采用.net core微服务技术搭建，采用传统的MVC模式，使用EF core来对mysql数据库进行CRUD操作项目。
本项目编写代码的软件是Visual Studio2022，基于 **.net6开发需要安装相关依赖如.Net6 SDK** 

# 项目浏览地址

[ZY个人博客](https://pljzy.top/)

# 配套后台管理系统

没怎么更新后台管理系统了，如果需要最新的源码吗，请关注公众号联系我，公众号在最下面。

[PersonalblogVue: 基于Vue3框架搭建的个人博客后台管理系统。 (gitee.com)](https://gitee.com/zyplj/personalblog-vue)

# 项目截图

![微信截图_20230202233418](https://gitee.com/zyplj/personalblog/raw/master/截图/微信截图_20230202233418.png)
只展示部分截图了

![微信截图_20230202233501](https://gitee.com/zyplj/personalblog/raw/master/截图/微信截图_20230202233501.png)

![微信截图_20230202233610](https://gitee.com/zyplj/personalblog/raw/master/截图/微信截图_20230202233610.png)

![微信截图_20230202233806](https://gitee.com/zyplj/personalblog/raw/master/截图/微信截图_20230202233806.png)

# 生成数据库

## 方法一 命令行生成

你需要修改数据库连接字符串，在本项目的Personalblog.Model中，找到MyDbContextDesignFac类。
MyDbContextDesignFac是用来创建数据库，修改该类其中的connStr即可， **然后在主项目的Program.cs中配置连接字符串** 。
修改完连接字符串后就可以着手生成数据库了，本项目采用core first模式.
在这个地方输入
![image](https://gitee.com/zyplj/personalblog/raw/master/截图/image.png)

```
Add-Migration Init(随便起名) //等待类文件的创建，创建成功后就可以下一步
Update-Database //输入这个成功后就可以看到mysql数据库已经成功创建了。
```

## 方法二 Sql文件生成

请下载：[Sql文件](http://47.113.150.96:4608/UpLoad/DownloadFile?filepath=%2Fwwwroot%2FUploadFolder%2FPersonalblog.sql)

本项目根目录已经包含该sql文件

# 项目打包

## 如何使用命令行打包项目

对于使用其他编辑器的人，可以选择命令行、终端打包项目
[命令行打包](https://www.cnblogs.com/ZYPLJ/p/17138996.html)

## 参考书签项目

文章地址[vue＋.net入门级书签项目 - 妙妙屋（zy） - 博客园 (cnblogs.com)](https://www.cnblogs.com/ZYPLJ/p/17133550.html)

# Docker部署

```
//进入项目目录中
docker build -t 镜像别名 .
//等待镜像下载,然后创建容器
//这里的端口是指docker内部端口和你本机的端口，你需要映射出来，才能访问到项目
docker run -d -p 端口:端口 --name 镜像名 启用镜像的别名
//列如:
docker build -t blog .
docker run -d -p 8080:8080 --name blog blog
//这样就能通过http/https://locahost:8080访问到项目了
```

# 运行必看

## 更据自己需求修改端口

![Dockerfile](https://gitee.com/zyplj/personalblog/raw/master/截图/Dockerfile.png)

Dockefile文件`EXPOSE`对应

![配置文件1](https://gitee.com/zyplj/personalblog/raw/master/截图/配置文件1.png)

## 设置数据库连接字符串和版本号

sqlite3如下：

```c#
string connStr = "Data Source=app.db";
opt.UseSqlite(connStr);
```

mysql如下：

`Server=ip;Port=3306;Database=Personalblog; User=用户;Password=密码;`

![配置文件2](https://gitee.com/zyplj/personalblog/raw/master/截图/配置文件2.png)

## 跨域

此处为你的博客后台管理系统的地址

![跨域](https://gitee.com/zyplj/personalblog/raw/master/截图/跨域.png)

## 没有Visual Studio如何运行项目

[命令行运行](https://www.cnblogs.com/ZYPLJ/p/17138996.html)

## 运行成功没有图片怎么办

在Personalblog\wwwroot\media添加如下文件夹即可

blog 为文章的存储文件夹 创建即可 后台管理上传

photofraphy 为博客图片存放的文件夹 创建即可 后台管理上传

Top 为博客首页文章显示的大图文件夹  创建之后放入自己喜欢的图片即可

yasuo 为博客首页文章显示的小图文件夹 创建之后放入自己喜欢的图片即可

yasuo2  为博客图片压缩图片存放路径，创建即可 后台管理上传![](D:\.net6\Presonablog最新\personalblog-master\截图\屏幕截图162907.png)

## 运行成功，文章列表显示不安全怎么办

> 这是因为我在列表跳转的时候加上了https，只需要修改Personalblog.Services下的CategoryService类中GetNodes方法的new { categoryId = c.Id },"https" 改成http即可

# 声明

该项目是基于 **[画星星博客主页](https://github.com/Deali-Axy/StarBlog)** 博主的项目而来。
本博主只是修改了部分代码。采用技术大相近同。

# 遇到问题

![](https://pljzy.top/images/5192045913af4a31a7988ed7077a1e0.jpg)

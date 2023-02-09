# 个人博客系统

#### 介绍
个人博客系统，采用.net core微服务技术搭建，采用传统的MVC模式，使用EF core来对mysql数据库进行CRUD操作项目。
本项目编写代码的软件是Visual Studio2022，基于 **.net6开发需要安装相关依赖如.Net6 SDK** 

#### 项目截图

![输入图片说明](https://gitee.com/zyplj/personalblog/raw/master/项目截图/%E5%BE%AE%E4%BF%A1%E6%88%AA%E5%9B%BE_20230202233418.png)
![输入图片说明](https://gitee.com/zyplj/personalblog/raw/master/项目截图/%E5%BE%AE%E4%BF%A1%E6%88%AA%E5%9B%BE_20230202233501.png)
![输入图片说明](https://gitee.com/zyplj/personalblog/raw/master/项目截图/%E5%BE%AE%E4%BF%A1%E6%88%AA%E5%9B%BE_20230202233610.png)
![输入图片说明](https://gitee.com/zyplj/personalblog/raw/master/项目截图/%E5%BE%AE%E4%BF%A1%E6%88%AA%E5%9B%BE_20230202233806.png)
只展示部分截图了

#### 安装项目
你需要修改数据库连接字符串，在本项目的Personalblog.Model中，找到MyDbContextDesignFac类。
MyDbContextDesignFac是用来创建数据库，修改该类其中的connStr即可， **然后在主项目的Program.cs中配置连接字符串** 。
修改完连接字符串后就可以着手生成数据库了，本项目采用code first模式.
在![创建数据库](https://gitee.com/zyplj/personalblog/raw/master/项目截图/image.png)这个地方输入

```
Add-Migration Init(随便起名) //等待类文件的创建，创建成功后就可以下一步
Update-Database //输入这个成功后就可以看到mysql数据库已经成功创建了。
```



#### Docker部署
 **前提需要将项目打包，一图流：** 

![输入图片说明](https://gitee.com/zyplj/personalblog/raw/master/项目截图/%E5%BE%AE%E4%BF%A1%E6%88%AA%E5%9B%BE_20230204174726.png)

![输入图片说明](https://gitee.com/zyplj/personalblog/raw/master/项目截图/%E5%BE%AE%E4%BF%A1%E6%88%AA%E5%9B%BE_20230204174746.png)

![输入图片说明](https://gitee.com/zyplj/personalblog/raw/master/项目截图/%E5%BE%AE%E4%BF%A1%E6%88%AA%E5%9B%BE_20230204174804.png)
```
//进入打包好的项目目录中
docker build -t 镜像别名 .
//等待镜像下载,然后创建容器
//这里的端口是指docker内部端口和你本机的端口，你需要映射出来，才能访问到项目
docker run -d -p 端口:端口 --name 镜像名 启用镜像的别名
//列如:
docker build -t blog .
docker run -d -p 8080:8080 --name blog blog
//这样就能通过http/https://locahost:8080访问到项目了
```

#### 声明
该项目是基于 **[画星星博客主页](https://github.com/Deali-Axy/StarBlog)** 博主的项目而来，他的主页有详细的教程哟。
本博主只是修改了部分代码。

#### 遇到问题
![输入图片说明](https://gitee.com/zyplj/personalblog/raw/master/Personalblog/wwwroot/images/5192045913af4a31a7988ed7077a1e0.jpg)


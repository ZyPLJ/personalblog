using Personalblog.Model;
using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalblog.Migrate
{
    public class CreateMd
    {
        public void C(MyDbContext myDbContext)
        {
            var exclusionDirs = new List<string> { ".git", "logseq", "pages" };
            var assetsPath = Path.GetFullPath("../../../.net6/Personalblog/Personalblog/wwwroot/media/blog");
            //md文件存放的路径
            const string importDir = @"D:\md";

            //数据导入
            WalkDirectoryTree(new DirectoryInfo(importDir));


            void WalkDirectoryTree(DirectoryInfo root)
            {
                Console.WriteLine($"正在扫描文件夹：{root.FullName}");
                FileInfo[]? files = null;
                DirectoryInfo[]? subDirs = null;
                try
                {
                    files = root.GetFiles("*.md");
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }

                if (files != null)
                {
                    foreach (FileInfo file in files)
                    {
                        Console.WriteLine(file.FullName);
                        var postPath = file.DirectoryName!.Replace(importDir, "");
                        Console.WriteLine(postPath);
                        /*
                         * 这里获取了md文件夹下面的文件名
                         * 做啥用？？？ 用来做目录分类
                        */
                        var categoryNames = postPath.Split("\\");
                        Console.WriteLine($"categoryNames: {string.Join("", categoryNames)}");
                        //这是一个文章分类的类
                        List<Category> categories = new List<Category>();
                        Database database = new Database(myDbContext);
                        if (categoryNames.Length > 0)
                        {
                            database.InsertC(categories, categoryNames);
                            /*
                             *id为1的话就是.net类别
                             *2就是其他
                             */
                            Category category = database.Select(categoryNames[1]);

                            //这里是读取文件了
                            var render = file.OpenText();
                            var content = render.ReadToEnd();
                            //Console.WriteLine(content);
                            render.Close();

                            //保存文字
                            Post post = new Post()
                            {
                                //这里id给的随机值
                                Id = GuidUtils.GuidTo16String(),
                                //把文件名当做标题
                                Title = file.Name.Replace(".md", ""),
                                //正文内容 交给前端去解析
                                Content = content,
                                //文件的相对目录位置
                                Path = postPath,
                                //文件的创建时间
                                CreationTime = file.CreationTime,
                                //文件的最后修改花时间
                                LastUpdateTime = file.LastWriteTime,
                                CategoryId = category.Id,
                                Categories = category
                            };

                            var processor = new PostProcessor(importDir, assetsPath, post);
                            //处理文章图片和内容
                            post.Content = processor.MarkdownParse();
                            //提取前200个字
                            post.Summary = processor.GetSummary(200);
                            Console.WriteLine(post.Summary);
                            database.Add(post);
                        }

                    }
                }


                subDirs = root.GetDirectories();
                foreach (var dirInfo in subDirs)
                {
                    if (exclusionDirs.Contains(dirInfo.Name))
                    {
                        continue;
                    }

                    if (dirInfo.Name.EndsWith(".assets"))
                    {
                        continue;
                    }

                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo);
                }
            }


        }
    }
}

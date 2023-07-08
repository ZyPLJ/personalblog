namespace Personalblog.Extensions.SendEmail;

public class MessageBoardNotificationEmailTemplate:EmailTemplate
{
    protected override string GetStyle()
    {
        return @"
            /* 添加样式 */
            body {
                font-family: Arial, sans-serif;
                font-size: 14px;
                line-height: 1.5;
                color: #333;
            }
            .box {
                background-color:#90F8FF ;
                border-radius: 5px;
                padding: 20px;
            }
            h1 {
                font-size: 24px;
                font-weight: bold;
                margin-bottom: 20px;
                color: #333;
            }
            p {
                margin-bottom: 10px;
                color: #333;
            }
            a {
                color: #333;
            }";
    }

    protected override string GetBodyContent(EmailContent emailContent)
    {
        return $@"
            <div class='box'>
                <h1>ZY知识库</h1>
                <h3>留言板通知</h3>
                <p>内容如下：{emailContent.Content}</p>
                <p>点击跳转：<a href='https://pljzy.top/MsgBoard?page=1'>留言板地址</a></p>
            </div>";
    }
}
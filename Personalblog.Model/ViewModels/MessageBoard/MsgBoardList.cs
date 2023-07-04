using Personalblog.Model.Entitys;
using X.PagedList;

namespace Personalblog.Model.ViewModels.MessageBoard;

public class MsgBoardList
{
    public List<Messages> MessagesList;
    public IPagedList<Messages> PagedList;
    public List<Replies> RepliesList;
}
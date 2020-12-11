using HeritageWebApplication.Models;

namespace HeritageWebApplication.Repository
{
    public interface IDataManager
    {
        IRepository<Building> BuildingRepository { get; set; }
        IRepository<HeritageObject> HeritageObjectRepository { get; set; }
        IRepository<Comment> CommentRepository { get; set; }
    }
}
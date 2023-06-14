using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooDAL.Entities;

namespace ZooDAL.Services.Intefaces
{
    public interface ICommentService : IZooServiceBase<Comment>
    {

        Task<IEnumerable<Comment>> GetCommentsForAnimal(Animal animal);

        Task DeleteAllCommentsForAnimal(Animal animal);
    }
}

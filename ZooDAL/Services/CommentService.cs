using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooDAL.Entities;
using ZooDAL.Services.Intefaces;

namespace ZooDAL.Services
{
    public class CommentService : ZooServiceBase<Comment>, ICommentService
    {
        public CommentService(myContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Comment>> GetCommentsForAnimal(Animal animal)
        {
            var allComments = await GetAllAsync();
            var specificAnimalsComments = allComments.Where(comment => comment.AnimalID == animal.Id);
            return specificAnimalsComments;
        }

        public async Task DeleteAllCommentsForAnimal(Animal animal)
        {
            var allComments = await GetAllAsync();
            var specificAnimalsComments = allComments.Where(comment => comment.AnimalID == animal.Id).ToList();

            for (var i = 0; i < specificAnimalsComments.Count; i++) {
                DeleteAsync(specificAnimalsComments[i].Id).Wait();
            }
        }
    }
}

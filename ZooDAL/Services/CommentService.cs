using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooDAL.Entities;
using ZooDAL.Services.Intefaces;

namespace ZooDAL.Services
{
    public class CommentService : ZooService<Comment>, ICommentService
    {
        public CommentService(myContext context) : base(context)
        {
        }
    }
}

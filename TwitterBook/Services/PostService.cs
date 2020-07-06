using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterBook.Data;
using TwitterBook.Domain;

namespace TwitterBook.Services
{
    public class PostService : IPostService
    {

        //private readonly List<Post> _posts;

        //public PostService()
        //{
        //    _posts = new List<Post>();
        //    for (var i = 0; i < 5; i++)
        //    {
        //        _posts.Add(new Post
        //        {
        //            Id = Guid.NewGuid(),
        //            Name = $"Post Name {i}"
        //        });
        //    }
        //}


        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            //return _posts;

            return await _dataContext.Posts.ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            await _dataContext.Posts.AddAsync(post);
            var created = await _dataContext.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            //var exists = GetPostByIdAsync(postToUpdate.Id) != null;

            //if (!exists)
            //    return false;

            //var index = _posts.FindIndex(x => x.Id == postToUpdate.Id);
            //_posts[index] = postToUpdate;

            _dataContext.Posts.Update(postToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            //var post = GetPostByIdAsync(postId);

            //if (post == null)
            //    return false;

            //_posts.Remove(post);
            var post = await GetPostByIdAsync(postId);

            if (post == null)
                return false;

            _dataContext.Posts.Remove(post);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted  > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);

            if(post == null)
            {
                return false;
            }

            if(post.UserId != userId)
            {
                return false;
            }

            return true;

        }
    }
}

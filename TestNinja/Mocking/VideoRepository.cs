using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNinja.Mocking
{
    public interface IVideoRepository
    {
        IEnumerable<Video> GetUnprocessedVideos();
    }
    public class VideoRepository : IVideoRepository
    {
        // We use IEnumerable b/c we only want to iterate over this list and nothing else
        public IEnumerable<Video> GetUnprocessedVideos()
        {
            using (var context = new VideoContext())
            {
                var videos = (from video in context.Videos
                              where !video.IsProcessed
                              select video).ToList();

                return videos;
            }
        }
    }
}

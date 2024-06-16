using asg_form.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace idvvideo_api.Controllers
{
    public class Upload : Controller
    {

        [Route("api/v1/updata_video")]
        [HttpPost]

        public async Task<ActionResult<object>> update_img(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("Invalid image file.");
            var uuidN = Guid.NewGuid().ToString("N");
            // 将文件保存到磁盘
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "video", $"{uuidN}.idvpack");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }    // 返回成功响应
            return Ok($"{uuidN}.idvpack");

        }

        [Route("api/v1/videos")]
        [HttpPost]

        public async Task<ActionResult<object>> update_videos([FromBody] Video video)
        {
            using(TestDbContext db = new TestDbContext())
            {
                await db.video.AddAsync(video);
                await db.SaveChangesAsync();
            }
            return Ok($"success");

        }

        [Route("api/v1/videos")]
        [HttpGet]

        public async Task<ActionResult<object>> get_videos()
        {
            using (TestDbContext db = new TestDbContext())
            {
               var alldb= await db.video.ToListAsync();
                return Ok(alldb);
            }
           

        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiTiBot.Models;

namespace MathBot.Managers
{
    [Serializable]
    public class UrlManager
    {
        private TiTiBotDataContext db = new TiTiBotDataContext();

        public async System.Threading.Tasks.Task<string> AddUrlAsync(Url url)
        {
            var _myUrls = db.Urls.Where(t => t.CreatedBy == url.CreatedBy).Select(t => t.Address);
            if (!_myUrls.Contains(url.Address))
            {
                db.Urls.Add(url);
                try
                {
                    await db.SaveChangesAsync();
                    return "Đã thêm vào danh sách Urls.";
                }
                catch(Exception ex)
                {
                    return "Có lỗi khi thêm Url. Mã lỗi: " + ex.Message;
                }
            }
            else
            {
                return "Url đã tồn tại trong danh sách của bạn.";
            }
        }

        public IEnumerable<Url> GetAllUrls()
        {
            return db.Urls.AsEnumerable();
        }
    }
}
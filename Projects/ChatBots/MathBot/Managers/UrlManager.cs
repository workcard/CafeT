using System;
using System.Collections.Generic;
using System.Linq;
using MathBot.Models;

namespace MathBot.Managers
{
    public class UrlManager
    {
        private MathBotDataContext db = new MathBotDataContext();

        public async System.Threading.Tasks.Task<string> AddUrlAsync(UrlModel url)
        {
            var _myUrls = db.Urls.Where(t => t.CreatedBy == url.CreatedBy)
                .Select(t => t.Url);
            if (!_myUrls.Contains(url.Url))
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

        public IEnumerable<UrlModel> GetAllUrls()
        {
            return db.Urls.AsEnumerable();
        }
    }
}
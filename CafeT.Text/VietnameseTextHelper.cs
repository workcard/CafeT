namespace CafeT.Text
{
    public static class VietnameseTextHelper
    {
        public static bool IsDaily(this string text)
        {
            string[] DailyPatterns = new string[] { "daily", "hàng ngày" };
            foreach (string pattern in DailyPatterns)
            {
                if (text.ToLower().Contains(pattern))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

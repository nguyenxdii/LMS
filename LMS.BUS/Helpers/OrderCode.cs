namespace LMS.BUS.Helpers
{
    public static class OrderCode
    {
        public const string PREFIX = "ORD0304";
        public static string ToCode(int id) => PREFIX + id.ToString(); // 03041, 03042, ...
        public static bool TryParseId(string code, out int id)
        {
            id = 0;
            if (string.IsNullOrWhiteSpace(code)) return false;
            var s = code.Trim();
            if (s.StartsWith(PREFIX)) s = s.Substring(PREFIX.Length);
            return int.TryParse(s, out id);
        }
    }
}
